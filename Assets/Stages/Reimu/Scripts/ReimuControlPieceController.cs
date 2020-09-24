using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuControlPieceController : MonoBehaviour
{
    [SerializeField]
    private ReimuControlPiece controlPiece;
    [SerializeField]
    private float separation;
    [SerializeField]
    private int xExtents = 8;
    [SerializeField]
    private int yExtents = 4;
    [SerializeField]
    private Vector2 appearTimeRange;

    private List<ReimuControlPiece> pieces;

    void Start ()
    {
        pieces = new List<ReimuControlPiece>();
        for (int i = -xExtents; i <= xExtents; i++)
        {
            for (int j = -yExtents; j <= yExtents; j++)
            {
                var piece = Instantiate(controlPiece, transform);
                var sortingOffset = pieces.Count * 10;
                pieces.Add(piece);
                piece.GetMask().transform.localPosition += new Vector3(i * separation, j * separation);
                piece.GetMask().frontSortingOrder += sortingOffset;
                piece.GetMask().backSortingOrder += sortingOffset;
                piece.GetSpriteRenderer().sortingOrder += sortingOffset;
            }
        }

        //ResetPieces();
	}

    public void ResetPieces()
    {
        if (!isActiveAndEnabled)
            return;
        foreach (var piece in pieces)
        {
            var c = piece.GetSpriteRenderer().color;
            c.a = 0f;
            piece.GetSpriteRenderer().color = c;
        }
    }

    public void StartPieces()
    {
        //if (!isActiveAndEnabled)
        //    return;

        var sprite = GetComponent<SpriteRenderer>().sprite;
        pieces.Shuffle();
        float currentAppearTime = 0f;
        foreach (var piece in pieces)
        {
            piece.SetAppearTime(currentAppearTime);
            piece.GetSpriteRenderer().sprite = sprite;
            currentAppearTime += MathHelper.randomRangeFromVector(appearTimeRange);
        }
    }
	
	void Update ()
    {
		
	}
}
