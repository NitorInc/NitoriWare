using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class ShakingText : MonoBehaviour
{

    [SerializeField]
    private float shakeXRange = .1f;
    [SerializeField]
    private float shakeYRange = .1f;
    [SerializeField]
    private float shakeSpeed = 3f;

    private TextMeshPro tmproComponenent;

    private List<CharAnimData> charAnimData;
    public class CharAnimData
    {
        public Vector3 position;
        public Vector3 goal;
    }

    void Awake()
    {
        tmproComponenent = GetComponent<TextMeshPro>();
        charAnimData = new List<CharAnimData>();
        for (int i = 0; i < tmproComponenent.text.Length; i++)
        {
            AddNewAnimData();
        }
    }

    public void ResetCharOffsets()
    {
        foreach (var charAnim in charAnimData)
        {
            charAnim.position = Vector2.zero;
        }
    }
    

    void AddNewAnimData()
    {
        var charAnim = new CharAnimData();
        ResetGoal(charAnim);
        charAnim.position = charAnim.goal;
        charAnimData.Add(charAnim);
    }

    public void ResetGoal(CharAnimData charAnim)
    {
        charAnim.goal = new Vector2(
            Random.Range(-shakeXRange, shakeXRange),
            Random.Range(-shakeYRange, shakeYRange));
    }

    public void UpdateChar(CharAnimData charAnim)
    {
        charAnim.position = Vector2.MoveTowards(charAnim.position, charAnim.goal, shakeSpeed * Time.deltaTime);
        if (MathHelper.Approximately((charAnim.position - charAnim.goal).magnitude, 0f, .001f))
            ResetGoal(charAnim);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        while (tmproComponenent.text.Length > charAnimData.Count)
            AddNewAnimData();

        tmproComponenent.ForceMeshUpdate();
        var vertices = tmproComponenent.mesh.vertices;
        for (int i = 0; i < tmproComponenent.text.Length; i++)
        {
            var charInfo = tmproComponenent.textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            var charAnim = charAnimData[i];
            var vertexIndex = charInfo.vertexIndex;

            UpdateChar(charAnim);
            var matrix = Matrix4x4.TRS(charAnim.position, Quaternion.identity, Vector3.one);

            for (int j = vertexIndex; j < vertexIndex + 4; j++)
            {
                vertices[j] = matrix.MultiplyPoint3x4(vertices[j]);
            }

            tmproComponenent.textInfo.characterInfo[i] = charInfo;
            tmproComponenent.UpdateVertexData();

        }
        tmproComponenent.mesh.vertices = vertices;
    }
}
