// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/DarkRoomSpotlightEffectLight"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LightTex("Light Texture", 2D) = "white" {}
		_LampPos("Lamp Position", Vector) = (0,0,0,0)
		_CursorPos("Cursor Position", Vector) = (0,0,0,0)
		_FadeStart("Fade Start Distance", Float) = 5
		_FadeEnd("Fade End Distance", Float) = 10
		_PulseSpeed("Pulse Speed", Float) = 10
		_AlphaPow("Pulse Exponent", Float) = .5
		_CursorAlphaPow("Cursor Pulse Exponent", Float) = .5
		_PulseAmpInv("Pulse Amplitude Inverse", Float) = 15
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 wpos : TEXCOORD1;
				float3 vpos : TEXCOORD2;
                float4 uvColor : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 wpos : TEXCOORD1;
				float3 vpos : TEXCOORD2;
                float4 uvColor : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.uvColor = v.uvColor;
				
				// World position calculations
				float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
				o.wpos = worldPos;
				o.vpos = v.vertex.xyz;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _LighTex;
			float4 _LampPos;
			float4 _CursorPos;
			float _FadeStart;
			float _FadeEnd;
			float _PulseSpeed;
			float _PulseAmpInv;
			float _AlphaPow;
			float _CursorAlphaPow;

			float distance(float2 a, float2 b)
			{
				return sqrt(pow(a.x - b.x, 2) + pow(a.y - b.y, 2));
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
                color *= i.uvColor;

				color.a *= 1 - (sin(_Time.w * _PulseSpeed) / _PulseAmpInv);

				//float lampDistance = distance((float2)i.wpos, (float2)_LampPos);
				//float lampAlpha = (lampDistance - _FadeStart) / abs(_FadeEnd - _FadeStart);
				//lampAlpha = clamp(lampAlpha, 0, 1);
				//lampAlpha = pow(lampAlpha, _AlphaPow);

				//float alpha = 1 - ((1 - lampAlpha) + (1 - cursorAlpha));
				//color.a *= clamp(alpha, 0, 1);

				return color;
			}
			ENDCG
		}
	}
}