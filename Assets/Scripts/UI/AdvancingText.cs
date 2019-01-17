using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class AdvancingText : MonoBehaviour
{
    
    [SerializeField]
    private float advanceSpeed;

    [SerializeField]
    private UnityEvent onComplete;

    private bool isComplete;
    public bool IsComplete => isComplete;

    private TMP_Text textMeshProComponent;
    private float progress;
    public float Progress
    {
        get { return progress; }
        set { progress = value; updateChars(); }
    }
    

    void Awake()
    {
        textMeshProComponent = GetComponent<TMP_Text>();
    }

    void Start ()
    {
        resetAdvance();
	}

    public void resetAdvance()
    {
        isComplete = false;
        progress = 0f;
        setVisibleChars(0);
    }

    void Update ()
    {
        if (progress < getTotalVisibleChars(false))
            updateText();
	}

    void updateText()
    {
        var totalVisibleChars = getTotalVisibleChars(false);
        progress = Mathf.MoveTowards(progress, totalVisibleChars, Time.deltaTime * advanceSpeed);
        updateChars();
    }

    void updateChars()
    {
        setVisibleChars((int)Mathf.Floor(progress));
        if (progress >= getTotalVisibleChars(false))
        {
            isComplete = true;
            onComplete.Invoke();
        }
    }

    public float getAdvanceSpeed()
    {
        return advanceSpeed;
    }

    public void setAdvanceSpeed(float speed)
    {
        advanceSpeed = speed;
    }

    void setVisibleChars(int amount)
    {
        textMeshProComponent.maxVisibleCharacters = amount;
    }

    public int getVisibleChars()
    {
        return textMeshProComponent.maxVisibleCharacters;
    }
    

    public int getTotalVisibleChars(bool forceMeshUpdate = true)
    {
        if (forceMeshUpdate)
            textMeshProComponent.ForceMeshUpdate();
        return textMeshProComponent.GetParsedText().Length;
    }
}
