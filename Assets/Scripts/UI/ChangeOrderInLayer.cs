using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ChangeOrderInLayer : MonoBehaviour
{
#pragma warning disable 0649    //Serialized Fields
    [SerializeField]
    private Renderer forceRenderer;
    [SerializeField]
    private int orderInLayer;
#pragma warning restore 0649


	void Awake ()
	{
        if (forceRenderer == null)
            forceRenderer = GetComponent<Renderer>();
		forceRenderer.sortingOrder = orderInLayer;
	}
	
	void Update ()
	{
		if (orderInLayer != forceRenderer.sortingOrder)
			forceRenderer.sortingOrder = orderInLayer;
	}
}
