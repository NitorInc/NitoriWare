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
    [ReadOnly] [SerializeField] private bool Cooked = false;

    // Color Debug Variables
    private static Color uncookedColor = new Color(240 / 255f, 230 / 255f, 140 / 255f);
    private static Color cookedColor = new Color(139 / 255f, 69 / 255f, 19 / 255f);
    private static Color burntColor = new Color(51 / 255f, 26 / 255f, 0 / 255f);

    private void Start() {
      SpriteRenderer.color = uncookedColor;
      StartCoroutine(CookingCoroutine());
    }

    // Timer till cooked potatoes
    private IEnumerator CookingCoroutine() {
      yield return new WaitForSeconds(FoodRoast_Controller.Instance.GetCookTime());
      CookedProcedure();
      yield return new WaitForSeconds(FoodRoast_Controller.Instance.GetPotatoBurnTime);
      BurntProcedure();
    }

    // Procedure that turns uncooked to cooked potatoes
    private void CookedProcedure() {
      Cooked = true;
      SpriteRenderer.color = cookedColor;
    }

    // Procedure that turns uncooked to cooked potatoes
    private void BurntProcedure() {
      Cooked = false;
      SpriteRenderer.color = burntColor;
    }

    // Procedure when the potato is clicked
    private void OnMouseDown() {
      if (Cooked) {
        FoodRoast_Controller.Instance.AddCookedPotato();
      } else {
        FoodRoast_Controller.Instance.AddUncookedPotato();
      }
      Destroy(gameObject);
    }
  }
}
