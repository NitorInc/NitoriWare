using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MaskPuzzleGrabbableFragmentsManager : MonoBehaviour {

    [Header("Mask Library")]
    [SerializeField]
    private GameObject maskLibrary;

    [Header("Fragments snap when closer than:")]
    public float maxSnapDistance = 1f;

    [Header("On victory move mask to:")]
    public Vector3 victoryGoal;

    [Header("On victory rotate the mask to:")]
    public Vector3 victoryRotation;

    [Header("Time to move to victory position")]
    public float victoryMoveTime = 1f;

    [Header("Background image")]
    public SpriteRenderer backgroundImage;

    [Header("")]
    public float victoryStartTime;
    public Vector3 victoryStartPosition;
    public Vector3 victoryStartRotation;
    public Color victoryStartBgColor;

    public List<MaskPuzzleMaskFragment> fragments;
    public MaskPuzzleMaskEdges edges;
    public int topDepth = 0;

    public MaskPuzzleFragmentGroup grabbedFragmentGroup;
    private Vector3 grabOffset;
    private float grabZ;

    // Initialization - choose and prepare the mask that will be assembled by the player
    void Start ()
    {
        // Choose a random mask from the library
        GameObject chosenMask = maskLibrary.transform.GetChild(Random.Range(0, maskLibrary.transform.childCount)).gameObject;
        print("Chosen " + chosenMask.name + ". It has " + chosenMask.transform.childCount + " fragments.");

        // Get info about the edges that should be connected
        edges = chosenMask.GetComponent<MaskPuzzleMaskEdges>();

        // Layers will be assigned starting from this one
        int layerCounter = 14;

        // Initialize all fragments of the chosen mask
        while (chosenMask.transform.childCount > 0)
        {
            GameObject currentFragment = chosenMask.transform.GetChild(0).gameObject;
            print("Taking " + currentFragment.name + " from the library");

            // Become the parent of the fragment
            currentFragment.transform.parent = transform;

            // Add script and collider to the fragment
            currentFragment.GetComponent<MaskPuzzleMaskFragment>().fragmentsManager = this;
            currentFragment.AddComponent<MeshCollider>();

            // Assign a layer to the fragment
            currentFragment.gameObject.layer = layerCounter++;

            // Add the fragment to list
            fragments.Add(currentFragment.GetComponent<MaskPuzzleMaskFragment>());
        }
    }

    // Called every frame
    void Update()
    {
        if (MicrogameController.instance.getVictory())
            return;

        // Grabbing a fragment
        if (grabbedFragmentGroup == null && Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(mouseRay, float.PositiveInfinity, 31 << 14);

            RaycastHit topHit = new RaycastHit();
            MaskPuzzleMaskFragment fragmentHit, topHitFragment = null;
            float topHitDepth = -1f;
            float topHitDistance = 0f;

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
                grabbedFragmentGroup.assignedCamera.depth = (++topDepth);
                grabZ = topHit.point.z;
                grabOffset = topHitFragment.transform.position
                    - CameraHelper.getCursorPosition(grabZ);
                print("Top hit=" + topHitFragment + "; depth=" + topHitDepth + "; dist=" + topHitDistance
                    + "; z=" + topHit.point.z);
            }
        }

        // Dropping a fragment
        else if (grabbedFragmentGroup != null && !Input.GetMouseButton(0)) {
            grabbedFragmentGroup.SnapToOtherFragments();
            CheckVictory();
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

    // To be called after dropping a fragment and snapping to other fragments
    // Check and handle victory condition
    void CheckVictory()
    {
        // A fragment group should contain all fragments, otherwise we haven't won yet
        if (fragments.Count != fragments[0].fragmentGroup.fragments.Count)
            return;

        // Victory!
        MicrogameController.instance.setVictory(victory: true, final: true);
        // Save the starting values for the victory animation
        victoryStartTime = Time.time;
        victoryStartPosition = fragments[0].transform.position;
        victoryStartRotation = fragments[0].transform.eulerAngles;
        victoryStartBgColor = backgroundImage.color;
    }
}
