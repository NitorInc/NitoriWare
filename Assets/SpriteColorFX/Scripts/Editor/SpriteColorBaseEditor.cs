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
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorEditor base.
  /// </summary>
  [CustomEditor(typeof(SpriteColorBase))]
  public abstract class SpriteColorBaseEditor : Editor
  {
    /// <summary>
    /// Help text.
    /// </summary>
    public string Help { get; set; }

    /// <summary>
    /// Warnings.
    /// </summary>
    public string Warnings { get; set; }

    /// <summary>
    /// Errors.
    /// </summary>
    public string Errors { get; set; }

    private SpriteColorBase baseTarget;

    private bool bumpFactorFoldout = false;

    /// <summary>
    /// OnInspectorGUI.
    /// </summary>
    public override void OnInspectorGUI()
    {
      if (baseTarget == null)
        baseTarget = this.target as SpriteColorBase;

      EditorGUIUtility.LookLikeControls();

      EditorGUI.indentLevel = 0;

      EditorGUIUtility.labelWidth = 125.0f;

      EditorGUILayout.BeginVertical();
      {
        EditorGUILayout.Separator();

        /////////////////////////////////////////////////
        // Lit.
        /////////////////////////////////////////////////

        baseTarget.LightMode = (LightMode)EditorGUILayout.EnumPopup(@"Lighting mode", baseTarget.LightMode);
        if (baseTarget.LightMode == LightMode.BumpLit)
        {
          EditorGUI.indentLevel++;

          baseTarget.shininess = SpriteColorFXEditorHelper.SliderWithReset(@"Shininess", @"", baseTarget.shininess, 0.03f, 1.0f, 0.078125f);

          baseTarget.specularColor = EditorGUILayout.ColorField(@"Specular color", baseTarget.specularColor);

          baseTarget.normalTex = EditorGUILayout.ObjectField(@"Bump texture", baseTarget.normalTex, typeof(Texture), false) as Texture;

          baseTarget.bumpIntensity = SpriteColorFXEditorHelper.SliderWithReset(@"Bump intensity", @"", baseTarget.bumpIntensity, 0.0f, 5.0f, 1.0f);

          bumpFactorFoldout = EditorGUILayout.Foldout(bumpFactorFoldout, @"Invert channels");
          if (bumpFactorFoldout == true)
          {
            EditorGUI.indentLevel++;

            baseTarget.invertRedNormalChannel = EditorGUILayout.Toggle(@"Red", baseTarget.invertRedNormalChannel);

            baseTarget.invertGreenNormalChannel = EditorGUILayout.Toggle(@"Green", baseTarget.invertGreenNormalChannel);

            EditorGUI.indentLevel--;
          }

          EditorGUI.indentLevel--;

          EditorGUILayout.Separator();
        }

        /////////////////////////////////////////////////
        // Common.
        /////////////////////////////////////////////////

        Inspector();

        EditorGUILayout.Separator();

        /////////////////////////////////////////////////
        // Misc.
        /////////////////////////////////////////////////

        EditorGUILayout.BeginHorizontal();
        {
          if (GUILayout.Button(new GUIContent("[web]", "Open website"), GUI.skin.label) == true)
            Application.OpenURL(SpriteColorFXEditorHelper.DocumentationURL);

          GUILayout.FlexibleSpace();

          if (GUILayout.Button("Reset ALL") == true)
            ResetDefaultValues();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        if (string.IsNullOrEmpty(Warnings) == false)
        {
          EditorGUILayout.HelpBox(Warnings, MessageType.Warning);

          EditorGUILayout.Separator();
        }

        if (string.IsNullOrEmpty(Errors) == false)
        {
          EditorGUILayout.HelpBox(Errors, MessageType.Error);

          EditorGUILayout.Separator();
        }

        if (string.IsNullOrEmpty(Help) == false)
          EditorGUILayout.HelpBox(Help, MessageType.Info);
      }
      EditorGUILayout.EndVertical();

      Warnings = Errors = string.Empty;

      if (GUI.changed == true)
        EditorUtility.SetDirty(target);

      EditorGUIUtility.LookLikeControls();

      EditorGUI.indentLevel = 0;

      EditorGUIUtility.labelWidth = 125.0f;
    }

    /// <summary>
    /// Set the default values.
    /// </summary>
    protected virtual void ResetDefaultValues()
    {
      baseTarget.bumpIntensity = 1.0f;

      baseTarget.shininess = 0.078125f;

      baseTarget.specularColor = Color.gray;
    }

    /// <summary>
    /// Inspector.
    /// </summary>
    protected virtual void Inspector()
    {
      DrawDefaultInspector();
    }
  }
}