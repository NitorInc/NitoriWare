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
  /// UV transform.
  /// </summary>
  public class UVTransform : MonoBehaviour
  {
    public Vector2 uvAnimationRate = Vector2.zero;

    protected Vector2 uvOffset = Vector2.zero;

    private MeshRenderer meshRenderer;

    private void OnEnable()
    {
      meshRenderer = base.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
      uvOffset += uvAnimationRate * Time.deltaTime;

      if (uvOffset.x >= 1.0f)
        uvOffset.x = uvOffset.x - 1.0f;

      if (uvOffset.y >= 1.0f)
        uvOffset.y = uvOffset.y - 1.0f;

      if (meshRenderer.material != null)
        meshRenderer.material.mainTextureOffset = uvOffset;
    }
  }
}