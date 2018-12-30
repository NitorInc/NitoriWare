using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeijaSpinSpinningChar : MonoBehaviour
{

    public int spinCount;
    public int moveType;
    private Vector2 lastVec2Angle;
    private Vector2 lastMouseAngle;
    private Vector2 currentAngle;
    private Vector3 rootOriginalPosition;
    public Animator seijaAnim, spinAnim, charFlingAnim, objFlingAnim;
    public AudioClip flingClip, crashClip, spinClip;
    public GameObject broomObject;
    private Component spinningArrows;
    private AudioSource _audioSource;
    private float lastAngle;
    private float currentAngleChange;
    private float angleDifference;
    private float totalSpin = 0f;
    private float moveValue = 0;
    private int crashTimer = 0;
    private bool flingDone = false;
    private bool crashDone = false;

    private State state = State.Default;
    // Borrowed with love from Spaceship for Seija
    private enum State
    {
        Default,
        Victory,
        Failure
    }

    private State spinState = State.Default;
    // For Reimu, Mari, and Saku
    private enum SpinState
    {
        Start,
        Spinning,
        SpinOut,
        FightBack
    }

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        spinningArrows = transform.Find("Arrows");
        rootOriginalPosition = transform.root.position;
        if (moveType == 2)
        {
            transform.position = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we can spin and whether the mouse is in position to
        if (totalSpin < spinCount * 360f)
        {
            if (CameraHelper.isMouseOver(gameObject.GetComponent<CircleCollider2D>()))
            {
                // Check for mouse input and interact if so
                if (Input.GetMouseButtonDown(0))
                {
                    lastVec2Angle = (Vector2)(CameraHelper.getCursorPosition() - transform.position);
                    lastAngle = lastVec2Angle.getAngle();
                }
                else if (Input.GetMouseButton(0))
                {
                    spinAnim.SetInteger("state", (int)SpinState.Spinning);
                    currentAngle = (Vector2)(CameraHelper.getCursorPosition() - transform.position);
                    currentAngleChange = currentAngle.getAngle();
                    lastMouseAngle = currentAngle;
                    angleDifference = MathHelper.getAngleDifference(currentAngleChange, lastAngle);

                    // Only try to rotate if the direction makes sense
                    if (angleDifference < 0)
                    {
                        if (spinningArrows.GetComponent<SeijaSpinArrows>().flipped)
                        {
                            totalSpin += Mathf.Abs(angleDifference);
                            transform.eulerAngles += Vector3.back * angleDifference;
                        }
                        lastAngle = currentAngleChange;
                    }
                    else if (angleDifference > 0)
                    {
                        if (!spinningArrows.GetComponent<SeijaSpinArrows>().flipped)
                        {
                            totalSpin += Mathf.Abs(angleDifference);
                            transform.eulerAngles += Vector3.back * angleDifference;
                        }
                        lastAngle = currentAngleChange;
                    }
                }
            }

            switch (moveType)
            {
                case 1:
                    moveValue += 1.5f * Time.deltaTime;
                    transform.root.position = new Vector3(rootOriginalPosition.x + 2 * Mathf.Cos(moveValue), rootOriginalPosition.y + 2 * Mathf.Sin(moveValue), rootOriginalPosition.z);
                    break;
                case 2:
                    moveValue += 1.5f * Time.deltaTime;
                    transform.root.position = new Vector3(rootOriginalPosition.x + 1 * moveValue, -(rootOriginalPosition.y + 2 * Mathf.Sin(moveValue)), rootOriginalPosition.z);
                    break;
                default:
                    break;
            }
        }
        // Stop if limit is reached
        if (totalSpin > spinCount * 360f)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            spinAnim.SetInteger("state", (int)SpinState.SpinOut);
            objFlingAnim.SetInteger("state", 1);
            charFlingAnim.SetInteger("state", 1);
            if (moveType != 2)
            {
                seijaAnim.SetInteger("state", (int)State.Victory);
                transform.Rotate(Vector3.back * Time.deltaTime, angleDifference);
            }
            if (moveType == 1)
            {
                broomObject.transform.Rotate(Vector3.forward * Time.deltaTime * 0.5f, angleDifference);
            }
            if (!flingDone)
            {
                PlayFling();
                flingDone = true;
            }
            if (crashTimer >= 60)
            {
                if (!crashDone)
                {
                    crashDone = true;
                    PlayCrash();
                    if (moveType != 2)
                    {
                        CameraShake.instance.setScreenShake(.5f);
                        CameraShake.instance.shakeSpeed = 15f;
                    }
                }
            }
            else
            {
                crashTimer++;
            }
        }
        else if (!Input.GetMouseButton(0) || !CameraHelper.isMouseOver(gameObject.GetComponent<CircleCollider2D>()))
        {
            spinAnim.SetInteger("state", (int)SpinState.Start);
        }
    }

    public void PlayCrash()
    {
        _audioSource.pitch = Time.timeScale;
        _audioSource.PlayOneShot(crashClip);
    }

    public void PlayFling()
    {
        _audioSource.pitch = Time.timeScale;
        _audioSource.PlayOneShot(flingClip);
    }
}