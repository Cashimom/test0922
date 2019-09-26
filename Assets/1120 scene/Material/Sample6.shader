Shader "Custom/Sample6" {
	Properties{
		_MainTex("Texture", 2D) = "white"{}
		_Dark("Dark",Range(0,1))=1
		_Light("Light",Range(0,1))=0.5
	}
	SubShader{
		Tags{ "Queue" = "Transparent-500" }
		LOD 200
		//Cull off

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		half _Dark;
		half _Light;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = (c.r*0.3 + c.g*0.6 + c.b*0.1 < 0.3) ? _Dark : _Light;
		}
		ENDCG
		}
	FallBack "Diffuse"
}
