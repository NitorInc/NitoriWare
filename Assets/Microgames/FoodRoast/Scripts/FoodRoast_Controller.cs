using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoodRoast {

  [System.Serializable]
  public struct PotatoCookTime {
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    public float GetTime { get { return Random.Range(minTime, maxTime); } }
  }

  [System.Serializable]
  public struct AmbienceClip {
    public AudioClip clip;
    [Range(0.0f, 1.0f)]
    public float volume;
    public AmbienceClip(AudioClip _clip, float _volume) {
      clip = _clip;
      volume = _volume;
    }
  }

  public class FoodRoast_Controller : MonoBehaviour {
    public static FoodRoast_Controller _singleton = null;
    public static FoodRoast_Controller singleton{
      get{
        // Unefficient, but we can't use script execution order.
        if (_singleton == null){
          _singleton = GameObject.Find("Controller").GetComponent<FoodRoast_Controller>();
        }
        return _singleton;
      }
    }

    [Header("Ambience")]
    [SerializeField] private AmbienceClip[] AmbienceClips; 

    [Header("Difficulty")]
    [ReadOnly] public int PotatoesExists = 0;
    [SerializeField] private int CookedPotatoesRequirement = 1;
    [SerializeField] private PotatoCookTime[] PotatoCookTimes;

    [Header("Collected Potatoes")]
    [ReadOnly] [SerializeField] private int UncookedPotatoesCollected = 0;
    [ReadOnly] [SerializeField] private int CookedPotatoesCollected = 0;

    private List<PotatoCookTime> CookTimeList;

    private void Start() {
      foreach (var clip in AmbienceClips)
        MicrogameController.instance.playSFX(clip.clip, volume: clip.volume);
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

    public float GetCookTime(){
      if (CookTimeList == null){
        CookTimeList = new List<PotatoCookTime>(PotatoCookTimes);
      }
      var index = Random.Range(0, CookTimeList.Count);
      var time = CookTimeList[index].GetTime;
      CookTimeList.RemoveAt(index);
      return time;
    }

    //---   Editor Script
    public void UpdatePotatoes() {
      PotatoesExists = GameObject.Find("Potatoes").transform.childCount;

      var temp = new PotatoCookTime[PotatoesExists];
      System.Array.Copy(PotatoCookTimes, temp, Mathf.Min(PotatoCookTimes.Length, PotatoesExists));
      PotatoCookTimes = temp;
    }
  }

}