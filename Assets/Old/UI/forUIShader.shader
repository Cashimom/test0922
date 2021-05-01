Shader "Custom/forUIShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Add ("Add", Range(-1,1)) = 0.5
        _Mul ("Mul", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half _Add;
        half _Mul;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

fixed2 random2(fixed2 st)
{
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

float fBm(fixed2 st)
{
    float f = 0;
    fixed2 q = st;

    f += 0.5000 * perlinNoise(q);
    q = q * 2.01;
    f += 0.2500 * perlinNoise(q);
    q = q * 2.02;
    f += 0.1250 * perlinNoise(q);
    q = q * 2.03;
    f += 0.0625 * perlinNoise(q);
    q = q * 2.01;

    return f;
}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
    float add = fBm(IN.uv_MainTex * 8) * _Add;
    float mul = fBm(IN.uv_MainTex * 8) * _Mul;
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb*mul+add;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
