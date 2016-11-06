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
  /// SpriteColorOutline Demo.
  /// </summary>
  [RequireComponent(typeof(SpriteColorOutline))]
  public sealed class DemoOutline : MonoBehaviour
  {
    public bool showGUI = true;

    public bool cycleColors = false;

    public bool cycleAlpha = false;

    public bool changeOutlineSize = false;

    public bool changeGradientScale = false;

    public bool changeGradientOffset = false;

    public bool cycleAngle = false;

    private SpriteColorOutline spriteColorOutline;

    private HSBColor hsbColor;

    private float originalOutlineSize = 0.0f;

    private void OnEnable()
    {
      spriteColorOutline = gameObject.GetComponent<SpriteColorOutline>();

      hsbColor = new HSBColor(Color.white);
      hsbColor.s = 1.0f;

      originalOutlineSize = spriteColorOutline.outlineSize;
    }

    private void Update()
    {
      if (cycleColors == true)
      {
        hsbColor.h += Time.deltaTime * 0.1f;
        if (hsbColor.h >= 1.0f)
          hsbColor.h = 0.0f;

        spriteColorOutline.outlineColor = hsbColor.ToColor();

        if (spriteColorOutline.Mode == SpriteColorOutline.OutlineMode.Gradient)
        {
          hsbColor.h = 1.0f - hsbColor.h;

          spriteColorOutline.outlineColor2 = hsbColor.ToColor();

          hsbColor.h = 1.0f - hsbColor.h;
        }
      }

      if (cycleAlpha == true)
      {
        spriteColorOutline.outlineColor.a = (1.0f + Mathf.Sin(Time.time * 2.0f)) * 0.5f;

        if (spriteColorOutline.Mode == SpriteColorOutline.OutlineMode.Gradient)
          spriteColorOutline.outlineColor2.a = 1.0f - spriteColorOutline.outlineColor.a;
      }

      if (changeOutlineSize == true)
        spriteColorOutline.outlineSize = (originalOutlineSize * 0.4f) + (originalOutlineSize * ((1.0f + Mathf.Sin(Time.time * 2.0f)) * 0.6f));

      if (changeGradientScale == true)
        spriteColorOutline.gradientScale = Mathf.Sin(Time.time * 2.0f) * 10.0f;

      if (changeGradientOffset == true)
        spriteColorOutline.gradientOffset = (1.0f + Mathf.Cos(Time.time * 2.0f)) * 1.0f;

      if (cycleAngle == true)
        spriteColorOutline.outlineTextureUVAngle = (spriteColorOutline.outlineTextureUVAngle >= 360.0f) ? 0.0f : spriteColorOutline.outlineTextureUVAngle + Time.deltaTime * 25.0f;
    }

    private void OnGUI()
    {
      if (showGUI == true)
      {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        GUILayout.BeginArea(new Rect(screenPosition.x - 75.0f, Screen.height - screenPosition.y + 96.0f, 100.0f, 50.0f));
        {
          GUILayout.BeginHorizontal();
          {
            GUILayout.FlexibleSpace();

            GUILayout.FlexibleSpace();
          }
          GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
      }
    }
  }
}