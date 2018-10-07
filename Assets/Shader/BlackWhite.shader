Shader "Effect/BlackWhite" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)	//颜色
		_MainTex ("Base (RGB)", 2D) = "gray" {}			//纹理
	}
	Category{
		SubShader {
			Material{				//材质属性
				Diffuse[_Color]		//漫反射
				Ambient[_Color]		//镜面反射
				Emission[_Color]	//荧光
			}
			Tags {"Queue"="Transparent" "RenderType"="Transparent"}	//渲染
			Blend SrcAlpha OneMinusSrcAlpha //透明通道
			LOD 200
			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;
			struct Input {
				float2 uv_MainTex;
			};

			void surf (Input IN, inout SurfaceOutput o) {		//去色(使换面变黑白)
				half4 c = tex2D (_MainTex, IN.uv_MainTex);
				o.Albedo = c.r * 0.299f + c.g * 0.587f + c.b * 0.114f;
				o.Alpha = c.a;
			}
			ENDCG
			
			Pass
			{
				ColorMaterial AmbientAndDiffuse
				Cull Off
				SetTexture [_MainTex] {
					Combine texture * primary, texture * primary
				}

				SetTexture [_MainTex] {
					constantColor [_Color]
					Combine previous * constant DOUBLE, previous * constant
				}  
			}
		} 
	}
	FallBack "Diffuse"
}
