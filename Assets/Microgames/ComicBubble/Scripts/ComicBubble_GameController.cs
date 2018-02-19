using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBubble_GameController : MonoBehaviour {

    [System.Serializable]
    class ComicBubbleDataCollection
    {
        public GameObject speechBubble;
        public GameObject speechTarget;
        public Vector2 finalPosition;

    }

    [SerializeField]
    List<ComicBubbleDataCollection> data;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

