using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoodRoast {
  public class FoodRoast_Controller : MonoBehaviour {
    public static FoodRoast_Controller singleton = null;

    [Header("Difficulty")]
    [ReadOnly] public int PotatoesExists = 0;
    [SerializeField] private int CookedPotatoesRequirement = 1;

    [Header("Collected Potatoes")]
    [ReadOnly] [SerializeField] private int UncookedPotatoesCollected = 0;
    [ReadOnly] [SerializeField] private int CookedPotatoesCollected = 0;

    private void Start() {
      singleton = this;
      Reset();
    }

    private void Reset() {
      PotatoesExists = GameObject.Find("Potatoes").transform.childCount;
      UncookedPotatoesCollected = 0;
      CookedPotatoesCollected = 0;
    }

    public void AddCookedPotato() {
      CookedPotatoesCollected++;
      if (CookedPotatoesCollected >= CookedPotatoesRequirement) {
        MicrogameController.instance.setVictory(true);
      }
    }

    public void AddUncookedPotato(){
      UncookedPotatoesCollected++;
      if (UncookedPotatoesCollected > PotatoesExists - CookedPotatoesRequirement){
        MicrogameController.instance.setVictory(false);
      }
    }
  }

}