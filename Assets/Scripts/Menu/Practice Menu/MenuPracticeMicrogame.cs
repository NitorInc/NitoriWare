using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MenuPracticeMicrogame : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private MenuButton menuButton;
    [SerializeField]
    private Text nameText;
    public Text NameText { get { return nameText; } set { nameText = value; } }
    [SerializeField]
    private Text[] creditsTexts;
    public Text[] CreditsTexts { get { return creditsTexts; } set { creditsTexts = value; } }
    [SerializeField]
    private GameMenu rootMenu;
    public GameMenu RootMenu { get { return rootMenu; } set { rootMenu = value; } }
    [SerializeField]
    private string[] creditsKeys;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Vector3 centerPosition;
    [SerializeField]
    private Vector3 scaleAtCenter;
    [SerializeField]
    private float timeToCenter;
    [SerializeField]
    private Image outlineBack;
    [SerializeField]
    private Color outlineBackBossColor;
    [SerializeField]
    private Vector3 oulineBackBossScale;
#pragma warning restore 0649

    private static MenuPracticeMicrogame selectedInstance;
    public static MenuPracticeMicrogame SelectedInstance => selectedInstance;
    
    private Vector2 initialLocalPosition;
    private Vector3 initialScale;
    private int initialSiblingIndex;
    private float centerArrivalTime;
    
    public Microgame microgame { get; private set; }

    void Awake()
    {
        selectedInstance = null;

        int microgameNumber = int.Parse(name.Split('(')[1].Split(')')[0]);
        var isBoss = name.Contains("Boss");
        var microgamePool = !isBoss ?
            MenuPracticeMicrogameSpawner.standardMicrogamePool :
            MenuPracticeMicrogameSpawner.microgameBossPool;
        if (microgameNumber >= microgamePool.Count)
        {
            gameObject.SetActive(false);
            return;
        }
        else
            microgame = microgamePool[microgameNumber];
    }

	void Start()
    {
        int microgameNumber = int.Parse(name.Split('(')[1].Split(')')[0]);
        var isBoss = microgame.isBossMicrogame();

        initialScale = transform.localScale;
        initialLocalPosition = transform.localPosition;
        initialSiblingIndex = transform.GetSiblingIndex();

        Sprite iconSprite = Resources.Load<Sprite>($"MicrogameIcons/{microgame.microgameId}Icon");
        if (iconSprite != null)
            icon.sprite = iconSprite;

        if (isBoss)
        {
            outlineBack.color = outlineBackBossColor;
            outlineBack.transform.localScale = oulineBackBossScale;
        }

        var credits = microgame.credits;
        if (credits.Length < 3 || credits.FirstOrDefault(a => string.IsNullOrEmpty(a)) != null)
            Debug.LogWarning($"Microgame {microgame.microgameId} is missing credits field(s)!");
	}

    public void SetSprite(Sprite sprite)
    {
        if (sprite != null)
            icon.sprite = sprite;
    }
	
	void LateUpdate()
    {
        if (selectedInstance == this)   //Move towards/away from center when selected
        {
            if (GameMenu.shifting)
            {
                if (GameMenu.subMenu == GameMenu.SubMenu.PracticeSelect)    //Moving towards center
                {
                    var startWorldPosition = transform.parent.TransformPoint(initialLocalPosition);
                    var timeUntilCenter = Mathf.Max(centerArrivalTime - Time.time, 0f);
                    var moveSpeed = timeUntilCenter > 0f ?
                        ((Vector2)(transform.position - centerPosition)).magnitude / timeUntilCenter
                        : 0f;
                    //transform.moveTowards2D(centerPosition, moveSpeed);
                    transform.position = Vector3.Lerp(centerPosition, startWorldPosition, timeUntilCenter / timeToCenter);
                    if (timeUntilCenter <= 0f)
                    {
                        transform.position = centerPosition;
                        GameMenu.shifting = false;
                    }
                    transform.localScale = Vector3.Lerp(scaleAtCenter, initialScale, timeUntilCenter / timeToCenter);
                }
                else                                                        //Moving away from center
                {
                    var startWorldPosition = transform.parent.TransformPoint(initialLocalPosition);
                    float moveSpeed = ((Vector2)(startWorldPosition - centerPosition)).magnitude / timeToCenter;
                    if (transform.moveTowards2D(startWorldPosition, moveSpeed))
                    {
                        transform.position = startWorldPosition;
                        transform.SetSiblingIndex(initialSiblingIndex);
                        transform.localPosition = initialLocalPosition;
                        GameMenu.shifting = false;
                        selectedInstance = null;
                    }
                    transform.localScale = Vector3.Lerp(scaleAtCenter, initialScale,
                        ((Vector2)(transform.position - centerPosition)).magnitude
                        / ((Vector2)(startWorldPosition - centerPosition)).magnitude);
                }
            }
        }
        //else if (GameMenu.shifting) //Stay constant scale when shifting to/from practice menu
        //{
        //    float mult = 1f / transform.parent.localScale.x;
        //    transform.localScale = initialScale * mult;

        //    transform.localPosition = transform.parent.localScale.x <= .011f ? Vector3.zero : initialPosition;
        //}
        else
            transform.localScale = initialScale;

        menuButton.forceDisable = selectedInstance != null; //Disable if something is selected
    }

    public void select()
    {
        selectedInstance = this;
        rootMenu.shift((int)GameMenu.SubMenu.PracticeSelect);
        transform.SetAsLastSibling();
        MicrogameStage.microgameId = microgame.microgameId;
        centerArrivalTime = Time.time + timeToCenter;

        nameText.text = TextHelper.getLocalizedText("microgame." + microgame.microgameId + ".igname", SpaceOutMicrogameId(microgame.microgameId));
        for (int i = 0; i < creditsTexts.Length; i++)
        {
            string creditsString = creditsTexts[i].text;
            creditsString = TextHelper.getLocalizedText(creditsKeys[i], creditsString);
            creditsString = string.Format(creditsString, microgame.credits[i]);
            creditsTexts[i].text = creditsString;
        }

    }

    public string SpaceOutMicrogameId(string microgameId)
    {
        var chars = microgameId.ToCharArray();
        var name = "";
        foreach (var ch in chars)
        {
            if (ch >= 'A' && ch <= 'Z')
                name += " ";
            name += ch;
        }
        return name;
    }
}
