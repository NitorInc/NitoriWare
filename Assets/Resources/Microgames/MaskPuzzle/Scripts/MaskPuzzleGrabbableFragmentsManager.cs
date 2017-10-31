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

    public List<GameObject> fragments;

    // Initialization - choose and prepare the mask that will be assembled by the player
    void Start ()
    {
        // Choose a random mask from the library
        GameObject chosenMask = maskLibrary.transform.GetChild(Random.Range(0, maskLibrary.transform.childCount)).gameObject;
        print("Chosen " + chosenMask.name + ". It has " + chosenMask.transform.childCount + " fragments.");

        // Initialize all fragments of the chosen mask
        while (chosenMask.transform.childCount > 0)
        {
            GameObject currentFragment = chosenMask.transform.GetChild(0).gameObject;
            print("Taking " + currentFragment.name + " from the library");

            // Become the parent of the fragment
            currentFragment.transform.parent = transform;

            // Add script to the fragment
            currentFragment.AddComponent<MaskPuzzleMaskFragment>();
            currentFragment.GetComponent<MaskPuzzleMaskFragment>().fragmentsManager = this;

            // Setup drag and drop
            currentFragment.AddComponent<PolygonCollider2D>();
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

            // Add the fragment to list
            fragments.Add(currentFragment);
        }

        // Disable the mask library - we won't need the other masks
        maskLibrary.SetActive(false);
	}
}
