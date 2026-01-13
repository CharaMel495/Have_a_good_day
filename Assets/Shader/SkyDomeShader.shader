Shader "Custom/SkyDome"
{
    Properties
    {
        // ===== Time =====
        _SkyTime ("Sky Time (0=Dawn)", Range(0,1)) = 0

        // ===== Sky Colors =====
        _SkyColorDawn ("Sky Color Dawn", Color) = (0.7,0.5,1.0,1)
        _SkyColorDay ("Sky Color Day", Color) = (0.5,0.8,1.0,1)
        _SkyColorDusk ("Sky Color Dusk", Color) = (1.0,0.6,0.3,1)
        _SkyColorNight ("Sky Color Night", Color) = (0.1,0.2,0.4,1)

        _SkyGradient ("Sky Vertical Gradient", Range(0,1)) = 0.4

        // ===== Cloud =====
        _CloudTex ("Cloud Texture (Grayscale)", 2D) = "white" {}
        _CloudIntensity ("Cloud Intensity", Range(0,1)) = 0.6
        _CloudScroll ("Cloud Scroll (XY)", Vector) = (0.002, 0.0, 0, 0)

        // ===== Glass / Crack =====
        _GlassTex ("Glass Animation Texture", 2D) = "white" {}
        _GlassCols ("Glass Columns", Int) = 4
        _GlassRows ("Glass Rows", Int) = 4
        _GlassProgress ("Glass Progress", Range(0,1)) = 0
        _GlassIntensity ("Glass Intensity", Range(0,1)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Background"
            "RenderType"="Opaque"
        }

        Cull Front
        ZWrite Off
        ZTest LEqual

        Pass
        {
            Name "Sky"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_CloudTex);
            SAMPLER(sampler_CloudTex);

            TEXTURE2D(_GlassTex);
            SAMPLER(sampler_GlassTex);

            float _SkyTime;

            float4 _SkyColorDawn;
            float4 _SkyColorDay;
            float4 _SkyColorDusk;
            float4 _SkyColorNight;
            float _SkyGradient;

            float _CloudIntensity;
            float4 _CloudScroll;

            int _GlassCols;
            int _GlassRows;
            float _GlassProgress;
            float _GlassIntensity;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            // ===== 4êFèzä¬ =====
            float3 GetSkyColor(float t)
            {
                float phase = t * 4.0;
                float localT = frac(phase);

                float3 c0, c1;

                if (phase < 1.0)
                {
                    c0 = _SkyColorDawn.rgb;
                    c1 = _SkyColorDay.rgb;
                }
                else if (phase < 2.0)
                {
                    c0 = _SkyColorDay.rgb;
                    c1 = _SkyColorDusk.rgb;
                }
                else if (phase < 3.0)
                {
                    c0 = _SkyColorDusk.rgb;
                    c1 = _SkyColorNight.rgb;
                }
                else
                {
                    c0 = _SkyColorNight.rgb;
                    c1 = _SkyColorDawn.rgb;
                }

                return lerp(c0, c1, smoothstep(0, 1, localT));
            }

            half4 frag (Varyings i) : SV_Target
            {
                // ===== Sky Base =====
                float3 skyColor = GetSkyColor(frac(_SkyTime));

                // vertical gradient
                float grad = lerp(1.0 - _SkyGradient, 1.0, saturate(i.uv.y));
                skyColor *= grad;

                // ===== Cloud Layer =====
                float2 cloudUV = i.uv + _CloudScroll.xy * _Time.y;
                float cloudMask = SAMPLE_TEXTURE2D(_CloudTex, sampler_CloudTex, cloudUV).r;

                float3 cloudColor =
                    skyColor * lerp(1.0, cloudMask, _CloudIntensity);

                float3 skyBase = lerp(skyColor, cloudColor, cloudMask);

                // ===== Glass Animation =====
                int totalFrames = _GlassCols * _GlassRows;
                float frame = floor(_GlassProgress * (totalFrames - 1));

                float col = fmod(frame, _GlassCols);
                float row = floor(frame / _GlassCols);

                float2 frameSize = float2(1.0 / _GlassCols, 1.0 / _GlassRows);

                float2 glassUV =
                    i.uv * frameSize +
                    float2(col, (_GlassRows - 1 - row)) * frameSize;

                float4 glass =
                    SAMPLE_TEXTURE2D(_GlassTex, sampler_GlassTex, glassUV);

                float3 result =
                    skyBase + glass.rgb * glass.a * _GlassIntensity;

                return float4(result, 1.0);
            }
            ENDHLSL
        }
    }
}
