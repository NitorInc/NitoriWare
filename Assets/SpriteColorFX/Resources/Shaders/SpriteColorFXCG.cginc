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

// Constants.

#define _PI			3.141592653589

// Luminance.
inline float Luminance601(float3 pixel)
{
	return dot(float3(0.299f, 0.587f, 0.114f), pixel);
}

// RGB -> HSV http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
inline float3 RGB2HSV(float3 c)
{
	const float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	const float Epsilon = 1.0e-10;

	float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
	float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

	float d = q.x - min(q.w, q.y);

	return float3(abs(q.z + (q.w - q.y) / (6.0 * d + Epsilon)), d / (q.x + Epsilon), q.x);
}

// HSV -> RGB http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
inline float3 HSV2RGB(float3 c)
{
	const float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);

	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

// 1D rand.
inline float Rand1(float value)
{
	return frac(sin(value) * 43758.5453123);
}

// 2D rand.
inline float Rand2(float2 value)
{
	return frac(sin(dot(value * 0.123, float2(12.9898, 78.233))) * 43758.5453);
}

// 3D rand.
inline float Rand3(float3 value)
{
	return frac(sin(dot(value, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
}

// Additive.
inline float3 Additive(float3 s, float3 d)
{
	return s + d;
}

// Color burn.
inline float3 Burn(float3 s, float3 d)
{
	return 1.0 - (1.0 - d) / s;
}

// Color.
float3 Color(float3 s, float3 d)
{
	s = RGB2HSV(s);
	s.z = RGB2HSV(d).z;

	return HSV2RGB(s);
}

// Darken.
inline float3 Darken(float3 s, float3 d)
{
	return min(s, d);
}

// Darker color.
inline float3 Darker(float3 s, float3 d)
{
	return (Luminance601(s) < Luminance601(d)) ? s : d;
}

// Difference.
inline float3 Difference(float3 s, float3 d)
{
	return abs(d - s);
}

// Divide.
inline float3 Divide(float3 s, float3 d)
{
	return (d > 0.0) ? s / d : s;
}

// Color dodge.
inline float3 Dodge(float3 s, float3 d)
{
	return (s < 1.0) ? d / (1.0 - s) : s;
}

// HardMix.
inline float3 HardMix(float3 s, float3 d)
{
	return floor(s + d);
}

// Hue.
float3 Hue(float3 s, float3 d)
{
	d = RGB2HSV(d);
	d.x = RGB2HSV(s).x;

	return HSV2RGB(d);
}

// HardLight.
float3 HardLight(float3 s, float3 d)
{
	return (s < 0.5) ? 2.0 * s * d : 1.0 - 2.0 * (1.0 - s) * (1.0 - d);
}

// Lighten.
inline float3 Lighten(float3 s, float3 d)
{
	return max(s, d);
}

// Lighter color.
inline float3 Lighter(float3 s, float3 d)
{
	return (Luminance601(s) > Luminance601(d)) ? s : d;
}

// Multiply.
inline float3 Multiply(float3 s, float3 d)
{
	return s * d;
}

// Overlay.
float3 Overlay(float3 s, float3 d)
{
	return (s > 0.5) ? 1.0 - 2.0 * (1.0 - s) * (1.0 - d) : 2.0 * s * d;
}

// Screen.
inline float3 Screen(float3 s, float3 d)
{
	return s + d - s * d;
}

// Solid.
inline float3 Solid(float3 s, float3 d)
{
	return d;
}

// Soft light.
float3 SoftLight(float3 s, float3 d)
{
	return (1.0 - s) * s * d + s * (1.0 - (1.0 - s) * (1.0 - d));
}

// Pin light.
float3 PinLight(float3 s, float3 d)
{
	return (2.0 * s - 1.0 > d) ? 2.0 * s - 1.0 : (s < 0.5 * d) ? 2.0 * s : d;
}

// Saturation.
float3 Saturation(float3 s, float3 d)
{
	d = RGB2HSV(d);
	d.y = RGB2HSV(s).y;

	return HSV2RGB(d);
}

// Subtract.
inline float3 Subtract(float3 s, float3 d)
{
	return s - d;
}

// VividLight.
float3 VividLight(float3 s, float3 d)
{
	return (s < 0.5) ? (s > 0.0 ? 1.0 - (1.0 - d) / (2.0 * s) : s) : (s < 1.0 ? d / (2.0 * (1.0 - s)) : s);
}

// Luminosity.
float3 Luminosity(float3 s, float3 d)
{
	float dLum = Luminance601(d);
	float sLum = Luminance601(s);

	float lum = sLum - dLum;

	float3 c = d + lum;
	float minC = min(min(c.r, c.g), c.b);
	float maxC = max(max(c.r, c.b), c.b);

	if (minC < 0.0)
		return sLum + ((c - sLum) * sLum) / (sLum - minC);
	else if (maxC > 1.0)
		return sLum + ((c - sLum) * (1.0 - sLum)) / (maxC - sLum);

	return c;
}

// Surface funtion for unlit sprites. 
fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
{
	fixed4 pixel;

	pixel.rgb = s.Albedo; 
	pixel.a = s.Alpha;

	return pixel;
}
