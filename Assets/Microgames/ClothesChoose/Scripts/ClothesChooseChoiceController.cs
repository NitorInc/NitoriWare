using UnityEngine;

public class ClothesChooseChoiceController : MonoBehaviour
{
    public ClothesChooseChoice choiceTemplate;
    public ClothesChooseDoll mannequin;
    public ClothesChooseDoll medicine;

    public Animator[] endAnimators;

    public SpriteRenderer highlight;
    public float highlightYOffset;
    public float highlightMoveDuration;

    public AudioClip chooseClip;
    public AudioClip victoryClip;
    public AudioClip lossClip;
    public float choosePitchIncrease = .2f;
    public float victoryClipDelay = .2f;

    Animator animator;

    int currentCategory;
    bool moveHighlight;
    float highlightMoveStartTime;

    void Start()
    {
        animator = GetComponent<Animator>();

        LoadCategory(currentCategory);

        // Plant highlight bar
        highlight.transform.SetParent(medicine.transform);
        float startY = medicine.GetCategoryHighlightPosition(currentCategory).y;
        highlight.transform.localPosition = new Vector2(
            highlight.transform.localPosition.x, startY + highlightYOffset);
    }

    void Update()
    {
        if (moveHighlight)
        {
            float targetY = medicine.GetCategoryHighlightPosition(currentCategory).y;
            float moveAmount = (Time.time - highlightMoveStartTime) / highlightMoveDuration;
            
            if (moveAmount > 1)
            {
                moveAmount = 1;
                moveHighlight = false;
            }
            
            float newY = Mathf.Lerp(highlight.transform.localPosition.y, targetY, moveAmount);
            highlight.transform.localPosition = new Vector2(
                highlight.transform.localPosition.x, newY + highlightYOffset);
        }
    }
    
    void LoadCategory(int i)
    {
        ClothesChooseChoice choice = Instantiate(choiceTemplate, medicine.GetCategoryTransform(i));
        
        // Set the correct choice
        choice.SetCorrect(
            mannequin.ClothingChoices[i].chosen,
            medicine.GetCategorySortingOrder(i),
            Correct);
        
        // Get an alternative at random
        int alternativeIndex =
            UnityEngine.Random.Range(0, mannequin.ClothingChoices[i].alternatives.Count);
        choice.SetIncorrect(
            mannequin.ClothingChoices[i].alternatives[alternativeIndex],
            medicine.GetCategorySortingOrder(i),
            Incorrect);
    }

    public void Correct()
    {
        int newCategory = currentCategory + 1;
        MicrogameController.instance.playSFX(chooseClip, pitchMult: 1f + ((newCategory - 1f) * choosePitchIncrease));
        if (newCategory < mannequin.ClothingChoices.Length)
        {
            currentCategory = newCategory;
            LoadCategory(currentCategory);
            highlightMoveStartTime = Time.time;
            moveHighlight = true;
        }
        else
        {
            // Win
            animator.SetTrigger("Fade");
            foreach (Animator animator in endAnimators)
            {
                animator.SetTrigger("Win");
            }

            MicrogameController.instance.setVictory(true, true);
            Invoke("playVictoryClip", victoryClipDelay);
        }
    }

    void playVictoryClip()
    {
        MicrogameController.instance?.playSFX(victoryClip);
    }
    
    public void Incorrect()
    {
        // Lose
        animator.SetTrigger("Fade");
        foreach (Animator animator in endAnimators)
        {
            animator.SetTrigger("Lose");
        }

        MicrogameController.instance.setVictory(false, true);
        MicrogameController.instance.playSFX(lossClip);
    }
}
