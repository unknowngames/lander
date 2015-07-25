Shader "Unknown games/Shadowmap Receiver" {
	Properties 
	{
    	_MainTex ("Base (RGB)", 2D) = "white" {}
        _ShadowMap ("Shadowmap", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertOut {
                float4 pos:SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 shadowPos : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            sampler2D _ShadowMap;
            uniform float4x4 _ProjectionMatrix;

            vertOut vert(appdata_base v) {
                vertOut o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord;
                //float4x4 texBias = float4x4(1.0,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1);
                //float4x4 texBias = float4x4(0.5,0,0,0, 0,0.5,0,0, 0,0,0.5,0, 0.5,0.5,0.5,1);
                float4 posWorld = mul (_Object2World, float4(v.vertex.xyz,1));
                //posWorld.z += 25;
                //o.shadowPos = mul(texBias * _ProjectionMatrix, float4(v.vertex.xyz, 1));
                o.shadowPos = mul(_ProjectionMatrix, posWorld);
                return o;
            }
            
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

            fixed4 frag(vertOut i) : SV_Target 
            {
            	float4 sh = i.shadowPos;
            	
            	sh.xyz /= sh.w;
            	sh.x = 0.5 * sh.x + 0.5;
            	sh.y = 0.5 * sh.y + 0.5;
            	
            	float depth = sh.z;
            	
            	float4 color = tex2D(_MainTex, i.uv.xy);   
            	float4 shadow = tex2D(_ShadowMap, sh.xy);
            	
            	//return shadow;
            	
            	float visibility = 1.0;
            	
            	depth = clamp(depth, 0.0, 1.0);
            	
            	if(shadow.r < depth)
	            {
	            	visibility = 0.2;
	            }
	            
                return color * visibility;
            }

            ENDCG
        }
    }
}