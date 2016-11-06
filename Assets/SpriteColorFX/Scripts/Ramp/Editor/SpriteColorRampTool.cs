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
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;

namespace SpriteColorFX
{
  /// <summary>
  /// Color Ramp Tool.
  /// </summary>
  public sealed class SpriteColorRampTool : EditorWindow
  {
    private bool interpolateColors = true;
    private bool sortColorsByLuminance = false;

    private List<SpriteColorRampPreset.ColorPreset> presets = new List<SpriteColorRampPreset.ColorPreset>();

    private bool assetChanged = false;

    private string assetFile;

    private readonly string assetPathKey = @"SpriteColorFX.AssetPath";

    private readonly string enumFile = @"/SpriteColorFX/Scripts/Ramp/SpriteColorRampPalettes.cs";
    private readonly string textureFile = @"SpriteColorFX/Resources/Textures/SpriteColorRamps.png";

    private Vector2 scroll;

    [MenuItem("Window/Sprite Color FX/Color Ramp Tool")]
    public static void OpenColorRampToolWindow()
    {
      EditorWindow.GetWindow<SpriteColorRampTool>(false, @"Color Ramp Tool").Initialize();
    }

    private void Initialize()
    {
      this.minSize = new Vector2(750.0f, 400.0f);

      assetFile = PlayerPrefs.GetString(assetPathKey, @"SpriteColorFX/Presets/Default.asset");
      if (File.Exists(Application.dataPath + "/" + assetFile) == true)
        LoadAsset(Application.dataPath + "/" + assetFile);
    }

    private void OnGUI()
    {
      EditorGUIUtility.LookLikeControls();

      EditorGUILayout.BeginVertical();
      {
        EditorGUIUtility.LookLikeControls(24.0f, 50.0f);

        // Colors
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
          if (presets.Count > 0)
          {
            scroll = EditorGUILayout.BeginScrollView(scroll, GUI.skin.box);

            int presetToDelete = -1;
            int moveUp = -1;
            int moveDown = -1;
            int insertUp = -1;

            for (int i = 0; i < presets.Count; ++i)
            {
              GUI.backgroundColor = (i % 2 == 0) ? Color.white * 0.1f : Color.white * 0.2f;

              EditorGUILayout.BeginHorizontal(SpriteColorFXEditorHelper.StyleScrollRow);
              {
                GUI.backgroundColor = Color.white;

                EditorGUILayout.LabelField((i + 1).ToString("0000"), GUILayout.Width(32.0f));

                EditorGUILayout.Space();

                string newName = EditorGUILayout.TextField(presets[i].name, GUILayout.MinWidth(128.0f));
                if (newName != presets[i].name)
                {
                  presets[i].name = newName;

                  assetChanged = true;
                }

                EditorGUILayout.Space();

                for (int j = 0; j < presets[i].colors.Length; ++j)
                {
                  Color newColor = EditorGUILayout.ColorField("#" + (j + 1), presets[i].colors[j]);
                  if (newColor != presets[i].colors[j])
                  {
                    presets[i].colors[j] = newColor;

                    assetChanged = true;
                  }

                  GUILayout.Space(5.0f);
                }

                EditorGUILayout.Space();

                if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, SpriteColorFXEditorHelper.TooltipInsertPallete, @"Icons/ic_playlist_add") == true)
                  insertUp = i;

                EditorGUILayout.Space();

                if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, SpriteColorFXEditorHelper.TooltipMoveUp, @"Icons/ic_keyboard_arrow_up") == true)
                  moveUp = i;

                if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, SpriteColorFXEditorHelper.TooltipMoveDown, @"Icons/ic_keyboard_arrow_down") == true)
                  moveDown = i;

                EditorGUILayout.Space();

