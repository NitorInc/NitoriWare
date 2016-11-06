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
  /// Demo Instagram.
  /// </summary>
  [RequireComponent(typeof(SpriteColorInstagram))]
  public class DemoInstagram : MonoBehaviour
  {
    public bool showGUI = true;

    public bool showControls = true;

    public float timeToChange = 1.0f;

    private SpriteColorInstagram spriteColorInstagram;

    private Array filterValues;

    private GUIStyle effectNameStyle;

    private float mustChange;

    private void OnEnable()
    {
      spriteColorInstagram = gameObject.GetComponent<SpriteColorInstagram>();

      filterValues = Enum.GetValues(typeof(SpriteColorInstagram.Filters));

      if (spriteColorInstagram != null && timeToChange > 0.0f)
      {
        spriteColorInstagram.Filter = (SpriteColorInstagram.Filters)UnityEngine.Random.Range(0, filterValues.Length);

        mustChange = timeToChange;
      }
    }

    private void Update()
    {
      if (timeToChange > 0.0f && spriteColorInstagram != null)
      {
        mustChange -= Time.deltaTime;
        if (mustChange <= 0.0f)
        {
          SpriteColorInstagram.Filters filter = (SpriteColorInstagram.Filters)UnityEngine.Random.Range(0, filterValues.Length);
          while (spriteColorInstagram.Filter == filter)
            filter = (SpriteColorInstagram.Filters)UnityEngine.Random.Range(0, filterValues.Length);
          
          spriteColorInstagram.Filter = filter;

          mustChange = timeToChange;
        }
      }
    }

    private void OnGUI()
    {
      if (showGUI == true && spriteColorInstagram != null)
      {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        if (effectNameStyle == null)
        {
          effectNameStyle = new GUIStyle(GUI.skin.textArea);
          effectNameStyle.alignment = TextAnchor.MiddleCenter;
          effectNameStyle.fontSize = 16;
        }

        GUILayout.BeginArea(new Rect(screenPosition.x - 65.0f, Screen.height - screenPosition.y + 85.0f, 125.0f, 27.0f));
        GUILayout.BeginHorizontal();
        {
          if (showControls == true && GUILayout.Button("<", GUILayout.Width(18)) == true)
          {
            timeToChange = 0.0f;
            
            if (spriteColorInstagram.Filter > 0)
              spriteColorInstagram.Filter--;
            else
              spriteColorInstagram.Filter = (SpriteColorInstagram.Filters)(filterValues.Length - 1);
          }

          GUILayout.Label(spriteColorInstagram.Filter.ToString().Replace("_", string.Empty), effectNameStyle, GUILayout.ExpandWidth(true));

          if (showControls == true && GUILayout.Button(">", GUILayout.Width(18)) == true)
          {
            timeToChange = 0.0f;

            if (spriteColorInstagram.Filter < (SpriteColorInstagram.Filters)(filterValues.Length - 1))
              spriteColorInstagram.Filter++;
            else
              spriteColorInstagram.Filter = 0;
          }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
      }
    }
  }
}