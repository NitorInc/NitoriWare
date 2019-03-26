using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindController : MonoBehaviour {
    enum GameState {
        InitState
    };

    [Header("List of all drawers")]
    [SerializeField]
    private GameObject[] drawerObjects;

    [Header("List of all items")]
    [SerializeField]
    private GameObject[] itemObjects;


    private List<CheeseFindDrawer> _drawerScripts;

    private GameState _currentState = GameState.InitState;

    /*
        mouseObject.SetActive(false);
        cheeseObject.transform.position += new Vector3(0f, 2f, 0f);
     */

	void Start () {
        _drawerScripts = new List<CheeseFindDrawer>();
		foreach(GameObject drawerObject in drawerObjects) {
            CheeseFindDrawer drawer = drawerObject.GetComponent<CheeseFindDrawer>();
            _drawerScripts.Add(drawer);

            drawer.SetController(this);
        }
        //TODO: Items animation.
        //TODO: Call this only after the items animation is completed. Then, after the drawers are closed, let the player interact with them.
        CloseAllDrawers();
	}

    void CloseAllDrawers() {
		foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.CloseDrawer();
        }
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
