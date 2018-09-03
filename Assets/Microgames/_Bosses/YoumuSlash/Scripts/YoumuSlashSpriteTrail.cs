using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashSpriteTrail : MonoBehaviour
{
    [SerializeField]
    Sprite trailSprite;
    [SerializeField]
    private bool enableSpawn;
    public bool EnableSpawn
    {
        get { return enableSpawn; }
        set { enableSpawn = value; }
    }
    [SerializeField]
    private Transform fragmentParent;
    [SerializeField]
    private float initialAlpha = 1f;
    [SerializeField]
    private float fragmentBrightness = 1f;
    [SerializeField]
    private float fragmentSaturation = 1f;
    [SerializeField]
    private float alphaFadeSpeed = 1f;
    [SerializeField]
    private float spawnDistance = .1f;
    [SerializeField]
    private float hueShiftPerFragment = .1f;
    [SerializeField]
    private int sortingOrderStart;

    private SpriteRenderer[] fragments;
    private int nextFragmentIndex;
    private float distanceSpawnProgress;
    private Vector2 lastPosition;
    private Vector3 initialPosition;
    private float currentHue;
    
	void Start ()
    {
        fragments = fragmentParent.GetComponentsInChildren<SpriteRenderer>();
        nextFragmentIndex = 0;
        initialPosition = transform.position;
	}

    public void resetTrail(float xPosition)
    {
        if (fragments == null)
            Start();

        currentHue = Random.Range(0f, 1f);
        lastPosition = new Vector3(xPosition, initialPosition.y, initialPosition.z);
        distanceSpawnProgress = 0f;
        foreach (var fragment in fragments)
        {
            fragment.enabled = false;
        }
    }

    public void setSprite(Sprite sprite)
    {
        foreach (var fragment in fragments)
        {
            fragment.sprite = sprite;
        }
    }

    private void OnDisable()
    {
        if (fragments == null || PauseManager.instance.Paused)
            return;
        foreach (var fragment in fragments)
        {
            if (fragment != null)
                fragment.enabled = false;
        }
    }

    void Update ()
    {
        foreach (var fragment in fragments)
        {
            float newAlpha = getAlpha(fragment) - alphaFadeSpeed * Time.deltaTime;
            if (newAlpha <= 0f)
            {
                fragment.enabled = false;
                newAlpha = 0f;
            }
            setAlpha(fragment, newAlpha);
        }

        if (enableSpawn)
        {
            Vector2 diff = (Vector2)transform.position - lastPosition;
            distanceSpawnProgress += diff.magnitude;
            if (distanceSpawnProgress > spawnDistance)
            {
                float initialProgress = distanceSpawnProgress - diff.magnitude;
                float distanceToTravel = spawnDistance - initialProgress;
                lastPosition = lastPosition + MathHelper.getVector2FromAngle(diff.getAngle(), distanceToTravel);
                createFragment(lastPosition);
                distanceSpawnProgress -= spawnDistance;
            }
            while(distanceSpawnProgress > spawnDistance)
            {
                lastPosition = lastPosition + MathHelper.getVector2FromAngle(diff.getAngle(), spawnDistance);
                createFragment(lastPosition);
                distanceSpawnProgress -= spawnDistance;
            }
        }

        lastPosition = transform.position;

    }

    void createFragment(Vector2 position)
    {
        var fragment = fragments[nextFragmentIndex];
        fragment.enabled = true;
        fragment.transform.position = new Vector3(position.x, position.y, fragment.transform.position.z);
        currentHue = (currentHue + hueShiftPerFragment) % 1f;
        fragment.color = new HSBColor(currentHue, fragmentSaturation, fragmentBrightness).ToColor();
        fragment.sortingOrder = sortingOrderStart + nextFragmentIndex;
        setAlpha(fragment, initialAlpha);
        fragment.sprite = trailSprite;

        nextFragmentIndex++;
        if (nextFragmentIndex >= fragments.Length)
        {
            foreach (var layerFragment in fragments)
            {
                layerFragment.sortingOrder -= fragments.Length;
            }
            nextFragmentIndex = 0;
        }
    }

    void setAlpha(SpriteRenderer fragment, float a)
    {
        Color color = fragment.color;
        color.a = a;
        fragment.color = color;
    }

    float getAlpha(SpriteRenderer fragment)
    {
        return fragment.color.a;
    }
}
