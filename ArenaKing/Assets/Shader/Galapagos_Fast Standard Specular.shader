Shader "Galapagos/Fast Standard Specular" {
	Properties {
		_FresnelPower ("Fresnel Power", Float) = 3
		_MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {}
		_NormalMap ("Bumpmap", 2D) = "bump" {}
		_SpecGlossMap ("Specular", 2D) = "white" {}
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "VertexLit"
	//CustomEditor "FastStandardSpecularEditor"
}