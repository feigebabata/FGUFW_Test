Shader "Hidden/Mask"
{
    Properties
    {
        _MainColor("MainColor", Color) = (0, 0, 0, 0.95)
        [NoScaleOffset]_MainTex ("PointsTex", 2D) = "white" {}
        _PointCount("PointCount",float) = 0
        _PointCapacity("PointCapacity",float) = 0
    }
    SubShader
    {        
        Tags
        {
            "Queue"="Transparent"
        }
        // No culling or depth
        Cull Off 
        ZWrite Off 
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainColor;
            float _PointCount;
            float _PointCapacity;

            float Remap(float In, float2 InMinMax, float2 OutMinMax)
            {
                return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = _MainColor;
                float alpha = _MainColor.w;
                int idx = 0;
                while(idx<_PointCount)
                {   
                    float2 uv = float2(+idx/_PointCapacity,0.5);
                    float4 pv = tex2D(_MainTex, uv);
                    // pv = float4(0.5,0.5,0.1,1);
                    float2 p_coord = float2(pv.x,pv.y);
                    float dis = distance(p_coord,i.uv);
                    float outSide = step(pv.z,dis);
                    float inSide = 1-outSide;
                    float a = Remap(dis,float2(0,pv.z),float2(0,_MainColor.w));
                    a = a*inSide + outSide*_MainColor.w;

                    // float minAlpha = step(alpha,a);
                    // alpha = alpha - minAlpha*(1);
                    // alpha = a;
                    if(a<alpha)
                    {
                        alpha = a;
                    }

                    idx++;
                }
                color.a = alpha;
                return color;
            }
            ENDCG
        }
    }
}
