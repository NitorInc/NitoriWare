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
    public Vector2 victoryGoal;

    [Header("On victory rotate the mask by:")]
    public float victoryRotation;

    [Header("Time to move to victory position")]
    public float victoryMoveTime = 1f;

    public float victoryMoveSpeed, victoryRotationSpeed;

    public List<MaskPuzzleMaskFragment> fragments;
    public MaskPuzzleMaskEdges edges;
    public int topDepth = 0;

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

            // Add script to the fragment
            //currentFragment.AddComponent<MaskPuzzleMaskFragment>();
            currentFragment.GetComponent<MaskPuzzleMaskFragment>().fragmentsManager = this;

            // Setup drag and drop
            currentFragment.AddComponent<MeshCollider>();
            currentFragment.AddComponent<MouseGrabbable>();
            currentFragment.GetComponent<MouseGrabbable>().disableOnVictory = true;
            GetComponent<MouseGrabbableGroup>().addGrabbable(currentFragment.GetComponent<MouseGrabbable>(), false);

            // Add hooks to drag and drop
            UnityEvent grabEvent = new UnityEvent();
            grabEvent.AddListener(currentFragment.GetComponent<MaskPuzzleMaskFragment>().OnGrab);
            currentFragment.GetComponent<MouseGrabbable>().onGrab = grabEvent;
            UnityEvent releaseEvent = new UnityEvent();
            releaseEvent.AddListener(currentFragment.GetComponent<MaskPuzzleMaskFragment>().OnRelease);
            currentFragment.GetComponent<MouseGrabbable>().onRelease = releaseEvent;

            // Assign a layer to the fragment
            currentFragment.gameObject.layer = layerCounter++;

            // Add the fragment to list
            fragments.Add(currentFragment.GetComponent<MaskPuzzleMaskFragment>());
        }
	}
}
