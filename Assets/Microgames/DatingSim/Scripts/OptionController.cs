using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour {
    
    public GameObject choiceMenu;
    public TinyMaterial tinyMaterial;
    public AudioClip cursorMoveClip;

    public delegate void OnSelection();
    public static event OnSelection OnWinning;
    public static event OnSelection OnLosing;

    public class OptionSet {
        public OptionSet(string option, string response, bool isWin) {
            optionText = option;
            responseText = response;
            this.isWin = isWin;
        }
        public string optionText = "";
        public string responseText = "";
        public bool isWin = true;
    }

    public GameObject optionLineProto;
    public Transform startPosition;
    public float optionBoxHeight = 2.3f;
    float totalOptions = 4.0f;
    float distancePerUnit {
        get {
            return optionBoxHeight / (totalOptions - 1.0f);
        }
    }
    List<OptionSet> sets;
    List<OptionLine> lines = new List<OptionLine>();
    int currentOption = 0;

    // total number of lines in the entire character data set
    public int totalWinLines = 2;
    // needed number of lines in the game
    public int neededWinLines = 2;
    public int totalLoseLines = 2;
    public int neededLoseLines = 2;


    bool enableUserControl = true;

    private void Awake()
    {
        OnWinning = null;
        OnLosing = null;
    }

    // Use this for initialization
    void Start () {
        DialoguePreset.OnCharacterSelection += InitializeOptions;
	}
	
	// Update is called once per frame
	void Update () {
        if (enableUserControl) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                lines[currentOption].ShowCursor(false);
                currentOption--;
                if (currentOption < 0)
                    currentOption = sets.Count - 1;
                lines[currentOption].ShowCursor(true);

                MicrogameController.instance.playSFX(cursorMoveClip);
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                lines[currentOption].ShowCursor(false);
                currentOption++;
                if (currentOption >= sets.Count)
                    currentOption = 0;
                lines[currentOption].ShowCursor(true);
                
                MicrogameController.instance.playSFX(cursorMoveClip);
            } else if (Input.GetKeyDown(KeyCode.Space)) {

                enableUserControl = false;

                //for (int i = 0; i < sets.Count; i++) {
                //    if (i != currentOption) {
                //        lines[currentOption].ShowText(false);
                //    }
                //}

                var dialogueCtrl = FindObjectOfType<DialogueController>();
                dialogueCtrl.SetDialogue(sets[currentOption].responseText);

                if (sets[currentOption].isWin) {
                    MicrogameController.instance.setVictory(true);
                    OnWinning();
                } else {
                    MicrogameController.instance.setVictory(false);
                    OnLosing();
                }
            }
        }
	}

    void InitializeOptions(int charIndex) {
        sets = new List<OptionSet>();
        var preset = FindObjectOfType<DialoguePreset>();
        var winList = SelectAtRandom(neededWinLines, totalWinLines);
        for (int i = 0; i < winList.Count; i++) {
            string optionText = preset.GetOption(charIndex, true, true, i);
            string responseText = preset.GetOption(charIndex, true, false, i);
            sets.Add(new OptionSet(optionText, responseText, true));
        }

        var loseList = SelectAtRandom(neededLoseLines, totalLoseLines);
        for (int i = 0; i < loseList.Count; i++) {
            string optionText = preset.GetOption(charIndex, false, true, i);
            string responseText = preset.GetOption(charIndex, false, false, i);
            sets.Add(new OptionSet(optionText, responseText, false));
        }

        tinyMaterial.ChangeMaterial(charIndex);

        enableUserControl = false;
        //Invoke("ShowOptions", optionDelay);
    }

    List<int> SelectAtRandom(int amount, int total) {
        List<int> result = new List<int>();
        for (int i = 0; i < total; i++) {
            result.Add(i);
        }
        result.Shuffle();
        int count = total - amount;
        if (count < 0)
            count = 0;
        if (amount > total)
            amount = total;
        result.RemoveRange(amount, count);
        return result;
    }

    public void InvokeOptions(float time)
    {
        if (MicrogameController.instance.getVictoryDetermined())
            return;

        Invoke("ShowMenu", time);
    }

    void ShowMenu()
    {
        choiceMenu.SetActive(true);
    }

    public void ShowOptions()
    {
        sets.Shuffle();
        currentOption = 0;
        for (int i = 0; i < sets.Count; i++) {
            var pos = startPosition.position;
            pos.y = startPosition.position.y - i * distancePerUnit;
            var go = Instantiate(optionLineProto, pos, Quaternion.identity) as GameObject;
            go.transform.parent = choiceMenu.transform;
            lines.Add(go.GetComponent<OptionLine>());
            lines[i].SetText(sets[i].optionText);
            if (i > 0) {
                lines[i].ShowCursor(false);
            }
            else {
                lines[i].ShowCursor(true);
            }
        }

        enableUserControl = true;

    }
}