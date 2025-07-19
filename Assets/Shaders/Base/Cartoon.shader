Shader "Unlit/Cartoon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("MainColor", Color) = (1, 1, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _SHADOWS_SOFT //软阴影
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                float3 normalWS  : TEXCOORD2;
                float4 shadowCoord : TEXCOORD4;
                float3 vertexSH : TEXCOORD5;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half3 _MainColor;
            CBUFFER_END

            

            v2f vert (appdata v)
            {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);//向量记得在片元归一化
                o.positionWS = TransformObjectToWorld(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertexSH = SampleSH(o.normalWS);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                i.normalWS = normalize(i.normalWS);
                //获取主光源
                i.shadowCoord = TransformWorldToShadowCoord(i.positionWS);
                Light mainLight = GetMainLight(i.shadowCoord);//雪地部分也用
                float shadow = mainLight.shadowAttenuation;
                
                half NoL = dot(i.normalWS, _MainLightPosition.xyz);
                half stepNoL = step(0, NoL);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                col.xyz *= _MainColor;
                col.xyz = lerp(col.xyz * i.vertexSH * 1.5, col.xyz * mainLight.color, stepNoL * shadow);

                return col;
            }
            ENDHLSL
        }
        UsePass "Universal Render Pipeline/Lit/DepthNormals"
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
