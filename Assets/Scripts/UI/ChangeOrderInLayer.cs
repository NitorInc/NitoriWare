using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ChangeOrderInLayer : MonoBehaviour
{
#pragma warning disable 0649
  [SerializeField]
  private Renderer _renderer;
  [SerializeField]
  private int orderInLayer;
#pragma warning restore 0649


  void Start()
  {
    if (_renderer == null)
      _renderer = GetComponent<Renderer>();
    _renderer.sortingOrder = orderInLayer;
  }

  void Update()
  {
    if (orderInLayer != _renderer.sortingOrder)
      _renderer.sortingOrder = orderInLayer;
  }

}
