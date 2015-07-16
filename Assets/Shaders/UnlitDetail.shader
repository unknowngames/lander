Shader "Unknown games/Unlit Detail" {
Properties {
            _MainTex ("Base (RGB)", 2D) = "white" {}
            _DetailTex ("Detail (RGB)", 2D) = "white" {}
            _Brightness ("Brightness", float) = 1.0
        }
SubShader {
    Pass {
        Fog { Mode Off }
        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag

        // vertex input: position, color
        struct appdata {
            float4 vertex : POSITION;
            float4 uv : TEXCOORD0;
            float4 uv2 : TEXCOORD1;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float4 uv : TEXCOORD0;
            float4 uv2 : TEXCOORD1;
        };
        
        sampler2D _MainTex;
        float4 _MainTex_ST;
        sampler2D _DetailTex;
        float4 _DetailTex_ST;
        float _Brightness;
        
        v2f vert (appdata v) {
            v2f o;
            o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
            o.uv = v.uv;
            o.uv2 = v.uv2;
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target
        {
        	fixed4 texColor = tex2D(_MainTex, i.uv * _MainTex_ST);
        	fixed4 detailColor = tex2D(_DetailTex, i.uv2 * _DetailTex_ST);
        	return texColor * detailColor * _Brightness; 
        }
        ENDCG
    }
}
}