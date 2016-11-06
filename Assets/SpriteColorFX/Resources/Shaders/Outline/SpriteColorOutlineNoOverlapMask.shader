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
Shader "Sprites/Sprite Color FX/Sprite Color Outline NoOverlap Mask"
{
	// http://unity3d.com/support/documentation/Components/SL-Properties.html
	Properties
	{
		[PerRendererData]
		_MainTex("Base (RGB)", 2D) = "white" {}

		_Color("Tint", Color) = (1, 1, 1, 1)

		[MaterialToggle]
		PixelSnap("Pixel snap", Float) = 0

		_OutlineSize("Outline size", Range(0, 0.03)) = 0.0075
		_OutlineColor("Outline color", Color) = (1, 1, 1, 1)
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

		CGINCLUDE
		#include "../SpriteColorFXCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		fixed4 _Color;

		float _OutlineSize;
		fixed4 _OutlineColor;
		int _WidthMode;

		float _AlphaThreshold;
		sampler2D _MaskTex;

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
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
		ENDCG

		CGPROGRAM
		#pragma surface surf Unlit alpha vertex:vert nofog noambient
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma multi_compile DUMMY PIXELSNAP_ON
		#pragma target 3.0

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 outlinePixel = (_WidthMode == 0) ?
				tex2D(_MainTex, IN.uv_MainTex + float2(_OutlineSize, _OutlineSize)) +
				tex2D(_MainTex, IN.uv_MainTex + float2(_OutlineSize, -_OutlineSize)) +
				tex2D(_MainTex, IN.uv_MainTex + float2(-_OutlineSize, _OutlineSize)) +
				tex2D(_MainTex, IN.uv_MainTex + float2(-_OutlineSize, -_OutlineSize))
				:
				tex2D(_MainTex, IN.uv_MainTex + float2(0.0, -_MainTex_TexelSize.y) * _OutlineSize) +
				tex2D(_MainTex, IN.uv_MainTex + float2(0.0, _MainTex_TexelSize.y) * _OutlineSize) +
				tex2D(_MainTex, IN.uv_MainTex + float2(-_MainTex_TexelSize.x, 0.0) * _OutlineSize) +
				tex2D(_MainTex, IN.uv_MainTex + float2(_MainTex_TexelSize.x, 0.0) * _OutlineSize);

			if (outlinePixel.a > _AlphaThreshold)
				outlinePixel.a = 1.0;

			fixed4 mainColor = outlinePixel.a * _OutlineColor.a * _OutlineColor;

			o.Albedo = mainColor.rgb;
			o.Alpha = mainColor.a;
		}
		ENDCG

		CGPROGRAM
		#pragma surface surf Unlit alpha vertex:vert nofog noambient
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma multi_compile DUMMY PIXELSNAP_ON
		#pragma target 3.0

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 spriteColor = tex2D(_MainTex, IN.uv_MainTex) * IN.color;

			o.Albedo = spriteColor.rgb;
			o.Alpha = spriteColor.a;
		}
		ENDCG
	}

	Fallback "Sprites/Default"
}
