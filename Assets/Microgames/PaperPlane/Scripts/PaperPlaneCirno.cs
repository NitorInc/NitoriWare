using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneCirno : MonoBehaviour {

    [SerializeField]
    float delay = 1f;
    [SerializeField]
    Sprite[] cirnos;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    SpriteRenderer plane;
    int cirnoIndex;

    void Start()
    {
        spriteRenderer.sprite = cirnos[0];
        Invoke("startThrow", delay);
    }

    void startThrow()
    {
        plane.gameObject.SetActive(false);
        StartCoroutine(throwPlane());
    }

    IEnumerator throwPlane()
    {
        spriteRenderer.sprite = cirnos[1];
        yield return null; //new WaitForSeconds(0.05f);
        spriteRenderer.sprite = cirnos[2];
    }
}
