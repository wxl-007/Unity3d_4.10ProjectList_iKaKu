Shader "Effect/BlackWhite" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)	//��ɫ
		_MainTex ("Base (RGB)", 2D) = "gray" {}			//����
	}
	Category{
		SubShader {
			Material{				//��������
				Diffuse[_Color]		//������
				Ambient[_Color]		//���淴��
				Emission[_Color]	//ӫ��
			}
			Tags {"Queue"="Transparent" "RenderType"="Transparent"}	//��Ⱦ
			Blend SrcAlpha OneMinusSrcAlpha //͸��ͨ��
			LOD 200
			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;
			struct Input {
				float2 uv_MainTex;
			};

			void surf (Input IN, inout SurfaceOutput o) {		//ȥɫ(ʹ�����ڰ�)
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
