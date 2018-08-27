using UnityEngine;
using System.Collections.Generic;
using System;
public class HinaCam
{
	public static float camSize;
    public static float camSize2;
    public static Vector3 lastCursorPosition;

    private static CameraCollisionResults cameraCollisionResults = new CameraCollisionResults(-1);
    private struct CameraCollisionResults
    {
        public int evaluatedFrame;
        public Dictionary<int, RaycastHit2D> calculatedHits;

        public CameraCollisionResults(int evaluatedFrame)
        {
            this.evaluatedFrame = evaluatedFrame;
            calculatedHits = new Dictionary<int, RaycastHit2D>();
        }
    }
	public static Vector3 GetCursorPosition()
	{
		return getCursorPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
	}
	public static Vector3 getCursorPosition(float z)
	{
		if (Camera.main.orthographic)
		{
			camSize = Camera.main.orthographicSize - 3f;
            camSize2 = Camera.main.orthographicSize - 2f;

            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(position.x > 0f)
			{
                position.x = Mathf.Clamp(position.x, Camera.main.transform.position.x - (camSize * -4f / -3f),
                Camera.main.transform.position.x + (camSize * -4f / -3f));
            }
            else
			{
				position.x = Mathf.Clamp(position.x, Camera.main.transform.position.x - (camSize2 * 4f / 3f),
					Camera.main.transform.position.x + (camSize2 * 4f / 3f));
			}
			position.y = 0.0f;
			position.z = 0.0f;
			lastCursorPosition = position;
			return position;
		}
        else
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
            float distance;
            xy.Raycast(mouseRay, out distance);
            return mouseRay.GetPoint(distance);
        }
    }
	public static bool isMouseOver(Collider2D collider, float distance = float.PositiveInfinity, int layerMask = Physics2D.DefaultRaycastLayers)
	{
        if (cameraCollisionResults.evaluatedFrame != Time.frameCount)
            cameraCollisionResults = new CameraCollisionResults(Time.frameCount);
        else if (cameraCollisionResults.calculatedHits.ContainsKey(layerMask))
            return (cameraCollisionResults.calculatedHits[layerMask].collider == collider);
        
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection(mouseRay, distance, layerMask);
        cameraCollisionResults.calculatedHits[layerMask] = hit;
        return (hit.collider == collider);
    }
}
