using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private Animator fsmAnimator;
    [SerializeField]
    private string scoreParameter = "Score";

    private TextMeshPro tmpComponent;
    private string prefix;

    void Awake()
    {
        tmpComponent = GetComponent<TextMeshPro>();
        var text = tmpComponent.text;
        prefix = text.Length > 3 ? text.Substring(0, text.Length - 3) : "";
    }

    public void UpdateText(bool subtractOne)
    {
        var score = fsmAnimator.GetInteger(scoreParameter);
        tmpComponent.text = prefix + (subtractOne ? score - 1 : score).ToString("D3");
    }
}
