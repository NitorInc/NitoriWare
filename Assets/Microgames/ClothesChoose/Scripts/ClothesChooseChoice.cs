using System;
using System.Collections.Generic;
using UnityEngine;

public class ClothesChooseChoice : MonoBehaviour
{
    [System.Serializable]
    public struct Slot
    {
        public string direction;
        public SpriteRenderer spriteRenderer;
    }

    public Slot leftSlot;
    public Slot rightSlot;

    Animator animator;
    bool listen;

    int correct;
    Action correctAction;
    int incorrect;
    Action incorrectAction;

    Slot[] slots;

    Dictionary<string, bool> choiceMap;

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        // Shuffle
        correct = UnityEngine.Random.Range(0, 2);
        incorrect = 1 - correct;
        slots = new Slot[2] { leftSlot, rightSlot };

        choiceMap = new Dictionary<string, bool>();
        choiceMap[slots[correct].direction] = true;
        choiceMap[slots[incorrect].direction] = false;

        listen = true;
    }
    
    public void SetCorrect(Sprite sprite, int sortingOrder, Action correctAction)
    {
        slots[correct].spriteRenderer.sprite = sprite;
        slots[correct].spriteRenderer.sortingOrder = sortingOrder;
        this.correctAction = correctAction;
    }

    public void SetIncorrect(Sprite sprite, int sortingOrder, Action incorrectAction)
    {
        slots[incorrect].spriteRenderer.sprite = sprite;
        slots[incorrect].spriteRenderer.sortingOrder = sortingOrder;
        this.incorrectAction = incorrectAction;
    }

    void Update()
    {
        if (listen)
        {
            if (Input.GetKeyDown("right"))
            {
                animator.SetTrigger("ChooseRight");
                Choose("right");
            }
            else if (Input.GetKeyDown("left"))
            {
                animator.SetTrigger("ChooseLeft");
                Choose("left");
            }
        }
    }

    void Choose(string direction)
    {
        listen = false;

        if (choiceMap[direction])
        {
            correctAction();
        }
        else
        {
            incorrectAction();
        }
    }
}
