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
  public sealed class Pedestal : MonoBehaviour
  {
    public bool showGUI = true;

    public List<GameObject> figures = new List<GameObject>();

    private GameObject currentFigure;

    private int currentFigureIdx = 0;

    private SpriteColorRampPalettes currentPallete;

    private bool currentFigureEnable = true;

    private System.Array valuePalletes;

    private float currentStrenth = 1.0f;

    private Texture2D textureRamp;

    private Texture2D[] texturePallete;

    private void OnEnable()
    {
      valuePalletes = System.Enum.GetValues(typeof(SpriteColorRampPalettes));
      currentPallete = (SpriteColorRampPalettes)Random.Range(0, valuePalletes.Length);

      textureRamp = Resources.Load<Texture2D>("Textures/SpriteColorRamps");

      texturePallete = new Texture2D[5];

      InstantiateFigure(Random.Range(0, figures.Count));

      for (int i = 0; i < 5; ++i)
        texturePallete[i] = MakeTexture(32, 16, textureRamp.GetPixel((textureRamp.width / 5) * (i + 1), (int)currentPallete));
    }

    private void OnDisable()
    {
      DestroyCurrentFigure();
    }

    private void OnGUI()
    {
      if (currentFigure != null && showGUI == true)
      {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        float width = 260.0f;
        const float height = 85.0f;

        GUILayout.BeginArea(new Rect(screenPosition.x - (width * 0.5f), 20.0f, width, height), GUI.skin.box);
        {
          GUILayout.BeginHorizontal();
          {
            currentFigureEnable = GUILayout.Toggle(currentFigureEnable, " Enable");

            currentFigure.GetComponent<SpriteColorRamp>().enabled = currentFigureEnable;

            GUI.enabled = currentFigureEnable;

            GUILayout.Space(10.0f);

            if (GUILayout.Button("<<") == true)
            {
              if (currentPallete > 0)
                currentPallete--;
              else
                currentPallete = (SpriteColorRampPalettes)valuePalletes.Length - 1;

              currentFigure.GetComponent<SpriteColorRamp>().palette = currentPallete;

              for (int i = 0; i < 5; ++i)
                texturePallete[i] = MakeTexture(32, 16, textureRamp.GetPixel((textureRamp.width / 5) * (i + 1), (int)currentPallete));
            }

            GUILayout.FlexibleSpace();

            GUILayout.Label(currentPallete.ToString(), GUILayout.ExpandWidth(true));

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(">>") == true)
            {
              if (currentPallete < (SpriteColorRampPalettes)valuePalletes.Length - 1)
                currentPallete++;
              else
                currentPallete = (SpriteColorRampPalettes)0;

              currentFigure.GetComponent<SpriteColorRamp>().palette = currentPallete;

              for (int i = 0; i < 5; ++i)
                texturePallete[i] = MakeTexture(32, 16, textureRamp.GetPixel((textureRamp.width / 5) * (i + 1), (int)currentPallete));
            }
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            for (int i = 0; i < 5; ++i)
              GUILayout.Box(texturePallete[i]);
          }
          GUILayout.EndHorizontal();

          GUI.color = Color.white;

          GUILayout.Space(5.0f);

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Strength", GUILayout.Width(65.0f));

            currentStrenth = GUILayout.HorizontalSlider(currentStrenth, 0.0f, 1.0f);

            currentFigure.GetComponent<SpriteColorRamp>().strength = currentStrenth;
          }
          GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();

        GUI.enabled = true;

        width = 200.0f;

        GUILayout.BeginArea(new Rect(screenPosition.x - (width * 0.5f), 535.0f, width, 30.0f), GUI.skin.box);
        {
          GUILayout.BeginHorizontal();
          {
            if (GUILayout.Button("<<") == true)
            {
              if (currentFigureIdx > 0)
                InstantiateFigure(currentFigureIdx - 1);
              else
                InstantiateFigure(figures.Count - 1);
            }

            GUILayout.FlexibleSpace();

            GUILayout.Label(currentFigure.name, GUILayout.ExpandWidth(true));

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(">>") == true)
            {
              if (currentFigureIdx < figures.Count - 1)
                InstantiateFigure(currentFigureIdx + 1);
              else
                InstantiateFigure(0);
            }
          }
          GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
      }
    }

    private void InstantiateFigure(int index)
    {
      if (index < figures.Count)
      {
        DestroyCurrentFigure();

        currentFigureIdx = index;

        currentFigure = Instantiate(figures[currentFigureIdx], Vector3.zero, Quaternion.identity) as GameObject;
        currentFigure.name = currentFigure.name.Replace("(Clone)", string.Empty);
        currentFigure.transform.parent = this.gameObject.transform;
        currentFigure.transform.localPosition = Vector3.zero;

        currentFigure.GetComponent<SpriteColorRamp>().enabled = currentFigureEnable;
        currentFigure.GetComponent<SpriteColorRamp>().palette = currentPallete;
        currentFigure.GetComponent<SpriteColorRamp>().strength = currentStrenth;

        Sun sun = GameObject.FindObjectOfType<Sun>();
        if (sun != null)
          currentFigure.GetComponent<SpriteColorRamp>().LightMode = sun.LightMode;
      }
    }

    private void DestroyCurrentFigure()
    {
      if (currentFigure != null)
      {
        if (Application.isEditor == true)
          DestroyImmediate(currentFigure);
        else
          Destroy(currentFigure);
      }
    }

    private Texture2D MakeTexture(int width, int height, Color col)
    {
      Color[] pix = new Color[width * height];
      for (int i = 0; i < pix.Length; ++i)
        pix[i] = col;

      Texture2D result = new Texture2D(width, height);
      result.SetPixels(pix);
      result.Apply();

      return result;
    }
  }
}
