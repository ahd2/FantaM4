// Custom/URP/Decal/BasicDecal.shader
Shader "Custom/URP/Decal/BasicDecal"
{
    Properties
    {
        _BaseMap("Base Map (RGB) Alpha (A)", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _Intensity("Intensity", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Overlay"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
        }

        Pass
        {
            Name "Decal"
            Tags { "LightMode" = "UniversalForward" }

            // URP Decal 通常由渲染管线控制混合，但显式声明更安全
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;
            half4 _BaseColor;
            half _Intensity;

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // 采样贴图
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                half3 baseColor = tex.rgb * _BaseColor.rgb;
                half alpha = tex.a * _BaseColor.a * _Intensity;

                // URP Decal 标准输出：RGBA 直接作为混合数据
                // 注意：URP 的 Decal 系统会自动解释 A 为混合权重
                return half4(baseColor, alpha);
            }
            ENDHLSL
        }
    }

    // 关键：Fallback 必须指向 URP 的内置 Decal Shader
    Fallback "Hidden/Universal Render Pipeline/Decal"
}