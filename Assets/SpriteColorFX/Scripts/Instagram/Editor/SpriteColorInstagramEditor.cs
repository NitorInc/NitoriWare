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
  /// SpriteColorNeon editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorInstagram))]
  public sealed class SpriteColorInstagramEditor : SpriteColorBaseEditor
  {
    private SpriteColorInstagram effect;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (effect == null)
        effect = this.target as SpriteColorInstagram;

      effect.Filter = SpriteColorInstagram.Filters._1977;
      effect.strength = 1.0f;
      effect.contrast = 1.0f;
      effect.gamma = 1.0f;
      effect.filmContrast = false;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (effect == null)
        effect = this.target as SpriteColorInstagram;

      EditorGUIUtility.fieldWidth = 40.0f;

      effect.Filter = (SpriteColorInstagram.Filters)EditorGUILayout.EnumPopup(@"Filter", effect.Filter);

      effect.strength = SpriteColorFXEditorHelper.SliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, effect.strength * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;

      effect.contrast = SpriteColorFXEditorHelper.SliderWithReset(@"Contrast", @"Contrast [0.0 - 10.0]", effect.contrast, 0.0f, 10.0f, 1.0f);

      effect.gamma = SpriteColorFXEditorHelper.SliderWithReset(@"Gamma", @"Gamma [0.001 - 10.0]", effect.gamma, 0.001f, 10.0f, 1.0f);

      effect.filmContrast = EditorGUILayout.Toggle(@"Film contrast", effect.filmContrast);
    }
  }
}
