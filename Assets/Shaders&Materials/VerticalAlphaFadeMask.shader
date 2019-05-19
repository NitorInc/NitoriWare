Shader "Hidden/VerticalAlphaFadeMask"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_YUpper1("Y Upper Alpha1", Float) = 2.5
		_YUpper0("Y Upper Alpha0", Float) = 4.5
		_YLower1("Y Lower Alpha1", Float) = -2.5
		_YLower0("Y Lower Alpha0", Float) = -4.5

		
         // required for UI.Mask
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
         _ColorMask ("Color Mask", Float) = 15
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
		}
		Stencil
        {
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
        }
		 ColorMask [_ColorMask]
         
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
				o.uv =  v.uv.xy;

				return o;
			}

			sampler2D _MainTex;
			float _YUpper1;
			float _YUpper0;
			float _YLower1;
			float _YLower0;

			float4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
                color *= i.uvColor;
				
				float yPos = i.wpos.y;

				if (yPos > _YUpper1)
					color.a *= 1 - ((yPos - _YUpper1) / (_YUpper0 - _YUpper1));
				else if (yPos < _YLower1)
					color.a *= (yPos - _YLower0) / (_YLower1 - _YLower0);

				if (color.a < 0)
					color.a = 0;

				return color;
			}
			ENDCG
		}
	}
}