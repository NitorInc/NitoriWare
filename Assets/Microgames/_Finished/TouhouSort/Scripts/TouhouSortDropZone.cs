using UnityEngine;

public class TouhouSortDropZone : MonoBehaviour
{
    // A zone that a sortable must be sorted into

    public SpriteRenderer icon;
    
    // The category that this zone represents
    [SerializeField]
    string style;
    // Inverts the zone (e.g. no hats allowed)
    [SerializeField]
    bool invert;
    
    public void SetCategory(string style, bool invert, Sprite icon)
    {
        this.style = style;
        this.invert = invert;

        this.icon.sprite = icon;
    }
    
	public bool Belongs(TouhouSortSortable touhou)
    {
		// Checks if a given sortable belongs in this zone
		bool belongs = false;
        
		if (touhou.GetStyle() == style)
        {
			belongs = true;
		}

		if (invert)
        {
			belongs = !belongs;
		}

		return belongs;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        TouhouSortSortable sortable = other.GetComponentInParent<TouhouSortSortable>();
        sortable.EnterZone(gameObject.GetComponent<TouhouSortDropZone>());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        TouhouSortSortable sortable = other.GetComponentInParent<TouhouSortSortable>();
        if (sortable != null)
            sortable.ExitZone(gameObject.GetComponent<TouhouSortDropZone>());
    }

}
