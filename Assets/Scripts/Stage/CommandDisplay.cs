using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandDisplay : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private Animator animator;
#pragma warning restore 0649
	
	void Update()
	{
		
	}

    public void play(string command)
    {
        setText(command);
        animator.SetBool("play", true);
    }

    public void play()
    {
        play(getText());
    }

    public string getText()
    {
        return textComponent.text;
    }

    public void setText(string text)
    {
        textComponent.text = text;
    }
}
