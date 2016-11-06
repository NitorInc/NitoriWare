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
Shader "Sprites/Sprite Color FX/Sprite Color Shift Linear"
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

    // Define this add noise effect.
    #define USE_NOISE
    
    float _NoiseAmount = 0.0;
    float _NoiseSpeed = 1.0;
    
    float _RedShiftX = 0.0;
    float _RedShiftY = 0.0;
    
    float _GreenShiftX = 0.0;
    float _GreenShiftY = 0.0;
    
    float _BlueShiftX = 0.0;
    float _BlueShiftY = 0.0;
	
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

    void surf(Input IN, inout SurfaceOutput o)
    {
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
      o.Alpha = final.a;
    }
    ENDCG
  }

  Fallback "Sprites/Default"
}