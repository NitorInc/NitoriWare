using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DatingSimCharacterNameText : MonoBehaviour
{
    TMP_Text textComp;

	void Start ()
    {
        textComp = GetComponent<TMP_Text>();
        //textComp.SetText(DatingSimHelper.getSelectedCharacter().getLocalizedDisplayName());
	}
}
