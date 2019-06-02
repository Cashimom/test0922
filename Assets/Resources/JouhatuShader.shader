Shader "Custom/JouhatuShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_NoizeScale("NoizeScale", Vector) = (1,1,1,0)
		_Move("Move", Vector) = (0,1,0,0)
		_Period("Period",Float) = 1.0
		_StartTime("StartTime",Float)=0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard vertex:vert alpha:fade 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 zahyo;
        };

        half _Glossiness;
        half _Metallic;
		float4 _Move;
        fixed4 _Color;
		half _Period;
		float4 _NoizeScale;
		half _StartTime;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		fixed2 random2(fixed2 st) {
			st = fixed2(dot(st, fixed2(127.1, 311.7)),
				dot(st, fixed2(269.5, 183.3)));
			return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
		}

		float perlinNoise(fixed2 st)
		{
			fixed2 p = floor(st);
			fixed2 f = frac(st);
			fixed2 u = f * f * (3.0 - 2.0 * f);

			float v00 = random2(p + fixed2(0, 0));
			float v10 = random2(p + fixed2(1, 0));
			float v01 = random2(p + fixed2(0, 1));
			float v11 = random2(p + fixed2(1, 1));

			return lerp(lerp(dot(v00, f - fixed2(0, 0)), dot(v10, f - fixed2(1, 0)), u.x),
				lerp(dot(v01, f - fixed2(0, 1)), dot(v11, f - fixed2(1, 1)), u.x),
				u.y) + 0.5f;
		}

		float rand(float3 co)
		{
			return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 56.787))) * 43758.5453);
		}

		float3 random3(float3 f3) {
			float3 p = floor(f3);
			return normalize(float3(
				rand(p + float3(0.1, 0, 0)),
				rand(p + float3(0, 0.1, 0)),
				rand(p + float3(0, 0, 0.1))
				));
		}

		float perlin_3D(float3 f3) {
			float3 p = floor(f3);
			float3 f = frac(f3);
			float v000 = random3(f3 + float3(0, 0, 0));
			float v100 = random3(f3 + float3(1, 0, 0));
			float v010 = random3(f3 + float3(0, 1, 0));
			float v110 = random3(f3 + float3(1, 1, 0));
			float v001 = random3(f3 + float3(0, 0, 1));
			float v101 = random3(f3 + float3(1, 0, 1));
			float v011 = random3(f3 + float3(0, 1, 1));
			float v111 = random3(f3 + float3(1, 1, 1));
			return lerp(lerp(	lerp(dot(v000, f - float3(0, 0, 0)), dot(v100, f - float3(1, 0, 0)), frac(f3.x)),
								lerp(dot(v010, f - float3(0, 1, 0)), dot(v110, f - float3(1, 1, 0)), frac(f3.x)), frac(f3.y)),
						lerp(	lerp(dot(v001, f - float3(0, 0, 1)), dot(v101, f - float3(1, 0, 1)), frac(f3.x)),
								lerp(dot(v011, f - float3(0, 1, 1)), dot(v111, f - float3(1, 1, 1)), frac(f3.x)), frac(f3.y)), frac(f3.z))+0.5;
		}



		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float amp = 1 * _Time;
			v.vertex.xyz = v.vertex.xyz + (_Move*amp);
			o.zahyo = v.vertex.xyz;
			//v.normal = normalize(float3(v.normal.x+offset_, v.normal.y, v.normal.z));
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			//float col = perlin(IN.zahyo.xyz * 10);
            //o.Albedo.rgb = c.rgb* perlinNoise(IN.zahyo.xy*10);
			o.Albedo.rgb = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			o.Emission = _Color*0.3;
            //o.Alpha = max((_Period-_Time)*rand(IN.zahyo.xyz),0);
			o.Alpha = max(step(0,-((_Time.y-_StartTime) /(_Period)) + perlin_3D(IN.zahyo.xyz*_NoizeScale)),0);
			//o.Alpha = perlinNoise(IN.zahyo.xy);
			//o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
