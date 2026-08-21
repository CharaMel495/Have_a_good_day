Shader "Custom/MyFog"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (0.8, 0.85, 0.9, 1)
        _FogDensity ("Fog Density", Range(0, 1)) = 0.35
        _FogStart ("Fog Start", Float) = 1.0
        _FogEnd ("Fog End", Float) = 8.0
        _FogFadeOutStart ("Fog Fade Out Start", Float) = 10.0
        _FogFadeOutEnd ("Fog Fade Out End", Float) = 18.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "Near Fog"

            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            TEXTURE2D_X(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            float4 _FogColor;
            float _FogDensity;
            float _FogStart;
            float _FogEnd;
            float _FogFadeOutStart;
            float _FogFadeOutEnd;

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;

                half4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

                float rawDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;
                float sceneDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

                float fogIn = saturate((sceneDepth - _FogStart) / max(0.0001, _FogEnd - _FogStart));
                float fogOut = 1.0 - saturate((sceneDepth - _FogFadeOutStart) / max(0.0001, _FogFadeOutEnd - _FogFadeOutStart));

                float fog = fogIn * fogOut * _FogDensity;

                col.rgb = lerp(col.rgb, _FogColor.rgb, fog);
                return col;
            }

            ENDHLSL
        }
    }
}