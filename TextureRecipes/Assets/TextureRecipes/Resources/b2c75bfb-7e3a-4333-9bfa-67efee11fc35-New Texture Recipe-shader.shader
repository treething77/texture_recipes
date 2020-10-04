// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TextureKit/b2c75bfb-7e3a-4333-9bfa-67efee11fc35-New Texture Recipe-shader"
{
	Properties
	{
		//https://docs.unity3d.com/Manual/SL-Properties.html
	    //$$STARTPROPS




scaleU26("scaleU", Float) = 0.0
scaleV26("scaleV", Float) = 0.0

noiseScale27("noise scale", Float) = 1.0
color30("color", Color) = (0,0,0,0)

scaleU24("scaleU", Float) = 0.0
scaleV24("scaleV", Float) = 0.0

translateU25("translateU", Float) = 0.0
translateV25("translateV", Float) = 0.0


scaleU5("scaleU", Float) = 0.0
scaleV5("scaleV", Float) = 0.0

translateU23("translateU", Float) = 0.0
translateV23("translateV", Float) = 0.0

texture22("texture", 2D) = "" {}
texture1("texture", 2D) = "" {}

translateU4("translateU", Float) = 0.0
translateV4("translateV", Float) = 0.0

texture21("texture", 2D) = "" {}
texture18("texture", 2D) = "" {}
color12("color", Color) = (0,0,0,0)
texture7("texture", 2D) = "" {}
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




float scaleU26;
float scaleV26;

float noiseScale27;

fixed4 color30;


float scaleU24;
float scaleV24;

float translateU25;
float translateV25;


float scaleU5;
float scaleV5;

float translateU23;
float translateV23;

sampler2D texture22;

sampler2D texture1;


float translateU4;
float translateV4;

sampler2D texture21;

sampler2D texture18;

fixed4 color12;

sampler2D texture7;

			//$$ENDINPUTS
			
			//$$STARTFUNCS
float4 getCompositeNode28(float2 uv, float w, float h, float4 input1, float4 input2)
{
   float t = input2.a;
   return (input1 * (1.0-t)) + (input2 * t);
}

float4 getCompositeNode20(float2 uv, float w, float h, float4 input1, float4 input2)
{
   float t = input2.a;
   return (input1 * (1.0-t)) + (input2 * t);
}

float4 getMultiplyNode31(float2 uv, float w, float h, float4 input1, float4 input2)
{
   return (input1 * input2);
}

float4 getCompositeNode19(float2 uv, float w, float h, float4 input1, float4 input2)
{
   float t = input2.a;
   return (input1 * (1.0-t)) + (input2 * t);
}

float4 getScaleNode26(float2 uv, float w, float h, float4 input1)
{
   return input1;
}

float4 getNoiseNode27(float2 uv, float w, float h)
{
   float s = snoise(uv * noiseScale27);
   return float4(s,s,s,s);
}

float4 getColorNode30(float2 uv, float w, float h)
{
   return color30;
}

float4 getCompositeNode2(float2 uv, float w, float h, float4 input1, float4 input2)
{
   float t = input2.a;
   return (input1 * (1.0-t)) + (input2 * t);
}

float4 getScaleNode24(float2 uv, float w, float h, float4 input1)
{
   return input1;
}

float4 getTranslateNode25(float2 uv, float w, float h, float4 input1)
{
   return input1;
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

float4 getTranslateNode23(float2 uv, float w, float h, float4 input1)
{
   return input1;
}

float4 getTextureNode22(float2 uv, float w, float h)
{
   float4 s = tex2D(texture22,uv);
   return s;
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

float4 getTextureNode21(float2 uv, float w, float h)
{
   float4 s = tex2D(texture21,uv);
   return s;
}

float4 getTextureNode18(float2 uv, float w, float h)
{
   float4 s = tex2D(texture18,uv);
   return s;
}

float4 getColorNode12(float2 uv, float w, float h)
{
   return color12;
}

float4 getTextureNode7(float2 uv, float w, float h)
{
   float4 s = tex2D(texture7,uv);
   return s;
}

			//$$ENDFUNCS

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				float2 uv = i.uv;
				//$$STARTSHADER
fixed4 outTextureNode1_0= getTextureNode1( uv,0,0 );
fixed4 outTextureNode18_0= getTextureNode18( uv,0,0 );
fixed4 outColorNode12_0= getColorNode12( uv,0,0 );
fixed4 outMultiplyNode16_0= getMultiplyNode16( uv,0,0, outTextureNode18_0, outColorNode12_0 );
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
fixed4 outTranslateNode4_0= getTranslateNode4( uv,0,0, outTextureNode7_0 );
uv = uv_store4;
fixed4 outScaleNode5_0= getScaleNode5( uv,0,0, outTranslateNode4_0 );
uv = uv_store5;
fixed4 outCompositeNode2_0= getCompositeNode2( uv,0,0, outCompositeNode17_0, outScaleNode5_0 );
float2 uv_store24 = uv;
float2 scaleFactor24 = float2(scaleU24, scaleV24);
uv -= float2(0.5, 0.5);
uv /= scaleFactor24;
uv += float2(0.5, 0.5);
float2 uv_store23 = uv;
float2 translateOffset23 = float2(translateU23, translateV23);
uv -= translateOffset23;
fixed4 outTextureNode21_0= getTextureNode21( uv,0,0 );
fixed4 outTranslateNode23_0= getTranslateNode23( uv,0,0, outTextureNode21_0 );
uv = uv_store23;
fixed4 outScaleNode24_0= getScaleNode24( uv,0,0, outTranslateNode23_0 );
uv = uv_store24;
fixed4 outCompositeNode19_0= getCompositeNode19( uv,0,0, outCompositeNode2_0, outScaleNode24_0 );
float2 uv_store26 = uv;
float2 scaleFactor26 = float2(scaleU26, scaleV26);
uv -= float2(0.5, 0.5);
uv /= scaleFactor26;
uv += float2(0.5, 0.5);
float2 uv_store25 = uv;
float2 translateOffset25 = float2(translateU25, translateV25);
uv -= translateOffset25;
fixed4 outTextureNode22_0= getTextureNode22( uv,0,0 );
fixed4 outTranslateNode25_0= getTranslateNode25( uv,0,0, outTextureNode22_0 );
uv = uv_store25;
fixed4 outScaleNode26_0= getScaleNode26( uv,0,0, outTranslateNode25_0 );
uv = uv_store26;
fixed4 outCompositeNode20_0= getCompositeNode20( uv,0,0, outCompositeNode19_0, outScaleNode26_0 );
fixed4 outNoiseNode27_0= getNoiseNode27( uv,0,0 );
fixed4 outColorNode30_0= getColorNode30( uv,0,0 );
fixed4 outMultiplyNode31_0= getMultiplyNode31( uv,0,0, outNoiseNode27_0, outColorNode30_0 );
fixed4 outCompositeNode28_0= getCompositeNode28( uv,0,0, outCompositeNode20_0, outMultiplyNode31_0 );
col = outCompositeNode28_0;
				//$$ENDSHADER
				return col;
			}
			ENDCG
		}
	}
}

