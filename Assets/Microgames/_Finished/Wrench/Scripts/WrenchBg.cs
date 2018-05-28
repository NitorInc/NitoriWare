using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WrenchBg : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
	private bool update;
	[SerializeField]
	private int linesX, linesY;
	[SerializeField]
	private float lineSeparation, lineExtent;
	[SerializeField]
	private Vector3 linesStart;
    #pragma warning restore 0649

    private LineRenderer lineRenderer;

	void Update()
	{
		if (update)
		{
			if (lineRenderer == null)
				lineRenderer = GetComponent<LineRenderer>();
			createLines();
			update = false;
		}
	}

	void createLines()
	{
		List<Vector3> points = new List<Vector3>();

		for (int i = 0; i < linesX; i++)
		{
			float x = linesStart.x + (lineSeparation * (float)i),
				y = i % 2 == 1 ? lineExtent : -lineExtent;
			points.Add(new Vector3(x, y, 0f));
			points.Add(new Vector3(x, -y, 0f));
		}
		for (int i = 0; i < linesY; i++)
		{
			float y = linesStart.y + (lineSeparation * (float)i),
				x = i % 2 == 1 ? lineExtent : -lineExtent;
			points.Add(new Vector3(x, y, 0f));
			points.Add(new Vector3(-x, y, 0f));
		}

		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPositions(points.ToArray());
	}
}
