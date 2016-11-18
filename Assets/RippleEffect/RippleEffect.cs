﻿//I just found this online as a quick thing to add to the title and edited it a bit

using UnityEngine;
using System.Collections;

public class RippleEffect : MonoBehaviour
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

		public void Reset()
		{
			//position = new Vector2(Random.value, Random.value);


			position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

			position.x -= Camera.main.transform.position.x;
			position.y -= Camera.main.transform.position.y;

			position.x += (Camera.main.orthographicSize * 4f / 3f);
			position.x /= (Camera.main.orthographicSize * 8f / 3f);
			position.y += Camera.main.orthographicSize;
			position.y /= (Camera.main.orthographicSize * 2f);
			position.y = 1f - position.y;

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
		for (var i = 0; i < gradTexture.width;var x = 1.0f / gradTexture.width * i,var a = waveform.Evaluate(x),
		gradTexture.SetPixel(i, 0, new Color(a, a, a, a)), i++);
		gradTexture.Apply();

		material = new Material(shader);
		material.hideFlags = HideFlags.DontSave;
		material.SetTexture("_GradTex", gradTexture);

		UpdateShaderParameters();
	}

	void Update()
	{
		foreach (var d in droplets) d.Update();

		UpdateShaderParameters();
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}

	public void Emit()
	{
		droplets[dropCount++ % droplets.Length].Reset();
	}
}
