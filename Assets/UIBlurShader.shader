Shader "UI/BlurUI" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 30)) = 5.0
    }
    
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        GrabPass { "_GrabTexture" }
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };
            
            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }
            
            sampler2D _GrabTexture;
            float _BlurSize;
            
            half4 frag (v2f i) : SV_Target {
                float2 grabTexCoord = i.grabPos.xy / i.grabPos.w;
                
                // Simple Gaussian blur - 9 samples
                half4 sum = half4(0, 0, 0, 0);
                #define BLUR_SAMPLES 9
                
                // Get screen texel size
                float2 texelSize = _BlurSize / _ScreenParams.xy;
                
                // Define offsets for sampling
                float2 offsets[BLUR_SAMPLES] = {
                    float2(-1, -1), float2(0, -1), float2(1, -1),
                    float2(-1, 0), float2(0, 0), float2(1, 0),
                    float2(-1, 1), float2(0, 1), float2(1, 1)
                };
                
                // Gaussian kernel weights
                float weights[BLUR_SAMPLES] = {
                    0.0625, 0.125, 0.0625,
                    0.125, 0.25, 0.125,
                    0.0625, 0.125, 0.0625
                };
                
                for (int j = 0; j < BLUR_SAMPLES; j++) {
                    float2 offset = grabTexCoord + offsets[j] * texelSize;
                    sum += tex2D(_GrabTexture, offset) * weights[j];
                }
                
                return sum;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}