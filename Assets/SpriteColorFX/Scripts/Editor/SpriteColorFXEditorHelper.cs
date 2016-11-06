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
  /// Utilities for the Editor.
  /// </summary>
  public static class SpriteColorFXEditorHelper
  {
    /// <summary>
    /// Tooltips.
    /// </summary>
    public static readonly string TooltipStrength = "The strength of the effect.\nFrom 0 (no effect) to 100 (full effect).";
    public static readonly string TooltipGamma = "Optimizes the contrast and brightness in the midtones.\nFrom 0.5 to 3.";
    public static readonly string TooltipUVScroll = "Move the UV coordinates.\nFrom 0 to 1.";
    public static readonly string TooltipLuminanceRange = "Luminance range used.\nFrom 0 to 1.";
    public static readonly string TooltipInvertLuminance = "Invert the luminance values.\nTrue or False.";
    public static readonly string TooltipTextureMask = "Texture mask for masks 1, 2 and 3 (RGB).";
    public static readonly string TooltipInfo = "Open web site.";
    public static readonly string TooltipResetAll = "Reset all values.";
    public static readonly string TooltipDistortion = "Distortion of the background.";

    public static readonly string TooltipInsertPallete = "Add a new palette below.";
    public static readonly string TooltipMoveUp = "Move this palette above.";
    public static readonly string TooltipMoveDown = "Move this palette below.";
    public static readonly string TooltipRemove = "Removes this palette.";

    public static readonly string TooltipInterpolateColors = "Interpolates colors to make a more smooth ramp.";
    public static readonly string TooltipSortByLuminance = "Sort colors by brightness.";

    public static readonly string TooltipApply = "Generates texture and data.";

    public static readonly string TooltipRedOffset = "Offset of the red channel [-1 - 1].";
    public static readonly string TooltipGreenOffset = "Offset of the red channel [-1 - 1].";
    public static readonly string TooltipBlueOffset = "Offset of the red channel [-1 - 1].";
    public static readonly string TooltipNoiseAmount = "Noise amount [0 - 100].";
    public static readonly string TooltipNoiseSpeed = "Noise speed [0 - 100].";

    public static readonly string TooltipRefraction = "The distortion of background colors.";

    /// <summary>
    /// Misc.
    /// </summary>
    public static readonly string DocumentationURL = @"http://www.ibuprogames.com/2015/05/04/sprite-color-fx";

    private static Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

    private static GUIStyle styleButtonImage;
    private static GUIStyle styleScrollRow;

    public static GUIStyle StyleScrollRow
    {
      get
      {
        if (styleScrollRow == null)
        {
          styleScrollRow = new GUIStyle(GUI.skin.box);
          styleScrollRow.margin = new RectOffset(0, 0, 0, 0);
        }

        return styleScrollRow;
      }
    }

    /// <summary>
    /// Button with icon.
    /// </summary>
    public static bool ButtonImage(string label, string tooltip, string imagePath)
    {
      if (styleButtonImage == null)
      {
        styleButtonImage = new GUIStyle(GUI.skin.button);
        styleButtonImage.margin = new RectOffset(0, 0, 0, 0);
      }

      Texture2D image = null;

      imagePath += EditorGUIUtility.isProSkin ? "_white" : "_black";
      if (images.ContainsKey(imagePath) == true)
        image = images[imagePath];
      else
      {
        image = Resources.Load(imagePath) as Texture2D;
        if (image != null)
          images.Add(imagePath, image);
      }

      styleButtonImage.normal.background = image;
      styleButtonImage.hover.background = image;
      styleButtonImage.active.background = image;

      return GUILayout.Button(new GUIContent(label, tooltip), styleButtonImage, GUILayout.Width(image.width), GUILayout.Height(image.height));
    }

    /// <summary>
    /// Toogle with a reset button.
    /// </summary>
    public static bool ToogleWithReset(string label, string tooltip, bool value, bool defaultValue)
    {
      EditorGUILayout.BeginHorizontal();
      {
        value = EditorGUILayout.Toggle(new GUIContent(label, tooltip), value);

        if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, string.Format("Reset to '{0}.'", defaultValue), @"Icons/ic_cached") == true)
          value = defaultValue;
      }
      EditorGUILayout.EndHorizontal();

      return value;
    }

    /// <summary>
    /// A slider with a reset button.
    /// </summary>
    public static float SliderWithReset(string label, string tooltip, float value, float minValue, float maxValue, float defaultValue)
    {
      EditorGUILayout.BeginHorizontal();
      {
        value = EditorGUILayout.Slider(new GUIContent(label, tooltip), value, minValue, maxValue);

        if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, string.Format("Reset to '{0}'.", defaultValue), @"Icons/ic_cached") == true)
          value = defaultValue;
      }
      EditorGUILayout.EndHorizontal();

      return value;
    }

    /// <summary>
    /// A slider with a reset button.
    /// </summary>
    public static int IntSliderWithReset(string label, string tooltip, int value, int minValue, int maxValue, int defaultValue)
    {
      EditorGUILayout.BeginHorizontal();
      {
        value = EditorGUILayout.IntSlider(new GUIContent(label, tooltip), value, minValue, maxValue);

        if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, string.Format("Reset to '{0}'", defaultValue), @"Icons/ic_cached") == true)
          value = defaultValue;
      }
      EditorGUILayout.EndHorizontal();

      return value;
    }

    /// <summary>
    /// A slider with a reset button.
    /// </summary>
    public static void MinMaxSliderWithReset(string label, string tooltip, ref float min, ref float max, float minLimit, float maxLimit, float defaultMinValue, float defaultMaxValue)
    {
      EditorGUILayout.BeginHorizontal();
      {
        EditorGUILayout.MinMaxSlider(new GUIContent(label, tooltip), ref min, ref max, minLimit, maxLimit);

        EditorGUILayout.LabelField(string.Format("{0:0.00}-{1:0.00}", min, max), GUI.skin.textField, GUILayout.Width(60.0f));

        if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, string.Format("Reset to ({0}..{1}).", defaultMinValue, defaultMaxValue), @"Icons/ic_cached") == true)
        {
          min = defaultMinValue;
          max = defaultMaxValue;
        }
      }
      EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Vector2 field with reset button.
    /// </summary>
    public static Vector2 Vector2WithReset(string label, string tooltip, Vector2 value, Vector2 defaultValue)
    {
      EditorGUILayout.BeginHorizontal();
      {
        EditorGUILayout.LabelField(new GUIContent(label, tooltip));

        float oldLabelWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = 20.0f;

        value.x = EditorGUILayout.FloatField("X", value.x);

        value.y = EditorGUILayout.FloatField("Y", value.y);

        EditorGUIUtility.labelWidth = oldLabelWidth;

        if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, string.Format("Reset to '{0}'", defaultValue), @"Icons/ic_cached") == true)
          value = defaultValue;
      }
      EditorGUILayout.EndHorizontal();

      return value;
    }

    /// <summary>
    /// Color field with reset button.
    /// </summary>
    public static Color ColorWithReset(string label, string tooltip, Color value, Color defaultValue)
    {
      EditorGUILayout.BeginHorizontal();
      {
        EditorGUILayout.LabelField(new GUIContent(label, tooltip));

        value = EditorGUILayout.ColorField(value);

        if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, string.Format("Reset to '{0}'", defaultValue), @"Icons/ic_cached") == true)
          value = defaultValue;
      }
      EditorGUILayout.EndHorizontal();

      return value;
    }

    /// <summary>
    /// Loads a texture.
    /// </summary>
    public static Texture2D LoadTexture(string fileName)
    {
      Texture2D texture = new Texture2D(0, 0);

      using (var stream = new FileStream(fileName, FileMode.Open))
      {
        if (stream == null)
        {
          Debug.LogError(string.Format("Error loading '{0}'.", fileName));
          return null;
        }

        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        if (texture.LoadImage(buffer) == false)
        {
          Debug.LogError(string.Format("LoadImage '{0}' failed.", fileName));
          return null;
        }
      }

      return texture;
    }
  }
}