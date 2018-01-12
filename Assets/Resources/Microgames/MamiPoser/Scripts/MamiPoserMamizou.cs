using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserMamizou : MonoBehaviour {
    // This class is used for the true form Mamizou
    // The disguised form is handled by the MamiPoserCharacter class

    [Header("Sprite when player chooses right")]
    [SerializeField]
    private GameObject choseRightSprite;

    [Header("Sprite when player chooses wrong")]
    [SerializeField]
    private GameObject choseWrongSprite;

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
