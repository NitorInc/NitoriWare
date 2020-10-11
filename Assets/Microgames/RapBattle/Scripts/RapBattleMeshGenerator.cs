using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RapBattleMeshGenerator : MonoBehaviour
{
    [SerializeField]
    private float refreshTime;
    [SerializeField]
    private Vector2 pointRandomizationRange;
    [SerializeField]
    private Color color;

    private float refreshTimer;

    void Start()
    {
        refresh();
        refreshTimer = refreshTime;
        InvokeRepeating("refresh", refreshTime - (Time.time % refreshTime), refreshTime);
    }

    void refresh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        var col = GetComponent<PolygonCollider2D>();

        var points = col.points.Select(a => new Vector2(
            a.x + Random.Range(-pointRandomizationRange.x, pointRandomizationRange.x),
            a.y + Random.Range(-pointRandomizationRange.y, pointRandomizationRange.y))).ToArray();
        var triangulator = new Triangulator(points);
        mesh.vertices = points.Select(a => (Vector3)a).ToArray();
        mesh.triangles = triangulator.Triangulate();
        mesh.colors = points.Select(a => color).ToArray();

        //mesh.RecalculateBounds();
        mesh.RecalculateNormals();

    }
}
