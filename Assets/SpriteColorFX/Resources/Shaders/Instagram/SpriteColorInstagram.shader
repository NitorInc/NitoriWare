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
Shader "Sprites/Sprite Color FX/Sprite Color Instagram"
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

    CGPROGRAM
    #include "../SpriteColorFXCG.cginc"

    #pragma surface surf Unlit alpha vertex:vert nofog noambient
    #pragma fragmentoption ARB_precision_hint_fastest
    #pragma multi_compile DUMMY PIXELSNAP_ON
    #pragma target 3.0

    sampler2D _MainTex;
    fixed4 _Color;
    fixed _Strength;
    fixed _Saturation;
    fixed _Contrast;
    fixed _Gamma;
    fixed4 _Slope;
    fixed4 _Offset;
    fixed4 _Power;
    int _FilmContrast;

	  struct Input
    {
      float2 uv_MainTex;
      fixed4 color;
    };

    inline float3 Saturation(float3 pixel, float adjustment)
    {
      const float3 W = float3(0.2126, 0.7152, 0.0722);
      
      float3 intensity = dot(pixel, W);
    
      return lerp(intensity, pixel, adjustment);
    }

    inline float3 Contrast(float3 pixel, float4 con)
    {
      float3 c = con.rgb * con.a;
      float3 t = (1.0 - c) / 2.0;
      
      t = 0.5;
      pixel = (1.0 - c.rgb) * t + c.rgb * pixel;
    
      return pixel;
    }

    inline float3 Sig(float3 s) 
    {
      return 1.0 / (1.0 + (exp(-(s - 0.5) * 7.0))); 
    }

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
      float4 pixel = tex2D(_MainTex, IN.uv_MainTex);

      float3 final = pow(pixel.rgb, _Gamma);
	    final = pow(clamp(((final * _Slope) + _Offset), 0.0, 1.0), _Power);
	    final = Saturation(final, _Saturation);
	    final = Contrast(final, _Contrast);

      if (_FilmContrast == 1)
        final = Sig(final);

      final = clamp(final, 0.0, 1.0);

      o.Albedo = lerp(pixel.rgb, final, _Strength) * pixel.a;
      o.Alpha = pixel.a;
    }
    ENDCG
  }

  Fallback "Sprites/Default"
}