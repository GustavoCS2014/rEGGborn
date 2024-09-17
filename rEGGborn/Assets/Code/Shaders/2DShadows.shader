Shader "Reggborn/2DShadows"
{
    Properties
    {
        [PerRendererData][HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _Blur("Blur Amount", float) = 0
        _ShadowDistance("Shadow Distance", int) = 0
        _Opacity ("Opacity", float) = .25
        _Tint ("Shadow Tint", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back 
        LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct V2F
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _Tint;
            float _Blur;
            float _Opacity;
            int _ShadowDistance;
            
            V2F vert (MeshData v)
            {
                V2F o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (V2F fragment) : SV_Target
            {
                fixed4 col = fixed4(0,0,0,0);
                
                if(_Blur <= 0){
                    col = fixed4(0,0,0,tex2D(_MainTex,fragment.uv).a);
                }

                if(_Blur > 0) 
                {
                    float kernelSum = 0.0;

                    int upper = ((_Blur - 1) / 2);
                    int lower = -upper;

                    for (int x = lower; x <= upper; ++x)
                    {
                        for (int y = lower; y <= upper; ++y)
                        {
                            kernelSum ++;

                            float2 offset = float2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
                            col += tex2D(_MainTex, fragment.uv + offset);
                        }
                    }

                    col /= kernelSum;
                }
                

                return fixed4(_Tint.rgb,(col.a * _Opacity));
            }

            ENDCG
        }
    }
}
