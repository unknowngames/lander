Shader "Unknown games/Vertex color" {
Properties {
            _MainTex ("Base (RGB)", 2D) = "white" {}
            _DetailTex ("Detail (RGB)", 2D) = "white" {}
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
            float4 color : COLOR;
            float4 uv : TEXCOORD0;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float4 color : COLOR;
            float4 uv : TEXCOORD0;
        };
        
        sampler2D _MainTex;
        float4 _MainTex_ST;
        sampler2D _DetailTex;
        float4 _DetailTex_ST;
        
        v2f vert (appdata v) {
            v2f o;
            o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
            o.color = v.color;
            o.uv = v.uv;
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target
        {
        	fixed4 texColor = tex2D(_MainTex, i.uv * _MainTex_ST.xy);
        	fixed4 detailColor = tex2D(_DetailTex, i.uv * _DetailTex_ST.xy); 
        	return i.color * texColor * detailColor; 
        }
        ENDCG
    }
}
}