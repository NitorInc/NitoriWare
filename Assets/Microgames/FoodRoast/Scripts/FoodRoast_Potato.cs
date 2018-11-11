using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoodRoast { 
  public enum PotatoCookedState { Uncooked, Cooked, Burnt}

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

    [System.Serializable]
    private struct PotatoSpritePack {
      public Sprite[] Pack;
    }

    [Header("Potato Sprites (0 - Uncooked, 1 - Cooked, 2 - Burnt)")]
    [SerializeField] private PotatoSpritePack[] PotatoSprites;
    private Sprite GetSprite => PotatoSprites[(int)PotatoCookedState].Pack[PotatoAnimationState];
    private int GetPackLength => PotatoSprites[0].Pack.Length;

    [Header("Animation States")]
    [SerializeField] private float StateDuration = 0.33f;
    [ReadOnly] [SerializeField] private int PotatoAnimationState = 0;
    [ReadOnly] [SerializeField] private PotatoCookedState PotatoCookedState = 0;

    private void Start() {
      PotatoAnimationState = FoodRoast_Controller.Instance.GetAnimationState(GetPackLength);
      PotatoCookedState = PotatoCookedState.Uncooked;
      StartCoroutine(AnimationCoroutine());
      StartCoroutine(CookingCoroutine());
    }

    private IEnumerator AnimationCoroutine(){
      float time = 0;
      while (true) {
        SpriteRenderer.sprite = GetSprite;
        while (time < StateDuration) {
          yield return null;
          time += Time.deltaTime;
        }
        time -= StateDuration;
        PotatoAnimationState = (PotatoAnimationState + 1) % GetPackLength;
      }
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
      PotatoCookedState = PotatoCookedState.Cooked;
      SpriteRenderer.sprite = GetSprite;
    }

    // Procedure that turns uncooked to cooked potatoes
    private void BurntProcedure() {
      PotatoCookedState = PotatoCookedState.Burnt;
      SpriteRenderer.sprite = GetSprite;
    }

    // Procedure when the potato is clicked
    private void OnMouseDown() {
      if (PotatoCookedState == PotatoCookedState.Cooked) {
        FoodRoast_Controller.Instance.AddCookedPotato();
      } else {
        FoodRoast_Controller.Instance.AddUncookedPotato();
      }
      Destroy(gameObject);
    }
  }
}
