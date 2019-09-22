Shader "Unlit/warpCircle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Brightness("Brightness",Range(0,1))=1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Brightness;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float Heart(float2 st)
			{
				// 位置とか形の調整
				st = (st - float2(0.5, 0.38)) * float2(2.1, 2.8);
				return (pow(st.x, 2) + pow(st.y - sqrt(abs(st.x)), 2));
			}
			float Heart_Inf(float2 uv) {
				float d = Heart(uv);
				return step(d, abs(sin(d * 8 - _Time.w * 2)));
			}

			
			fixed4 frag (v2f i) : SV_Target
			{
				//// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//return col;

				//float d = distance(float2(0.5, 0.5), i.uv);
				//d = d * 30;
				//d = abs(sin(d));
				//d = step(0.5, d);
				//float a = 0.4;  // 閾値
				//return step(a, d);

				//return Heart_Inf(i.uv);
				float d = distance(float2(0.5, 0.5), i.uv);
				d -= _Time.y/2;
				d = abs( sin(d * 5 * 3.14159265359));
				float4 color= _Color;
				color += _Brightness*abs(sin(_Time.y / 1)) - d;
				return color;

			}
			ENDCG
		}
	}
}
