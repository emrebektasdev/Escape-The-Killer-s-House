Shader "Custom/UIBlur"
{
    Properties
    {
        _Radius ("Blur Radius", Range(0, 5)) = 2.0
        _Tint ("Tint Color", Color) = (1, 1, 1, 0.5)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        GrabPass { "_BackgroundTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 grabPos : TEXCOORD1;
            };

            sampler2D _BackgroundTexture;
            float4 _BackgroundTexture_TexelSize;
            float _Radius;
            float4 _Tint;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _Tint;
                o.texcoord = v.texcoord;
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = half4(0,0,0,0);
                float weight = 0;
                
                // Simple 5x5 box blur
                for(int x = -2; x <= 2; x++)
                {
                    for(int y = -2; y <= 2; y++)
                    {
                        float2 offset = float2(x, y) * _BackgroundTexture_TexelSize.xy * _Radius;
                        col += tex2Dproj(_BackgroundTexture, i.grabPos + float4(offset.x, offset.y, 0, 0));
                        weight++;
                    }
                }
                
                col /= weight;
                return half4(col.rgb * i.color.rgb, i.color.a);
            }
            ENDCG
        }
    }
    Fallback "UI/Default"
}
