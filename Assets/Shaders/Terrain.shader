Shader "Corruption/Terrain" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		
		_Mask ("Mask", 2D) = "white" {}
		
		_Pattern1_Color ("Pattern 1 Color", Color) = (1,1,1,1)
		_Pattern1 ("Pattern 1", 2D) = "black" {}
		_Pattern2_Color ("Pattern 2 Color", Color) = (1,1,1,1)
		_Pattern2 ("Pattern 2", 2D) = "black" {}
		_Pattern3_Color ("Pattern 3 Color", Color) = (1,1,1,1)
		_Pattern3 ("Pattern 3", 2D) = "black" {}
		
		_PatternsLevels ("Patterns Levels", Range(0.5, 10)) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert nolightmap 
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Mask;
		sampler2D _Pattern1;
		float4 _Pattern1_Color;
		sampler2D _Pattern2;
		float4 _Pattern2_Color;
		sampler2D _Pattern3;
		float4 _Pattern3_Color;
		float _PatternsLevels;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Pattern1;
			float2 uv_Pattern2;
			float2 uv_Pattern3;
			float2 uv2_Mask;
			float4 screenPos;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 p1 = tex2D (_Pattern1, IN.uv_Pattern1) * _Pattern1_Color;
			fixed4 p2 = tex2D (_Pattern2, IN.uv_Pattern2) * _Pattern2_Color;
			fixed4 p3 = tex2D (_Pattern3, IN.uv_Pattern3) * _Pattern3_Color;
			
			fixed4 m = tex2D (_Mask, IN.uv2_Mask);
			fixed4 m1 =  clamp(pow(m * 8 * _PatternsLevels, 12), 0, 1);
			fixed4 m2 =  clamp(pow(m * 4 * _PatternsLevels, 12), 0, 1);
			fixed4 m3 =  clamp(pow(m * 2 * _PatternsLevels, 12), 0, 1);
			
			fixed4 diff = lerp(c, p1, m1);
			diff = lerp(diff, p2, m2);
			diff = lerp(diff, p3, m3);
			
			o.Albedo = diff;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
