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
using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// Sprite Color Instagram.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
  [AddComponentMenu("Sprite Color FX/Instagram")]
  public sealed class SpriteColorInstagram : SpriteColorBase
  {
    /// <summary>
    /// Instagram filters.
    /// </summary>
    public enum Filters
    {
      _1977,
      Apollo,
      Brannan,
      EarlyBird,
      Gotham,
      Hefe,
      Inkwell,
      Lily,
      LomoFi,
      LordKevin,
      Nashville,
      Poprocket,
      Sutro,
      Walden,
      XProII,
    }

    /// <summary>
    /// Instagram filter.
    /// </summary>
    public Filters Filter
    {
      get { return filter; }
      set { UpdateFilter(value); }
    }

    /// <summary>
    /// Filter strength [0.0 - 1.0].
    /// </summary>
    public float strength = 1.0f;

    /// <summary>
    /// Contrast [0.0 - 10.0].
    /// </summary>
    public float contrast = 1.0f;

    /// <summary>
    /// Gamma [0.001 - 10.0].
    /// </summary>
    public float gamma = 1.0f;

    /// <summary>
    /// Apply film contrast [true - false].
    /// </summary>
    public bool filmContrast = false;

    // Current filter.
    private Filters filter = Filters._1977;

    // -5.0 - 5.0
    private float saturation = 1.0f;

    private Vector3 slope = Vector3.one;
    private Vector3 offset = Vector3.zero;
    private Vector3 power = Vector3.one;

    /// <summary>
    /// Shader path.
    /// </summary>
    protected override string ShaderPath
    {
      get
      {
        return string.Format("Shaders/Instagram/{0}SpriteColorInstagram{1}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + "/" : string.Empty,
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
    }

    protected override void UpdateShader()
    {
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthParam, strength);
      spriteRenderer.sharedMaterial.SetInt(SpriteColorHelper.ShaderFilmContrastParam, filmContrast == true ? 1 : 0);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderSaturationParam, saturation);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderContrastParam, contrast);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderGammaParam, gamma);
      spriteRenderer.sharedMaterial.SetVector(SpriteColorHelper.ShaderSlopeParam, slope);
      spriteRenderer.sharedMaterial.SetVector(SpriteColorHelper.ShaderOffsetParam, offset);
      spriteRenderer.sharedMaterial.SetVector(SpriteColorHelper.ShaderPowerParam, power);
    }

    private void UpdateFilter(Filters newFilter)
    {
      if (newFilter != filter)
      {
        filter = newFilter;
        switch (filter)
        {
          case Filters._1977:
            slope = new Vector3(1.002344f, 1.002344f, 1.002344f);
            offset = new Vector3(0.177295f, 0.102685f, 0.124902f);
            power = new Vector3(1.271409f, 1.412865f, 1.441414f);
            saturation = 1.0f;
            break;

          case Filters.Apollo:
            slope = new Vector3(1.027020f, 1.019360f, 0.997714f);
            offset = new Vector3(-0.012891f, 0.139844f, 0.139844f);
            power = new Vector3(1.186510f, 1.859080f, 1.896160f);
            saturation = 0.600441f;
            break;

          case Filters.Brannan:
            slope = new Vector3(0.842500f, 0.956808f, 0.805664f);
            offset = new Vector3(-0.035071f, -0.052115f, 0.172788f);
            power = new Vector3(0.985901f, 1.188610f, 2.283580f);
            saturation = 0.607811f;
            break;

          case Filters.EarlyBird:
            slope = new Vector3(1.050910f, 0.806908f, 0.758130f);
            offset = new Vector3(-0.052488f, -0.022408f, -0.043116f);
            power = new Vector3(1.294280f, 1.117030f, 1.186460f);
            saturation = 1.019480f;
            break;

          case Filters.Gotham:
            slope = new Vector3(1.002633f, 1.283138f, -1.022549f);
            offset = new Vector3(-0.894868f, -0.192727f, 1.629342f);
            power = new Vector3(0.694179f, 1.469600f, 0.0f);
            saturation = 0.026953f;
            break;

          case Filters.Hefe:
            slope = new Vector3(1.086620f, 1.086620f, 1.086620f);
            offset = new Vector3(0.123243f, -0.034278f, 0.024562f);
            power = new Vector3(1.501950f, 1.312590f, 2.0f);
            saturation = 0.776757f;
            break;

          case Filters.Inkwell:
            slope = new Vector3(-1.956830f, 1.401900f, 3.810730f);
            offset = new Vector3(0.013761f, -0.143172f, -21.281799f);
            power = new Vector3(1.497810f, 1.277210f, 1.985840f);
            saturation = 0.0f;
            break;

          case Filters.Lily:
            slope = new Vector3(0.723593f, 0.715874f, 0.6668720f);
            offset = new Vector3(0.094940f, 0.141620f, 0.097916f);
            power = new Vector3(0.910115f, 1.177830f, 1.714090f);
            saturation = 0.754297f;
            break;

          case Filters.LomoFi:
            slope = new Vector3(1.435720f, 1.250320f, 1.186830f);
            offset = new Vector3(-0.279101f, -0.226710f, -0.081201f);
            power = new Vector3(1.178660f, 0.889618f, 1.169710f);
            saturation = 1.0f;
            break;

          case Filters.LordKevin:
            slope = new Vector3(0.649603f, 0.769153f, 0.752337f);
            offset = new Vector3(0.364704f, 0.213678f, 0.031597f);
            power = new Vector3(1.096905f, 1.688936f, 1.286792f);
            saturation = 0.743945f;
            break;

          case Filters.Nashville:
            slope = new Vector3(1.24203f, 0.87111f, 0.52847f);
            offset = new Vector3(0.01127f, 0.21499f, 0.48637f);
            power = new Vector3(1.58391f, 1.92828f, 2.36919f);
            saturation = 0.56308f;
            break;

          case Filters.Poprocket:
            slope = new Vector3(0.511196f, 0.754472f, 1.069880f);
            offset = new Vector3(0.349511f, -0.022850f, 0.128856f);
            power = new Vector3(0.995112f, 1.195600f, 1.462390f);
            saturation = 0.510202f;
            break;

          case Filters.Sutro:
            slope = new Vector3(1.310010f, 1.216570f, 1.286590f);
            offset = new Vector3(-0.068845f, -0.278657f, -0.001999f);
            power = new Vector3(0.507390f, 0.963987f, 1.188950f);
            saturation = 0.328368f;
            break;

          case Filters.Walden:
            slope = new Vector3(1.077660f, 1.197070f, 1.15260f);
            offset = new Vector3(-0.087942f, -0.051414f, 0.026514f);
            power = new Vector3(1.264750f, 1.251250f, 1.247840f);
            saturation = 0.547076f;
            break;

          case Filters.XProII:
            slope = new Vector3(1.047730f, 1.109550f, 1.226080f);
            offset = new Vector3(-0.122850f, -0.088573f, -0.113134f);
            power = new Vector3(1.214190f, 1.361770f, 0.992396f);
            saturation = 1.0f;
            break;
        }
      }
    }
  }
}
