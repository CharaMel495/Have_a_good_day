Shader "Custom/PhongSpecular"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)

        _SpecColor ("Specular Color", Color) = (1,1,1,1)
        _Shininess ("Shininess (Power)", Range(1,256)) = 32
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

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _SpecColor;
                float  _Shininess;
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

                // ビュー方向（カメラ→ピクセルの逆：ピクセル→カメラ）
                float3 camPosWS = GetCameraPositionWS();
                half3 V = normalize((half3)(camPosWS - i.positionWS));

                Light mainLight = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
                half3 L = normalize(mainLight.direction);

                // --- Diffuse (Lambert)
                half ndotl = saturate(dot(N, L));
                half3 diffuse = albedo * mainLight.color * ndotl;

                // --- Specular (Phong)
                // 反射ベクトル R = reflect(-L, N)
                half3 R = reflect(-L, N);
                half rdotv = saturate(dot(R, V));
                half spec = pow(rdotv, (half)_Shininess);

                half3 specular = (half3)_SpecColor.rgb * mainLight.color * spec;

                // 影（拡散・鏡面どっちに掛けるかは好み。ここは両方に掛ける）
                half shadow = mainLight.shadowAttenuation;
                half3 color = (diffuse + specular) * shadow;

                return half4(color, _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
