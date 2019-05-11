using UnityEngine;
using System.Collections;

public class TextOutline : MonoBehaviour
{

	public float pixelSize = 1;
	public int cloneCount = 8;
	public Color outlineColor = Color.black;
	public bool resolutionDependant = false;
	public int doubleResolution = 1024;
    public bool squareAlign = false;

    public bool updateAttributes;

	private TextMesh textMesh;
    private TextMesh[] childMeshes;
    private MeshRenderer[] childMeshRenderers;
	private MeshRenderer meshRenderer;

	void Start()
	{
		textMesh = GetComponent<TextMesh>();
		meshRenderer = GetComponent<MeshRenderer>();

        if (cloneCount != 8)
            squareAlign = false;

        childMeshes = new TextMesh[cloneCount];
        childMeshRenderers = new MeshRenderer[cloneCount];
		for (int i = 0; i < cloneCount; i++)
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
            childMeshRenderers[i] = otherMeshRenderer;

            childMeshes[i] = otherMeshRenderer.GetComponent<TextMesh>();
            updateAttributes = true;
		}
	}

	void LateUpdate()
	{
		Vector3 screenPoint = MainCameraSingleton.instance.WorldToScreenPoint(transform.position);

		outlineColor.a = textMesh.color.a * textMesh.color.a;
        
		for (int i = 0; i < childMeshes.Length; i++)
		{
            

			TextMesh other = childMeshes[i];
            other.text = textMesh.text;

            if (updateAttributes)
            {
                other.color = outlineColor;
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
                other.gameObject.layer = gameObject.layer;
            }

			bool doublePixel = resolutionDependant && (Screen.width > doubleResolution || Screen.height > doubleResolution);
			Vector3 pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * getFunctionalPixelSize() : getFunctionalPixelSize());
            Vector3 worldPoint = MainCameraSingleton.instance.ScreenToWorldPoint(screenPoint +
                (pixelOffset * ((float)Screen.currentResolution.width / 1400f)));
            other.transform.position = worldPoint + new Vector3(0f, 0f, .001f);

			MeshRenderer otherMeshRenderer = childMeshRenderers[i];
			otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
			otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
		}
	}

	float getFunctionalPixelSize()
	{
		return pixelSize * 5f / MainCameraSingleton.instance.orthographicSize;
	}

	Vector3 GetOffset(int i)
	{
        if (squareAlign)
            return MathHelper.getVector2FromAngle(360f * ((float)i / (float)cloneCount), i % 2 == 0 ? 1f : Mathf.Sqrt(2f));
        else
            return MathHelper.getVector2FromAngle(360f * ((float)i / (float)cloneCount), 1f);
	}
}