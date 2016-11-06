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
using System.Collections.Generic;

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// Blender object.
  /// </summary>
  [RequireComponent(typeof(SpriteColorBlend))]
  public class Blender : MonoBehaviour
  {
    public bool showGUI = true;

    public float timeToChangeBlend = 0.0f;

    public float velocity = 0.0f;

    private SpriteColorBlend spriteColorBlend;

    private Array blendModesValues;

    private float mustChangeBlend = 0.0f;

    private GUIStyle effectNameStyle;

    private void OnEnable()
    {
      spriteColorBlend = gameObject.GetComponent<SpriteColorBlend>();

      blendModesValues = Enum.GetValues(typeof(SpriteColorHelper.PixelOp));

      if (spriteColorBlend != null && timeToChangeBlend > 0.0f)
      {
        spriteColorBlend.SetPixelOp((SpriteColorHelper.PixelOp)UnityEngine.Random.Range(0, blendModesValues.Length));

        mustChangeBlend = timeToChangeBlend;
      }
    }

    private void Update()
    {
      if (velocity != 0.0f)
        this.transform.position = new Vector3(this.transform.position.x + (velocity * Time.deltaTime), this.transform.position.y, this.transform.position.z);

      if (timeToChangeBlend > 0.0f && spriteColorBlend != null)
      {
        mustChangeBlend -= Time.deltaTime;
        if (mustChangeBlend <= 0.0f)
        {
          spriteColorBlend.SetPixelOp((SpriteColorHelper.PixelOp)UnityEngine.Random.Range(0, blendModesValues.Length));

          mustChangeBlend = timeToChangeBlend;
        }
      }
    }

    private void OnGUI()
    {
      if (showGUI == true && spriteColorBlend != null)
      {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        if (effectNameStyle == null)
        {
          effectNameStyle = new GUIStyle(GUI.skin.textArea);
          effectNameStyle.alignment = TextAnchor.MiddleCenter;
          effectNameStyle.fontSize = 16;
        }

        GUILayout.BeginArea(new Rect(screenPosition.x - 50.0f, Screen.height - screenPosition.y + 50.0f, 90.0f, 25.0f), spriteColorBlend.pixelOp.ToString(), effectNameStyle);
        GUILayout.EndArea();
      }
    }
  }
}