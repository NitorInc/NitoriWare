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
using System;

using UnityEditor;
using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorMasks3 editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorMasks3))]
  public sealed class SpriteColorMasks3Editor : SpriteColorBaseEditor
	{
		private SpriteColorMasks3 effect;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (effect == null)
        effect = this.target as SpriteColorMasks3;

      effect.strength = 1.0f;
      effect.pixelOp = SpriteColorHelper.PixelOp.Multiply;

      effect.strengthMaskRed = 1.0f;
      effect.colorMaskRed = Color.white;
      effect.textureMaskRedUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
      effect.textureMaskRedUVAngle = 0.0f;

      effect.strengthMaskGreen = 1.0f;
      effect.colorMaskGreen = Color.white;
      effect.textureMaskGreenUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
      effect.textureMaskGreenUVAngle = 0.0f;

      effect.strengthMaskBlue = 1.0f;
      effect.colorMaskBlue = Color.white;
      effect.textureMaskBlueUVParams = new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
      effect.textureMaskBlueUVAngle = 0.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
			if (effect == null)
				effect = base.target as SpriteColorMasks3;

			EditorGUIUtility.fieldWidth = 40.0f;
			
      effect.strength = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(effect.strength * 100.0f), 0, 100, 100) * 0.01f;

      SpriteColorHelper.PixelOp newPixelOp = (SpriteColorHelper.PixelOp)EditorGUILayout.EnumPopup(new GUIContent(@"Blend mode", @"Blend modes"), effect.pixelOp);
      if (newPixelOp != effect.pixelOp)
        effect.SetPixelOp(newPixelOp);

      EditorGUILayout.LabelField(@"#1 mask (red)");
			{
				EditorGUI.indentLevel++;

				effect.strengthMaskRed = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(effect.strengthMaskRed * 100f), 0, 100, 100) * 0.01f;
				  
        effect.colorMaskRed = EditorGUILayout.ColorField(@"Color", effect.colorMaskRed);
				  
        effect.textureMaskRed = (EditorGUILayout.ObjectField(new GUIContent(@"Texture (RGB)", SpriteColorFXEditorHelper.TooltipTextureMask), effect.textureMaskRed, typeof(Texture2D), false, GUILayout.Height(54.0f)) as Texture2D);
				if (effect.textureMaskRed != null)
        {
          EditorGUILayout.LabelField(@"UV params");
            
          UVParamsInspectorGUI(ref effect.textureMaskRedUVParams, ref effect.textureMaskRedUVAngle);
        }

				EditorGUI.indentLevel--;
			}

      EditorGUILayout.LabelField(@"#2 mask (green)");
			{
				EditorGUI.indentLevel++;
				  
        effect.strengthMaskGreen = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(effect.strengthMaskGreen * 100f), 0, 100, 100) * 0.01f;
				  
        effect.colorMaskGreen = EditorGUILayout.ColorField(@"Color", effect.colorMaskGreen);

        effect.textureMaskGreen = (EditorGUILayout.ObjectField(new GUIContent(@"Texture (RGB)", SpriteColorFXEditorHelper.TooltipTextureMask), effect.textureMaskGreen, typeof(Texture2D), false, GUILayout.Height(54.0f)) as Texture2D);
        if (effect.textureMaskGreen != null)
        {
          EditorGUILayout.LabelField(@"UV params");

          UVParamsInspectorGUI(ref effect.textureMaskGreenUVParams, ref effect.textureMaskGreenUVAngle);
        }

				EditorGUI.indentLevel--;
			}

      EditorGUILayout.LabelField(@"#3 mask (blue)");
			{
				EditorGUI.indentLevel++;

				effect.strengthMaskBlue = (float)SpriteColorFXEditorHelper.IntSliderWithReset(@"Strength", SpriteColorFXEditorHelper.TooltipStrength, Mathf.RoundToInt(effect.strengthMaskBlue * 100f), 0, 100, 100) * 0.01f;
          
        effect.colorMaskBlue = EditorGUILayout.ColorField(@"Color", effect.colorMaskBlue);

        effect.textureMaskBlue = (EditorGUILayout.ObjectField(new GUIContent(@"Texture (RGB)", SpriteColorFXEditorHelper.TooltipTextureMask), effect.textureMaskBlue, typeof(Texture2D), false, GUILayout.Height(54.0f)) as Texture2D);
				if (effect.textureMaskBlue != null)
        {
          EditorGUILayout.LabelField(@"UV params");
            
          UVParamsInspectorGUI(ref effect.textureMaskBlueUVParams, ref effect.textureMaskBlueUVAngle);
        }

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Separator();

      effect.textureMask = (EditorGUILayout.ObjectField(new GUIContent(@"Mask #1 (RGB)", SpriteColorFXEditorHelper.TooltipTextureMask), this.effect.textureMask, typeof(Texture2D), false, GUILayout.Height(54.0f)) as Texture2D);
		}

		private void UVParamsInspectorGUI(ref Vector4 uvParams, ref float angle)
		{
      EditorGUI.indentLevel++;

      uvParams.x = SpriteColorFXEditorHelper.SliderWithReset(@"U coord scale", @"U texture coordinate scale", uvParams.x, -5f, 5f, 1f);
      uvParams.y = SpriteColorFXEditorHelper.SliderWithReset(@"V coord scale", @"V texture coordinate scale", uvParams.y, -5f, 5f, 1f);
      uvParams.z = SpriteColorFXEditorHelper.SliderWithReset(@"U coord vel", @"U texture coordinate velocity", uvParams.z, -2f, 2f, 0f);
      uvParams.w = SpriteColorFXEditorHelper.SliderWithReset(@"V coord vel", @"V texture coordinate velocity", uvParams.w, -2f, 2f, 0f);
      angle = SpriteColorFXEditorHelper.SliderWithReset(@"UV angle", @"UV rotation angle", angle, 0f, 360f, 0f);
			
      EditorGUI.indentLevel--;
		}
	}
}
