using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ChangeOrderInLayer : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private int orderInLayer;
    [SerializeField]
    private string sortingLayer = "Default";
    [SerializeField]
    private SortingLayer sortingLayerAbsolute;
    [SerializeField]
    private bool disableOnPlay = true;
    [SerializeField]
    private bool applyToChildren;

    public void SetLayer(string layer)
    {
        sortingLayer = layer;
        Update();
    }

    public void SetOrderInLayer(int order)
    {
        orderInLayer = order;
        Update();
    }

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        if (disableOnPlay && Application.isPlaying)
            enabled = false;
    }

    void Start()
	{
        if (enabled)
            Update();
	}
	
	void Update ()
    {
        if (applyToChildren)
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>() )
            {
                UpdateRenderer(renderer);
            }
        }
        else
            UpdateRenderer(_renderer);
    }
    
    void UpdateRenderer(Renderer renderer)
    {
        if (orderInLayer != renderer.sortingOrder)
            renderer.sortingOrder = orderInLayer;
        if (!string.IsNullOrEmpty(sortingLayer))
            renderer.sortingLayerName = sortingLayer;
    }
}
