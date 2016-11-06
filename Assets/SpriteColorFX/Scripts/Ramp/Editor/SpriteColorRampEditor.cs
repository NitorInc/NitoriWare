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
  /// SpriteColorRamp editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorRamp))]
  public sealed class SpriteColorRampEditor : SpriteColorBaseEditor
  {
    private SpriteColorRamp effect;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (effect == null)
        effect = this.target as SpriteColorRamp;

      effect.strength = 1.0f;

      effect.gammaCorrect = 1.2f;

      effect.uvScroll = 0.0f;

      effect.invertLum = false;

      effect.luminanceRangeMin = 0.0f;

      effect.luminanceRangeMax = 1.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (effect == null)
        effect = this.target as SpriteColorRamp;

      EditorGUIUtility.fieldWidth = 40.0f;

      effect.palette = (SpriteColorRampPalettes)EditorGUILayout.EnumPopup(@"Palette", effect.palette);

      effect.strength = SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(effect.strength * 100.0f), 0, 100, 100) * 0.01f;

      effect.gammaCorrect = SpriteColorFXEditorHelper.SliderWithReset(@"Gamma", SpriteColorFXEditorHelper.TooltipGamma, effect.gammaCorrect, 0.5f, 3.0f, 1.2f);

      effect.uvScroll = SpriteColorFXEditorHelper.SliderWithReset(@"UV Scroll", SpriteColorFXEditorHelper.TooltipUVScroll, effect.uvScroll, 0.0f, 1.0f, 0.0f);

      EditorGUIUtility.fieldWidth = 60.0f;

      SpriteColorFXEditorHelper.MinMaxSliderWithReset(@"Luminance range", SpriteColorFXEditorHelper.TooltipLuminanceRange, ref effect.luminanceRangeMin, ref effect.luminanceRangeMax, 0.0f, 1.0f, 0.0f, 1.0f);

      effect.invertLum = SpriteColorFXEditorHelper.ToogleWithReset(@"Invert luminance", SpriteColorFXEditorHelper.TooltipInvertLuminance, effect.invertLum, false);
    }
  }
}