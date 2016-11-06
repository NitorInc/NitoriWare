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
  /// Flare object.
  /// </summary>
  [RequireComponent(typeof(Light))]
  [RequireComponent(typeof(SpriteRenderer))]
  public class Flare : MonoBehaviour
  {
    public float velocity = 0.0f;

    private Sun sun;

    private void OnEnable()
    {
      HSBColor hsbColor = new HSBColor();
      hsbColor.h = UnityEngine.Random.value;
      hsbColor.s = 1.0f;
      hsbColor.b = 1.0f;

      Color color = hsbColor.ToColor();
      color.a = 1.0f;

      Light light = gameObject.GetComponent<Light>();
      light.color = color;

      SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      spriteRenderer.color = color;

      sun = GameObject.FindObjectOfType<Sun>();
    }

    private void Update()
    {
      if (velocity != 0.0f)
      {
        this.transform.position = new Vector3(this.transform.position.x + (velocity * Time.deltaTime), this.transform.position.y, this.transform.position.z);

        this.transform.eulerAngles += new Vector3(0.0f, 0.0f, Time.deltaTime * 100.0f);
      }

      if (sun != null && sun.LightMode == LightMode.UnLit)
        GameObject.DestroyImmediate(this.gameObject);
    }
  }
}