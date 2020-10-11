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
        if (disableOnPlay && Application.isPlaying)
            enabled = false;
    }

    void Start()
	{
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
		_renderer.sortingOrder = orderInLayer;
	}
	
	void Update ()
	{
        if (orderInLayer != _renderer.sortingOrder)
            _renderer.sortingOrder = orderInLayer;
        if (!string.IsNullOrEmpty(sortingLayer))
            _renderer.sortingLayerName = sortingLayer;
	}
}
