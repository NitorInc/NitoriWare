using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MaskPuzzleGrabbableFragmentsManager : MonoBehaviour {

    [Header("Masks to be used, with fragmented and whole versions")]
    [SerializeField]
    private Mask[] maskPrefabs;

    [Header("Coordinates the masks can spawn in")]
    [SerializeField]
    private List<Vector2> spawnCoordinates;

    [Header("Fragments snap when closer than:")]
    public float maxSnapDistance = 1f;

    [System.Serializable]
    public class Background
    {
        public GameObject backgroundImage;

        [Header("On victory move mask to:")]
        public Vector3 victoryGoal;

        [Header("On victory rotate the mask to:")]
        public Vector3 victoryRotation;
    };

    [Header("Backgrounds and victory mask positions")]
    public Background[] backgroundVariants;

    [Header("How much fragments scale increases on grabbing")]
    public float grabScaleIncrease = .001f;

    [Header("Time to move to victory position")]
    public float victoryMoveTime = 1f;

    [Header("Speed of background animation")]
    public float backgroundAnimSpeed = 10f;

    [Header("Background sprite mask")]
    public Transform backgroundMask;

    [Header("Sound for picking up a fragment")]
    [SerializeField]
    private AudioClip grabSound;

    [Header("Sound for dropping a fragment")]
    [SerializeField]
    private AudioClip dropSound;
    [SerializeField]
    private float dropPitchMult = .8f;

    [Header("Sound for placing a fragment correctly")]
    [SerializeField]
    private AudioClip placeSound;

    [Header("Sound for victory")]
    [SerializeField]
    private AudioClip victorySound;

    [Header("")]
    // These get filled with a random choice from backgroundVariants during initialization
    public Vector3 victoryGoal;
    public Vector3 victoryRotation;

    public float victoryStartTime;
    public Vector3 victoryStartPosition;
    public Vector3 victoryStartRotation;

    public List<MaskPuzzleMaskFragment> fragments;
    public MaskPuzzleMaskEdges edges;
    public float topDepth = 0f;

    public MaskPuzzleFragmentGroup grabbedFragmentGroup;
    private Vector3 grabOffset;
    private float grabZ;

    [System.Serializable]
    public class Mask
    {
        public GameObject fragmented;
        public GameObject assembled;
    };

    Mask chosenMask;

    private const int FIRST_MASK_LAYER = 14;

    // Initialization - choose and prepare the mask that will be assembled by the player
    // as well as the background and victory position
    void Start ()
    {
        // Choose a random mask
        chosenMask = maskPrefabs[Random.Range(0, maskPrefabs.Length)];
        GameObject chosenMaskFragmented = Instantiate(
            chosenMask.fragmented,
            Vector2.zero,
            Quaternion.identity
        );
        print("Chosen " + chosenMaskFragmented.name + ". It has " + chosenMaskFragmented.transform.childCount + " fragments.");

        // Get info about the edges that should be connected
        edges = chosenMaskFragmented.GetComponent<MaskPuzzleMaskEdges>();

        // Layers will be assigned starting from this one
        int layerCounter = FIRST_MASK_LAYER;

        // Initialize all fragments of the chosen mask
        while (chosenMaskFragmented.transform.childCount > 0)
        {
            GameObject currentFragment = chosenMaskFragmented.transform.GetChild(0).gameObject;
            print("Taking " + currentFragment.name + " from the library");

            // Become the parent of the fragment
            currentFragment.transform.parent = transform;

            // Randomize the spawn location
            int spawnPointIndex = Random.Range(0, spawnCoordinates.Count);
            currentFragment.transform.position = spawnCoordinates[spawnPointIndex];
            spawnCoordinates.RemoveAt(spawnPointIndex);

            // Add script and collider to the fragment
            currentFragment.GetComponent<MaskPuzzleMaskFragment>().fragmentsManager = this;
            currentFragment.AddComponent<MeshCollider>();

            // Assign a layer to the fragment
            currentFragment.gameObject.layer = layerCounter++;

            // Add the fragment to list
            fragments.Add(currentFragment.GetComponent<MaskPuzzleMaskFragment>());
        }

        // Choose a random background, ensure it's the only one visible,
        // and copy its victory position values
        if (backgroundVariants.Length > 0)
        {
            Background chosenBackground = backgroundVariants[Random.Range(0, backgroundVariants.Length)];

            foreach (Background backgroundVariant in backgroundVariants)
                if (backgroundVariant.backgroundImage != chosenBackground.backgroundImage)
                    backgroundVariant.backgroundImage.SetActive(false);
            chosenBackground.backgroundImage.SetActive(true);

            victoryGoal = chosenBackground.victoryGoal;
            victoryRotation = chosenBackground.victoryRotation;
        }
    }

    // Handle dragging and dropping the fragments
    void HandleDragging()
    {
        // Grabbing a fragment
        if (grabbedFragmentGroup == null && Input.GetMouseButtonDown(0))
        {
            // Get an array of all the fragments under the cursor
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(mouseRay, float.PositiveInfinity, 31 << 14);

            RaycastHit topHit = new RaycastHit();
            MaskPuzzleMaskFragment fragmentHit, topHitFragment = null;
            float topHitDepth = -1f;
            float topHitDistance = 0f;

            // There might be multiple fragments under the cursor
            // We need to determine which one is on top - that one will be grabbed
            // We pick the one whose assigned camera has the highest depth
            // In case of equal depth we pick the fragment closest to the camera
            foreach (RaycastHit hit in hits)
            {
                fragmentHit = hit.collider.GetComponent<MaskPuzzleMaskFragment>();
                if (fragmentHit.fragmentGroup.assignedCamera.depth <= topHitDepth)
                    continue;
                if (fragmentHit.fragmentGroup.assignedCamera.depth == topHitDepth
                        && hit.distance >= topHitDistance)
                    continue;
                topHit = hit;
                topHitFragment = fragmentHit;
                topHitDepth = fragmentHit.fragmentGroup.assignedCamera.depth;
                topHitDistance = hit.distance;
            }

            if (topHitFragment) {
                grabbedFragmentGroup = topHitFragment.fragmentGroup;
                shiftGroupScale(grabbedFragmentGroup, grabScaleIncrease);
                // Grabbed fragment group should be on top
                grabbedFragmentGroup.assignedCamera.depth = (topDepth += .005f);
                // Save the grabbed point's coordinates needed for calculating position when dragging
                grabZ = topHit.point.z;
                grabOffset = topHitFragment.transform.position
                    - CameraHelper.getCursorPosition(grabZ);
                print("Top hit=" + topHitFragment + "; depth=" + topHitDepth + "; dist=" + topHitDistance
                    + "; z=" + topHit.point.z);
                MicrogameController.instance.playSFX(
                    grabSound,
                    volume: 1f,
                    panStereo: AudioHelper.getAudioPan(topHitFragment.transform.position.x)
                );
            }
        }

        // Dropping a fragment
        else if (grabbedFragmentGroup != null && !Input.GetMouseButton(0)) {
            shiftGroupScale(grabbedFragmentGroup, -grabScaleIncrease);
            MicrogameController.instance.playSFX(
                dropSound,
                volume: 1f,
                pitchMult: dropPitchMult,
                panStereo: AudioHelper.getAudioPan(grabbedFragmentGroup.fragments[0].transform.position.x)
            );
            if (grabbedFragmentGroup.SnapToOtherFragments())
            {
                MicrogameController.instance.playSFX(
                    placeSound,
                    volume: 1f,
                    panStereo: AudioHelper.getAudioPan(grabbedFragmentGroup.fragments[0].transform.position.x)
                );
                if (CheckVictory())
                {
                    MicrogameController.instance.playSFX(
                        victorySound,
                        volume: 1f,
                        panStereo: 0f
                    );
                }
            }
            grabbedFragmentGroup = null;
        }

        // Dragging fragments
        else if (grabbedFragmentGroup != null) {
            Vector3 position = CameraHelper.getCursorPosition(grabZ);
            position += grabOffset;
            foreach (MaskPuzzleMaskFragment fragment in grabbedFragmentGroup.fragments)
                fragment.transform.position = position;
        }
    }

    void shiftGroupScale(MaskPuzzleFragmentGroup group, float amount)
    {
        foreach (var fragment in grabbedFragmentGroup.fragments)
        {
            fragment.transform.localScale += Vector3.one * amount;
        }
    }

    // Animate the background mask
    void VictoryAnimation()
    {
        backgroundMask.localScale = Vector2.one * (Time.time - victoryStartTime) * backgroundAnimSpeed;
    }

    // Called every frame
    void Update()
    {
        if (MicrogameController.instance.getVictory())
            VictoryAnimation();
        else
            HandleDragging();
    }

    // To be called after dropping a fragment and snapping to other fragments
    // Check and handle victory condition
    bool CheckVictory()
    {
        // A fragment group should contain all fragments, otherwise we haven't won yet
        if (fragments.Count != fragments[0].fragmentGroup.fragments.Count)
            return false;

        // Victory!
        MicrogameController.instance.setVictory(victory: true, final: true);

        // Save the starting values for the victory animation
        victoryStartTime = Time.time;
        victoryStartPosition = fragments[0].transform.position;
        victoryStartRotation = fragments[0].transform.eulerAngles;
        backgroundMask.position = victoryStartPosition;

        // Replace the mask fragments with the assembled model
        foreach (MaskPuzzleMaskFragment fragment in fragments)
            fragment.gameObject.SetActive(false);
        MaskPuzzleMaskFragment assembledMask = Instantiate(
            chosenMask.assembled,
            Vector2.zero,
            Quaternion.identity
        ).transform.GetChild(0).GetComponent<MaskPuzzleMaskFragment>();
        assembledMask.fragmentsManager = this;
        assembledMask.gameObject.layer = FIRST_MASK_LAYER;
        assembledMask.VictoryAnimation();

        return true;
    }
}
