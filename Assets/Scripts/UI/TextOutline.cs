//Found this online

using UnityEngine;
using System.Collections;

public class TextOutline : MonoBehaviour
{

	public float pixelSize = 1;
	public Color outlineColor = Color.black;
	public bool resolutionDependant = false;
	public int doubleResolution = 1024;

	private TextMesh textMesh;
	private MeshRenderer meshRenderer;

	void Start()
	{
		textMesh = GetComponent<TextMesh>();
		meshRenderer = GetComponent<MeshRenderer>();

		for (int i = 0; i < 8; i++)
		{
			GameObject outline = new GameObject("outline", typeof(TextMesh));
			outline.transform.parent = transform;
			outline.transform.localScale = new Vector3(1, 1, 1);

			MeshRenderer otherMeshRenderer = outline.GetComponent<MeshRenderer>();
			otherMeshRenderer.material = new Material(meshRenderer.material);
			otherMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			otherMeshRenderer.receiveShadows = false;
			otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
			otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
			otherMeshRenderer.sortingOrder = meshRenderer.sortingOrder;
		}
	}

	void LateUpdate()
	{
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

		outlineColor.a = textMesh.color.a * textMesh.color.a;

		// copy attributes
		for (int i = 0; i < transform.childCount; i++)
		{

			TextMesh other = transform.GetChild(i).GetComponent<TextMesh>();
			other.color = outlineColor;
			other.text = textMesh.text;
			other.alignment = textMesh.alignment;
			other.anchor = textMesh.anchor;
			other.characterSize = textMesh.characterSize;
			other.font = textMesh.font;
			other.fontSize = textMesh.fontSize;
			other.fontStyle = textMesh.fontStyle;
			other.richText = textMesh.richText;
			other.tabSize = textMesh.tabSize;
			other.lineSpacing = textMesh.lineSpacing;
			other.offsetZ = textMesh.offsetZ;

			bool doublePixel = resolutionDependant && (Screen.width > doubleResolution || Screen.height > doubleResolution);
			Vector3 pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * pixelSize : pixelSize);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint + pixelOffset);
			other.transform.position = worldPoint + new Vector3(0f, 0f, .001f);

			MeshRenderer otherMeshRenderer = transform.GetChild(i).GetComponent<MeshRenderer>();
			otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
			otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
		}
	}

	private static Vector3[] offsets = {
		new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(1, -1, 0),
		new Vector3(0, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 0, 0), new Vector3(-1, 1, 0)
	};

	Vector3 GetOffset(int i)
	{
		return offsets[i % 8];
	}
}