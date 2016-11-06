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
Shader "Sprites/Sprite Color FX/Sprite Color Masks 3 Darker Bump Lit"
{
  // http://unity3d.com/support/documentation/Components/SL-Properties.html
  Properties
  {
    [PerRendererData]
	_MainTex("Base (RGB)", 2D) = "white" {}

	_MaskTex("Mask (RGB)", 2D) = "black" {}

	_MaskRedTex("Mask (RGB)", 2D) = "white" {}
	_MaskGreenTex("Mask (RGB)", 2D) = "white" {}
	_MaskBlueTex("Mask (RGB)", 2D) = "white" {}

    _Color("Tint", Color) = (1, 1, 1, 1)

	[MaterialToggle]
	PixelSnap("Pixel snap", Float) = 0

	[PerRendererData]
    _BumpMap("Normalmap", 2D) = "bump" {}

    _BumpIntensity("NormalMap intensity", Float) = 1

	_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)

	_Shininess("Shininess", Range(0.03, 1)) = 0.078125
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
    Lighting On
    ZWrite Off
    Fog { Mode Off }

    CGPROGRAM
    #include "../../SpriteColorFXCG.cginc"

    #pragma surface surf BlinnPhong alpha vertex:vert nofog
    #pragma fragmentoption ARB_precision_hint_fastest
    #pragma multi_compile DUMMY PIXELSNAP_ON
    #pragma target 3.0

    sampler2D _MainTex;
    sampler2D _BumpMap;
    fixed4 _Color;
    fixed _Shininess;
    fixed _BumpIntensity;
    fixed4 _BumpFactorChannels;

    float _Strength = 1.0;

    float _StrengthRed = 1.0;
    fixed4 _ColorRed;
    fixed4 _UVRedTexParams;
    float _UVRedTexAngle;

    float _StrengthGreen = 1.0;
    fixed4 _ColorGreen;
    fixed4 _UVGreenTexParams;
    float _UVGreenTexAngle;

    float _StrengthBlue = 1.0;
    fixed4 _ColorBlue;
    fixed4 _UVBlueTexParams;
    float _UVBlueTexAngle;

    sampler2D _MaskTex;
    sampler2D _MaskRedTex;
    sampler2D _MaskGreenTex;
    sampler2D _MaskBlueTex;
	
	struct Input
    {
      float2 uv_MainTex;
      float2 uv_BumpMap;
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

    inline fixed2 UVCoordOp(fixed2 uv, float2 scale, float2 velocity, float angle)
    {
      float cosAngle = cos(angle);
      float sinAngle = sin(angle);
    
      uv = mul(uv, float2x2(cosAngle, -sinAngle, sinAngle, cosAngle));
      uv *= scale;
      uv += velocity * _Time.y;
    
      return uv;
    }

    void surf(Input IN, inout SurfaceOutput o)
    {
      float4 pixel = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
      float3 mask = tex2D(_MaskTex, IN.uv_MainTex).rgb;

      float3 colorMaskR = float3(0.0, 0.0, 0.0);
      float3 colorMaskG = float3(0.0, 0.0, 0.0);
      float3 colorMaskB = float3(0.0, 0.0, 0.0);

      float3 colorMask = float3(0.0, 0.0, 0.0);

      colorMaskR = mask.r * _StrengthRed;
      colorMask += _ColorRed.rgb * colorMaskR * tex2D(_MaskRedTex, UVCoordOp(IN.uv_MainTex, _UVRedTexParams.xy, _UVRedTexParams.zw, _UVRedTexAngle)).rgb;

      colorMaskG = mask.g * _StrengthGreen;
      colorMask += _ColorGreen.rgb * colorMaskG * tex2D(_MaskGreenTex, UVCoordOp(IN.uv_MainTex, _UVGreenTexParams.xy, _UVGreenTexParams.zw, _UVGreenTexAngle)).rgb;

      colorMaskB = mask.b * _StrengthBlue;
      colorMask += _ColorBlue.rgb * colorMaskB * tex2D(_MaskBlueTex, UVCoordOp(IN.uv_MainTex, _UVBlueTexParams.xy, _UVBlueTexParams.zw, _UVBlueTexAngle)).rgb;

      float3 final = lerp(pixel.rgb, Darker(pixel.rgb, colorMask), colorMaskR + colorMaskG + colorMaskB);

	  o.Albedo = lerp(pixel.rgb, final, _Strength) * pixel.a;
      o.Gloss = pixel.a;
      o.Alpha = pixel.a * _Color.a;
      o.Specular = _Shininess;
	  o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
      o.Normal = fixed3(o.Normal.x * _BumpIntensity * _BumpFactorChannels.x, o.Normal.y * _BumpIntensity * _BumpFactorChannels.y, o.Normal.z);
    }
    ENDCG
  }

  Fallback "Sprites/Default"
}