Shader "gemmine/dissolves/dissolveTransitionShader" {
	Properties {
		_MainTex ("Main Texture Albedo (RGB)", 2D) = "white" {}
		_MainTex1 ("Main Texture 2 Albedo (RGB)", 2D) = "white" {}
		_Gradient ("Gradient Albedo (RGB)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_Blend ( "Blend", Range ( 0, 1 ) ) = 0
	}
	
	SubShader {
		Tags {
			 "RenderType"="Opaque"
            "Queue" = "Transparent"
//			"RenderType"="TransparentCutout" 
			}
		LOD 200
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		// surface shader named surf
		// Lambert lighting model
		// keepalpha to allow alpha in unity 5
		#pragma surface surf Lambert alphatest:Zero

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex1;
		sampler2D _Gradient;
		float _Blend;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex1;
			float2 uv_Gradient;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			half4 col = tex2D (_MainTex, IN.uv_MainTex);
			half4 col1 = tex2D (_MainTex1, IN.uv_MainTex1);
			half4 grad = tex2D(_Gradient, IN.uv_Gradient);
			
			half fract = grad.r - _Blend;
						
			if (fract < 0) {
				o.Albedo = col1.rgb;
			}
			else if (fract > 0.01 )
				o.Albedo = col.rgb;
			else {
				o.Albedo = col1.rgb*(1-fract) + col.rgb*fract;			
			}
			
			o.Alpha = 1;

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