                if (SpriteColorFXEditorHelper.ButtonImage(string.Empty, SpriteColorFXEditorHelper.TooltipRemove, @"Icons/ic_clear") == true)
                  presetToDelete = i;
              }
              EditorGUILayout.EndHorizontal();
            }

            if (presetToDelete != -1)
            {
              assetChanged = true;

              presets.RemoveAt(presetToDelete);
            }

            if (insertUp != -1)
            {
              assetChanged = true;

              presets.Insert(insertUp, new SpriteColorRampPreset.ColorPreset());
            }

            if (moveUp > 0)
            {
              assetChanged = true;

              SpriteColorRampPreset.ColorPreset colorPreset = presets[moveUp];

              presets.RemoveAt(moveUp);
              presets.Insert(moveUp - 1, colorPreset);
            }

            if (moveDown != -1 && moveDown < (presets.Count - 1))
            {
              assetChanged = true;

              SpriteColorRampPreset.ColorPreset colorPreset = presets[moveDown];

              presets.RemoveAt(moveDown);
              presets.Insert(moveDown + 1, colorPreset);
            }

            EditorGUILayout.EndScrollView();
          }

          GUILayout.FlexibleSpace();

          EditorGUILayout.BeginHorizontal();
          {
            if (string.IsNullOrEmpty(assetFile) == true)
              EditorGUILayout.Space();
            else
            {
              if (assetChanged == true)
                EditorGUILayout.LabelField(string.Format("*'{0}'*", assetFile));
              else
              {
                GUI.enabled = false;

                EditorGUILayout.LabelField(string.Format("'{0}'", assetFile));

                GUI.enabled = true;
              }
            }

/* Planned for upcoming updates.
            if (GUILayout.Button("Adobe Color CC") == true)
              AddPaletteFromAdobeColorCC();
*/

/* Planned for upcoming updates.
            if (GUILayout.Button("COLOURlovers") == true)
              AddPaletteFromCOLOURlovers();
*/

/* Planned for upcoming updates.
            if (GUILayout.Button("Add pallete from image") == true)
              AddPaletteFromImage();
*/
            if (GUILayout.Button(@"Add pallete", GUILayout.Width(75.0f)) == true)
            {
              presets.Add(new SpriteColorRampPreset.ColorPreset());

              assetChanged = true;
            }

            if (GUILayout.Button(@"Clear all", GUILayout.Width(75.0f)) == true && presets.Count > 0)
            {
              if (EditorUtility.DisplayDialog(@"Clear all palletes.", @"Delete all palettes?", @"Ok", @"Cancel") == true)
              {
                presets.Clear();

                assetChanged = true;
              }
            }
          }
          EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUIUtility.LookLikeControls(128.0f, 50.0f);

        // Buttons
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
          EditorGUILayout.BeginVertical();
          {
            bool option = EditorGUILayout.Toggle(new GUIContent(@"Interpolate colors", SpriteColorFXEditorHelper.TooltipInterpolateColors), interpolateColors);
            if (option != interpolateColors)
            {
              interpolateColors = option;

              assetChanged = true;
            }

            option = EditorGUILayout.Toggle(new GUIContent(@"Sort by luminance", SpriteColorFXEditorHelper.TooltipSortByLuminance), sortColorsByLuminance);
            if (option != sortColorsByLuminance)
            {
              sortColorsByLuminance = option;

              assetChanged = true;
            }
          }
          EditorGUILayout.EndVertical();

          GUILayout.FlexibleSpace();

          EditorGUILayout.BeginVertical();
          {
            if (GUILayout.Button("Load", GUILayout.Width(75.0f)) == true)
            {
              string filePath = EditorUtility.OpenFilePanel(@"Load asset", Application.dataPath + "/" + Path.GetDirectoryName(assetFile), "asset");
              if (string.IsNullOrEmpty(filePath) == false)
                LoadAsset(filePath);
            }

            if (GUILayout.Button("Save", GUILayout.Width(75.0f)) == true)
            {
              string filePath = EditorUtility.SaveFilePanel(@"Save asset", Application.dataPath + "/" + Path.GetDirectoryName(assetFile), Path.GetFileNameWithoutExtension(assetFile), "asset");
              if (string.IsNullOrEmpty(filePath) == false)
                SaveAsset(filePath);
            }
          }
          EditorGUILayout.EndVertical();

          EditorGUILayout.BeginHorizontal();
          {
            if (GUILayout.Button(new GUIContent(@"Apply", SpriteColorFXEditorHelper.TooltipApply), GUILayout.ExpandHeight(true), GUILayout.Width(75.0f)) == true)
              Process();
          }
          EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
    }

