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
    [SerializeField] private AudioClip clip;
    public AudioClip Clip { get { return clip; } }

    [Range(0.0f, 1.0f)]
    [SerializeField] private float volume;
    public float Volume { get { return volume; } }

    public AmbienceClip(AudioClip _clip, float _volume) {
      clip = _clip;
      volume = _volume;
    }
  }

  public class FoodRoast_Controller : MonoBehaviour {
    public static FoodRoast_Controller _instance = null;
    public static FoodRoast_Controller Instance{
      get{
        // Unefficient, but we can't use script execution order.
        if (_instance == null){
          _instance = GameObject.Find("Controller").GetComponent<FoodRoast_Controller>();
        }
        return _instance;
      }
    }

    [Header("Ambience")]
    [SerializeField] private AmbienceClip[] ambienceClips; 

    [Header("Difficulty")]
    [ReadOnly] [SerializeField] private int potatoesExists = 0;
    [SerializeField] private int cookedPotatoesRequirement = 1;
    [SerializeField] private PotatoCookTime[] potatoCookTimes;
    [SerializeField] private float potatoBurnTime;
    public float GetPotatoBurnTime { get { return potatoBurnTime; } }

    [Header("Collected Potatoes")]
    [ReadOnly] [SerializeField] private int uncookedPotatoesCollected = 0;
    [ReadOnly] [SerializeField] private int cookedPotatoesCollected = 0;

    private List<PotatoCookTime> cookTimeList;
    private List<int> randomAnimationStates;

        private void Awake()
        {
            _instance = this;
        }

        private void Start() {
      foreach (var clip in ambienceClips)
        MicrogameController.instance.playSFX(clip.Clip, volume: clip.Volume);
    }

    public void AddCookedPotato() {
      cookedPotatoesCollected++;
      if (cookedPotatoesCollected >= cookedPotatoesRequirement) {
        MicrogameController.instance.setVictory(true);
      }
    }

    public void AddUncookedPotato(){
      uncookedPotatoesCollected++;
      if (uncookedPotatoesCollected > potatoesExists - cookedPotatoesRequirement){
        MicrogameController.instance.setVictory(false);
      }
    }

    public float GetCookTime(){
      if (cookTimeList == null){
        cookTimeList = new List<PotatoCookTime>(potatoCookTimes);
      }
      if (cookTimeList.Count == 0){
        Debug.LogError("cookTimeList empty! Did you forget to 'Update Potatoes' or did you call GetCookTime() too many times?");
      }

      var index = Random.Range(0, cookTimeList.Count);
      var time = cookTimeList[index].GetTime;
      cookTimeList.RemoveAt(index);
      return time;
    }

    //---   Editor Script
    public void UpdatePotatoes() {
      potatoesExists = GameObject.Find("Potatoes").transform.childCount;

      var oldArray = potatoCookTimes ?? new PotatoCookTime[0];
      var newArray = new PotatoCookTime[potatoesExists];
      System.Array.Copy(oldArray, newArray, Mathf.Min(oldArray.Length, newArray.Length));
      potatoCookTimes = newArray;
    }
  }

}