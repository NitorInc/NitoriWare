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
  /// SpriteColorDissolve Demo.
  /// </summary>
  [RequireComponent(typeof(SpriteColorDissolve))]
  public sealed class DemoDissolve : MonoBehaviour
  {
    public bool showGUI = true;

    private SpriteColorDissolve spriteColorDissolve;

    private static int knightGlobalID = 0;

    private int knightID;

    private void OnEnable()
    {
      spriteColorDissolve = gameObject.GetComponent<SpriteColorDissolve>();

      knightID = knightGlobalID++;
    }

    private void Update()
    {
      spriteColorDissolve.dissolveAmount = (0.75f + (knightID % 2 == 0 ? Mathf.Sin(Time.time * 0.75f) : Mathf.Cos(Time.time * -0.75f))) * 0.6f;
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

            GUILayout.Label(spriteColorDissolve.dissolveTextureType.ToString());

            GUILayout.FlexibleSpace();
          }
          GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
      }
    }
  }
}