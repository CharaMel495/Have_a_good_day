Shader "Custom/URP/CharacterToon"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _MainTex ("Base Texture", 2D) = "white" {}

        _ShadowColor ("Shadow Color", Color) = (0.7,0.75,0.85,1)
        _ShadowThreshold ("Shadow Threshold", Range(0,1)) = 0.5
        _ShadowSoftness ("Shadow Softness", Range(0.001,0.3)) = 0.05

        _RimStrength ("Rim Strength", Range(0,1)) = 0.15
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
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float3 positionWS  : TEXCOORD2;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _ShadowColor;
            float _ShadowThreshold;
            float _ShadowSoftness;
            float _RimStrength;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.positionWS  = TransformObjectToWorld(v.positionOS.xyz);
                o.normalWS    = TransformObjectToWorldNormal(v.normalOS);
                o.uv          = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float3 N = normalize(i.normalWS);
                float3 V = normalize(GetWorldSpaceViewDir(i.positionWS));

                Light mainLight = GetMainLight();
                float3 L = normalize(mainLight.direction);

                float ndl = saturate(dot(N, L));

                float shadowStep = smoothstep(
                    _ShadowThreshold - _ShadowSoftness,
                    _ShadowThreshold + _ShadowSoftness,
                    ndl
                );

                half4 baseCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * _BaseColor;
                half4 toonCol = lerp(_ShadowColor * baseCol, baseCol, shadowStep);

                float rim = 1.0 - saturate(dot(N, V));
                rim = smoothstep(0.6, 0.85, rim);
                toonCol.rgb *= lerp(1.0, 1.0 - _RimStrength, rim);

                toonCol.rgb *= mainLight.color;

                return toonCol;
            }
            ENDHLSL
        }
    }
}
