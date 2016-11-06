///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
//
// Copyright (c) Ibuprogames <hello@ibuprogames.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// http://unity3d.com/support/documentation/Components/SL-Shader.html
Shader "Sprites/Sprite Color FX/Sprite Color Glass"
{
	// http://unity3d.com/support/documentation/Components/SL-Properties.html
	Properties
	{
		[PerRendererData]
		_MainTex("Base (RGB)", 2D) = "white" {}

		_Color("Tint", Color) = (1, 1, 1, 1)

		[MaterialToggle]
		PixelSnap("Pixel snap", Float) = 0
	}

	// Techniques (http://unity3d.com/support/documentation/Components/SL-SubShader.html).
	SubShader
	{
		// Tags (http://docs.unity3d.com/Manual/SL-CullAndDepth.html).
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }

		// GrabPass (http://docs.unity3d.com/es/current/Manual/SL-GrabPass.html)
		GrabPass
		{
		}

		CGPROGRAM
		#include "../SpriteColorFXCG.cginc"

		#pragma surface surf Unlit alpha vertex:vert nofog noambient
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma multi_compile DUMMY PIXELSNAP_ON
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _GrabTexture;
		float4 _GrabTexture_TexelSize;
		fixed4 _Color;

		fixed _Strength;
		fixed _Distortion;
		fixed _Refraction;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
			float4 screenPos;
			INTERNAL_DATA
		};

		void vert(inout appdata_full v, out Input o)
		{
#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
			v.vertex = UnityPixelSnap(v.vertex);
#endif
			v.normal = float3(0.0, 0.0, -1.0);
			v.tangent = float4(1.0, 0.0, 0.0, -1.0);

			UNITY_INITIALIZE_OUTPUT(Input, o);

			o.color = _Color * v.color;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 pixel = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			
			float4 screenPos = IN.screenPos;
			screenPos.y = 1.0 - screenPos.y;
			screenPos.xy = (pixel.rgb * _Distortion * screenPos.z) + screenPos.xy;

			float4 glass = tex2Dproj(_GrabTexture, screenPos);

			if (_Refraction > 0.0)
			{
				_Refraction *= _GrabTexture_TexelSize.x;
				glass.r = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(screenPos.x + _Refraction, screenPos.y + _Refraction, screenPos.z, screenPos.w))).r;
				glass.b = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(screenPos.x - _Refraction, screenPos.y - _Refraction, screenPos.z, screenPos.w))).r;
			}

			float4 final = lerp(pixel, glass * pixel.a, _Strength);

			o.Albedo = final.rgb * final.a;
			o.Alpha = pixel.a * _Color.a;
		}
	
		ENDCG
	}

	Fallback "Sprites/Default"
}