using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class DollDanceColorShade : MonoBehaviour
{
    [SerializeField]
    private bool shaded;
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color shadedColor;

    void Start ()
    {
        setShaded(shaded);
	}

    //private void Update()
    //{
    //    spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    //}

    public void setShaded(bool shaded)
    {
        this.shaded = shaded;

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = shaded ? shadedColor : defaultColor;
        }
    }
}
