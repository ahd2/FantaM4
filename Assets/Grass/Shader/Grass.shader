Shader "Unlit/Grass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Col ("Color", Color) = (0,1,0,1)
        _Height ("Height", Range(0.1, 2)) = 0.2
        _Width ("Width", Range(0, 2)) = 0.1
        _Density ("Density", Range(1, 20)) = 2
        _SwingStrength ("Swing Strength", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct gs_input
            {
                float4 pos : POSITION;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float color : COLOR0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Col;
            float _Width;
            float _Height;
            int _Density;
            float _SwingStrength;

            // 工具函数：伪随机
            float2 randPos(float2 seed)
            {
                return frac(sin(seed * float2(127.1, 337.1)) * 43758.5453);
            }

            float rand(float seed)
            {
                return frac(sin(seed) * 43758.5453);
            }

            // 绕 Y 轴旋转（输入为 XZ 平面点，Y 不变）
            float3 rotateY(float3 pos, float angleDegrees)
            {
                float rad = radians(angleDegrees);
                float s = sin(rad);
                float c = cos(rad);
                float x = pos.x;
                float z = pos.z;
                pos.x = x * c - z * s;
                pos.z = x * s + z * c;
                return pos;
            }

            void genGrassWorld(float3 worldBasePos, inout TriangleStream<g2f> triStream)
            {
                const int split = 2;
                float baseHeight = _Height;
                float heightVariation = rand(worldBasePos.x * 24.2315 + worldBasePos.z * 0.1545) * (baseHeight * 0.75);
                float height = baseHeight + heightVariation;
                float maxHeight = height * split;

                float layerStep = 1.0 / split - 0.01;
                float v = layerStep;

                // 摆动
                float swing = cos(_Time.y * 2.0 + worldBasePos.x * 0.3 + worldBasePos.z * 0.3);
                float2 swingRand = (randPos(worldBasePos.xz) * 2.0 - 1.0);
                float3 swingOffset = float3(swingRand.x, 0, swingRand.y) * _SwingStrength * swing;

                // 随机倾斜 & 颜色
                float randLean = (rand(worldBasePos.x + worldBasePos.z + 123.45) * 2.0 - 1.0) * 0.3;
                float col = 1.0 - rand((worldBasePos.x + worldBasePos.z) * 15615) * 0.3;
                float angle = rand((worldBasePos.x + worldBasePos.z) * 5204 + height) * 360.0;

                float3 currentBase = worldBasePos; // 草从 worldBasePos.y 开始向上生长

                for (int i = 0; i < split; i++)
                {
                    float localY0 = i * height;
                    float localY1 = (i + 1) * height;

                    float bend0 = pow(localY0 / maxHeight, 2.0);
                    float bend1 = pow(localY1 / maxHeight, 2.0);

                    // 构建局部草片（在 XZ 平面，Y 向上）
                    float3 p1 = float3(_Width, localY0, 0);
                    float3 p2 = float3(-_Width, localY0, 0);
                    float3 p3 = float3(_Width, localY1, 0);
                    float3 p4 = float3(-_Width, localY1, 0);

                    // 添加倾斜（Z 方向偏移，模拟风吹弯）
                    p1.z += randLean * bend0;
                    p2.z += randLean * bend0;
                    p3.z += randLean * bend1;
                    p4.z += randLean * bend1;

                    // 绕 Y 轴旋转
                    p1 = rotateY(p1, angle);
                    p2 = rotateY(p2, angle);
                    p3 = rotateY(p3, angle);
                    p4 = rotateY(p4, angle);

                    // 转换到世界位置
                    float3 w1 = currentBase + p1 + swingOffset * bend0;
                    float3 w2 = currentBase + p2 + swingOffset * bend0;
                    float3 w3 = currentBase + p3 + swingOffset * bend1;
                    float3 w4 = currentBase + p4 + swingOffset * bend1;

                    // 输出三角形（两个三角形组成一个四边形）
                    g2f f1 = (g2f)0;
                    f1.vertex = UnityWorldToClipPos(w1);
                    f1.uv = float2(1, v - layerStep);
                    f1.color = col;
                    triStream.Append(f1);

                    g2f f2 = (g2f)0;
                    f2.vertex = UnityWorldToClipPos(w2);
                    f2.uv = float2(0, v - layerStep);
                    f2.color = col;
                    triStream.Append(f2);

                    g2f f3 = (g2f)0;
                    f3.vertex = UnityWorldToClipPos(w3);
                    f3.uv = float2(1, v);
                    f3.color = col;
                    triStream.Append(f3);

                    // 第二个三角形（f2-f3-f4）
                    triStream.Append(f2);
                    triStream.Append(f3);
                    g2f f4 = (g2f)0;
                    f4.vertex = UnityWorldToClipPos(w4);
                    f4.uv = float2(0, v);
                    f4.color = col;
                    triStream.Append(f4);

                    v += layerStep;
                }
                triStream.RestartStrip();
            }

            gs_input vert(appdata_base v)
            {
                gs_input o;
                o.pos = v.vertex;
                return o;
            }

            [maxvertexcount(120)] // 每根草最多 12 顶点（2 层 × 6），20 密度 → 240，保守设 120（可调）
            void geom(triangle gs_input p[3], inout TriangleStream<g2f> triStream)
            {
                // 计算输入三角形的对象空间中心
                float3 objCenter = (p[0].pos + p[1].pos + p[2].pos) / 3.0;

                // 转换到世界空间
                float3 worldCenter = mul(unity_ObjectToWorld, float4(objCenter, 1.0)).xyz;

                // 为每根草添加微小随机偏移（避免重叠）
                for (int i = 0; i < _Density; i++)
                {
                    float seed = worldCenter.x + worldCenter.z + i * 123.45;
                    float dx = (rand(seed + 0.1) * 2.0 - 1.0) * 0.3;
                    float dz = (rand(seed + 0.2) * 2.0 - 1.0) * 0.3;
                    float3 grassPos = worldCenter;
                    grassPos.x += dx;
                    grassPos.z += dz;
                    genGrassWorld(grassPos, triStream);
                }
            }

            fixed4 frag(g2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.1);
                col = col * _Col * i.color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}