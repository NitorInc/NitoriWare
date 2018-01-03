using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserMamizou : MonoBehaviour {
    [Header("Sprite when player chooses right")]
    public GameObject choseRightSprite;

    [Header("Sprite when player chooses wrong")]
    public GameObject choseWrongSprite;

    // Change Mamizou's look - player chose right (Mamizou)
    public void ChoseRight()
    {
        if (choseRightSprite)
            choseRightSprite.SetActive(true);
        if (choseWrongSprite)
            choseWrongSprite.SetActive(false);
    }

    // Change Mamizou's look - player chose wrong (not Mamizou)
    public void ChoseWrong()
    {
        if (choseRightSprite)
            choseRightSprite.SetActive(false);
        if (choseWrongSprite)
            choseWrongSprite.SetActive(true);
    }
}
