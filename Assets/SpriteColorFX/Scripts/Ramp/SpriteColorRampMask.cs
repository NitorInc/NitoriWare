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
  /// Sprite Color Ramp with maks.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
  [AddComponentMenu("Sprite Color FX/Color Ramp Mask")]
  public sealed class SpriteColorRampMask : SpriteColorBase
  {
    /// <summary>
    /// Effect strength.
    /// </summary>
    public float strength = 1.0f;

    /// <summary>
    /// Gamma.
    /// </summary>
    public float gammaCorrect = 1.2f;

    /// <summary>
    /// UV scroll.
    /// </summary>
    public float uvScroll = 0.0f;

    /// <summary>
    /// Invert luminance.
    /// </summary>
    public bool invertLum = false;

    /// <summary>
    /// Min luminance range (0.0f).
    /// </summary>
    public float luminanceRangeMin = 0.0f;

    /// <summary>
    /// Max luminance range (0.0f).
    /// </summary>
    public float luminanceRangeMax = 1.0f;

    /// <summary>
    /// Palettes.
    /// </summary>
    [SerializeField]
    public SpriteColorRampPalettes[] palettes;

    /// <summary>
    /// Texture mask (RGBA).
    /// </summary>
    public Texture2D textureMask;

    private float textureHeightInv = 1.0f;

    /// <summary>
    /// Shader path.
    /// </summary>
    protected override string ShaderPath
    {
      get
      {
        return string.Format("Shaders/Ramp/{0}SpriteColorRampMask{1}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + "/" : string.Empty,
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
    }

    /// <summary>
    /// Initialize the effect.
    /// </summary>
    protected override void Initialize()
    {
      palettes = new SpriteColorRampPalettes[3];

      Texture2D rampsTex = Resources.Load<Texture2D>(@"Textures/SpriteColorRamps");
      if (rampsTex != null)
      {
        rampsTex.wrapMode = TextureWrapMode.Repeat;
        rampsTex.filterMode = FilterMode.Point;

        textureHeightInv = 1.0f / rampsTex.height;

        spriteRenderer.sharedMaterial.SetTexture(@"_RampsTex", rampsTex);
      }
      else
      {
        Debug.LogWarning("Failed to load 'Textures/SpriteColorRamps', disabled.");

        this.enabled = false;
      }
    }

    protected override void UpdateShader()
    {
      spriteRenderer.sharedMaterial.SetTexture(SpriteColorHelper.ShaderMaskTex, textureMask);

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthParam, strength);

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderGammaCorrectParam, gammaCorrect);

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderUVScrollParam, Mathf.Clamp(uvScroll, 0.0f, 1.0f));

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderInvertLumParam, invertLum ? -1.0f : 1.0f);

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderLumRangeMinParam, luminanceRangeMin);

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderLumRangeMaxParam, luminanceRangeMax);

      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderRampRedIdx, ((float)palettes[0] + 0.5f) * textureHeightInv);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderRampGreenIdx, ((float)palettes[1] + 0.5f) * textureHeightInv);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderRampBlueIdx, ((float)palettes[2] + 0.5f) * textureHeightInv);
    }
  }
}