/* Planned for upcoming updates.
    private void AddPaletteFromImage()
    {
      string filePath = EditorUtility.OpenFilePanel(@"Select image", Application.dataPath + "/" + Path.GetDirectoryName(assetFile), "png");
      if (string.IsNullOrEmpty(filePath) == false)
      {
        Texture2D image = SpriteColorFXEditorHelper.LoadTexture(filePath);
        if (image != null)
        {

        }
        else
          Debug.LogError(string.Format("Error loading '{0}'", filePath));
      }
    }
*/

    private void LoadAsset(string path)
    {
      int assetIndex = path.LastIndexOf(@"Assets");
      if (assetIndex > 0)
        path = path.Remove(0, assetIndex);

      SpriteColorRampPreset asset = AssetDatabase.LoadAssetAtPath(path, typeof(SpriteColorRampPreset)) as SpriteColorRampPreset;
      if (asset != null)
      {
        if (asset.version == 1)
        {
          interpolateColors = asset.interpolateColors;
          sortColorsByLuminance = asset.sortColorsByLuminance;
          presets = asset.presets.GetRange(0, asset.presets.Count);
          assetChanged = false;

          assetFile = path.Replace(@"Assets/", string.Empty);
          PlayerPrefs.SetString(assetPathKey, assetFile);
        }
        else
          Debug.LogWarning(string.Format("'{0}' unknown version.", path));
      }
      else
        Debug.LogWarning(string.Format("Error loading '{0}'.", path));
    }

    private void SaveAsset(string path)
    {
      int assetIndex = path.LastIndexOf(@"Assets");
      if (assetIndex > 0)
        path = path.Remove(0, assetIndex);

      AssetDatabase.DeleteAsset(path);

      SpriteColorRampPreset asset = ScriptableObject.CreateInstance<SpriteColorRampPreset>();

      asset.version = 1;
      asset.interpolateColors = interpolateColors;
      asset.sortColorsByLuminance = sortColorsByLuminance;
      asset.presets = presets.GetRange(0, presets.Count);

      AssetDatabase.CreateAsset(asset, path);

      AssetDatabase.SaveAssets();
      
      assetChanged = false;

      assetFile = path.Replace(@"Assets/", string.Empty);
      PlayerPrefs.SetString(assetPathKey, assetFile);
    }

    private void Process()
    {
      string warnings = string.Empty;
      string errors = string.Empty;

      int textureHeight = NextPow2(presets.Count);
      textureHeight = Mathf.Clamp(textureHeight, 32, 1024);

      // Bake texture.
      Texture2D texture = new Texture2D(256, textureHeight, TextureFormat.RGB24, false);

      Color[] colors = new Color[5];

      int palletesSaved = 0;
      for (int y = 0; y < (int)textureHeight; ++y)
      {
        if (y < presets.Count)
        {
          for (int i = 0; i < presets[y].colors.Length; ++i)
            colors[i] = presets[y].colors[i];

          if (sortColorsByLuminance == true)
            Array.Sort(colors, CompareLuminance);

          palletesSaved++;
        }

        for (int x = 0; x < 256; ++x)
        {
          Color color = Color.magenta;

          if (y < presets.Count)
          {
            int colorIdx = (int)Math.Floor((float)x / (256.0f / 5.0f));

            if (interpolateColors == true)
            {
              Color color1 = (colorIdx == 0 ? Color.black : colors[colorIdx - 1]);
              Color color2 = colors[colorIdx];

              float lerpFactor = (float)(x - (colorIdx * (256.0f / 5.0f))) / (256.0f / 5.0f);
              color = Color.Lerp(color1, color2, lerpFactor);
            }
            else
              color = colors[colorIdx];
          }

          texture.SetPixel(x, y, color);
        }
      }

      byte[] bytes = texture.EncodeToPNG();

      DestroyImmediate(texture);

      File.WriteAllBytes(Application.dataPath + "/" + textureFile, bytes);

      // Generate Enums.
      List<string> lines = new List<string>();

      lines.Add(@"///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");
      lines.Add(@"// Sprite Color FX.");
      lines.Add(@"// Copyright (c) Ibuprogames. All rights reserved.");
      lines.Add(@"///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");
      lines.Add(@"");
      lines.Add(@"namespace SpriteColorFX");
      lines.Add(@"{");
      lines.Add(@"  /// <summary>");
      lines.Add(@"  /// This code was automatically generated by 'Color Ramp Tool', do not modify!");
      lines.Add(@"  /// </summary>");
      lines.Add(@"  public enum SpriteColorRampPalettes");
      lines.Add(@"  {");

      // Name validation.
      List<string> validNames = new List<string>();

      Regex regex = new Regex(@"[^a-zA-Z0-9 -]");
      for (int i = 0; i < presets.Count; ++i)
      {
        int idx = 0;
        string originalName = regex.Replace(presets[i].name, string.Empty);

        if (string.IsNullOrEmpty(originalName) == true)
          originalName = @"NotValidName";

        if (Char.IsDigit(originalName[0]) == true)
          originalName = "_" + originalName;

        string validated = originalName;
        while (validNames.Contains(validated) == true && idx < 1000)
          validated = originalName + "_" + (++idx).ToString("000");

        validNames.Add(validated);
      }

      for (int i = 0; i < validNames.Count; ++i)
        lines.Add(string.Format("    {0} = {1},", validNames[i], i));

      lines.Add(@"  }");
      lines.Add(@"}");

      File.WriteAllLines(Application.dataPath + enumFile, lines.ToArray());

      AssetDatabase.Refresh();

      if (errors == string.Empty)
        EditorUtility.DisplayDialog(@"Task completed.", string.Format("Resources updated with {0} palletes.\n{1}", palletesSaved, warnings), "Ok");
    }

    private int NextPow2(int x)
    {
      x -= 1;
      x |= (x >> 1);
      x |= (x >> 2);
      x |= (x >> 4);
      x |= (x >> 8);
      x |= (x >> 16);

      return x + 1;
    }

    private static int CompareLuminance(Color color1, Color color2)
    {
      float colorLum1 = color1.r * 0.299f + color1.g * 0.587f + color1.b * 0.114f;
      float colorLum2 = color2.r * 0.299f + color2.g * 0.587f + color2.b * 0.114f;

      if (colorLum1 > colorLum2)
        return 1;

      if (colorLum2 > colorLum1)
        return -1;

      return 0;
    }
  }
}