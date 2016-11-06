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
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// Predator controller.
  /// </summary>
  [RequireComponent(typeof(SpriteColorGlass))]
  public sealed class PredatorController : MonoBehaviour
  {
    private enum PredatorJob
    {
      Wait,
      MoveLeft,
      MoveRight,
      CamouflageOn,
      CamouflageOff,
    }

    private SpriteColorGlass spriteColorGlass;

    private List<PredatorJob> jobs = new List<PredatorJob>();

    private bool busy = false;

    private int jobIdx = 0;

    private void OnEnable()
    {
      spriteColorGlass = gameObject.GetComponent<SpriteColorGlass>();
      if (spriteColorGlass == null)
        this.enabled = false;
      else
      {
        jobs.Add(PredatorJob.Wait);
        jobs.Add(PredatorJob.CamouflageOn);
        jobs.Add(PredatorJob.MoveLeft);
        jobs.Add(PredatorJob.CamouflageOff);
        jobs.Add(PredatorJob.Wait);
        jobs.Add(PredatorJob.CamouflageOn);
        jobs.Add(PredatorJob.MoveRight);
        jobs.Add(PredatorJob.CamouflageOff);
      }
    }

    private void Update()
    {
      if (busy == false)
      {
        if (jobIdx >= jobs.Count)
          jobIdx = 0;

        switch (jobs[jobIdx++])
        {
          case PredatorJob.Wait:          this.StartCoroutine(DoWait(0.75f)); break;
          case PredatorJob.MoveLeft:      this.StartCoroutine(DoMove(4.0f, -2.0f)); break;
          case PredatorJob.MoveRight:     this.StartCoroutine(DoMove(4.0f, 2.0f)); break;
          case PredatorJob.CamouflageOn:  this.StartCoroutine(DoCamouflage(true)); break;
          case PredatorJob.CamouflageOff: this.StartCoroutine(DoCamouflage(false)); break;
        }
      }
    }

    private IEnumerator DoWait(float seconds)
    {
      busy = true;

      yield return new WaitForSeconds(seconds);

      busy = false;
    }

    private IEnumerator DoMove(float distance, float speed)
    {
      busy = true;

      while (distance > 0.0f)
      {
        this.transform.position = new Vector3(this.transform.position.x + (speed * Time.deltaTime), this.transform.position.y, this.transform.position.z);

        distance -= Mathf.Abs(speed) * Time.deltaTime;

        yield return null;
      }

      busy = false;
    }

    private IEnumerator DoCamouflage(bool camouflageOn)
    {
      busy = true;

      if (camouflageOn == true)
      {
        while (spriteColorGlass.strength < 0.8f)
        {
          spriteColorGlass.strength += Time.deltaTime;

          yield return null;
        }

        spriteColorGlass.strength = 0.8f;
      }
      else
      {
        while (spriteColorGlass.strength > 0.0f)
        {
          spriteColorGlass.strength -= Time.deltaTime;

          yield return null;
        }

        spriteColorGlass.strength = 0.0f;
      }

      busy = false;
    }
  }
}
