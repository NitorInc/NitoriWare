using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_ItemScript : MonoBehaviour {

    public Color selectColor = Color.white;
    public GameObject rngMaster;
    public GameObject KaguyaChan;
    public GameObject correctIndicator;
    public GameObject wrongIndicator;
    public GameObject timingMaster;
    public bool isMoving = false;
    public bool isCorrect = false;
    public float initialScale;
    public float floatFactor = 0.2f;


    private Vector3 startingPosition;
    private bool isSelectable = false;
    private Quaternion defaultRotation;
    private int floatDirection = 1;
    private bool isFloating = false;
    private float floatStartDelay = 0;

    [SerializeField]
    private AudioClip correctSound;

    [SerializeField]
    private AudioClip wrongSound;

    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _capsuleCollider;
    private CircleCollider2D _circleCollider;
    private SineWave _sineWave;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _sineWave = GetComponent<SineWave>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start ()
    {

        _spriteRenderer.enabled = false;
        _sineWave.enabled = false;

        if (_capsuleCollider != null)
            _capsuleCollider.enabled = false;
        if (_circleCollider != null)
            _circleCollider.enabled = false;

        defaultRotation = transform.rotation;
        _rigidbody.gravityScale = 0;
        initialScale = transform.localScale.x;

        Invoke("obtainStartingPosition", 0.01f);
    }

    void OnMouseDown()
    {
        
        if(rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished == false && isSelectable == true)
        {
            GameObject theIndicator;
            if (isCorrect == true)
            {
                theIndicator = Instantiate(correctIndicator);
                MicrogameController.instance.setVictory(true, true);
                KaguyaChan.GetComponent<KaguyaMemory_KaguyaEndAnimation>().isWin = true;
                MicrogameController.instance.playSFX(correctSound, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(0));
            }
            else
            {
                theIndicator = Instantiate(wrongIndicator);
                MicrogameController.instance.setVictory(false, true);
                KaguyaChan.GetComponent<KaguyaMemory_KaguyaEndAnimation>().isLose = true;
                MicrogameController.instance.playSFX(wrongSound, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(0));
            }
            _rigidbody.velocity = new Vector2(0, 0);
            theIndicator.transform.position = transform.position;
            rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished = true;
            isSelectable = false;
        }
        
    }

    void Update()
    {
        if (transform.rotation != defaultRotation)
        {
            transform.rotation = defaultRotation;
        }
        if (_rigidbody.angularVelocity != 0)
        {
            _rigidbody.angularVelocity = 0;
        }
        

        if (isSelectable && rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished == false && isFloating == true)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y + (floatFactor * floatDirection));
            if (_rigidbody.velocity.y > 3 && floatDirection == 1)
            {
                floatDirection = -1;
            }
            else if (_rigidbody.velocity.y < -3 && floatDirection == -1)
            {
                floatDirection = 1;
            }
        }
    }

    public void appearSelectable()
    {
        _sineWave.enabled = true;
        if (_capsuleCollider != null)
            _capsuleCollider.enabled = true;
        if (_circleCollider != null)
            _circleCollider.enabled = true;

        _spriteRenderer.color = selectColor;

        transform.rotation = defaultRotation;
        _rigidbody.angularVelocity = 0;
        transform.position = startingPosition;
        _spriteRenderer.enabled = true;
        _rigidbody.velocity = new Vector2(0, 0);

        _rigidbody.gravityScale = 0;
        isSelectable = true;

        float currentX = transform.position.x + 7;
        floatStartDelay = currentX / 30;
        Invoke("beginFloating", floatStartDelay);
    }

    void beginFloating()
    {
        isFloating = true;
    }

    void obtainStartingPosition()
    {
        startingPosition = transform.position;
    }
}
