using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Number of cheese to find")]
    [SerializeField]
    private int maxItemsToFind;

    private CheeseFindCamera _cameraScript;

    private CheeseFindDrawer[] _drawerScripts;
    private CheeseFindItem[] _itemScripts;

    private GameState _currentState = GameState.InitState;

    private int currentItemsFound = 0;

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

        List<CheeseFindDrawer> drawers = _drawerScripts.ToList();
        List<CheeseFindItem> items = _itemScripts.ToList();

        for(int i = 0; i < maxItemsToFind; i ++) {
            int drawerIndex = Random.Range(0, drawers.Count);
            int itemIndex = Random.Range(0, items.Count);

            drawers[drawerIndex].item = items[itemIndex];
            items[itemIndex].isUsed = true;

            drawers.RemoveAt(drawerIndex);
            items.RemoveAt(itemIndex);
        }

		yield return new WaitForSeconds(1f);

        for(int i = 0; i < _itemScripts.Length; i ++) {
            if(_itemScripts[i].isUsed) {
                _itemScripts[i].MoveTo(_itemScripts[i].drawerPosition, 1f);
                continue;
            }
            _itemScripts[i].MoveAway(1f);
        }
		yield return new WaitForSeconds(1f);
        
        foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.isOpen = false;
        }
        _cameraScript.MoveCameraDown();
		yield return new WaitForSeconds(1f);

        foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.isLocked = false;
        }
        //TODO: Replace with displayLocalizedCommand when the text is localized.
		MicrogameController.instance.displayCommand("Find!");
    }

    public void SetVictory(bool isVictorious) {
        foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.isLocked = true;
        }
        MicrogameController.instance.setVictory(isVictorious, true);
    }

    public void AddPoint(int points) {
        currentItemsFound += points;
        if(currentItemsFound >= maxItemsToFind) {
            currentItemsFound = maxItemsToFind;
            SetVictory(true);
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
}
