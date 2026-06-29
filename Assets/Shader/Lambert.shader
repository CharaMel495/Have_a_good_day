Shader "Custom/Lambert"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalRenderPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // 影など要らない最小構成（必要なら後で足せる）
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float3 positionWS  : TEXCOORD1;
                float2 uv          : TEXCOORD2;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                VertexPositionInputs posInputs = GetVertexPositionInputs(v.positionOS.xyz);
                VertexNormalInputs   nrmInputs = GetVertexNormalInputs(v.normalOS);

                o.positionHCS = posInputs.positionCS;
                o.positionWS  = posInputs.positionWS;
                o.normalWS    = nrmInputs.normalWS;
                o.uv          = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                half3 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).rgb * (half3)_BaseColor.rgb;

                half3 N = normalize(i.normalWS);

                // メインライト取得
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
                half3 L = normalize(mainLight.direction);

                // Lambert: max(0, dot(N, L))
                half ndotl = saturate(dot(N, L));
                half3 diffuse = albedo * mainLight.color * (ndotl * mainLight.shadowAttenuation);

                return half4(diffuse, _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
