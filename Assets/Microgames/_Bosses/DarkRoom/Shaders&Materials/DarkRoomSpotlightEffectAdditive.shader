// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/DarkRoomSpotlightEffectAdditive"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LightTex("Light Texture", 2D) = "white" {}
		_LampPos("Lamp Position", Vector) = (0,0,0,0)
		_CursorPos("Cursor Position", Vector) = (0,0,0,0)
		_FadeStart("Fade Start Distance", Float) = 5
		_FadeEnd("Fade End Distance", Float) = 10
		_CursorFadeEnd("Cursor Fade End Distance", Float) = 10
		_PulseSpeed("Pulse Speed", Float) = 10
		_AlphaPow("Pulse Exponent", Float) = .5
		_CursorAlphaPow("Cursor Pulse Exponent", Float) = .5
		_PulseAmpInv("Pulse Amplitude Inverse", Float) = 15
		_CursorPulseAmpInv("Cursor Pulse Amplitude Inverse", Float) = 15
		_LampAlphaBoost("Lamp Alpha Boost", Float) = 0
		_LampRadiusBoost("Lamp Radius Boost", Float) = 0
		_CursorAlphaBoost("Cursor Alpha Boost", Float) = 0
		_FinalMult("Final Mult", Float) = 1
		_FinalCursorMult("Final Cursor Mult", Float) = 1
		
		_LampAnim("Lamp Animation Boost", Float) = 0
		_CursorAnim("Cursor Animation Boost", Float) = 0
		_FlipX("Flip X", Float) = 0
		_IsAdditive("Is Additive", Float) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType"="Transparent"
			"PreviewType" = "Plane"
		}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			#include "DarkRoomSpotlightShader.cginc"

			float4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				color.a *= calculate(i);
                color *= i.uvColor;

				return color;
			}
			ENDCG
		}
	}
}