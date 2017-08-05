﻿using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    private static bool DisableGlobally = true;

	float deltaTime = 0.0f;

    void Start()
    {
        if (DisableGlobally)
        {
            enabled = false;
            return;
        }
    }
	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		fps *= Time.timeScale;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);


		//float trueFPS = 1f / Time.deltaTime;
		//if (trueFPS< 50f)
		//{
		//	Debug.Log("SLOW FPS: " + trueFPS + " " + Time.deltaTime);
		//}
	}
}