Shader "Custom/RingInnerScrollingText"
{
    Properties
    {
        _MainTex ("Text RT", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 0.2
        _Radius ("Ring Radius", Float) = 0.5
        _Emission ("Emission", Float) = 2.0
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }
            Cull Front
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // ===== URP / VR =====
            #pragma multi_compile_instancing
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 positionOS : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _ScrollSpeed;
            float _Radius;
            float _Emission;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionOS = IN.positionOS.xyz;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                // 中心基準（XZ平面）
                float2 p = normalize(float2(IN.positionWS.x, IN.positionWS.z));

                // 角度取得 (-PI ~ PI)
                float angle = atan2(p.y, p.x);

                // 0 ~ 1 に正規化
                float u = angle / (2.0 * PI) + 0.5;

                // スクロール
                u += _Time.y * _ScrollSpeed;
                u = 1.0 - u;

                // 高さ方向は Y を適当に
                //float v = saturate(IN.positionWS.y * 0.5 + 0.5);
                float v = saturate(IN.positionOS.y + 0.5);
                //float v = 0.5f;

                float2 uv = float2(u, v);

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

                // 電光掲示板っぽく
                col.rgb *= _Emission;

                return col;
            }
            ENDHLSL
        }
    }
}
