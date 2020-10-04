// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TextureKit/a68a4151-b08b-4d55-b32d-90d16f644de1-tutorial-Shader Layer"
{
	Properties
	{
		//https://docs.unity3d.com/Manual/SL-Properties.html
	    //$$STARTPROPS
	    //$$ENDPROPS
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			//$$STARTINCLUDES
			#include "UnityCG.cginc"
			#include "Assets/TextureRecipes/Shaders/noise2D.glsl.txt"
			#include "Assets/TextureRecipes/Shaders/noise3D.glsl.txt"
			#include "Assets/TextureRecipes/Shaders/shaderHelpers.glsl.txt"
			//$$ENDINCLUDES


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
			
			//$$STARTINPUTS
			//$$ENDINPUTS
			
			//$$STARTFUNCS
			//$$ENDFUNCS

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				float2 uv = i.uv;
				//$$STARTSHADER

				//$$ENDSHADER
				return col;
			}
			ENDCG
		}
	}
}

