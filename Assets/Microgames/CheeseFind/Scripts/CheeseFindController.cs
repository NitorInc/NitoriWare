using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindController : MonoBehaviour {
    enum GameState {
        InitState
    };

    [Header("Camera")]
    [SerializeField]
    private Camera camera;

    [Header("List of all drawers")]
    [SerializeField]
    private GameObject[] drawerObjects;

    [Header("List of all items")]
    [SerializeField]
    private GameObject[] itemObjects;

    private CheeseFindCamera _cameraScript;

    private CheeseFindDrawer[] _drawerScripts;
    private CheeseFindItem[] _itemScripts;

    private GameState _currentState = GameState.InitState;

    /*
        mouseObject.SetActive(false);
        cheeseObject.transform.position += new Vector3(0f, 2f, 0f);
     */

	void Start () {
        _drawerScripts = new CheeseFindDrawer[drawerObjects.Length];
        _itemScripts = new CheeseFindItem[itemObjects.Length];

		for(int i = 0; i < drawerObjects.Length; i ++) {
            CheeseFindDrawer drawerScript = drawerObjects[i].GetComponent<CheeseFindDrawer>();
            _drawerScripts[i] = drawerScript;

            drawerScript.controller = this;
        }

		for(int i = 0; i < itemObjects.Length; i ++) {
            CheeseFindItem itemScript = itemObjects[i].GetComponent<CheeseFindItem>();
            _itemScripts[i] = itemScript;
        }

        _cameraScript = camera.GetComponent<CheeseFindCamera>();

        StartCoroutine(HideItem());
	}

    private IEnumerator HideItem() {
        //TODO: Items animation.
        
		yield return new WaitForSeconds(1f);
        foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.isOpen = false;
        }

        _cameraScript.MoveCameraDown();

		yield return new WaitForSeconds(1f);

        //TODO: Unlock drawers here.
        foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.isLocked = false;
        }

        //TODO: Replace with displayLocalizedCommand when the text is localized.
		MicrogameController.instance.displayCommand("Find!");


    }

    void CloseAllDrawers() {
		
        //Invoke("StartGame", 1f);
    }

    void StartGame() {
        foreach(CheeseFindDrawer drawer in _drawerScripts) {
        //    drawer.UnlockDrawer();
        }
    }
	
	void Update () {
		switch(_currentState) {
        case GameState.InitState:
            break;
        default:
            break;
        }
	}

    //MicrogameController.instance.setVictory(victory: true, final: true);
}
