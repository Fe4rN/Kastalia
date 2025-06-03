Shader "Custom/FloatingGlow"
{
    Properties
    {
        _Color    ("Glow Color", Color)                   = (1,1,0,1)
        _Emission ("Emission Intensity", Range(0,5))      = 2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;    
            half   _Emission;  

            struct appdata { float4 vertex : POSITION; };
            struct v2f      { float4 pos : SV_POSITION; fixed4 col : COLOR0; };

            v2f vert (appdata v)
            {
                
                float3 p = v.vertex.xyz;

                v2f o;
                o.pos = UnityObjectToClipPos(float4(p, 1));
                o.col = _Color * _Emission;   
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.col;  //Devuelve el color luminoso 
            }
            ENDCG
        }
    }
    FallBack "Unlit/Color"
}
