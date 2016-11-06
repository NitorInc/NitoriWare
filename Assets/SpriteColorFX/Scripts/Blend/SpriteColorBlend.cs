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
  /// Sprite Color Blend.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
	[AddComponentMenu("Sprite Color FX/Color Blend")]
  public sealed class SpriteColorBlend : SpriteColorBase
	{
    /// <summary>
    /// Strength [0 - 1].
    /// </summary>
    public float strength = 1.0f;

    /// <summary>
    /// Use SetPixelOp().
    /// </summary>
    public SpriteColorHelper.PixelOp pixelOp = SpriteColorHelper.PixelOp.Solid;

    /// <summary>
    /// Shader path.
    /// </summary>
    protected override string ShaderPath
    {
      get
      {
        return string.Format("Shaders/Blend/{0}SpriteColorBlend{1}{2}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + "/" : string.Empty,
          pixelOp.ToString(),
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
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

    protected override void UpdateShader()
    {
			spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthParam, strength);
		}
	}
}
