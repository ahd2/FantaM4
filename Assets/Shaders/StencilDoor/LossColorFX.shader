Shader "Unlit/LossColorFX"
{
    Properties
    {
        _EdgeColor("EdgeColor",Color)=(0,0,0,1)
        _Mask("Mask", Range(0.00001,0.17)) = 0.1
        _Light("Light", Range(0.00001,20)) = 0.1
        _Sature("Sature", Float) = 0.0

        _Ref("Stencil Ref",int) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline"
        }
        Cull Off
        Blend Off
        ZTest Off
        ZWrite Off
        Stencil
        {
            Ref [_Ref]
            Comp equal
            Pass keep
        }
        Pass
        {
            Name "LossColor"
            HLSLPROGRAM
            #pragma vertex OutlineVert
            #pragma fragment Frag
            //这两个头文件包括了大多数需要用到的变量
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            TEXTURE2D(_CameraNormalsTexture);
            TEXTURE2D(_CameraOpaqueTexture);
            TEXTURE2D(_DBufferTexture0);
            
            //这个需要自己声明，xy表示纹素的长宽，zw表示整个BlitTexture的长宽，BlitTexture就是当前摄像机的颜色缓冲区
            float4 _BlitTexture_TexelSize;
            float2 uvs[9];
            //常规CBUFFER，上面自定义的属性要写在这里
            CBUFFER_START(UnityPerMaterial)
            half4 _EdgeColor;
            float _Scale;
            float _DepthThreshold;
            float _NormalThreshold;
            float _DepthNormalThreshold;
			float _DepthNormalThresholdScale;
            float _Mask;
            float _Light;
            float _Sature;
            CBUFFER_END

            struct OutlineVaryings
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
                float3 viewSpaceDir : TEXCOORD5;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            // 假设你有相机的近平面四个角的世界空间方向
            // 或者你可以通过 ComputeViewSpaceDirectionFromUv 实现

            // 例如：
            float3 ComputeViewSpaceDirectionFromUv(float2 uv)
            {
                float x = uv.x * 2.0 - 1.0;
                float y = (1.0 - uv.y) * 2.0 - 1.0;
                float4 hclip = float4(x, y, 1.0, 1.0);
                float4 hview = mul(UNITY_MATRIX_I_P, hclip);
                return hview.xyz / hview.w;
            }

            OutlineVaryings OutlineVert(Attributes input)
            {
                OutlineVaryings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            #if SHADER_API_GLES
                float4 pos = input.positionOS;
                float2 uv  = input.uv;
            #else
                float4 pos = GetFullScreenTriangleVertexPosition(input.vertexID);
                float2 uv  = GetFullScreenTriangleTexCoord(input.vertexID);
            #endif

                output.positionCS = pos;
                output.texcoord   = uv * _BlitScaleBias.xy + _BlitScaleBias.zw;

                
                //output.viewSpaceDir = mul(UNITY_MATRIX_I_P, output.positionCS).xyz;
                float3 viewPos = (mul(UNITY_MATRIX_I_P, output.positionCS).xyz);
                output.viewSpaceDir = ComputeViewSpaceDirectionFromUv(uv);
                return output;
            }

            half4 Frag(OutlineVaryings i) : SV_TARGET
            {
                half2 uv = i.texcoord;

                // 从深度图采样线性深度
                float depth01 = SampleSceneDepth(uv); // 0~1, 非线性
                float linearDepth = LinearEyeDepth(depth01, _ZBufferParams); // 转线性深度（视空间下的距离）

                // 从UV转到NDC空间坐标（[-1,1]）
                float4 ndc = float4(uv * 2 - 1, 1, 1); // z=1是裁剪空间远平面方向

                // 反投影到视空间
                float4 viewPos = mul(UNITY_MATRIX_I_P, ndc);
                viewPos.xyz /= viewPos.w;
                viewPos.xyz = normalize(viewPos.xyz) * linearDepth;

                //视空间 → 世界空间
                float3 worldPos = mul(UNITY_MATRIX_I_V, float4(viewPos.xyz, 1)).xyz;
                
                // === 计算与相机距离并生成雾效 ===
                //float3 camPos = _WorldSpaceCameraPos;
                float3 camPos = float3(-6.94, 41,131.5);
                float distance = length(worldPos - camPos);
                
                //return edge;
                half4 withEdgeColor = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

                half4 color = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_LinearClamp, uv);

                float3 hsv = RgbToHsv(color.xyz);
                hsv.y += _Sature;
                color.xyz = HsvToRgb(hsv);
                
                half4 decal = SAMPLE_TEXTURE2D_X(_DBufferTexture0, sampler_LinearClamp, uv);

                half4 finalcol = lerp(withEdgeColor , color * withEdgeColor, 1-saturate(max(decal.r, distance*  _Mask)));
                //再加个场景亮度和饱和度吧
                return finalcol* _Light;
            }
            ENDHLSL
        }
    }
    //后处理不需要Fallback，不满足的时候不显示即可
    Fallback off
}
