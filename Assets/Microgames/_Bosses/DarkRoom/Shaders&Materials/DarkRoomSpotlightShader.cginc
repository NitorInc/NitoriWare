

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
			float _CursorFadeEnd;
			float _PulseSpeed;
			float _PulseAmpInv;
			float _CursorPulseAmpInv;
			float _AlphaPow;
			float _CursorAlphaPow;
			float _LampAlphaBoost;
			float _CursorAlphaBoost;
			float _LampAnim;
			float _CursorAnim;
			float _FlipX;
			float _FinalMult;
			float _FinalCursorMult;
			float _LampRadiusBoost;
			float _IsAdditive;

			float distance(float2 a, float2 b)
			{
				return sqrt(pow(a.x - b.x, 2) + pow(a.y - b.y, 2));
			}

			float calculate(v2f i)
			{
				if (_FlipX > .5)
					i.uv.x = 1.0-i.uv.x;
				
				if (_FlipX > .5)
					i.uv.x = 1.0-i.uv.x;

				float lampDistance = distance((float2)i.wpos, (float2)_LampPos);
				float lampAlpha = (lampDistance - _FadeStart) / abs((_FadeEnd + _LampRadiusBoost) - _FadeStart);
				lampAlpha *= 1 + (sin(_Time.w * .8 * _PulseSpeed) / _PulseAmpInv);
				//lampAlpha = clamp(lampAlpha, 0, 1);
				lampAlpha = pow(lampAlpha, _AlphaPow);
				//if (lampAlpha < 1)
				lampAlpha = clamp(lampAlpha, 0, 1);
				lampAlpha -= _LampAlphaBoost;
				lampAlpha -= _LampAnim;
				lampAlpha *= _FinalMult;
				lampAlpha = clamp(lampAlpha, 0, 1);\
				if (_IsAdditive > .5)
					lampAlpha = 1.0 - (lampAlpha * .95);
				lampAlpha -= _LampAnim;

				float cursorDistance = distance((float2)i.wpos, (float2)_CursorPos);
				float cursorAlpha = (cursorDistance - _FadeStart) / abs(_CursorFadeEnd - _FadeStart);
				//cursorAlpha *= 1 + (sin(_Time.w * .7 * _PulseSpeed) / _CursorPulseAmpInv);
				cursorAlpha = clamp(cursorAlpha, 0, 1);
				cursorAlpha = pow(cursorAlpha, _CursorAlphaPow);
				cursorAlpha -= _CursorAlphaBoost;
				cursorAlpha -= _CursorAnim;
				cursorAlpha *= _FinalCursorMult;
				cursorAlpha = clamp(cursorAlpha, 0, 1);

				float alpha = 1 - ((1 - lampAlpha) + (1 - cursorAlpha));
				return clamp(alpha, 0, 1);
			}