// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Transparent/Diffuse ZWrite" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
    }
    SubShader{
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Stencil {
                Comp Always
                Pass Keep
                ZFail Zero
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc" // for _LightColor0

            struct v2f
            {
                fixed4 diff : COLOR0; 
                float4 vertex : SV_POSITION;
            };
            

            v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
            {
                // return UnityObjectToClipPos(vertex);

                v2f o;

                o.vertex = UnityObjectToClipPos(vertex);

                half3 worldNormal = UnityObjectToWorldNormal(normal);
                
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

                o.diff = nl * _LightColor0;

                return o;
                

            }
            
            fixed4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _Color;

                col.xyz *= i.diff.xyz;

                return col;
            }

            ENDCG
        }
        
        // paste in forward rendering passes from Transparent/Diffuse
        // UsePass "Transparent/Diffuse/FORWARD"
    }
}
