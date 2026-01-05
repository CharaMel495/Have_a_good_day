Shader "Custom/SkyDomeShader"
{
    Properties
    {
        _MainTex ("Sky Texture", 2D) = "white" {}
        _CrackTex ("Crack Mask", 2D) = "black" {}
        _CrackProgress ("Crack Progress", Range(0,1)) = 0
        _CrackColor ("Crack Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        LOD 100

        // 内側を描画する
        Cull Front
        ZWrite Off
        ZTest LEqual

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _CrackTex;
            float4 _MainTex_ST;

            float _CrackProgress;
            float4 _CrackColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 sky = tex2D(_MainTex, i.uv);

                float crackMask = tex2D(_CrackTex, i.uv).r;

                // 進行度に応じてヒビを出す
                float crackVisible = step(1.0 - _CrackProgress, crackMask);

                fixed4 crack = _CrackColor * crackVisible;

                return sky + crack;

                // return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
