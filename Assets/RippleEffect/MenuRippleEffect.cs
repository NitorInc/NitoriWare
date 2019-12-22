using UnityEngine;
using System.Collections;


public class MenuRippleEffect : MonoBehaviour
{
	public AnimationCurve waveform = new AnimationCurve(
		new Keyframe(0.00f, 0.50f, 0, 0),
		new Keyframe(0.05f, 1.00f, 0, 0),
		new Keyframe(0.15f, 0.10f, 0, 0),
		new Keyframe(0.25f, 0.80f, 0, 0),
		new Keyframe(0.35f, 0.30f, 0, 0),
		new Keyframe(0.45f, 0.60f, 0, 0),
		new Keyframe(0.55f, 0.40f, 0, 0),
		new Keyframe(0.65f, 0.55f, 0, 0),
		new Keyframe(0.75f, 0.46f, 0, 0),
		new Keyframe(0.85f, 0.52f, 0, 0),
		new Keyframe(0.99f, 0.50f, 0, 0)
	);

    public float effectTime, rippleCoolTime = .25f;
    private float rippleCoolTimer, dropTimer;
    public bool randomDrops;

    [Range(0.01f, 1.0f)]
	public float refractionStrength = 0.5f;

	public Color reflectionColor = Color.gray;

	[Range(0.01f, 1.0f)]
	public float reflectionStrength = 0.7f;

	[Range(1.0f, 3.0f)]
	public float waveSpeed = 1.25f;

	[Range(0.0f, 2.0f)]
	public float dropInterval = 0.5f;

	[SerializeField, HideInInspector]
	Shader shader;

	class Droplet
	{
		Vector2 position;
		float time;

		public Droplet()
		{
			time = 1000;
		}

		public void Reset(bool mousePosition)
		{
            if (mousePosition)
            {
                position = (Vector2)MainCameraSingleton.instance.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                Vector2 bounds = new Vector2(MainCameraSingleton.instance.orthographicSize * 4f / 3f, MainCameraSingleton.instance.orthographicSize);
                position = MainCameraSingleton.instance.transform.position + new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y), 0f);
            }

            position.x -= MainCameraSingleton.instance.transform.position.x;
            position.y -= MainCameraSingleton.instance.transform.position.y;

            position.x += (MainCameraSingleton.instance.orthographicSize * 4f / 3f);
            position.x /= (MainCameraSingleton.instance.orthographicSize * 8f / 3f);
            position.y += MainCameraSingleton.instance.orthographicSize;
            position.y /= (MainCameraSingleton.instance.orthographicSize * 2f);
			time = 0;
		}

		public void Update()
		{
			time += Time.deltaTime;
		}

		public Vector4 MakeShaderParameter(float aspect)
		{
			return new Vector4(position.x * aspect, position.y, time, 0);
		}
	}

	Droplet[] droplets;
	Texture2D gradTexture;
	Material material;
	float timer;
	int dropCount;

	void UpdateShaderParameters()
	{
		var c = GetComponent<Camera>();

		material.SetVector("_Drop1", droplets[0].MakeShaderParameter(c.aspect));
		material.SetVector("_Drop2", droplets[1].MakeShaderParameter(c.aspect));
		material.SetVector("_Drop3", droplets[2].MakeShaderParameter(c.aspect));

		material.SetColor("_Reflection", reflectionColor);
		material.SetVector("_Params1", new Vector4(c.aspect, 1, 1 / waveSpeed, 0));
		material.SetVector("_Params2", new Vector4(1, 1 / c.aspect, refractionStrength, reflectionStrength));
	}

	void Awake()
	{
		droplets = new Droplet[3];
		for (byte b = 0; b < 3; droplets[b] = new Droplet(), b++);

		gradTexture = new Texture2D(2048, 1, TextureFormat.Alpha8, false);
		gradTexture.wrapMode = TextureWrapMode.Clamp;
		gradTexture.filterMode = FilterMode.Bilinear;
        for (var i = 0; i < gradTexture.width; i++) {
            var x = 1.0f / gradTexture.width * i;
            var a = waveform.Evaluate(x);
            gradTexture.SetPixel(i, 0, new Color(a, a, a, a));
        }
		gradTexture.Apply();

		material = new Material(shader);
		material.hideFlags = HideFlags.DontSave;
		material.SetTexture("_GradTex", gradTexture);

		UpdateShaderParameters();
	}

    void Start()
    {
        if (GameMenu.subMenu != GameMenu.SubMenu.Splash)
        {
            effectTime = .01f;
        }
    }

    void Update()
	{
		foreach (var d in droplets) d.Update();

		UpdateShaderParameters();

        if (effectTime > 0f)
            effectTime -= Time.deltaTime;
        else
        {
            dropTimer -= Time.deltaTime;
            if (randomDrops)
            {
                if (dropTimer <= 0f && GameMenu.subMenu == GameMenu.SubMenu.Title && !GameMenu.shifting)
                {
                    Emit(false);
                    dropTimer += dropInterval;
                }
            }
            else
            {
                if (rippleCoolTimer > 0f)
                    rippleCoolTimer -= Time.deltaTime;
                else if (Input.GetMouseButtonDown(0))
                    checkCollision();

            }
        }
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
    }

    void checkCollision()
    {
        Ray mouseRay = MainCameraSingleton.instance.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(mouseRay, Mathf.Infinity);

        if (hit && hit.collider.name == "Ripple Collider")
        {
            Emit(true);
            rippleCoolTimer = rippleCoolTime;
            dropTimer = dropInterval;
        }
    }

    public void Emit(bool mousePosition)
    {
        droplets[dropCount++ % droplets.Length].Reset(mousePosition);
    }
}
