using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerFolder : MonoBehaviour {

    [SerializeField]
    private GameObject File;

    [SerializeField]
    private Animation BGAnim;

    private void OnMouseDown()
    {
        BGAnim.Play();
        File.SetActive(true);
        this.enabled = false;

    }
}
