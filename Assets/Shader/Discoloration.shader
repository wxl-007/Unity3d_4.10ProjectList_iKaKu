Shader "Effect/Discoloration" {
	Properties {
		_Alpha ("Alpha", range(0, 1)) = 0.5
		_MainTex ("Base (RGB)", 2D) = "gray" {}			//纹理
	}
	Category{
		SubShader {
			Tags {"Queue"="Transparent" "RenderType"="Transparent"}	//渲染
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha //设置混合因子
			LOD 200
			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;
			uniform float _Alpha;
			struct Input {
				float2 uv_MainTex;
			};

			void surf (Input IN, inout SurfaceOutput o) {		//去色(使换面变黑白)
				half4 c = tex2D (_MainTex, IN.uv_MainTex);
				if(c.a != 0){
					o.Albedo = c.r * 0.299 + c.g * 0.587 + c.b * 0.114;
					o.Alpha = _Alpha;
				}else{
					o.Albedo = c.rgb;
					o.Alpha = c.a;
				}
			}
			ENDCG
		} 
	}
	FallBack "Diffuse"
}
