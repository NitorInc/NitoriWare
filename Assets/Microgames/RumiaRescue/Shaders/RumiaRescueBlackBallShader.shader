Shader "Unlit/CircleAdd" {
	Properties {
		_MainTex("Texture", Color) = (1,1,1,1)
		_MainTex2("Texture2", Color) = (1,0,0,1)

		_Radius1("_Radius1",Range(0, 1)) = 0.01

		_Pox1("_Pox1",Range(0, 1)) = 0.01

		_Poy1("_Poy1",Range(0, 1)) = 0.01
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			half4 _MainTex, _MainTex2;
			float _Pox1;
			float _Poy1;
			float _Radius1;

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}


			float DrawCircle(float x,float y,float r, v2f i) {
				float x_r = i.uv.x - x;
				float y_r = i.uv.y - y;
				float dis = (x_r) * (x_r)+(y_r) * (y_r);
				if (dis <= r) {
					//return 1;
					if (r - dis < 0.002) {
						return (r - dis) / 0.002;
					} else {
						return 1;
					}
				} else {
					return 0;
				}
			}

			fixed4 frag(v2f i) : SV_Target {
				float c1 = DrawCircle(_Pox1,_Poy1,_Radius1,i);
				if (c1 == 1) {
					return _MainTex2;
				}
				return fixed4(1, 1, 1, 0);
			}
			ENDCG
		}
	}
}