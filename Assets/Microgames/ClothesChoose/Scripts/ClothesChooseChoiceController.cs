using UnityEngine;

public class ClothesChooseChoiceController : MonoBehaviour
{
    public ClothesChooseChoice choiceTemplate;
    public ClothesChooseDoll mannequin;
    public ClothesChooseDoll medicine;

    public Animator[] endAnimators;

    public SpriteRenderer highlight;
    public float highlightMoveDuration;

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
        float startY = medicine.GetCategoryTransform(currentCategory).localPosition.y;
        highlight.transform.localPosition = new Vector2(highlight.transform.localPosition.x, startY);
    }

    void Update()
    {
        if (moveHighlight)
        {
            float targetY = medicine.GetCategoryTransform(currentCategory).localPosition.y;
            float moveAmount = (Time.time - highlightMoveStartTime) / highlightMoveDuration;
            
            if (moveAmount > 1)
            {
                moveAmount = 1;
                moveHighlight = false;
            }
            
            float newY = Mathf.Lerp(highlight.transform.localPosition.y, targetY, moveAmount);
            highlight.transform.localPosition = new Vector2(highlight.transform.localPosition.x, newY);
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
        }
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
    }
}
