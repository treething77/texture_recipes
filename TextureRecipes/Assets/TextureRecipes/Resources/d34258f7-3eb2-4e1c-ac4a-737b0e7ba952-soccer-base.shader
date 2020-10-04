// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TextureKit/d34258f7-3eb2-4e1c-ac4a-737b0e7ba952-soccer-base"
{
	Properties
	{
		//https://docs.unity3d.com/Manual/SL-Properties.html
	    //$$STARTPROPS


scaleU14("scaleU", Float) = 0.0
scaleV14("scaleV", Float) = 0.0

texture0("texture", 2D) = "" {}
weightOne8("weightOne", Float) = 0.0
weightTwo8("weightTwo", Float) = 0.0

translateU13("translateU", Float) = 0.0
translateV13("translateV", Float) = 0.0



texture11("texture", 2D) = "" {}
color4("color", Color) = (0,0,0,0)


color5("color", Color) = (0,0,0,0)
texture2("texture", 2D) = "" {}
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


float scaleU14;
float scaleV14;

sampler2D texture0;

float weightOne8;
float weightTwo8;

float translateU13;
float translateV13;



sampler2D texture11;

fixed4 color4;



fixed4 color5;

sampler2D texture2;

			//$$ENDINPUTS
			
			//$$STARTFUNCS
float4 getCompositeNode12(float2 uv, float w, float h, float4 input1, float4 input2)
{
   float t = input2.a;
   return (input1 * (1.0-t)) + (input2 * t);
}

float4 getCompositeNode10(float2 uv, float w, float h, float4 input1, float4 input2)
{
   float t = input2.a;
   return (input1 * (1.0-t)) + (input2 * t);
}

float4 getScaleNode14(float2 uv, float w, float h, float4 input1)
{
   return input1;
}

float4 getTextureNode0(float2 uv, float w, float h)
{
   float4 s = tex2D(texture0,uv);
   return s;
}

float4 getAddNode8(float2 uv, float w, float h, float4 input1, float4 input2)
{
   return (input1 * weightOne8) + (input2 * weightTwo8);
}

float4 getTranslateNode13(float2 uv, float w, float h, float4 input1)
{
   return input1;
}

float4 getMultiplyNode6(float2 uv, float w, float h, float4 input1, float4 input2)
{
   return (input1 * input2);
}

float4 getMultiplyNode9(float2 uv, float w, float h, float4 input1, float4 input2)
{
   return (input1 * input2);
}

float4 getTextureNode11(float2 uv, float w, float h)
{
   float4 s = tex2D(texture11,uv);
   return s;
}

float4 getColorNode4(float2 uv, float w, float h)
{
   return color4;
}

float4 getSplitNode3_G(float2 uv, float w, float h, float4 input1)
{
   return input1.g;
}

float4 getSplitNode3_B(float2 uv, float w, float h, float4 input1)
{
   return input1.b;
}

float4 getColorNode5(float2 uv, float w, float h)
{
   return color5;
}

float4 getTextureNode2(float2 uv, float w, float h)
{
   float4 s = tex2D(texture2,uv);
   return s;
}

			//$$ENDFUNCS

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				float2 uv = i.uv;
				//$$STARTSHADER
fixed4 outTextureNode0_0= getTextureNode0( uv,0,0 );
fixed4 outColorNode4_0= getColorNode4( uv,0,0 );
fixed4 outTextureNode2_0= getTextureNode2( uv,0,0 );
fixed4 outSplitNode3_1= getSplitNode3_G( uv,0,0, outTextureNode2_0 );
fixed4 outMultiplyNode6_0= getMultiplyNode6( uv,0,0, outColorNode4_0, outSplitNode3_1 );
outTextureNode2_0= getTextureNode2( uv,0,0 );
fixed4 outSplitNode3_2= getSplitNode3_B( uv,0,0, outTextureNode2_0 );
fixed4 outColorNode5_0= getColorNode5( uv,0,0 );
fixed4 outMultiplyNode9_0= getMultiplyNode9( uv,0,0, outSplitNode3_2, outColorNode5_0 );
fixed4 outAddNode8_0= getAddNode8( uv,0,0, outMultiplyNode6_0, outMultiplyNode9_0 );
fixed4 outCompositeNode10_0= getCompositeNode10( uv,0,0, outTextureNode0_0, outAddNode8_0 );
float2 uv_store14 = uv;
float2 scaleFactor14 = float2(scaleU14, scaleV14);
uv -= float2(0.5, 0.5);
uv /= scaleFactor14;
uv += float2(0.5, 0.5);
float2 uv_store13 = uv;
float2 translateOffset13 = float2(translateU13, translateV13);
uv -= translateOffset13;
fixed4 outTextureNode11_0= getTextureNode11( uv,0,0 );
fixed4 outTranslateNode13_0= getTranslateNode13( uv,0,0, outTextureNode11_0 );
uv = uv_store13;
fixed4 outScaleNode14_0= getScaleNode14( uv,0,0, outTranslateNode13_0 );
uv = uv_store14;
fixed4 outCompositeNode12_0= getCompositeNode12( uv,0,0, outCompositeNode10_0, outScaleNode14_0 );
col = outCompositeNode12_0;
				//$$ENDSHADER
				return col;
			}
			ENDCG
		}
	}
}

