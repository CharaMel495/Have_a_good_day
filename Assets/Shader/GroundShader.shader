Shader "Custom/GroundShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.8,0.8,0.8,1)
        _MainTex ("Base Texture", 2D) = "white" {}
        _ShadowStrength ("Shadow Strength", Range(0,1)) = 0.8
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "GroundShadow"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            // ===== Shadow variants =====
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float4 shadowCoord : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;
            float4 _BaseColor;
            float _ShadowStrength;

            Varyings vert (Attributes v)
            {
                UNITY_SETUP_INSTANCE_ID(v); //追加
                Varyings o;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //追加
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.positionWS = TransformObjectToWorld(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.shadowCoord = TransformWorldToShadowCoord(o.positionWS);

                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // ベース色
                half4 col =
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv)
                    * _BaseColor;

                // メインライト（影込み）
                Light mainLight = GetMainLight(i.shadowCoord);
                float shadow = mainLight.shadowAttenuation;

                // 影で暗くするだけ
                float shadowFactor = lerp(1.0 - _ShadowStrength, 1.0, shadow);
                col.rgb *= shadowFactor;

                return col;
            }
            ENDHLSL
        }
    }
}
