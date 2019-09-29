// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "JaronCustom/CheckerboardShader"
{

	//Code based on : https://www.ronja-tutorials.com/2018/05/18/Chessboard.html
	//Code based on : https://forum.unity.com/threads/wireframe-grid-shader.60071/
	Properties
	{
		_GridThickness("Grid Thickness", Float) = 0.01
		_GridSpacing("Grid Spacing", Float) = 10.0
		_GridColour("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
		_BaseColour("Base Colour", Color) = (0.0, 0.0, 0.0, 0.0)
	}
		SubShader
	{
		Tags { "Queue" = "Geometry" }

		
		Pass
		{
			//ZWrite Off
			//Blend SrcAlpha OneMinusSrcAlpha
			//These made it transparent, not wanted.

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform float _GridThickness;
			uniform float _GridSpacing;
			float4 _GridColour;
			float4 _BaseColour;

			struct vInput //Normally called appdata, but for learning purposes, I have called in vInput
			{
				float4 vertex : POSITION;
			};

			struct vOutput //Normally called v2f but for learning purposes, I have called it vOutput
			{
				float4 position : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};

			vOutput vert(vInput v)
			{
				vOutput output;
				output.position = UnityObjectToClipPos(v.vertex);

				output.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return output;
			}

			fixed4 frag(vOutput i) : SV_TARGET
			{
				//frac - returns the fractional portion of a scalar or each vector component.
				//Using i.worldPos.y made the geo do weird clipping when set to {0,0,0}, setting it to Z seems to work.
				if (frac(i.worldPos.x / _GridSpacing) < _GridThickness || frac(i.worldPos.z / _GridSpacing) < _GridThickness)
				{
					return _GridColour;
				}
				else if (frac(i.worldPos.x / _GridSpacing) < _GridThickness || frac(i.worldPos.z / _GridSpacing) < _GridThickness)
				{
					return _GridColour;
				}
				else
				{
					return _BaseColour;
				}
			}



			ENDCG
		}
    }
    FallBack "Diffuse"
}



//CGPROGRAM
// Physically based Standard lighting model, and enable shadows on all light types
//#pragma surface surf Standard fullforwardshadows

// Use shader model 3.0 target, to get nicer looking lighting
//#pragma target 3.0