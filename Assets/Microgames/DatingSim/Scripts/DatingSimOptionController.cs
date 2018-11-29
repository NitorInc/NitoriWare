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

    private void Awake()
    {
        optionBoxHeight *= boxHeightMult;
        menuExpander.TargetYScale *= boxHeightMult;
        menuExpander.ScaleSpeed *= boxHeightMult;
        materialAnimation.setMaterialYScaleMult(boxHeightMult);
    }

    void Start ()
    {
        enableUserControl = false;
    }

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

                MicrogameController.instance.setVictory(lines[currentOption].isRight());
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