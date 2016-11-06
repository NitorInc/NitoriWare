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
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// Sprite Color Glass editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorGlass))]
  public sealed class SpriteColorGlassEditor : SpriteColorBaseEditor
  {
    private SpriteColorGlass effect;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (effect == null)
        effect = this.target as SpriteColorGlass;

      effect.strength = 1.0f;
      effect.distortion = 1.0f;
      effect.refraction = 0.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (effect == null)
        effect = this.target as SpriteColorGlass;

      EditorGUIUtility.fieldWidth = 40.0f;

      effect.strength = SpriteColorFXEditorHelper.SliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, effect.strength * 100.0f, 0.0f, 100.0f, 100.0f) * 0.01f;

      effect.distortion = SpriteColorFXEditorHelper.SliderWithReset(@"Distortion", SpriteColorFXEditorHelper.TooltipDistortion, effect.distortion, 0.0f, 10.0f, 1.0f);

      effect.refraction = SpriteColorFXEditorHelper.SliderWithReset(@"Refraction", SpriteColorFXEditorHelper.TooltipRefraction, effect.refraction, 0.0f, 10.0f, 0.0f);
    }
  }
}