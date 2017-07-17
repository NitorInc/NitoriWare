//Found this online, modified it

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasTextOutline : MonoBehaviour
{

    public float pixelSize = 1, sortingOrder;
    public int cloneCount = 8;
    public Color outlineColor = Color.black;
    public bool resolutionDependant = false;
    public int doubleResolution = 1024;
    public bool updateAttributes;

    private Text text;
    private RectTransform rectTransform;
    private Text[] childTexts;
    private RectTransform[] childRectTransforms;

    void Start()
    {
        text = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();

        childTexts = new Text[cloneCount];
        childRectTransforms = new RectTransform[cloneCount];
        for (int i = 0; i < cloneCount; i++)
        {
            GameObject outline = new GameObject("Text Outline", typeof(Text));

            //Renderer otherMeshRenderer = outline.GetComponent<Renderer>();
            //otherMeshRenderer.material = new Material(textRenderer.material);
            //otherMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //otherMeshRenderer.receiveShadows = false;
            //otherMeshRenderer.sortingLayerID = textRenderer.sortingLayerID;
            //otherMeshRenderer.sortingLayerName = textRenderer.sortingLayerName;
            //otherMeshRenderer.sortingOrder = textRenderer.sortingOrder;
            //childMeshRenderers[i] = otherMeshRenderer;

            childTexts[i] = outline.GetComponent<Text>();
            RectTransform outlineTransform = outline.GetComponent<RectTransform>();
            outlineTransform.SetParent(transform.parent == null ? transform : transform.parent);
            outlineTransform.sizeDelta = rectTransform.sizeDelta;
            outlineTransform.transform.localScale = transform.localScale;
            childRectTransforms[i] = outlineTransform;
            outlineTransform.anchorMax = rectTransform.anchorMax;
            outlineTransform.anchorMin = rectTransform.anchorMax;
            outlineTransform.pivot = rectTransform.pivot;
            updateAttributes = true;
        }
        Transform parent = transform.parent;

        Vector3 holdPosition = transform.localPosition;
        rectTransform.SetParent(null);
        rectTransform.SetParent(parent);
        transform.localPosition = holdPosition;
    }

    void LateUpdate()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        outlineColor.a = text.color.a * text.color.a;

        for (int i = 0; i < childTexts.Length; i++)
        {
            Text other = childTexts[i];
            other.text = text.text;
            other.fontSize = text.fontSize;

            if (updateAttributes)
            {
                other.color = outlineColor;
                other.alignment = text.alignment;
                //other.anchor = text.anchor;
                //other.characterSize = text.characterSize;
                other.font = text.font;
                other.fontStyle = text.fontStyle;
                //other.richText = text.richText;
                //other.tabSize = text.tabSize;
                other.lineSpacing = text.lineSpacing;
                //other.offsetZ = text.offsetZ;
                other.gameObject.layer = gameObject.layer;
            }

            RectTransform childTransform = childRectTransforms[i];
            childTransform.sizeDelta = rectTransform.sizeDelta;

            bool doublePixel = resolutionDependant && (Screen.width > doubleResolution || Screen.height > doubleResolution);
            Vector3 pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * getFunctionalPixelSize() : getFunctionalPixelSize());
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint +
                (pixelOffset * (float)Screen.currentResolution.width / 1280f));
            other.transform.position = worldPoint + new Vector3(0f, 0f, .001f);

            //Renderer otherMeshRenderer = childMeshRenderers[i];
            //otherMeshRenderer.sortingLayerID = textRenderer.sortingLayerID;
            //otherMeshRenderer.sortingLayerName = textRenderer.sortingLayerName;
        }
    }

    float getFunctionalPixelSize()
    {
        return pixelSize * 5f / Camera.main.orthographicSize;
    }

    Vector3 GetOffset(int i)
    {
        return (Vector3)MathHelper.getVector2FromAngle(360f * ((float)i / (float)cloneCount), 1f);
    }
}