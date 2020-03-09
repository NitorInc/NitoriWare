// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Microgame/BandConductor/Squiggle"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0,1)) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
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
                float4 uvColor : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
                float4 uvColor : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.uvColor = v.uvColor;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _DisplaceTex;
			float _Magnitude;

			float4 frag(v2f i) : SV_Target
			{
				float speed = 15;
				float mod = .075;
				float offset = (_Time.x * speed - (_Time.x * speed % mod));
				float2 normaluv = float2(i.uv.x + offset, i.uv.y + offset);
				float2 normaluvcolor = tex2D(_DisplaceTex, normaluv);
				//float2 distuv2 = float2(i.uv.x - (_Time.x * 2) % 1, i.uv.y - (_Time.x * 2) % 1);

				//float2 disp = tex2D(_MainTex, i.uv + distuv).xy;
				//disp += tex2D(_DisplaceTex, distuv2).xy;
				//disp *= _Magnitude;

				float4 col = tex2D(_MainTex, i.uv + (normaluvcolor * _Magnitude));
                //col *= i.uvColor;
				//col.r += disp.x;
				//col.g += disp.y;
				return col;
			}
			ENDCG
		}
	}
}
