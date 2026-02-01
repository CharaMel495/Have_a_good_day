// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Debris" {

    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }

	SubShader {
   		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha // alpha blending
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 
 			
 			#include "UnityCG.cginc"

 			struct appdata_custom {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

 			struct v2f
 			{
 				float4 pos:SV_POSITION;
 				fixed4 color:COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
 			};
 			
 			float4x4 _PrevInvMatrix;
			float3   _TargetPosition;
			float    _Range;
			float    _RangeR;
			float    _MoveTotal;
			float    _Move;
            fixed4   _Color;

   
            v2f vert(appdata_custom v)
            {
                UNITY_SETUP_INSTANCE_ID(v); //’Ç‰Á
				v.vertex.y += _MoveTotal;
				float3 target = _TargetPosition;
				float3 trip;
				trip = floor( ((target - v.vertex.xyz)*_RangeR + 1) * 0.5 );
				trip *= (_Range * 2);
				v.vertex.xyz += trip;

            	float4 tv0 = v.vertex * v.texcoord.x;
            	tv0 = UnityObjectToClipPos (tv0);
            	
				v.vertex.y -= _Move;
            	float4 tv1 = v.vertex * v.texcoord.y;
            	tv1 = mul (UNITY_MATRIX_MV, tv1);
            	tv1 = mul (_PrevInvMatrix, tv1);
            	tv1 = mul (UNITY_MATRIX_P, tv1);
            	
            	v2f o;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //’Ç‰Á
            	o.pos = tv0 + tv1;
            	float depth = o.pos.z * 0.02;
            	float normalized_depth = (1 - depth);
            	o.color = v.color * _Color;
            	o.color.a *= (normalized_depth);

            	return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }
}
