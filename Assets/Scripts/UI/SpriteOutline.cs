using UnityEngine;
using System.Collections;

public class SpriteOutline : MonoBehaviour
{

	public float thickness;

	private GameObject[] sprites;

	void Start ()
	{
		sprites = new GameObject[8];
		for (int i = 0; i < 8; i++)
		{
			sprites[i] =  (GameObject)Instantiate(Resources.Load("Prefabs/OutlineSprite"), transform.position, transform.rotation);
			sprites[i].transform.parent = transform.parent;
			sprites[i].transform.localScale = transform.localScale;
			sprites[i].transform.parent = transform;
			sprites[i].transform.localPosition = getOffset(i);
		}
	}

	private Vector3 getOffset(int sprite)
	{
		float angle = (float)sprite * (Mathf.PI * 2f / 8f);
		Vector3 offset = MathHelper.getVector2FromAngle(angle, thickness) * Mathf.Rad2Deg;
		offset.z = .001f;
		return offset;
	}
}
