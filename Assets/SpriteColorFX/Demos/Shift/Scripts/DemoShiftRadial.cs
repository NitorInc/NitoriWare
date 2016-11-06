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
  /// SpriteColorShiftRadial demo.
  /// </summary>
  [RequireComponent(typeof(SpriteColorShiftRadial))]
  public sealed class DemoShiftRadial : MonoBehaviour
  {
    public bool showGUI = true;

    private SpriteColorShiftRadial spriteColorShiftRadial;

    private void OnEnable()
    {
      spriteColorShiftRadial = gameObject.GetComponent<SpriteColorShiftRadial>();
    }

    private void OnGUI()
    {
      if (showGUI == true)
      {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        float width = 290.0f;
        const float height = 110.0f;

        GUILayout.BeginArea(new Rect(screenPosition.x - (width * 0.5f), 10.0f, width, height), GUI.skin.box);
        {
          GUILayout.BeginHorizontal();
          {
            spriteColorShiftRadial.enabled = GUILayout.Toggle(spriteColorShiftRadial.enabled, @" Enable color shift radial");
          }
          GUILayout.EndHorizontal();

          GUI.color = Color.white;

          GUILayout.Space(5.0f);

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label(@"Strength", GUILayout.Width(90.0f));

            spriteColorShiftRadial.strength = GUILayout.HorizontalSlider(spriteColorShiftRadial.strength, 0.0f, 1.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label(@"Noise amount", GUILayout.Width(90.0f));

            spriteColorShiftRadial.noiseAmount = GUILayout.HorizontalSlider(spriteColorShiftRadial.noiseAmount, 0.0f, 1.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label(@"Noise speed", GUILayout.Width(90.0f));

            spriteColorShiftRadial.noiseSpeed = GUILayout.HorizontalSlider(spriteColorShiftRadial.noiseSpeed, 0.0f, 1.0f);
          }
          GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
      }
    }
  }
}