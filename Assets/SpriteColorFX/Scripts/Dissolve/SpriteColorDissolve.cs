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
  /// Shader used for the effect.
  /// </summary>
  public enum DissolveShaderType
  {
    /// <summary>
    /// Normal dissolve effect.
    /// </summary>
    Normal,

    /// <summary>
    /// Dissolve effect with border color.
    /// </summary>
    BorderColor,

    /// <summary>
    /// Dissolve effect with border texture.
    /// </summary>
    BorderTexture,
  }

  /// <summary>
  /// Dissolve texture type. If you want to use your own, set 'Custom' and change 'borderTexture'.
  /// </summary>
  public enum DissolveTextureType
  {
    Burn,
    Explosion,
    Grow,
    Horizontal,
    Organic,
    Pixel,
    Plasma,
    Sphere,
    Vertical,
    Radial,
    Radial5,
    RaysCenter,
    RaysCorner,
    Spiral,
    SpiralFast1,
    SpiralFast2,
    SpiralFract,
    Squares,
    Waves,
    WavesVertical,

    Custom = 99,
  }

  /// <summary>
  /// Color dissolve.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
  [AddComponentMenu("Sprite Color FX/Color Dissolve")]
  public sealed class SpriteColorDissolve : SpriteColorBase
  {
    /// <summary>
    /// Use SetShaderType().
    /// </summary>
    public DissolveShaderType shaderType = DissolveShaderType.Normal;

    /// <summary>
    /// Use SetPixelOp().
    /// </summary>
    public SpriteColorHelper.PixelOp pixelOp = SpriteColorHelper.PixelOp.Solid;

    /// <summary>
    /// Use SetTextureType().
    /// </summary>
    public DissolveTextureType dissolveTextureType = DissolveTextureType.Burn;

    /// <summary>
    /// Dissolve texture.
    /// </summary>
    public Texture dissolveTexture;

    /// <summary>
    /// Border texture. Change this if you want to use a custom texture.
    /// </summary>
    public Texture borderTexture;

    /// <summary>
    /// Dissolve amount [0 - 1].
    /// </summary>
    public float dissolveAmount = 0.0f;

    /// <summary>
    /// Invert the effect.
    /// </summary>
    public bool dissolveInverse = false;

    /// <summary>
    /// Dissolve line witdh [0 - 0.2].
    /// </summary>
    public float dissolveBorderWitdh = 0.1f;

    /// <summary>
    /// Dissolve line color.
    /// </summary>
    public Color dissolveBorderColor = Color.grey;

    /// <summary>
    /// Dissolve noise amount [0 - 1].
    /// </summary>
    public float dissolveNoiseAmount = 0.25f;

    /// <summary>
    /// Dissolve UV scale [0.1 - 5].
    /// </summary>
    public float dissolveUVScale = 1.0f;

    /// <summary>
    /// Border UV scale [0.1 - 5].
    /// </summary>
    public float borderUVScale = 1.0f;

    /// <summary>
    /// Shader path.
    /// </summary>
    protected override string ShaderPath
    {
      get
      {
        return string.Format("Shaders/Dissolve/{0}SpriteColorDissolve{1}{2}{3}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + @"/" : string.Empty,
          shaderType.ToString(),
          shaderType.ToString() != @"Normal" ? pixelOp.ToString() : string.Empty,
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
    }

    /// <summary>
    /// Set the shader type.
    /// </summary>
    public void SetShaderType(DissolveShaderType shaderType)
    {
      this.shaderType = shaderType;

      SetPixelOp(pixelOp);
    }

    /// <summary>
    /// Set the pixel color operation.
    /// </summary>
    public void SetPixelOp(SpriteColorHelper.PixelOp pixelOp)
    {
      this.pixelOp = pixelOp;

      if (this.spriteRenderer != null)
        CreateMaterial();
    }

    /// <summary>
    /// Set the dissolve texture type.
    /// </summary>
    public void SetTextureType(DissolveTextureType textureType)
    {
      dissolveTextureType = textureType;

      if (dissolveTextureType != DissolveTextureType.Custom)
      {
        string texturePath = string.Format("Textures/Dissolve/{0}", dissolveTextureType.ToString());

        Texture texture = Resources.Load<Texture>(texturePath);
        if (texture != null)
          dissolveTexture = texture;
        else
        {
          Debug.LogWarning(string.Format("Failed to load '{0}', SpriteColorDissolve disabled.", texturePath));

          this.enabled = false;
        }
      }
    }

    /// <summary>
    /// Initialize the effect.
    /// </summary>
    protected override void Initialize()
    {
      SetTextureType(dissolveTextureType);
    }

    protected override void UpdateShader()
    {
      spriteRenderer.sharedMaterial.SetTexture(@"_DissolveTex", dissolveTexture);

      if (shaderType == DissolveShaderType.BorderTexture)
      {
        spriteRenderer.sharedMaterial.SetTexture(@"_BorderTex", borderTexture);
        spriteRenderer.sharedMaterial.SetFloat(@"_BorderUVScale", borderUVScale);
      }

      spriteRenderer.sharedMaterial.SetFloat(@"_DissolveAmount", 1.0f - dissolveAmount);

      if (shaderType != DissolveShaderType.Normal)
        spriteRenderer.sharedMaterial.SetFloat(@"_DissolveLineWitdh", dissolveBorderWitdh);

      if (shaderType == DissolveShaderType.BorderColor)
        spriteRenderer.sharedMaterial.SetColor(@"_DissolveLineColor", dissolveBorderColor);

      spriteRenderer.sharedMaterial.SetFloat(@"_DissolveUVScale", dissolveUVScale);
      spriteRenderer.sharedMaterial.SetFloat(@"_DissolveInverseOne", dissolveInverse == true ? 0.0f : 1.0f);
      spriteRenderer.sharedMaterial.SetFloat(@"_DissolveInverseTwo", dissolveInverse == true ? -1.0f : 1.0f);
    }
  }
}
