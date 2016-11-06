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
  /// SpriteColorShiftLinear demo.
  /// </summary>
  [RequireComponent(typeof(SpriteColorShiftLinear))]
  public sealed class DemoShiftLinear : MonoBehaviour
  {
    public bool showGUI = true;

    private SpriteColorShiftLinear spriteColorShiftLinear;

    private void OnEnable()
    {
      spriteColorShiftLinear = gameObject.GetComponent<SpriteColorShiftLinear>();
    }

    private void OnGUI()
    {
      if (showGUI == true)
      {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        float width = 290.0f;
        const float height = 155.0f;

        GUILayout.BeginArea(new Rect(screenPosition.x - (width * 0.5f), 10.0f, width, height), GUI.skin.box);
        {
          GUILayout.BeginHorizontal();
          {
            spriteColorShiftLinear.enabled = GUILayout.Toggle(spriteColorShiftLinear.enabled, @" Enable color shift linear");
          }
          GUILayout.EndHorizontal();

          GUI.color = Color.white;

          GUILayout.Space(5.0f);

          spriteColorShiftLinear.redShift = GUILayoutVector2(@"Red shift", spriteColorShiftLinear.redShift);

          spriteColorShiftLinear.greenShift = GUILayoutVector2(@"Green shift", spriteColorShiftLinear.greenShift);

          spriteColorShiftLinear.blueShift = GUILayoutVector2(@"Blue shift", spriteColorShiftLinear.blueShift);

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label(@"Noise amount", GUILayout.Width(90.0f));

            spriteColorShiftLinear.noiseAmount = GUILayout.HorizontalSlider(spriteColorShiftLinear.noiseAmount, 0.0f, 1.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label(@"Noise speed", GUILayout.Width(90.0f));

            spriteColorShiftLinear.noiseSpeed = GUILayout.HorizontalSlider(spriteColorShiftLinear.noiseSpeed, 0.0f, 1.0f);
          }
          GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
      }
    }

    private Vector2 GUILayoutVector2(string name, Vector2 value)
    {
      GUILayout.BeginHorizontal();
      {
        GUILayout.Label(name, GUILayout.Width(65.0f));

        GUILayout.Label(@"X", GUILayout.Width(10.0f));

        value.x = GUILayout.HorizontalSlider(value.x, -1.0f, 1.0f);

        GUILayout.Label(@"Y", GUILayout.Width(10.0f));

        value.y = GUILayout.HorizontalSlider(value.y, -1.0f, 1.0f);
      }
      GUILayout.EndHorizontal();

      return value;
    }
  }
}
