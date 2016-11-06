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
  /// Sprite Color Glass.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(SpriteRenderer))]
  [AddComponentMenu("Sprite Color FX/Color Glass")]
  public sealed class SpriteColorGlass : SpriteColorBase
  {
    /// <summary>
    /// The strength of the effect [0 - 1].
    /// </summary>
    public float strength = 1.0f;

    /// <summary>
    /// The distortion of the background [0 - 10].
    /// </summary>
    public float distortion = 1.0f;

    /// <summary>
    /// The distortion of background colors [0 - 1].
    /// </summary>
    public float refraction = 0.0f;

    /// <summary>
    /// Shader path.
    /// </summary>
    protected override string ShaderPath
    {
      get
      {
        return string.Format("Shaders/Glass/{0}SpriteColorGlass{1}",
          LightMode != SpriteColorFX.LightMode.UnLit ? LightMode.ToString() + "/" : string.Empty,
          LightMode == SpriteColorFX.LightMode.UnLit ? string.Empty : LightMode.ToString());
      }
    }

    protected override void UpdateShader()
    {
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderStrengthParam, strength);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderDistortionParam, distortion);
      spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderRefractionParam, refraction);
    }
  }
}
