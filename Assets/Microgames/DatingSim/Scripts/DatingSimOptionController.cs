using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DatingSimOptionController : MonoBehaviour
{
    [SerializeField]
    private DatingSimOptionData optionsData;

    public DifficultySetting[] difficultySettings;
    [System.Serializable]
    public class DifficultySetting
    {
        public int winOptionCount;
        public int lossOptionCount;
    }
    

    public GameObject choiceMenu;
    public AudioClip cursorMoveClip;
    public DatingSimDialogueController dialogueController;

    public delegate void OnSelection();

    [Header("Difficulty aesthetic changes")]
    public float boxHeightMultPerDifficultyUp = .2f;
    public float optionStartYPerDifficutlyUp = 0.3279009f;

    [Header("Edit this variable to adjust box height")]
    public float boxHeightMult = 1f;
    public float distancePerUnit;
    public GameObject optionLineProto;
    public Transform startPosition;
    public DatingSimMenuExpand menuExpander;
    public DatingSimMaterialAnimation materialAnimation;
    public float optionBoxHeight = 2.3f;
    List<DatingSimOptionLine> lines = new List<DatingSimOptionLine>();
    int currentOption = 0;

    bool enableUserControl = true;

    void Start ()
    {
        boxHeightMult += boxHeightMultPerDifficultyUp * (MicrogameController.instance.difficulty - 1);

        optionBoxHeight *= boxHeightMult;
        menuExpander.TargetYScale *= boxHeightMult;
        menuExpander.ScaleSpeed *= boxHeightMult;
        materialAnimation.setMaterialYScaleMult(boxHeightMult);

        enableUserControl = false;
    }

    public void ShowOptions()
    {
        currentOption = 0;
        
        var difficultySetting = difficultySettings[MicrogameController.instance.session.Difficulty - 1];

        var choiceIndexList = Enumerable.Range(0, optionsData.rightOptions.Length).ToList();
        choiceIndexList.Shuffle();
        for (int i = 0; i < difficultySetting.winOptionCount; i++)
        {
            lines.Add(createLine(choiceIndexList[i], true));
        }

        choiceIndexList = Enumerable.Range(0, optionsData.wrongOptions.Length).ToList();
        choiceIndexList.Shuffle();
        for (int i = 0; i < difficultySetting.lossOptionCount; i++)
        {
            lines.Add(createLine(choiceIndexList[i], false));
        }

        lines.Shuffle();
        positionLines();
        lines[0].ShowCursor(true);
        enableUserControl = true;
    }

    DatingSimOptionLine createLine(int optionIndex, bool isRight)
    {
        var newLine = (Instantiate(optionLineProto) as GameObject).GetComponent<DatingSimOptionLine>();
        newLine.transform.parent = choiceMenu.transform;
        newLine.initialize(optionsData, optionIndex, isRight);

        return newLine;
    }

    void positionLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            var pos = startPosition.position;
            pos.y = startPosition.position.y - i * distancePerUnit;
            pos.y += optionStartYPerDifficutlyUp * (MicrogameController.instance.difficulty - 1);
            lines[i].transform.position = pos;
        }
    }

    void LateUpdate ()
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

                MicrogameController.instance.setVictory(lines[currentOption].IsRight());
                dialogueController.resetDialogueSpeed();
                dialogueController.SetDialogue(lines[currentOption].getLocalizedResponse());
            }
        }
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
}