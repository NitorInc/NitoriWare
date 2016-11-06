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
  /// Spawner.
  /// </summary>
  public sealed class Spawner : MonoBehaviour
  {
    public List<GameObject> prefabs = new List<GameObject>();

    public float timeToSpawn = 0.0f;

    public float xPosition = 0.0f;

    public float zPosition = 0.0f;

    public Vector2 yMargin = Vector2.zero;

    public int sortingOrder = -1;

    public float velocity = 0.0f;

    public float colorRange = 0.0f;

    public float scale = 1.0f;

    private float mustSpam = 0.0f;

    private void OnEnable()
    {
      mustSpam = timeToSpawn * UnityEngine.Random.value;
    }

    private void Update()
    {
      if (timeToSpawn > 0.0f && prefabs.Count > 0)
      {
        mustSpam -= Time.deltaTime;
        if (mustSpam <= 0.0f)
        {
          GameObject instance = GameObject.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Count)]);
          instance.transform.parent = this.gameObject.transform;

          if (instance.GetComponent<Blender>() != null)
          {
            instance.GetComponent<Blender>().velocity = velocity + UnityEngine.Random.Range(velocity * -0.25f, velocity * 0.25f);

            // t = s / v
            GameObject.Destroy(instance.gameObject, 18.0f / Mathf.Abs(instance.GetComponent<Blender>().velocity));
          }

          if (instance.GetComponent<Flare>() != null)
          {
            instance.GetComponent<Flare>().velocity = velocity + UnityEngine.Random.Range(velocity * -0.25f, velocity * 0.25f);

            // t = s / v
            GameObject.Destroy(instance.gameObject, 18.0f / Mathf.Abs(instance.GetComponent<Flare>().velocity));
          }

          if (GameObject.FindObjectOfType<Sun>() != null && instance.GetComponent<SpriteColorBase>() != null)
            instance.GetComponent<SpriteColorBase>().LightMode = GameObject.FindObjectOfType<Sun>().LightMode;

          instance.transform.position = new Vector3(xPosition, UnityEngine.Random.Range(yMargin.x, yMargin.y), zPosition);

          if (scale != 1.0f)
            instance.transform.localScale = instance.transform.localScale * scale;

          if (sortingOrder == -1)
            instance.GetComponent<SpriteRenderer>().sortingOrder = 100 - Mathf.RoundToInt((instance.transform.position.y + 3.0f) * (100.0f / 7.0f));

          if (colorRange > 0.0f)
            instance.GetComponent<SpriteRenderer>().color = PrettyColor(colorRange);
          
          mustSpam = timeToSpawn + UnityEngine.Random.Range(timeToSpawn * -0.25f, timeToSpawn * 0.25f);
        }
      }
    }

    private Color PrettyColor(float range)
    {
      Color color = new Color();

      color.r = 1.0f - UnityEngine.Random.Range(0.0f, range);
      color.g = 1.0f - UnityEngine.Random.Range(0.0f, range);
      color.b = 1.0f - UnityEngine.Random.Range(0.0f, range);
      color.a = 1.0f;

      return color;
    }
  }
}