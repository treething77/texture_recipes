// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TextureKit/452cdc3a-d0a7-4ce5-af6a-5622d235987a-spaceship-shader"
{
	Properties
	{
		//https://docs.unity3d.com/Manual/SL-Properties.html
	    //$$STARTPROPS


scaleU5("scaleU", Float) = 0.0
scaleV5("scaleV", Float) = 0.0

texture1("texture", 2D) = "" {}

translateU4("translateU", Float) = 0.0
translateV4("translateV", Float) = 0.0

texture11("texture", 2D) = "" {}


color12("color", Color) = (0,0,0,0)
color13("color", Color) = (0,0,0,0)
color14("color", Color) = (0,0,0,0)
color15("color", Color) = (0,0,0,0)
texture7("texture", 2D) = "" {}
texture9("texture", 2D) = "" {}
texture8("texture", 2D) = "" {}
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


float scaleU5;
float scaleV5;

sampler2D texture1;


float translateU4;
float translateV4;

sampler2D texture11;

int select10;

int select6;

fixed4 color12;

fixed4 color13;

fixed4 color14;

fixed4 color15;

sampler2D texture7;

sampler2D texture9;

sampler2D texture8;

			//$$ENDINPUTS
			
			//$$STARTFUNCS
float4 getCompositeNode2(float2 uv, float w, float h, float4 input1, float4 input2)
{
   float t = input2.a;
   return (input1 * (1.0-t)) + (input2 * t);
}

float4 getCompositeNode17(float2 uv, float w, float h, float4 input1, float4 input2)
{
   float t = input2.a;
   return (input1 * (1.0-t)) + (input2 * t);
}

float4 getScaleNode5(float2 uv, float w, float h, float4 input1)
{
   return input1;
}

float4 getTextureNode1(float2 uv, float w, float h)
{
   float4 s = tex2D(texture1,uv);
   return s;
}

float4 getMultiplyNode16(float2 uv, float w, float h, float4 input1, float4 input2)
{
   return (input1 * input2);
}

float4 getTranslateNode4(float2 uv, float w, float h, float4 input1)
{
   return input1;
}

float4 getTextureNode11(float2 uv, float w, float h)
{
   float4 s = tex2D(texture11,uv);
   return s;
}

float4 getRandomizeNode10(float2 uv, float w, float h, float4 input1, float4 input2, float4 input3, float4 input4)
{
   if (select10 == 0)
      return input1;
   else if (select10 == 1)
      return input2;
   else if (select10 == 2)
      return input3;
   return input4;
}

float4 getRandomizeNode6(float2 uv, float w, float h, float4 input1, float4 input2, float4 input3, float4 input4)
{
   if (select6 == 0)
      return input1;
   else if (select6 == 1)
      return input2;
   else if (select6 == 2)
      return input3;
   return input4;
}

float4 getColorNode12(float2 uv, float w, float h)
{
   return color12;
}

float4 getColorNode13(float2 uv, float w, float h)
{
   return color13;
}

float4 getColorNode14(float2 uv, float w, float h)
{
   return color14;
}

float4 getColorNode15(float2 uv, float w, float h)
{
   return color15;
}

float4 getTextureNode7(float2 uv, float w, float h)
{
   float4 s = tex2D(texture7,uv);
   return s;
}

float4 getTextureNode9(float2 uv, float w, float h)
{
   float4 s = tex2D(texture9,uv);
   return s;
}

float4 getTextureNode8(float2 uv, float w, float h)
{
   float4 s = tex2D(texture8,uv);
   return s;
}

			//$$ENDFUNCS

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				float2 uv = i.uv;
				//$$STARTSHADER
fixed4 outTextureNode1_0= getTextureNode1( uv,0,0 );
fixed4 outTextureNode11_0= getTextureNode11( uv,0,0 );
fixed4 outColorNode12_0= getColorNode12( uv,0,0 );
fixed4 outColorNode13_0= getColorNode13( uv,0,0 );
fixed4 outColorNode14_0= getColorNode14( uv,0,0 );
fixed4 outColorNode15_0= getColorNode15( uv,0,0 );
fixed4 outRandomizeNode10_0= getRandomizeNode10( uv,0,0, outColorNode12_0, outColorNode13_0, outColorNode14_0, outColorNode15_0 );
fixed4 outMultiplyNode16_0= getMultiplyNode16( uv,0,0, outTextureNode11_0, outRandomizeNode10_0 );
fixed4 outCompositeNode17_0= getCompositeNode17( uv,0,0, outTextureNode1_0, outMultiplyNode16_0 );
float2 uv_store5 = uv;
float2 scaleFactor5 = float2(scaleU5, scaleV5);
uv -= float2(0.5, 0.5);
uv /= scaleFactor5;
uv += float2(0.5, 0.5);
float2 uv_store4 = uv;
float2 translateOffset4 = float2(translateU4, translateV4);
uv -= translateOffset4;
fixed4 outTextureNode7_0= getTextureNode7( uv,0,0 );
fixed4 outTextureNode9_0= getTextureNode9( uv,0,0 );
fixed4 outTextureNode8_0= getTextureNode8( uv,0,0 );
fixed4 outRandomizeNode6_0= getRandomizeNode6( uv,0,0, outTextureNode7_0, outTextureNode9_0, outTextureNode8_0, float4(0,0,0,0) );
fixed4 outTranslateNode4_0= getTranslateNode4( uv,0,0, outRandomizeNode6_0 );
uv = uv_store4;
fixed4 outScaleNode5_0= getScaleNode5( uv,0,0, outTranslateNode4_0 );
uv = uv_store5;
fixed4 outCompositeNode2_0= getCompositeNode2( uv,0,0, outCompositeNode17_0, outScaleNode5_0 );
col = outCompositeNode2_0;
				//$$ENDSHADER
				return col;
			}
			ENDCG
		}
	}
}

