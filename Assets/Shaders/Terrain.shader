Shader "Corruption/Terrain" {
SubShader {
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

	struct v2f {
		float4 pos : SV_POSITION;
		float4 worldpos : COLOR;
		};
		
	v2f vert (appdata_base v){
		v2f o;
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		o.worldpos = mul (_Object2World, v.vertex);
		return o;
		}

	fixed4 frag (v2f i) : SV_Target {
		return i.worldpos.y ;
		}
	ENDCG
	}
  } 
}