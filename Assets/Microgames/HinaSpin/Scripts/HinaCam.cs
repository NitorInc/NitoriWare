using UnityEngine;
using System.Collections.Generic;
using System;
public class HinaCam
{
    public static float camSize;
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

    public static Vector2 GCP()
    {
        camSize = Camera.main.orthographicSize - 3f;
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.x = Mathf.Clamp(position.x, Camera.main.transform.position.x - (camSize * 4f / 3f), Camera.main.transform.position.x + (camSize * 4f / 3f));
        position.y = 0f;
        return position;
    }
}