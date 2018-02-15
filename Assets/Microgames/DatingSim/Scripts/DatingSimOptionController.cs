using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimOptionController : MonoBehaviour
{
    public int totalWinLines;
    public int totalLoseLines;

    public GameObject choiceMenu;
    public AudioClip cursorMoveClip;
    public DatingSimDialogueController dialogueController;

    public delegate void OnSelection();

    //public class OptionSet {
    //    public OptionSet(string option, string response, bool isWin) {
    //        optionText = option;
    //        responseText = response;
    //        this.isWin = isWin;
    //    }
    //    public string optionText = "";
    //    public string responseText = "";
    //    public bool isWin = true;
    //}

    public GameObject optionLineProto;
    public Transform startPosition;
    public float optionBoxHeight = 2.3f;
    float totalOptions = 4.0f;
    float distancePerUnit {
        get {
            return optionBoxHeight / (totalOptions - 1.0f);
        }
    }
    //List<OptionSet> sets;
    List<DatingSimOptionLine> lines = new List<DatingSimOptionLine>();
    int currentOption = 0;



    bool enableUserControl = true;
    
    void Start ()
    {
        enableUserControl = false;
    }

    //void InitializeOptions()
    //{
    //    //for (int i = 0; i < winList.Count; i++)
    //    //{
    //    //    string optionText = preset.GetOption(charIndex, true, true, i);
    //    //    string responseText = preset.GetOption(charIndex, true, false, i);
    //    //    sets.Add(new OptionSet(optionText, responseText, true));
    //    //}

    //    //var loseList = SelectAtRandom(neededLoseLines, totalLoseLines);
    //    //for (int i = 0; i < loseList.Count; i++)
    //    //{
    //    //    string optionText = preset.GetOption(charIndex, false, true, i);
    //    //    string responseText = preset.GetOption(charIndex, false, false, i);
    //    //    sets.Add(new OptionSet(optionText, responseText, false));
    //    //}

    //    enableUserControl = false;
    //}

    public void ShowOptions()
    {
        currentOption = 0;

        var optionPool = new List<DatingSimCharacters.CharacterOption>(DatingSimHelper.getSelectedCharacter().rightOptions);
        optionPool.Shuffle();
        for (int i = 0; i < totalWinLines; i++)
        {
            lines.Add(createLine(optionPool[i]));
        }

        optionPool = new List<DatingSimCharacters.CharacterOption>(DatingSimHelper.getSelectedCharacter().wrongOptions);
        optionPool.Shuffle();
        for (int i = 0; i < totalLoseLines; i++)
        {
            lines.Add(createLine(optionPool[i]));
        }

        lines.Shuffle();
        positionLines();
        lines[0].ShowCursor(true);
        enableUserControl = true;
    }

    DatingSimOptionLine createLine(DatingSimCharacters.CharacterOption option)
    {
        var newLine = (Instantiate(optionLineProto) as GameObject).GetComponent<DatingSimOptionLine>();
        newLine.transform.parent = choiceMenu.transform;
        newLine.initialize(option);

        return newLine;
    }

    void positionLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            var pos = startPosition.position;
            pos.y = startPosition.position.y - i * distancePerUnit;
            lines[i].transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (enableUserControl) {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                lines[currentOption].ShowCursor(false);
                currentOption--;
                if (currentOption < 0)
                    currentOption = lines.Count - 1;
                lines[currentOption].ShowCursor(true);

                MicrogameController.instance.playSFX(cursorMoveClip);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                lines[currentOption].ShowCursor(false);
                currentOption++;
                if (currentOption >= lines.Count)
                    currentOption = 0;
                lines[currentOption].ShowCursor(true);
                
                MicrogameController.instance.playSFX(cursorMoveClip);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {

                enableUserControl = false;

                //for (int i = 0; i < sets.Count; i++) {
                //    if (i != currentOption) {
                //        lines[currentOption].ShowText(false);
                //    }
                //}
                
                MicrogameController.instance.setVictory(lines[currentOption].isRight());
                dialogueController.resetDialogueSpeed();
                dialogueController.SetDialogue(lines[currentOption].getLocalizedResponse());
            }
        }
	}

    //List<int> SelectAtRandom(int amount, int total) {
    //    List<int> result = new List<int>();
    //    for (int i = 0; i < total; i++) {
    //        result.Add(i);
    //    }
    //    result.Shuffle();
    //    int count = total - amount;
    //    if (count < 0)
    //        count = 0;
    //    if (amount > total)
    //        amount = total;
    //    result.RemoveRange(amount, count);
    //    return result;
    //}

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
}