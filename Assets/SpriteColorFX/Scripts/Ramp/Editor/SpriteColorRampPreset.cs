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
  /// Color ramp preset.
  /// </summary>
  [Serializable]
  public sealed class SpriteColorRampPreset : ScriptableObject
  {
    [Serializable]
    public class ColorPreset
    {
      [SerializeField]
      public string name = "Unnamed";
      
      [SerializeField]
      public Color[] colors = new Color[5];
    }

    [SerializeField]
    public int version = 0;

    [SerializeField]
    public bool interpolateColors = true;

    [SerializeField]
    public bool sortColorsByLuminance = false;

    [SerializeField]
    public List<ColorPreset> presets = new List<ColorPreset>();
  }
}