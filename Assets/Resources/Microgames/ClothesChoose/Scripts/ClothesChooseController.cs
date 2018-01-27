using UnityEngine;

public class ClothesChooseController : MonoBehaviour
{
    public ClothesChooseCarousel[] carousels;
    public ClothesChooseDoll mannequin;

    int currentCarousel;

    void Start()
    {
        carousels[0].Activate();
    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("space")) ||
            Event.current.Equals(Event.KeyboardEvent("down")))
        {
            SelectCarousel(1);
        }
        else if (Event.current.Equals(Event.KeyboardEvent("up")))
        {
            SelectCarousel(-1);
        }
    }

    void SelectCarousel(int offset)
    {
        int newCarousel = currentCarousel + offset;
        if (newCarousel >= 0 && newCarousel < carousels.Length)
        {
            currentCarousel = newCarousel;
            for (int i = 0; i < carousels.Length; i++)
            {
                if (i == currentCarousel)
                    carousels[i].Activate();
                else
                    carousels[i].Deactivate();
            }
        }
    }

    public void CheckWin()
    {
        bool win = true;

        int correctIndex = mannequin.GetOutfitIndex();
        foreach (ClothesChooseCarousel carousel in carousels)
        {
            int itemIndex = carousel.GetCurrentIndex();
            if (itemIndex != correctIndex)
            {
                win = false;
                break;
            }
        }

        if (win)
        {
            MicrogameController.instance.setVictory(true, true);
        }
    }
}
