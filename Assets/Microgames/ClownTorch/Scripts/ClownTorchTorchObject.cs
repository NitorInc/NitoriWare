using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownTorchTorchObject : MonoBehaviour {

    public GameObject fireEff;
    public ParticleSystem smokeEff;
    public ParticleSystem extinguishEff;
    public float requiredTime = 0.5f;
    float timer = 0.0f;

    bool countedThisFrame = false;
	// Use this for initialization
	void Start () {
        Debug.Log(GetComponent<ClownTorchTag>().type);
	}
	
	// Update is called once per frame
	void Update () {
        if (timer >= requiredTime) {
            fireEff.SetActive(true);
            smokeEff.Stop();
        }
	}

    public bool IsLit() {
        return fireEff.activeSelf;
    }

    public void TurnOn() {
        fireEff.SetActive(true);
    }

    public void TurnOff() {
        if (IsLit()) {
            fireEff.SetActive(false);
            timer = 0.0f;
            extinguishEff.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        var tag = col.GetComponentInParent<ClownTorchTag>().type;
        switch(tag) {
            case ClownTorchTag.Type.Pyre:
                if (GetComponent<ClownTorchTag>().type != ClownTorchTag.Type.ClownTorch)
                    smokeEff.Play();
                break;
            case ClownTorchTag.Type.Water:
                if (GetComponent<ClownTorchTag>().type != ClownTorchTag.Type.ClownTorch)
                    TurnOff();
                break;
            case ClownTorchTag.Type.PlayerTorch:
                var obj = col.GetComponentInParent<ClownTorchTorchObject>();
                if (obj.IsLit() && !IsLit()) {
                    smokeEff.Play();
                }
                break;
            case ClownTorchTag.Type.ClownTorch:
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        var tag = col.GetComponentInParent<ClownTorchTag>().type;
        switch (tag) {
            case ClownTorchTag.Type.Pyre:
                timer += Time.deltaTime;
                break;
            case ClownTorchTag.Type.Water:
                break;
            case ClownTorchTag.Type.PlayerTorch:
                var obj = col.GetComponentInParent<ClownTorchTorchObject>();
                if (obj.IsLit() && !this.IsLit()) {
                    timer += Time.deltaTime;
                }
                break;
            case ClownTorchTag.Type.ClownTorch:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        smokeEff.Stop();
    }
}
