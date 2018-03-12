using System.Collections.Generic;
using UnityEngine;

public class ClothesChooseDoll : MonoBehaviour
{
    [System.Serializable]
    public struct Category
    {
        public SpriteRenderer spriteRenderer;
        public Sprite[] clothes;
    }
    
    public struct ClothingChoice
    {
        public Sprite chosen;
        public List<Sprite> alternatives;
    }

    public ClothingChoice[] ClothingChoices
    {
        get { return clothingChoices; }
    }

    public Category[] categories;

    ClothingChoice[] clothingChoices;

    void Awake()
    {
        ChooseOutfit();
    }

    void ChooseOutfit()
    {
        List<ClothingChoice> choices = new List<ClothingChoice>();
        
        for (int i = 0; i < categories.Length; i++)
        {
            Category category = categories[i];

            if (category.spriteRenderer.enabled)
            {
                List<Sprite> clothes = new List<Sprite>(category.clothes);

                // Choose a piece of clothing
                ClothingChoice choice = new ClothingChoice();
                int choiceIndex = UnityEngine.Random.Range(0, clothes.Count);
                choice.chosen = clothes[choiceIndex];

                // Store alternatives
                clothes.RemoveAt(choiceIndex);
                choice.alternatives = clothes;

                // Wear
                category.spriteRenderer.sprite = choice.chosen;

                choices.Add(choice);
            }
        }

        this.clothingChoices = choices.ToArray();
    }
    
    public Transform GetCategoryTransform(int i)
    {
        return categories[i].spriteRenderer.transform;
    }

    public int GetCategorySortingOrder(int i)
    {
        return categories[i].spriteRenderer.sortingOrder;
    }
}
