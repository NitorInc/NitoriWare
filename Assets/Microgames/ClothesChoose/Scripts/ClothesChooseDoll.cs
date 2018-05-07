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

    public bool randomOutfit;

    public ClothingChoice[] ClothingChoices { get; private set; }

    public Category[] categories;

    void Awake()
    {
        ChooseOutfit();
    }

    void ChooseOutfit()
    {
        List<ClothingChoice> choices = new List<ClothingChoice>();
        
        int outfitIndex = 0;
        bool chosen = false;
        for (int i = 0; i < categories.Length; i++)
        {
            Category category = categories[i];

            if (category.spriteRenderer.enabled)
            {
                List<Sprite> clothes = new List<Sprite>(category.clothes);
                ClothingChoice choice = new ClothingChoice();

                // Choose a piece of clothing
                if (!chosen)
                {
                    outfitIndex = UnityEngine.Random.Range(0, clothes.Count);
                    if (!randomOutfit)
                        chosen = true;
                }
                choice.chosen = clothes[outfitIndex];

                // Store alternatives
                clothes.RemoveAt(outfitIndex);
                choice.alternatives = clothes;

                // Wear
                category.spriteRenderer.sprite = choice.chosen;

                choices.Add(choice);
            }
        }

        ClothingChoices = choices.ToArray();
    }
    
    public Transform GetCategoryTransform(int i)
    {
        return categories[i].spriteRenderer.transform;
    }

    public Vector2 GetCategoryHighlightPosition(int i)
    {
        var slot = categories[i].spriteRenderer.gameObject.GetComponent<ClothesChooseSlot>();
        Vector2 highlightPosition = new Vector2(
            slot.transform.localPosition.x,
            slot.transform.localPosition.y + slot.highlightOffset);

        return highlightPosition;
    }

    public int GetCategorySortingOrder(int i)
    {
        return categories[i].spriteRenderer.sortingOrder;
    }
}
