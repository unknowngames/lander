Shader "Unknown games/Vertex color" {
Properties {
            _MainTex ("Base (RGB)", 2D) = "white" {}
            _DetailTex ("Detail (RGB)", 2D) = "white" {}
            _FresnelPower ("Fresnel power", float) = 1.0
            _FresnelColor ("Fresnel color", color) = (1,1,1,1)
            _ShadowMap ("Shadowmap", 2D) = "white" {}
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
            float3 normal : NORMAL;
            float4 color : COLOR;
            float4 uv : TEXCOORD0;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float4 color : COLOR;
            float4 uv : TEXCOORD0;
            float4 shadowPos : TEXCOORD1;
        };
        
        sampler2D _MainTex;
        float4 _MainTex_ST;
        sampler2D _DetailTex;
        float4 _DetailTex_ST;
        float _FresnelPower;
        float4 _FresnelColor;
        sampler2D _ShadowMap;
        uniform float4x4 _ProjectionMatrix;
        
        float calculateShadowFactor(float4 shadowPos)
        {
        	float4 sh = shadowPos;
        	
        	sh.xyz /= sh.w;
        	sh.x = 0.5 * sh.x + 0.5;
        	sh.y = 0.5 * sh.y + 0.5;
        	
        	float depth = sh.z;
        	
        	float4 shadow = tex2D(_ShadowMap, sh.xy);
        	
        	float shadowFactor = 1.0;
        	
        	depth = clamp(depth, 0.0, 1.0);
        	
        	if(shadow.r < depth)
            {
            	shadowFactor = 0.2;
            }
            
            return shadowFactor;
        }    
            
        v2f vert (appdata v) {
            v2f o;
            o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
            o.color = v.color;
            o.uv = v.uv;
            
            float4 wPos = mul( _Object2World, v.vertex );
            float3 direction = wPos - _WorldSpaceCameraPos;
            direction = normalize(direction);
            float3 wNormal = mul(_Object2World, float4(v.normal, 0.0));
            float fresnel = 1 - clamp(dot(-direction, wNormal), 0, 1);
            fresnel = pow(fresnel, _FresnelPower);
            o.color.a = fresnel;
            
            float4 posWorld = mul (_Object2World, float4(v.vertex.xyz,1));
            o.shadowPos = mul(_ProjectionMatrix, posWorld);
            
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target
        {
        	float shadow = calculateShadowFactor(i.shadowPos);
        	fixed4 texColor = tex2D(_MainTex, i.uv * _MainTex_ST.xy);
        	fixed4 detailColor = tex2D(_DetailTex, i.uv * _DetailTex_ST.xy); 
        	float3 diff = i.color.xyz * texColor.xyz * detailColor.xyz; 
        	return float4(lerp(diff, _FresnelColor.xyz, i.color.a * _FresnelColor.a) * shadow, 1.0);
        	
        }
        ENDCG
    }
}
}