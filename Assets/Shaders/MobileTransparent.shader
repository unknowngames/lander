Shader "Mobile/Transparent"
{
    Properties
    {
           _MainTex ("Base (RGB)", 2D) = "white" {}
           _TintColor ("TintColor", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader 
	{ 
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Fog { Mode Off }
   
		BindChannels
		{   
			Bind "texcoord", texcoord0
			Bind "vertex", vertex
			Bind "color", color
		}
   
		Pass
		{   
			SetTexture [_MainTex] { Combine texture * primary }
			SetTexture [_MainTex]
			{       
				ConstantColor [_TintColor]
				Combine previous * constant DOUBLE, previous * constant
			}
		}
	}
}