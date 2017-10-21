using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaJizouController : MonoBehaviour {

    public delegate void Action();
    public static event Action onVictory;

    public int requiredJizou = 3;
    int totalJizou {
        get {
            return spawnLocations.Count;
        }
    }
    int successCounter = 0;

    public List<Transform> spawnLocations;

    public GameObject[] specialJizouProtos;
    public GameObject normalJizouProto;
    List<GameObject> jizouList;
	// Use this for initialization
	void Start () {

        spawnLocations.Shuffle();

        jizouList = new List<GameObject>();
        GameObject go = null;
        for (int i = 0; i < totalJizou; i++) {
            if (i <= (totalJizou - requiredJizou)) {
                go = Instantiate(specialJizouProtos[i]);
                go.GetComponent<MarisaJizouJizou>().Register(this);
            }
            else {
                go = Instantiate(normalJizouProto);
            }
            jizouList.Add(go);
            go.transform.position = spawnLocations[i].position;
        }
	}

    public void Notify(bool succeed) {
        if (succeed) {
            successCounter++;
            if (successCounter >= requiredJizou) {
                MicrogameController.instance.setVictory(true, true);
            }
        }
        else {
            MicrogameController.instance.setVictory(false, true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

public static class ListExtension {
    
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
