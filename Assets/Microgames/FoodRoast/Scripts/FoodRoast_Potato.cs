using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoodRoast { 
  [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
  public class FoodRoast_Potato : MonoBehaviour {
    private SpriteRenderer _SpriteRenderer;
    public SpriteRenderer SpriteRenderer {
      get {
        if (_SpriteRenderer == null)
          _SpriteRenderer = GetComponent<SpriteRenderer>();
        return _SpriteRenderer;
      }
    }

    [Header("Ripe Variables")]
    [ReadOnly] [SerializeField] private bool Ripe = false;
    [SerializeField] private float TimeTillRipeness = 2.0f;

    // Color Debug Variables
    private static Color NotRipeColor = new Color(240 / 255f, 230 / 255f, 140 / 255f);
    private static Color RipeColor = new Color(139 / 255f, 69 / 255f, 19 / 255f);

    private void Start() {
      SpriteRenderer.color = NotRipeColor;
      StartCoroutine(WaitTillRipenessCoroutine());
    }

    // Timer till cooked potatoes
    private IEnumerator WaitTillRipenessCoroutine() {
      yield return new WaitForSeconds(TimeTillRipeness);
      RipenessProcedure();
    }

    // Procedure that turns uncooked to cooked potatoes
    private void RipenessProcedure() {
      Ripe = true;
      SpriteRenderer.color = RipeColor;
    }

    // Procedure when the potato is clicked
    private void OnMouseDown() {
      if (Ripe) {
        FoodRoast_Controller.singleton.AddCookedPotato();
      } else {
        FoodRoast_Controller.singleton.AddUncookedPotato();
      }
      Destroy(gameObject);
    }
  }
}
