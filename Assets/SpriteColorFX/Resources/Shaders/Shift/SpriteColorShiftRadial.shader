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
Shader "Sprites/Sprite Color FX/Sprite Color Shift Radial"
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

    float _Strength = 0.0;
    float _NoiseAmount = 0.0;
    float _NoiseSpeed = 0.0;
	
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
      float shift = _Strength;
#ifdef USE_NOISE
      shift += _NoiseAmount * Rand1(_SinTime.w * _NoiseSpeed);
#endif
      shift *= distance(IN.uv_MainTex, 0.5);

      float2 colorVec = normalize(IN.uv_MainTex - 0.5);

      float2 uvR = float2(IN.uv_MainTex.x - (colorVec.x * shift), IN.uv_MainTex.y - (colorVec.y * shift));
      float2 uvG = float2(IN.uv_MainTex.x, IN.uv_MainTex.y);
      float2 uvB = float2(IN.uv_MainTex.x + (colorVec.x * shift), IN.uv_MainTex.y + (colorVec.y * shift));

      float2 red = tex2D(_MainTex, uvR).ra * IN.color.r;
      float2 green = tex2D(_MainTex, uvG).ga * IN.color.g;
      float2 blue = tex2D(_MainTex, uvB).ba * IN.color.b;
 
      float4 final = float4(red.x * red.y, green.x * green.y, blue.x * blue.y, (red.y + green.y + blue.y) * 0.333);

	  o.Albedo = final.rgb * final.a;
      o.Alpha = final.a;
    }
    ENDCG
  }

  Fallback "Sprites/Default"
}