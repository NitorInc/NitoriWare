using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheeseFindController : MonoBehaviour {
    [Header("Camera")]
    [SerializeField]
    private Camera cameraObject;

    [Header("Chest Prefab")]
    [SerializeField]
    private GameObject chestPrefab;

    [Header("Item Prefab")]
    [SerializeField]
    private GameObject itemPrefab;

    [Header("Nazrin")]
    [SerializeField]
    private GameObject nazrinObject;

    [Header("Number of cheese")]
    [SerializeField]
    private int cheeseCount;

    [Header("Number of mice")]
    [SerializeField]
    private int miceCount;

    [Header("Number of drawers")]
    [SerializeField]
    private int drawersCount;

    [Header("Score required")]
    [SerializeField]
    private int scoreRequired;

    [Header("Difficulty (0-2)")]
    [SerializeField]
    private int difficulty;

    [Header("Background")]
    [SerializeField]
    private Transform background;

    [SerializeField]
    private AudioClip victoryClip;
    [SerializeField]
    private AudioClip failureClip;

    private CheeseFindCamera _cameraScript;
    private CheeseFindNazrin _nazrinObject;
    private CheeseFindDrawer[] _drawerScripts;
    private CheeseFindItem[] _miceScripts;
    private CheeseFindItem[] _cheeseScripts;
    private CheeseFindItem[] _itemsScripts;

    private int currentItemsFound = 0;

	void Start () {
        switch(difficulty) {
        case 0:
            background.Find("Bg1").gameObject.SetActive(true);
            break;
        case 1:
            background.Find("Bg2").gameObject.SetActive(true);
            break;
        case 2:
            background.Find("Bg3").gameObject.SetActive(true);
            break;
        default:
            break;
        }

        int itemsCount = cheeseCount + miceCount;
        _itemsScripts = new CheeseFindItem[cheeseCount + miceCount];

        //Cheese
        _cheeseScripts = new CheeseFindItem[cheeseCount];
        for(int i = 0; i < cheeseCount; i ++) {
            GameObject cheese = GameObject.Instantiate(itemPrefab);
            CheeseFindItem cheeseScript = cheese.GetComponent<CheeseFindItem>();
            cheeseScript.isCheese = true;
            _cheeseScripts[i] = cheeseScript;
            _itemsScripts[i] = cheeseScript;
        }

        //Mice
        _miceScripts = new CheeseFindItem[miceCount];
        for(int i = 0; i < miceCount; i ++) {
            GameObject mouse = GameObject.Instantiate(itemPrefab);
            CheeseFindItem mouseScript = mouse.GetComponent<CheeseFindItem>();
            mouseScript.isCheese = false;
            _miceScripts[i] = mouseScript;
            _itemsScripts[cheeseCount + i] = mouseScript;
        }

        //Shuffle items order
        for(int i = 0; i < itemsCount; i ++) {
            int itemIndex = Random.Range(0, itemsCount);
            CheeseFindItem tmp = _itemsScripts[itemIndex];
            _itemsScripts[itemIndex] = _itemsScripts[i];
            _itemsScripts[i] = tmp;
        }

        for(int i = 0; i < itemsCount; i ++) {
            _itemsScripts[i].angleFactor = (float)i / (float)itemsCount;
        }

        //Drawers
        _drawerScripts = new CheeseFindDrawer[drawersCount];
        for(int i = 0; i < drawersCount; i ++) {
            GameObject chest = GameObject.Instantiate(chestPrefab);
            float angle = 2f * Mathf.PI * ((float)i / (float)drawersCount);
            chest.transform.position = new Vector3(Mathf.Cos(angle) * 4.2f, Mathf.Sin(angle) * 3.2f, 0f);

            GameObject drawer = chest.transform.Find("Drawer").gameObject;
            CheeseFindDrawer drawerScript = drawer.GetComponent<CheeseFindDrawer>();
            _drawerScripts[i] = drawerScript;
            drawerScript.controller = this;
        }

        _cameraScript = cameraObject.GetComponent<CheeseFindCamera>();
        _nazrinObject = nazrinObject.GetComponent<CheeseFindNazrin>();

        StartCoroutine(HideItem());
	}

    private IEnumerator HideItem() {
        List<CheeseFindDrawer> drawers = _drawerScripts.ToList();
        List<CheeseFindItem> cheese = _cheeseScripts.ToList();

        for(int i = 0; i < scoreRequired; i ++) {
            int drawerIndex = Random.Range(0, drawers.Count);
            int cheeseIndex = Random.Range(0, cheese.Count);

            drawers[drawerIndex].item = cheese[cheeseIndex];
            cheese[cheeseIndex].isUsed = true;

            drawers.RemoveAt(drawerIndex);
            cheese.RemoveAt(cheeseIndex);
        }

        if(drawersCount > scoreRequired) {
            List<CheeseFindItem> mice = _miceScripts.ToList();
            int miceToUse = drawersCount - scoreRequired;
            if(miceToUse > mice.Count)
                miceToUse = mice.Count;

            for(int i = 0; i < miceToUse; i ++) {
                int drawerIndex = Random.Range(0, drawers.Count);
                int mouseIndex = Random.Range(0, mice.Count);

                drawers[drawerIndex].item = mice[mouseIndex];
                mice[mouseIndex].isUsed = true;

                drawers.RemoveAt(drawerIndex);
                mice.RemoveAt(mouseIndex);
            }
        }

		yield return new WaitForSeconds(1.5f - (difficulty * 0.2f));

        for(int i = 0; i < _miceScripts.Length; i ++) {
            if(_miceScripts[i].isUsed) {
                _miceScripts[i].MoveTo(_miceScripts[i].drawerPosition, 1f, true);
                continue;
            }
            _miceScripts[i].MoveAway(1.3f - (difficulty * 0.4f), false);
        }

        for(int i = 0; i < _cheeseScripts.Length; i ++) {
            if(_cheeseScripts[i].isUsed) {
                _cheeseScripts[i].MoveTo(_cheeseScripts[i].drawerPosition, 1f, true);
                continue;
            }
            _cheeseScripts[i].MoveAway(1.3f - (difficulty * 0.4f), false);
        }
		yield return new WaitForSeconds(1f - (difficulty * 0.25f));
        
        foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.isOpen = false;
            drawer.FrontSprite.enabled = true;
        }
        _cameraScript.MoveCameraDown();
		yield return new WaitForSeconds(1f);

        foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.isLocked = false;
        }
        //TODO: Replace with displayLocalizedCommand when the text is localized.
		MicrogameController.instance.displayCommand("Find all cheeses!");
		//MicrogameController.instance.displayLocalizedCommand("commandb", "Find all cheeses!");
    }

    public void SetVictory(bool isVictorious) {
        foreach(CheeseFindDrawer drawer in _drawerScripts) {
            drawer.isLocked = true;
        }
        _nazrinObject.Activate(isVictorious, scoreRequired);
        MicrogameController.instance.setVictory(isVictorious, true);
        MicrogameController.instance.playSFX(isVictorious ? victoryClip : failureClip);
    }

    public void AddPoint(int points) {
        currentItemsFound += points;
        if(currentItemsFound >= scoreRequired) {
            currentItemsFound = scoreRequired;
            SetVictory(true);
        }
    }
}
