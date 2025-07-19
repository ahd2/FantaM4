Shader"Unlit/Outline"
{
    Properties
    {
        _EdgeColor("EdgeColor",Color)=(0,0,0,1)
        _Scale("Scale", Float) = 0.0
        _DepthThreshold("DepthThreshold", Float) = 0.0
        _NormalThreshold("NormalThreshold", Float) = 0.0
        _DepthNormalThreshold("DepthNormalThreshold", Float) = 0.0
        _DepthNormalThresholdScale("DepthNormalThresholdScale", Float) = 0.0
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
        Pass
        {
            Name "Outline"
            HLSLPROGRAM
            #pragma vertex OutlineVert
            #pragma fragment Frag
            //这两个头文件包括了大多数需要用到的变量
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"

            TEXTURE2D(_CameraNormalsTexture);
            TEXTURE2D(_CameraDepthTexture);
            
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
            CBUFFER_END

            struct OutlineVaryings
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
                float3 viewSpaceDir : TEXCOORD5;
                UNITY_VERTEX_OUTPUT_STEREO
            };

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
                output.viewSpaceDir = mul(UNITY_MATRIX_I_P, output.positionCS).xyz;
                return output;
            }

            half NormalSobel()
            {
                const half Gx[9] = {-1, -2, -1, 0, 0, 0, 1, 2, 1};
                const half Gy[9] = {-1, 0, 1, -2, 0, 2, -1, 0, 1};
                half texColor;
                half edgeX = 0, edgeY = 0;
                for (int it = 0; it < 9; it++)
                {
                    //RGB转亮度
                    texColor = Luminance(SAMPLE_TEXTURE2D_X(_CameraNormalsTexture, sampler_LinearClamp, uvs[it]));
                    //计算亮度在XY方向的导数，如果导数越大，越接近一个边缘点
                    edgeX += texColor * Gx[it];
                    edgeY += texColor * Gy[it];
                }
                //edge越小，越可能是个边缘点
                half edge = 1 - abs(edgeX) - abs(edgeY);
                return edge;
            }

            half DepthSobel()
            {
                const half Gx[9] = {-1, -1, -1, 0, 0, 0, 1, 1, 1};
                const half Gy[9] = {-1, 0, 1, -2, 0, 2, -1, 0, 1};
                half texColor;
                half edgeX = 0, edgeY = 0;
                for (int it = 0; it < 9; it++)
                {
                    //采样原始深度
                    float rawDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_LinearClamp, uvs[it]);
                    //转换成linear01深度
                    float linear01Depth = LinearEyeDepth(rawDepth, _ZBufferParams);
                    //RGB转亮度
                    texColor = Luminance(float3(linear01Depth,linear01Depth,linear01Depth));
                    //计算亮度在XY方向的导数，如果导数越大，越接近一个边缘点
                    edgeX += texColor * Gx[it];
                    edgeY += texColor * Gy[it];
                }
                //edge越小，越可能是个边缘点
                half edge = 1 - abs(edgeX) - abs(edgeY);
                return edge;
            }

            float CalculateEdge(OutlineVaryings i)
            {
                
                float halfScaleFloor = floor(_Scale * 0.5);
				float halfScaleCeil = ceil(_Scale * 0.5);

                float2 bottomLeftUV = i.texcoord - float2(_BlitTexture_TexelSize.x, _BlitTexture_TexelSize.y) * halfScaleFloor;
                float2 topRightUV = i.texcoord + float2(_BlitTexture_TexelSize.x, _BlitTexture_TexelSize.y) * halfScaleCeil;  
                float2 bottomRightUV = i.texcoord + float2(_BlitTexture_TexelSize.x * halfScaleCeil, -_BlitTexture_TexelSize.y * halfScaleFloor);
                float2 topLeftUV = i.texcoord + float2(-_BlitTexture_TexelSize.x * halfScaleFloor, _BlitTexture_TexelSize.y * halfScaleCeil);

                float depth0 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, bottomLeftUV).r;
                float depth1 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, topRightUV).r;
                float depth2 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, bottomRightUV).r;
                float depth3 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, topLeftUV).r;

                float3 normal0 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_LinearClamp, bottomLeftUV).rgb * 2 - 1;
                float3 normal1 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_LinearClamp, topRightUV).rgb* 2 - 1;
                float3 normal2 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_LinearClamp, bottomRightUV).rgb* 2 - 1;
                float3 normal3 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_LinearClamp, topLeftUV).rgb* 2 - 1;

                float3 normalFiniteDifference0 = normal1 - normal0;
                float3 normalFiniteDifference1 = normal3 - normal2;

                float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
                edgeNormal = edgeNormal > _NormalThreshold ? 1 : 0;

                float depthFiniteDifference0 = depth1 - depth0;
                float depthFiniteDifference1 = depth3 - depth2;
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;

                float3 viewNormal = normal0 * 2 - 1;
                float NdotV = 1 - dot(viewNormal, -i.viewSpaceDir);
                float normalThreshold01 = saturate((NdotV - _DepthNormalThreshold) / (1 - _DepthNormalThreshold));
                float normalThreshold = normalThreshold01 * _DepthNormalThresholdScale + 1;

                float depthThreshold = _DepthThreshold * depth0 * normalThreshold;
                edgeDepth = edgeDepth > depthThreshold ? 1 : 0;

                float edge = max(edgeDepth, edgeNormal);

                if (normal3.r == 1 && normal3.g == 1 && normal3.b == 1
                    && normal0.r == 1 && normal0.g == 1 && normal0.b == 1
                    && normal1.r == 1 && normal1.g == 1 && normal1.b == 1
                    && normal2.r == 1 && normal2.g == 1 && normal2.b == 1)
                {
                    edge = 0;
                }

                return edge;
            }

            half4 Frag(OutlineVaryings i) : SV_TARGET
            {
                half2 uv = i.texcoord;
                uvs[0] = uv + _BlitTexture_TexelSize.xy * half2(-1, -1);
                uvs[1] = uv + _BlitTexture_TexelSize.xy * half2(0, -1);
                uvs[2] = uv + _BlitTexture_TexelSize.xy * half2(1, -1);
                uvs[3] = uv + _BlitTexture_TexelSize.xy * half2(-1, 0);
                uvs[4] = uv + _BlitTexture_TexelSize.xy * half2(0, 0);
                uvs[5] = uv + _BlitTexture_TexelSize.xy * half2(1, 0);
                uvs[6] = uv + _BlitTexture_TexelSize.xy * half2(-1, 1);
                uvs[7] = uv + _BlitTexture_TexelSize.xy * half2(0, 1);
                uvs[8] = uv + _BlitTexture_TexelSize.xy * half2(1, -1);
                half edgeNormal = NormalSobel();
                half edgeDepth = DepthSobel();
                edgeDepth = step(0.2, edgeDepth);
                edgeNormal = step(0.5, edgeNormal);
                //根据edge的大小，在边缘颜色和原本颜色之间插值，edge为0时，完全是边缘，edge为1时，完全是原始颜色
                half edge = min(edgeNormal, edgeDepth);
                edge = CalculateEdge(i);
                //return edge;
                half4 withEdgeColor = lerp(_EdgeColor,SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv), 1-edge);
                
                return withEdgeColor;
                
            }
            ENDHLSL
        }
    }
    //后处理不需要Fallback，不满足的时候不显示即可
    Fallback off
}
