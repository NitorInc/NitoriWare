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
  /// Sprite Color Mask 3.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
	[AddComponentMenu("Sprite Color FX/Color Masks 3")]
  public sealed class SpriteColorMasks3 : SpriteColorBase
	{
    /// <summary>
    /// Strength [0 - 1].
    /// </summary>
    public float strength = 1.0f;

    /// <summary>
    /// Texture mask (RGB).
    /// </summary>
    public Texture2D textureMask;

    /// <summary>
    /// Use SetPixelOp().
    /// </summary>
    public SpriteColorHelper.PixelOp pixelOp = SpriteColorHelper.PixelOp.Multiply;

#region #1 Mask (red channel).
    /// <summary>
    /// Mask strength [0 - 1].
    /// </summary>
    public float strengthMaskRed = 1.0f;

    /// <summary>
    /// Mask color.
    /// </summary>
    public Color colorMaskRed = Color.white;

    /// <summary>
    /// Mask texture.
    /// </summary>
    public Texture2D textureMaskRed;

    /// <summary>
    /// Mask texture UV coordinate params (U scale, V scale, U velocity, V velocity).
    /// </summary>
    public Vector4 textureMaskRedUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);

    /// <summary>
    /// Mask texture angle [0 - 360].
    /// </summary>
    public float textureMaskRedUVAngle;
#endregion

#region #2 Mask (green channel).
    /// <summary>
    /// Mask strength [0 - 1].
    /// </summary>
    public float strengthMaskGreen = 1.0f;

    /// <summary>
    /// Mask color.
    /// </summary>
    public Color colorMaskGreen = Color.white;

    /// <summary>
    /// Mask texture.
    /// </summary>
    public Texture2D textureMaskGreen;

    /// <summary>
    /// Mask texture UV coordinate params (U scale, V scale, U velocity, V velocity).
    /// </summary>
    public Vector4 textureMaskGreenUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);

    /// <summary>
    /// Mask texture angle [0 - 360].
    /// </summary>
    public float textureMaskGreenUVAngle;
#endregion

#region #3 Mask (blue channel).
    /// <summary>
    /// Mask strength [0 - 1].
    /// </summary>
    public float strengthMaskBlue = 1.0f;

    /// <summary>
    /// Mask color.
    /// </summary>
    public Color colorMaskBlue = Color.white;

    /// <summary>
    /// Mask texture.
    /// </summary>
    public Texture2D textureMaskBlue;

    /// <summary>
    /// Mask texture UV coordinate params (U scale, V scale, U velocity, V velocity).
    /// </summary>
    public Vector4 textureMaskBlueUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);

    /// <summary>
    /// Mask texture angle [0 - 360].
    /// </summary>
    public float textureMaskBlueUVAngle;
#endregion

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
    /// Shader path.
    /// </summary>
    protected override string ShaderPath
    {
      get
      {
        return string.Format("Shaders/Masks/{0}SpriteColorMasks3{1}{2}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + "/" : string.Empty,
          pixelOp.ToString(),
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
    }

    protected override void UpdateShader()
    {
			spriteRenderer.sharedMaterial.SetTexture(SpriteColorHelper.ShaderMaskTex, textureMask);
			spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthParam, strength);

      // #1 mask red.
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthRedParam, strengthMaskRed);
      spriteRenderer.sharedMaterial.SetColor(SpriteColorHelper.ShaderColorRedParam, colorMaskRed);
      spriteRenderer.sharedMaterial.SetTexture(SpriteColorHelper.ShaderMaskRedParam, textureMaskRed);
      spriteRenderer.sharedMaterial.SetVector(SpriteColorHelper.ShaderUVRedParam, textureMaskRedUVParams);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderUVAngleRedParam, textureMaskRedUVAngle * Mathf.Deg2Rad);

      // #2 mask green.
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthGreenParam, strengthMaskGreen);
      spriteRenderer.sharedMaterial.SetColor(SpriteColorHelper.ShaderColorGreenParam, colorMaskGreen);
      spriteRenderer.sharedMaterial.SetTexture(SpriteColorHelper.ShaderMaskGreenParam, textureMaskGreen);
      spriteRenderer.sharedMaterial.SetVector(SpriteColorHelper.ShaderUVGreenParam, textureMaskGreenUVParams);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderUVAngleGreenParam, textureMaskGreenUVAngle * Mathf.Deg2Rad);

      // #3 mask blue.
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthBlueParam, strengthMaskBlue);
      spriteRenderer.sharedMaterial.SetColor(SpriteColorHelper.ShaderColorBlueParam, colorMaskBlue);
      spriteRenderer.sharedMaterial.SetTexture(SpriteColorHelper.ShaderMaskBlueParam, textureMaskBlue);
      spriteRenderer.sharedMaterial.SetVector(SpriteColorHelper.ShaderUVBlueParam, textureMaskBlueUVParams);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderUVAngleBlueParam, textureMaskBlueUVAngle * Mathf.Deg2Rad);
		}
	}
}
