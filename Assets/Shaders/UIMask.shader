Shader "UI/UIMask"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
			
		_Center ("Center", vector) = (0.0,0.0,0.0,0.0) 
		_Radius ("Radius", float) = 0.5
		_Hardness("Hardness", float) = 1.0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
                float2 screenPos : TEXCOORD1;
			};
			
			fixed4 _Color;
	
			float2 _Center;
			float _Radius, _Hardness;
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				
				OUT.screenPos.xy = 0.5*(OUT.vertex.xy+1.0) * _ScreenParams.xy;

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
				
				float x = length(_Center.xy - IN.screenPos.xy);
 
				float circleTest = saturate(x / _Radius);

				color.a *= (pow(circleTest, _Hardness));

				return color;
			}
		ENDCG
		}
	}
}
