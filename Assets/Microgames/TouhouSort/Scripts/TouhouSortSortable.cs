using UnityEngine;

public class TouhouSortSortable : MonoBehaviour
{
    // Defines an object (usually a touhou)
    // that can be sorted into a category
    
    public Collider2D hitBox;

    public AudioClip grabClip;

    // The style of this touhou instance
    [SerializeField]
    TouhouSortSorter.Style style;

    // Tracks the current zone that the object is in
    [SerializeField]
    TouhouSortDropZone currentZone;

    [SerializeField]
    private float grabScaleMult = 1.2f;
    
    void Start ()
    {
        Collider2D grabBox = GetComponent<Collider2D> ();
        Physics2D.IgnoreCollision(grabBox, hitBox);

        if (MathHelper.randomBool())
        {
            transform.localEulerAngles = Vector3.up * 180f;
        }
    }

    public TouhouSortSorter.Style GetStyle() => style;
    public void SetStyle(TouhouSortSorter.Style style) => this.style = style;
    public bool InWinZone { get; set; }

    

    public void OnGrab()
    {
        MicrogameController.instance.playSFX(grabClip, AudioHelper.getAudioPan(transform.position.x),
                pitchMult: 0.7F, volume: 0.8F);
        transform.localScale *= grabScaleMult;
    }

    public void OnRelease()
    {
        MicrogameController.instance.playSFX(grabClip, AudioHelper.getAudioPan(transform.position.x),
                pitchMult: 0.6F, volume: 0.8F);
        transform.localScale /= grabScaleMult;
    }

}
