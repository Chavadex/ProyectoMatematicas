Shader "Custom/GradientShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _ColorLow ("Color Low", Color) = (0, 1, 0, 1) // Color para pendiente baja (verde)
        _ColorHigh ("Color High", Color) = (1, 0, 0, 1) // Color para pendiente alta (rojo)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 worldPos : TEXCOORD0; // Posición en el espacio mundial
                float3 normal : TEXCOORD1;   // Normal
            };

            // Parámetros del material
            uniform float4 _ColorLow;
            uniform float4 _ColorHigh;

            // Función para calcular el gradiente de f(x, y)
            float3 CalculateGradient(float x, float y)
            {
                // Derivadas parciales de f(x, y) = sin(x) * cos(y)
                float df_dx = cos(x) * cos(y);
                float df_dy = -sin(x) * sin(y);

                return float3(df_dx, 0, df_dy); // Gradiente en el espacio (dx, dy)
            }

            // Función de vértice
            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Convertir la posición a espacio mundial
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Calcular el gradiente en el espacio mundial
                float3 gradient = CalculateGradient(o.worldPos.x, o.worldPos.z);
                o.normal = gradient; // Usamos el gradiente para modificar la normal

                return o;
            }

            // Función de fragmento
            half4 frag(v2f i) : SV_Target
            {
                // Calcular la magnitud del gradiente
                float gradientMagnitude = length(i.normal);

                // Interpolar entre los colores según la magnitud del gradiente
                float t = saturate(gradientMagnitude); // Asegura que esté entre 0 y 1
                return lerp(_ColorLow, _ColorHigh, t); // Color entre verde (baja pendiente) y rojo (alta pendiente)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}