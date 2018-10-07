using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandCard : MonoBehaviour {
    [Header("Updated by scripts. Changing here does nothing.")]
    public int value;

    bool active;
    
    public bool IsSelected() {
        return active;
    }

    public bool Select() {
        if (!active) {
            transform.position += new Vector3(0, 0.2f, 0);
            active = true;
            return true;
        }
        return false;
    }

	void Start () {
	}
	
	void Update () {
    }
}
