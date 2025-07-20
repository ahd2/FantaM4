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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            TEXTURE2D(_CameraNormalsTexture);
            
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
            
            float CalculateEdge(OutlineVaryings i)
            {
                float2 UV = i.positionCS.xy / _ScaledScreenParams.xy;

                // 从摄像机深度纹理中采样深度。
                #if UNITY_REVERSED_Z
                    real depth = SampleSceneDepth(UV);
                #else
                    //  调整 Z 以匹配 OpenGL 的 NDC ([-1, 1])
                    real depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SampleSceneDepth(UV));
                #endif

                // 重建世界空间位置。
                float3 worldPos = ComputeWorldSpacePosition(UV, depth, UNITY_MATRIX_I_VP);
                
                float halfScaleFloor = floor(_Scale * 0.5) * 0.5;
				float halfScaleCeil = ceil(_Scale * 0.5) * 0.5;

                float2 bottomLeftUV = i.texcoord - float2(_BlitTexture_TexelSize.x, _BlitTexture_TexelSize.y) * halfScaleFloor;
                float2 topRightUV = i.texcoord + float2(_BlitTexture_TexelSize.x, _BlitTexture_TexelSize.y) * halfScaleCeil;  
                float2 bottomRightUV = i.texcoord + float2(_BlitTexture_TexelSize.x * halfScaleCeil, -_BlitTexture_TexelSize.y * halfScaleFloor);
                float2 topLeftUV = i.texcoord + float2(-_BlitTexture_TexelSize.x * halfScaleFloor, _BlitTexture_TexelSize.y * halfScaleCeil);

                float depth0 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, bottomLeftUV).r;
                float depth1 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, topRightUV).r;
                float depth2 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, bottomRightUV).r;
                float depth3 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, topLeftUV).r;

                float3 normal0 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_LinearClamp, bottomLeftUV).rgb;
                float3 normal1 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_LinearClamp, topRightUV).rgb;
                float3 normal2 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_LinearClamp, bottomRightUV).rgb;
                float3 normal3 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_LinearClamp, topLeftUV).rgb;

                float3 normalFiniteDifference0 = normal1 - normal0;
                float3 normalFiniteDifference1 = normal3 - normal2;

                float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
                edgeNormal = edgeNormal > _NormalThreshold ? 1 : 0;

                float depthFiniteDifference0 = depth1 - depth0;
                float depthFiniteDifference1 = depth3 - depth2;
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;

                float3 viewdir = GetWorldSpaceViewDir(worldPos);
                float3 viewNormal = normalize(normal0);
                float NdotV = saturate(1 - dot(viewNormal, viewdir));
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

                //return NdotV;
                //return edgeDepth;
                return edge;
            }

            half4 Frag(OutlineVaryings i) : SV_TARGET
            {
                half2 uv = i.texcoord;
                //根据edge的大小，在边缘颜色和原本颜色之间插值，edge为0时，完全是边缘，edge为1时，完全是原始颜色
                half edge = CalculateEdge(i);
                //return edge;
                half4 withEdgeColor = lerp(_EdgeColor,SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv), 1-edge);

                //return half4(edge,1);
                return withEdgeColor;
                
            }
            ENDHLSL
        }
    }
    //后处理不需要Fallback，不满足的时候不显示即可
    Fallback off
}
