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
  /// Sprite Color Shift Linear editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorShiftLinear))]
  public sealed class SpriteColorShiftLinearEditor : SpriteColorBaseEditor
  {
    private SpriteColorShiftLinear effect;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (effect == null)
        effect = this.target as SpriteColorShiftLinear;

      effect.redShift = Vector2.zero;

      effect.greenShift = Vector2.zero;

      effect.blueShift = Vector2.zero;

      effect.noiseAmount = 0.0f;

      effect.noiseSpeed = 1.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (effect == null)
        effect = this.target as SpriteColorShiftLinear;

      EditorGUIUtility.fieldWidth = 40.0f;

      effect.redShift = SpriteColorFXEditorHelper.Vector2WithReset(@"Red offset", SpriteColorFXEditorHelper.TooltipRedOffset, effect.redShift, Vector2.zero);

      effect.greenShift = SpriteColorFXEditorHelper.Vector2WithReset(@"Green offset", SpriteColorFXEditorHelper.TooltipGreenOffset, effect.greenShift, Vector2.zero);

      effect.blueShift = SpriteColorFXEditorHelper.Vector2WithReset(@"Blue offset", SpriteColorFXEditorHelper.TooltipBlueOffset, effect.blueShift, Vector2.zero);

      effect.noiseAmount = SpriteColorFXEditorHelper.SliderWithReset(@"Noise amount", SpriteColorFXEditorHelper.TooltipNoiseAmount, effect.noiseAmount * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;

      effect.noiseSpeed = SpriteColorFXEditorHelper.SliderWithReset(@"Noise speed", SpriteColorFXEditorHelper.TooltipNoiseSpeed, effect.noiseSpeed * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;
    }
  }
}