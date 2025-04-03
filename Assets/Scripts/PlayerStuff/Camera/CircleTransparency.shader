Shader "Custom/CircleTransparency"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _FadeCenter ("Fade Center", Vector) = (0,0,0,0)
        _FadeRadius ("Fade Radius", Float) = 2.0
        _FadeSmoothness ("Fade Smoothness", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _Color;
            float3 _FadeCenter;
            float _FadeRadius;
            float _FadeSmoothness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
               fixed4 col = tex2D(_MainTex, i.uv); // Keep original texture
                float dist = distance(i.worldPos, _FadeCenter);
                float alpha = smoothstep(_FadeRadius, _FadeRadius + _FadeSmoothness, dist);
                col.a *= alpha; // Apply transparency without removing texture
    return col;

    Pass
{
    Blend SrcAlpha OneMinusSrcAlpha
    ZWrite Off
    ColorMask RGB
    SetTexture [_MainTex] { combine primary }
}
            }
            ENDCG
        }
    }
}
