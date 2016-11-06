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
  /// SpriteColorDissolve editor.
  /// </summary>
  [CustomEditor(typeof(SpriteColorDissolve))]
  public sealed class SpriteColorDissolveEditor : SpriteColorBaseEditor
  {
    private SpriteColorDissolve effect;

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected override void ResetDefaultValues()
    {
      if (effect == null)
        effect = this.target as SpriteColorDissolve;

      effect.dissolveAmount = 0.0f;
      
      effect.dissolveBorderWitdh = 0.1f;
      
      effect.dissolveBorderColor = Color.grey;
      
      effect.dissolveUVScale = 1.0f;
      
      effect.borderUVScale = 1.0f;

      base.ResetDefaultValues();
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected override void Inspector()
    {
      if (effect == null)
        effect = this.target as SpriteColorDissolve;

      EditorGUIUtility.fieldWidth = 40.0f;

      effect.dissolveAmount = SpriteColorFXEditorHelper.SliderWithReset(@"Amount", SpriteColorFXEditorHelper.TooltipNoiseAmount, effect.dissolveAmount * 100.0f, 0.0f, 100.0f, 0.0f) * 0.01f;

      DissolveShaderType newShaderType = (DissolveShaderType)EditorGUILayout.EnumPopup(new GUIContent(@"Shader", @"Texture type"), effect.shaderType);
      if (newShaderType != effect.shaderType)
        effect.SetShaderType(newShaderType);

      if (effect.shaderType != DissolveShaderType.Normal)
      {
        SpriteColorHelper.PixelOp newPixelOp = (SpriteColorHelper.PixelOp)EditorGUILayout.EnumPopup(new GUIContent(@"Blend mode", @"Blend modes"), effect.pixelOp);
        if (newPixelOp != effect.pixelOp)
          effect.SetPixelOp(newPixelOp);
      }

      DissolveTextureType newTextureType = (DissolveTextureType)EditorGUILayout.EnumPopup(@"Dissolve type", effect.dissolveTextureType);
      if (newTextureType != effect.dissolveTextureType)
        effect.SetTextureType(newTextureType);

      if (effect.dissolveTextureType == DissolveTextureType.Custom)
        effect.dissolveTexture = EditorGUILayout.ObjectField(@"Dissolve texture", effect.dissolveTexture, typeof(Texture), false) as Texture;

      effect.dissolveUVScale = SpriteColorFXEditorHelper.SliderWithReset(@"Dissolve UV scale", SpriteColorFXEditorHelper.TooltipNoiseAmount, effect.dissolveUVScale, 0.1f, 5.0f, 1.0f);

      effect.dissolveInverse = EditorGUILayout.Toggle(new GUIContent(@"Invert", SpriteColorFXEditorHelper.TooltipNoiseAmount), effect.dissolveInverse);

      if (effect.shaderType != DissolveShaderType.Normal)
        effect.dissolveBorderWitdh = SpriteColorFXEditorHelper.SliderWithReset(@"Border witdh", SpriteColorFXEditorHelper.TooltipNoiseAmount, effect.dissolveBorderWitdh * 500.0f, 0.0f, 100.0f, 50.0f) * 0.002f;

      if (effect.shaderType == DissolveShaderType.BorderColor)
        effect.dissolveBorderColor = SpriteColorFXEditorHelper.ColorWithReset(@"Border color", SpriteColorFXEditorHelper.TooltipNoiseAmount, effect.dissolveBorderColor, Color.grey);
      else if (effect.shaderType == DissolveShaderType.BorderTexture)
      {
        effect.borderTexture = EditorGUILayout.ObjectField(@"Border texture", effect.borderTexture, typeof(Texture), false) as Texture;
        effect.borderUVScale = SpriteColorFXEditorHelper.SliderWithReset(@"Border UV scale", SpriteColorFXEditorHelper.TooltipNoiseAmount, effect.borderUVScale, 0.1f, 5.0f, 1.0f);
      }
    }
  }
}