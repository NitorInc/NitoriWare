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
Shader "Sprites/Sprite Color FX/Sprite Color Shift Linear Bump Lit"
{
  // http://unity3d.com/support/documentation/Components/SL-Properties.html
  Properties
  {
    [PerRendererData]
	  _MainTex("Base (RGB)", 2D) = "white" {}
    
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

    // Define this add noise effect.
    #define USE_NOISE
    
    float _NoiseAmount;
    float _NoiseSpeed;
    
    float _RedShiftX;
    float _RedShiftY;
    
    float _GreenShiftX;
    float _GreenShiftY;
    
    float _BlueShiftX;
    float _BlueShiftY;
	
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

    void surf(Input IN, inout SurfaceOutput o)
    {
      float4 pixel = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
      
      float2 uvR = float2(-_RedShiftX, _RedShiftY);
      float2 uvG = float2(-_GreenShiftX, _GreenShiftY);
      float2 uvB = float2(-_BlueShiftX, _BlueShiftY);

#ifdef USE_NOISE
      float randSin = 1.0 - (Rand1(_SinTime.x * _NoiseSpeed) * 2.0);
      float randCos = 1.0 - (Rand1(_CosTime.x * _NoiseSpeed) * 2.0);

      uvR += float2(randSin, randCos) * _NoiseAmount;
      uvB += float2(randCos, randSin) * _NoiseAmount;
#endif
      float2 red = tex2D(_MainTex, IN.uv_MainTex + uvR).ra;
      float2 green = tex2D(_MainTex, IN.uv_MainTex + uvG).ga;
      float2 blue = tex2D(_MainTex, IN.uv_MainTex + uvB).ba;

      float4 final = float4(red.x * red.y * IN.color.x, green.x * green.y * IN.color.y, blue.x * blue.y * IN.color.z, (red.y + green.y + blue.y) * 0.333);

      o.Albedo = final.rgb * final.a;
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