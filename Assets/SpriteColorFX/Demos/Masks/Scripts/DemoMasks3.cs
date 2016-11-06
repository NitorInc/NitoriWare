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
//#define SECUENCIAL_BLENDMODES

using System.Collections.Generic;

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// SpriteColorMasks3 Demo.
  /// </summary>
  [RequireComponent(typeof(SpriteColorMasks3))]
  public sealed class DemoMasks3 : MonoBehaviour
  {
    public bool showGUI = true;

    public bool changeColors = true;

    public bool changeBlendModes = true;

    private readonly float changeTime = 1.0f;

    private SpriteColorMasks3 spriteColorMask;

    private float timeToChange = 0.0f;

    private SpriteColorHelper.PixelOp[] blendModesValues;

    private Texture[] textureColors = new Texture[3];

#if SECUENCIAL_BLENDMODES
    private int currentBlendMode = 0;
#endif

    private void OnEnable()
    {
      spriteColorMask = gameObject.GetComponent<SpriteColorMasks3>();
      if (spriteColorMask == null)
        this.enabled = false;
      else
      {
        blendModesValues = new SpriteColorHelper.PixelOp[11];
        blendModesValues[0] = SpriteColorHelper.PixelOp.Burn;
        blendModesValues[1] = SpriteColorHelper.PixelOp.Darken;
        blendModesValues[2] = SpriteColorHelper.PixelOp.Darker;
        blendModesValues[3] = SpriteColorHelper.PixelOp.Dodge;
        blendModesValues[4] = SpriteColorHelper.PixelOp.HardMix;
        blendModesValues[5] = SpriteColorHelper.PixelOp.HardLight;
        blendModesValues[6] = SpriteColorHelper.PixelOp.Lighten;
        blendModesValues[7] = SpriteColorHelper.PixelOp.Luminosity;
        blendModesValues[8] = SpriteColorHelper.PixelOp.Overlay;
        blendModesValues[9] = SpriteColorHelper.PixelOp.SoftLight;
        blendModesValues[10] = SpriteColorHelper.PixelOp.VividLight;

        if (changeBlendModes == true)
          ChangeBlendMode();

        if (changeColors == true)
          ChangeColors();

        timeToChange = Random.Range(0.0f, changeTime * 0.5f);

        UpdateTextureColors();
      }
    }

    private void Update()
    {
      timeToChange += Time.deltaTime;

      if (spriteColorMask.enabled == true && timeToChange >= changeTime)
      {
        if (changeBlendModes == true)
          ChangeBlendMode();

        if (changeColors == true)
          ChangeColors();

        timeToChange = 0.0f;
      }
    }

    private void OnGUI()
    {
      if (showGUI == false)
        return;

      Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

      float width = 150.0f;
      const float height = 150.0f;

      GUILayout.BeginArea(new Rect(screenPosition.x - (width * 0.5f), screenPosition.y - (height * 0.5f), width, height), GUI.skin.box);
      {
        spriteColorMask.enabled = GUILayout.Toggle(spriteColorMask.enabled, @" Enable masks");

        GUI.enabled = spriteColorMask.enabled;

        GUILayout.BeginHorizontal();
        {
          GUILayout.Label(@"Strength", GUILayout.Width(50.0f));

          spriteColorMask.strength = GUILayout.HorizontalSlider(spriteColorMask.strength, 0.0f, 1.0f);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
          if (GUILayout.Button("<<") == true)
          {
            changeBlendModes = false;

            if (spriteColorMask.pixelOp > 0)
              spriteColorMask.SetPixelOp(spriteColorMask.pixelOp - 1);
            else
              spriteColorMask.SetPixelOp(SpriteColorHelper.PixelOp.VividLight);
          }

          GUILayout.FlexibleSpace();

          GUILayout.Label(spriteColorMask.pixelOp.ToString(), GUILayout.ExpandWidth(true));

          GUILayout.FlexibleSpace();

          if (GUILayout.Button(">>") == true)
          {
            changeBlendModes = false;

            if (spriteColorMask.pixelOp < SpriteColorHelper.PixelOp.VividLight)
              spriteColorMask.SetPixelOp(spriteColorMask.pixelOp + 1);
            else
              spriteColorMask.SetPixelOp(SpriteColorHelper.PixelOp.Additive);
          }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
          GUILayout.Label(@"Mask #1", GUILayout.Width(50.0f));

          spriteColorMask.strengthMaskRed = GUILayout.HorizontalSlider(spriteColorMask.strengthMaskRed, 0.0f, 1.0f);

          GUILayout.Box(textureColors[0], GUILayout.Width(24.0f));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
          GUILayout.Label(@"Mask #2", GUILayout.Width(50.0f));

          spriteColorMask.strengthMaskGreen = GUILayout.HorizontalSlider(spriteColorMask.strengthMaskGreen, 0.0f, 1.0f);

          GUILayout.Box(textureColors[1], GUILayout.Width(24.0f));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
          GUILayout.Label(@"Mask #3", GUILayout.Width(50.0f));

          spriteColorMask.strengthMaskBlue = GUILayout.HorizontalSlider(spriteColorMask.strengthMaskBlue, 0.0f, 1.0f);

          GUILayout.Box(textureColors[2], GUILayout.Width(24.0f));
        }
        GUILayout.EndHorizontal();
      }
      GUILayout.EndArea();
    }

    private void ChangeBlendMode()
    {
#if SECUENCIAL_BLENDMODES
      spriteColorMask.SetPixelOp(blendModesValues[currentBlendMode++]);

      if (currentBlendMode >= blendModesValues.Length)
        currentBlendMode = 0;
#else
      spriteColorMask.SetPixelOp(blendModesValues[Random.Range(0, blendModesValues.Length)]);
#endif
    }

    private void ChangeColors()
    {
      spriteColorMask.colorMaskRed = PrettyColor();
      spriteColorMask.colorMaskGreen = PrettyColor();
      spriteColorMask.colorMaskBlue = PrettyColor();

      UpdateTextureColors();
    }

    private Color PrettyColor()
    {
      Color color = new Color();

      const float rgbRange = 0.35f;

      color.r = Random.Range(-rgbRange, rgbRange) + 0.5f;
      color.g = Random.Range(-rgbRange, rgbRange) + 0.5f;
      color.b = Random.Range(-rgbRange, rgbRange) + 0.5f;
      color.a = 1.0f;

      return color;
    }

    private void UpdateTextureColors()
    {
      textureColors[0] = MakeTexture(12, 12, spriteColorMask.colorMaskRed);
      textureColors[1] = MakeTexture(12, 12, spriteColorMask.colorMaskGreen);
      textureColors[2] = MakeTexture(12, 12, spriteColorMask.colorMaskBlue);
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
