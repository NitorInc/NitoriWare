using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerFolder : MonoBehaviour {

    [SerializeField]
    private GameObject File1;

    [SerializeField]
    private GameObject File2;

    private void OnMouseDown()
    {
        File1.SetActive(true);
        File2.SetActive(true);
        gameObject.SetActive(false);

    }
}
