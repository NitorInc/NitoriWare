using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

public class TouhouSortSorter : MonoBehaviour
{
    // Primary class for TouhouSort game handling
    // Handles objects and win/loss
    
    // Spacing between starting sortables
    [SerializeField]
    private float GAP = 2.2f;
    [SerializeField]
    private float placementOffsetXRange;
    [SerializeField]
    private float placementOffsetYRange;
    
    [SerializeField]
    private TouhouSortCategory categoryData;

    [Header("Max number of sortable touhous per difficulty")]
    public int[] difficultySlotCounts = { 3, 4, 4 };
    int slotCount;

    [Header("What dificulty (and above) we allow non-canon situations")]
    public int NonCanonDifficulty = 3;
    bool allowNonCanon => MicrogameController.instance.difficulty >= NonCanonDifficulty;

    [Header("Stage elements")]
    public Transform stagingArea;
    public TouhouSortZoneManager zoneManager;

    public TouhouSortSortable touhouTemplate;

    public ParticleSystem confettiParticles;
    public AudioClip victoryClip;
    
    TouhouSortSortable[] touhous;
    Vector3[] slots;
    
    public struct Style
    {
        public string name;
        public Sprite sprite;
        public bool isRightSide;
    }

    void Start() {
        var category = categoryData;


        // Scoop up as many touhous as we can
        touhous = LoadTouhous(category, difficultySlotCounts[MicrogameController.instance.difficulty - 1]);

        // Hand target data to the Zone Managers
        zoneManager.SetTargets(
            leftTargets: touhous
                .Where(a => !a.GetStyle().isRightSide)
                .ToList(),
            rightTargets: touhous
                .Where(a => a.GetStyle().isRightSide)
                .ToList());

        slotCount = touhous.Length;

        // Fill starting slots with touhous
        CreateSlots();
        FillSlots();

        // Check the sort at the start, just in case
        CheckSort();
    }

    TouhouSortSortable[] LoadTouhous(TouhouSortCategory categoryData, int amount)
    {
        List<Style> leftStyles = new List<Style>();
        List<Style> rightStyles = new List<Style>();

        var pool = new List<Sprite>(categoryData.LeftPool);
        if (allowNonCanon)
            pool.AddRange(categoryData.LeftPoolNonCanon);
        foreach (Sprite sprite in pool)
        {
            Style style = new Style();
            style.name = categoryData.IdName;
            style.sprite = sprite;
            style.isRightSide = false;

            leftStyles.Add(style);
        }
        pool = new List<Sprite>(categoryData.RightPool);
        if (allowNonCanon)
            pool.AddRange(categoryData.RightPoolNonCanon);
        foreach (Sprite sprite in pool)
        {
            Style style = new Style();
            style.sprite = sprite;
            style.isRightSide = true;

            rightStyles.Add(style);
        }
        
        MouseGrabbableGroup grabGroup = stagingArea.GetComponent<MouseGrabbableGroup>();
        TouhouSortSortable[] randomTouhous = new TouhouSortSortable[amount];

        int lastPickedSide = 0; // 0 for left, 1 for right
        for (int i = 0; i < amount; i++)
        {
            Style style;
            if (leftStyles.Count == 0)
            {
                style = rightStyles[Random.Range(0, rightStyles.Count)];
                lastPickedSide = 0;
                rightStyles.Remove(style);
            }
            else if (rightStyles.Count == 0)
            {
                style = leftStyles[Random.Range(0, leftStyles.Count)];
                lastPickedSide = 1;
                leftStyles.Remove(style);
            }
            else if (i == 1)    // Ensure one of each type
            {
                if (lastPickedSide == 1)
                {
                    style = leftStyles[Random.Range(0, leftStyles.Count)];
                    leftStyles.Remove(style);
                }
                else
                {
                    style = rightStyles[Random.Range(0, leftStyles.Count)];
                    rightStyles.Remove(style);
                }
                lastPickedSide = 1 - lastPickedSide;
            }
            else
            {
                int coin = Random.Range(0, 2);
                if (coin == 0)
                {
                    style = leftStyles[Random.Range(0, leftStyles.Count)];
                    lastPickedSide = 0;
                    leftStyles.Remove(style);
                }
                else
                {
                    style = rightStyles[Random.Range(0, rightStyles.Count)];
                    lastPickedSide = 1;
                    rightStyles.Remove(style);
                }
            }

            // Build a new touhou instance
            TouhouSortSortable touhou = Instantiate(touhouTemplate, transform.position, transform.rotation);
            touhou.GetComponent<SpriteRenderer>().sprite = style.sprite;
            touhou.gameObject.AddComponent<PolygonCollider2D>();

            touhou.SetStyle(style);

            MouseGrabbable grab = touhou.gameObject.AddComponent<MouseGrabbable>();
            grab.detectGrab = false;

            UnityEvent grabEvent = new UnityEvent();
            grabEvent.AddListener(touhou.OnGrab);

            UnityEvent releaseEvent = new UnityEvent();
            releaseEvent.AddListener(touhou.OnRelease);
            releaseEvent.AddListener(CheckSort);

            grab.onGrab = grabEvent;
            grab.onRelease = releaseEvent;
            grab.disableOnVictory = true;
            
            touhou.transform.parent = stagingArea;
            
            grabGroup.addGrabbable(grab, true);
            randomTouhous[i] = touhou;
        }
        randomTouhous.Shuffle();

        return randomTouhous;
    }

    void CreateSlots()
    {
		// Instantiate a list of Vector3 objects
		// which will be the starting spots of
		// our touhous
		slots = new Vector3[slotCount];
		Vector3 origin = stagingArea.position;

		if (slotCount % 2 == 0) {
			origin.x = origin.x + (GAP / 2);
		}

		for (int i = 0; i < slotCount; i++) {
			Vector3 slot = origin;
			int multiplier = (i + 1) / 2;

			if (i % 2 == 0) {
				slot.x = origin.x + (multiplier * GAP);
			}
			else {
				slot.x = origin.x - (multiplier * GAP);
			}

            slot += new Vector3(Random.Range(
                -placementOffsetXRange, placementOffsetXRange),
                Random.Range(-placementOffsetYRange, placementOffsetYRange), 0f);

			slots [i] = slot;
		}
	}

	void FillSlots()
    {
		// Fill instantiated slots with sortable touhous
		for (int i = 0; i < slotCount; i++)
        {
			touhous [i].transform.position = slots [i];
		}
	}

	public void CheckSort()
    {
        // Check the current state of the sort
        // End the game if everything is sorted
        bool allSorted =
            !touhous.Any(a => !a.InWinZone);

		if (allSorted)
        {
            confettiParticles.gameObject.SetActive(true);
            confettiParticles.Play();
            MicrogameController.instance.playSFX(victoryClip, 0f, 1f, .75f);

            MicrogameController.instance.setVictory(true, true);
        }
	}

}
