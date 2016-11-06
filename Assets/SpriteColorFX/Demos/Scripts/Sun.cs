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
  /// Sun.
  /// </summary>
  public sealed class Sun : MonoBehaviour
  {
    public bool changeColor;

    public bool changeX = true;

    public bool changeY = true;

    public bool showGUI = false;

    public Vector2 positionGUI = new Vector2(5.0f, 5.0f);

    public string artCopyright;

    public LightMode LightMode { get { return lightModeSelected == 0 ? LightMode.UnLit : LightMode.BumpLit; } }

    private SpriteRenderer spriteRenderer;

    private Light sunLight;

    private HSBColor hsbColor;

    private float x = 0.0f;

    private string[] lightModes;
    private int lightModeSelected = 0;

    private List<Spawner> flareSpawners = new List<Spawner>();

    private void OnEnable()
    {
      lightModes = new string[2];
      lightModes[0] = @"UnLit";
      lightModes[1] = @"BumpLit";

      spriteRenderer = this.GetComponent<SpriteRenderer>();

      sunLight = this.GetComponent<Light>();

      hsbColor = new HSBColor(Color.white);
      hsbColor.s = 1.0f;

      if (GameObject.Find("FlareSpawner0") != null)
      {
        flareSpawners.Add(GameObject.Find("FlareSpawner0").GetComponent<Spawner>());
        flareSpawners[0].enabled = false;
      }

      if (GameObject.Find("FlareSpawner1") != null)
      {
        flareSpawners.Add(GameObject.Find("FlareSpawner1").GetComponent<Spawner>());
        flareSpawners[1].enabled = false;
      }

      if (GameObject.Find("FlareSpawner2") != null)
      {
        flareSpawners.Add(GameObject.Find("FlareSpawner2").GetComponent<Spawner>());
        flareSpawners[2].enabled = false;
      }
    }

    private void Update()
    {
      if (sunLight == null || sunLight.enabled == false)
        return;

      if (changeColor == true)
      {
        hsbColor.h += Time.deltaTime * 0.1f;
        if (hsbColor.h >= 1.0f)
          hsbColor.h = 0.0f;

        sunLight.color = hsbColor.ToColor();

        spriteRenderer.color = sunLight.color;
      }

      x += Time.deltaTime;

      this.transform.position = new Vector3(changeX == true ? Mathf.Sin(x) * 7.0f : 0.0f,
                                            changeY == true ? Mathf.Cos(x) * 3.0f - 0.5f : 0.0f,
                                            this.transform.position.z);
    }

    private void OnGUI()
    {
      if (showGUI == true)
      {
        GUILayout.BeginArea(new Rect(positionGUI.x, positionGUI.y, 150.0f, 30.0f), GUI.skin.box);
        {
          GUILayout.BeginHorizontal();
          {
            int newLightModeSelected = GUILayout.SelectionGrid(lightModeSelected, lightModes, 2);
            if (newLightModeSelected != lightModeSelected)
            {
              lightModeSelected = newLightModeSelected;

              switch (lightModeSelected)
              {
                case 0:
                  if (spriteRenderer != null)
                    spriteRenderer.enabled = false;

                  if (sunLight != null)
                    sunLight.enabled = false;
                break;

                case 1:
                  if (spriteRenderer != null)
                    spriteRenderer.enabled = true;

                  if (sunLight != null)
                    sunLight.enabled = true;
                break;
              }

              SpriteColorBase[] sprites = GameObject.FindObjectsOfType<SpriteColorBase>();
              for (int i = 0; i < sprites.Length; ++i)
                sprites[i].LightMode = LightMode;

              for (int i = 0; i < flareSpawners.Count; ++i)
                flareSpawners[i].enabled = (LightMode == SpriteColorFX.LightMode.BumpLit ? true : false);
            }
          }
          GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();

        if (artCopyright != string.Empty)
          GUI.Label(new Rect(Screen.width - 275.0f, Screen.height - 22.0f, 200.0f, 42.0f), artCopyright);
      }
    }
  }
}
