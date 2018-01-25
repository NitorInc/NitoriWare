using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPracticeMicrogame : MonoBehaviour
{

#pragma warning disable 0649	
    [SerializeField]
    private MenuButton menuButton;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text[] creditsTexts;
    [SerializeField]
    private string[] creditsKeys;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Vector3 scaleAtCenter;
    [SerializeField]
    private float timeToCenter;
#pragma warning restore 0649

    private static List<MicrogameCollection.Microgame> microgamePool;
    private static MenuPracticeMicrogame selectedInstance;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private int initialSiblingIndex;
    
    private MicrogameCollection.Microgame microgame;

	void Start()
	{
        selectedInstance = null;
        if (microgamePool == null)
            microgamePool = GameController.instance.microgameCollection.getCollectionMicrogames(MicrogameCollection.Restriction.StageReady);

        if (name.Contains("Boss"))
        {
            microgame = GameController.instance.microgameCollection.getCollectionBossMicrogames()[0];
        }
        else
        {
            int microgameNumber = int.Parse(name.Split('(')[1].Split(')')[0]);
            if (microgameNumber >= microgamePool.Count)
            {
                gameObject.SetActive(false);
                return;
            }
            else
                microgame = microgamePool[microgameNumber];
        }
        
        initialScale = transform.localScale;
        initialPosition = transform.localPosition;
        initialSiblingIndex = transform.GetSiblingIndex();

        Sprite iconSprite = microgame.menuIcon;
        if (iconSprite != null)
            icon.sprite = iconSprite;
	}
	
	void LateUpdate()
    {
        if (selectedInstance == this)   //Move towards/away from center when selected
        {
            if (GameMenu.shifting)
            {
                
                float moveSpeed = ((Vector2)initialPosition).magnitude / timeToCenter;
                if (GameMenu.subMenu == GameMenu.SubMenu.PracticeSelect)    //Moving towards center
                {
                    if (transform.moveTowardsLocal2D(Vector2.zero, moveSpeed))
                    {
                        GameMenu.shifting = false;
                    }
                }
                else                                                        //Moving away from center
                {
                    if (transform.moveTowardsLocal2D(initialPosition, moveSpeed))
                    {
                        transform.SetSiblingIndex(initialSiblingIndex);
                        GameMenu.shifting = false;
                        selectedInstance = null;
                    }
                }
                transform.localScale = Vector3.Lerp(scaleAtCenter, initialScale,
                    ((Vector2)transform.localPosition).magnitude / ((Vector2)initialPosition).magnitude);
            }
        }
        else if (GameMenu.shifting) //Stay constant scale when shifting to/from practice menu
        {
            float mult = 1f / transform.parent.localScale.x;
            transform.localScale = initialScale * mult;

            transform.localPosition = transform.parent.localScale.x <= .011f ? Vector3.zero : initialPosition;
        }
        else
            transform.localScale = initialScale;

        menuButton.forceDisable = selectedInstance != null; //Disable if something is selected
    }

    public void select()
    {
        selectedInstance = this;
        transform.SetAsLastSibling();
        MicrogameStage.microgameId = microgame.microgameId;

        nameText.text = TextHelper.getLocalizedText("microgame." + microgame.microgameId + ".igname", microgame.microgameId);
        for (int i = 0; i < creditsTexts.Length; i++)
        {
            string creditsString = creditsTexts[i].text;
            creditsString= TextHelper.getLocalizedText(creditsKeys[i], creditsString);
            creditsString = string.Format(creditsString, microgame.difficultyTraits[0].credits[i]);
            creditsTexts[i].text = creditsString;
        }

    }
}
