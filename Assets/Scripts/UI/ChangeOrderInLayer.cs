using UnityEngine;
using System.Collections;

public class ChangeOrderInLayer : MonoBehaviour
{
	public int orderInLayer;

	private Renderer _renderer;


	void Awake ()
	{
		_renderer = GetComponent<Renderer>();
		_renderer.sortingOrder = orderInLayer;
	}
	
	void Update ()
	{
		if (orderInLayer != _renderer.sortingOrder)
			_renderer.sortingOrder = orderInLayer;
	}
}
