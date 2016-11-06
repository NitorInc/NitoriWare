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

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorRampMask demo.
  /// </summary>
  [RequireComponent(typeof(SpriteColorRampMask))]
  public sealed class DemoRamp : MonoBehaviour
  {
    public bool showGUI = true;

    private SpriteColorRampMask spriteColorMaskFX;

    private SpriteColorRampPalettes[] currentPalletes;

    private bool currentFigureEnable = true;

    private System.Array valuePalletes;

    private float currentStrenth = 1.0f;

    private Texture2D textureRamp;

    private Texture2D[,] texturePalletes;

    private void OnEnable()
    {
      currentPalletes = new SpriteColorRampPalettes[3];

      valuePalletes = System.Enum.GetValues(typeof(SpriteColorRampPalettes));
      currentPalletes[0] = (SpriteColorRampPalettes)Random.Range(0, valuePalletes.Length);
      currentPalletes[1] = (SpriteColorRampPalettes)Random.Range(0, valuePalletes.Length);
      currentPalletes[2] = (SpriteColorRampPalettes)Random.Range(0, valuePalletes.Length);

      spriteColorMaskFX = gameObject.GetComponent<SpriteColorRampMask>();

      textureRamp = Resources.Load<Texture2D>("Textures/SpriteColorRamps");

      texturePalletes = new Texture2D[3, 5];
      for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 3; ++j)
          texturePalletes[j, i] = MakeTexture(14, 14, textureRamp.GetPixel((textureRamp.width / 5) * (i + 1), (int)currentPalletes[j]));
    }

    private void OnGUI()
    {
      if (showGUI == true)
      {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        float width = 280.0f;
        const float height = 140.0f;

        GUILayout.BeginArea(new Rect(screenPosition.x - (width * 0.5f), 20.0f, width, height), GUI.skin.box);
        {
          GUILayout.BeginHorizontal();
          {
            currentFigureEnable = GUILayout.Toggle(currentFigureEnable, @" Enable");

            spriteColorMaskFX.enabled = currentFigureEnable;

            GUI.enabled = currentFigureEnable;

            if (GUILayout.Button(@"I'm Feeling Lucky") == true)
            {
              for (int i = 0; i < 3; ++i)
              {
                currentPalletes[i] = (SpriteColorRampPalettes)Random.Range(0, valuePalletes.Length);

                for (int j = 0; j < 5; ++j)
                  texturePalletes[i, j] = MakeTexture(14, 14, textureRamp.GetPixel((textureRamp.width / 5) * (j + 1), (int)currentPalletes[i]));
              }
            }
          }
          GUILayout.EndHorizontal();

          for (int i = 0; i < 3; ++i)
          {
            GUILayout.BeginHorizontal();
            {
              GUILayout.Space(10.0f);

              if (GUILayout.Button("<") == true)
              {
                if (currentPalletes[i] > 0)
                  currentPalletes[i]--;
                else
                  currentPalletes[i] = (SpriteColorRampPalettes)valuePalletes.Length - 1;

                for (int j = 0; j < 5; ++j)
                  texturePalletes[i, j] = MakeTexture(14, 14, textureRamp.GetPixel((textureRamp.width / 5) * (j + 1), (int)currentPalletes[i]));
              }

              GUILayout.FlexibleSpace();

              GUILayout.Label(currentPalletes[i].ToString(), GUILayout.ExpandWidth(true));

              GUILayout.FlexibleSpace();

              if (GUILayout.Button(">") == true)
              {
                if (currentPalletes[i] < (SpriteColorRampPalettes)valuePalletes.Length - 1)
                  currentPalletes[i]++;
                else
                  currentPalletes[i] = (SpriteColorRampPalettes)0;

                for (int j = 0; j < 5; ++j)
                  texturePalletes[i, j] = MakeTexture(14, 14, textureRamp.GetPixel((textureRamp.width / 5) * (j + 1), (int)currentPalletes[i]));
              }

              for (int j = 0; j < 5; ++j)
                GUILayout.Box(texturePalletes[i, j]);
            }
            GUILayout.EndHorizontal();
          }

          GUI.color = Color.white;

          GUILayout.Space(5.0f);

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label(@"Strength", GUILayout.Width(65.0f));

            currentStrenth = GUILayout.HorizontalSlider(currentStrenth, 0.0f, 1.0f);

            spriteColorMaskFX.strength = currentStrenth;
          }
          GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();

        GUI.enabled = true;

        spriteColorMaskFX.palettes[0] = currentPalletes[0];
        spriteColorMaskFX.palettes[1] = currentPalletes[1];
        spriteColorMaskFX.palettes[2] = currentPalletes[2];
      }
    }

    private Texture2D MakeTexture(int width, int height, Color col)
    {
      Color[] pix = new Color[width * height];
      for (int i = 0; i < pix.Length; ++i)
      {
        pix[i] = col;
      }

      Texture2D result = new Texture2D(width, height);
      result.SetPixels(pix);
      result.Apply();

      return result;
    }
  }
}
