using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YoumuSlashTargetSlice : MonoBehaviour
{
    [SerializeField]
    private Transform imageTransform;
    [SerializeField]
    private Transform maskTransform;
    [Header("Edit sliced falling speeds here")]
    [SerializeField]
    private bool disableFall;
    [SerializeField]
    private float direction = 1f;
    [SerializeField]
    private Vector2 rotSpeedRange;
    [SerializeField]
    private Vector2 xSpeedRange;
    [SerializeField]
    private Vector2 ySpeedRange;
    [SerializeField]
    private float gravityScale;
    [SerializeField]
    private YoumuSlashTarget baseObject;

    private float rotSpeed;
    private Vector2 speed;
    private bool falling;

    private void Start()
    {
        setSpeed();
    }

    public void setSpeed()
    {
        rotSpeed = MathHelper.randomRangeFromVector(rotSpeedRange) * direction 
            * (baseObject.MapInstance.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right ? -1f : 1f);
        speed = new Vector2(MathHelper.randomRangeFromVector(xSpeedRange * -direction), MathHelper.randomRangeFromVector(ySpeedRange));
    }

    void Update ()
    {
        if (falling && !disableFall)
        {
            updateFallingTransform();
        }
	}

    void updateFallingTransform()
    {
        transform.localEulerAngles += Vector3.forward * rotSpeed * Time.deltaTime;
        speed += Vector2.down * gravityScale * Time.deltaTime;
        transform.position += (Vector3)speed * Time.deltaTime;
    }

    public void setImageActive(Sprite sprite)
    {
        var imageComponent = imageTransform.GetComponent<Image>();
        imageComponent.sprite = sprite;
        imageComponent.color = Color.white;
    }

    public Transform getImageTransform() => imageTransform;
    public Transform getMaskTransform() => maskTransform;
    public bool isFalling() => falling;

    public void setFalling(bool falling) => this.falling = falling;

}
