Shader "JaronCustom/CustomPostProcess"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Luminosity("Luminosity", Range(0,1)) = 1.0
    }
    SubShader
    {
		Tags {"Queue" = "Background" }
        LOD 200

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			#pragma target 3.0

			sampler2D _MainTex;
			fixed _Luminosity;

			fixed4 frag(v2f_img i) : COLOR
			{
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				float luminosity = 0.299 * renderTex.r + 0.587 * renderTex.g + 0.114 * renderTex.b;
				fixed4 finalColour = lerp(renderTex, luminosity, _Luminosity);

				return finalColour;
			}

			
			ENDCG
		}

        
    }
    FallBack off
}
