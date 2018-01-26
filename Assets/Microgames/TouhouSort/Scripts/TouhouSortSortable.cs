using UnityEngine;

public class TouhouSortSortable : MonoBehaviour
{
    // Defines an object (usually a touhou)
    // that can be sorted into a category
    
    public Collider2D hitBox;

    public AudioClip grabClip;

    // The style of this touhou instance
    [SerializeField]
    string style;

    // Tracks the current zone that the object is in
    [SerializeField]
    TouhouSortDropZone currentZone;

    void Start ()
    {
        Collider2D grabBox = GetComponent<Collider2D> ();
        Physics2D.IgnoreCollision(grabBox, hitBox);
    }

    public string GetStyle()
    {
        return style;
    }

    public void SetStyle(string style)
    {
        this.style = style;
    }

    public TouhouSortDropZone GetCurrentZone()
    {
        return currentZone;
    }

    public void EnterZone(TouhouSortDropZone zone)
    {
        currentZone = zone;
    }

    public void ExitZone(TouhouSortDropZone zone)
    {
        if (currentZone == zone)
        {
            currentZone = null;
        }
    }

    public void OnGrab()
    {
        MicrogameController.instance.playSFX(grabClip, AudioHelper.getAudioPan(transform.position.x),
                pitchMult: 0.7F, volume: 0.8F);
    }

    public void OnRelease()
    {
        MicrogameController.instance.playSFX(grabClip, AudioHelper.getAudioPan(transform.position.x),
                pitchMult: 0.6F, volume: 0.8F);
    }

}
