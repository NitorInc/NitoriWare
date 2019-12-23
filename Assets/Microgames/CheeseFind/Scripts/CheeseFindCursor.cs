using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindCursor : MonoBehaviour {
	[Header("Grabbing Sound")]
    [SerializeField]
    private AudioClip grabSfx;

	[Header("Releasing Sound")]
    [SerializeField]
    private AudioClip releaseSfx;

    private GameObject _defaultRig, _grabbingRig;

	void Start () {
        _defaultRig = transform.Find("Default").gameObject;
        _grabbingRig = transform.Find("Grabbing").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnGrab() {
		MicrogameController.instance.playSFX(grabSfx);
		_defaultRig.SetActive(false);
		_grabbingRig.SetActive(true);
	}

	public void OnRelease() {
		MicrogameController.instance.playSFX(releaseSfx);
		_defaultRig.SetActive(true);
		_grabbingRig.SetActive(false);
	}
}
