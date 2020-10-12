using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShakingText : MonoBehaviour
{

    [SerializeField]
    private float shakeXRange = .1f;
    [SerializeField]
    private float shakeYRange = .1f;
    [SerializeField]
    private float shakeSpeed = 3f;
    [SerializeField]
    private float targetTiltMax = 60f;
    [SerializeField]
    private bool resetAnimDataOnTextChanged = true;

    private TextMeshPro tmproComponent;
    private string previousText;

    private List<CharAnimData> charAnimData;
    public class CharAnimData
    {
        public Vector3 position;
        public Vector3 goal;
        public float Tilt;
    }

    void Awake()
    {
        tmproComponent = GetComponent<TextMeshPro>();
        ResetAnimData();
        previousText = tmproComponent.text;
    }

    void ResetAnimData()
    {
        charAnimData = new List<CharAnimData>();
        for (int i = 0; i < tmproComponent.text.Length; i++)
        {
            AddNewAnimData();
        }
    }

    public void ResetCharOffsets()
    {
        if (tmproComponent == null)
            return;
        foreach (var charAnim in charAnimData)
        {
            charAnim.position = Vector2.zero;
        }
        tmproComponent.ForceMeshUpdate();
    }

    void AddNewAnimData()
    {
        var charAnim = new CharAnimData();
        ResetGoal(charAnim);
        charAnim.position = charAnim.goal;
        charAnim.Tilt = Random.Range(-targetTiltMax, targetTiltMax);
        charAnimData.Add(charAnim);
    }

    public void ResetGoal(CharAnimData charAnim)
    {
        if (tmproComponent == null)
            return;
        charAnim.goal = new Vector2(
            Random.Range(-shakeXRange, shakeXRange),
            Random.Range(-shakeYRange, shakeYRange));
        charAnim.goal = MathHelper.getVector2FromAngle(
            MathHelper.getAngle(charAnim.goal) + charAnim.Tilt,
            charAnim.goal.magnitude);
    }

    public void UpdateChar(CharAnimData charAnim)
    {
        if (tmproComponent == null)
            return;
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
        if (resetAnimDataOnTextChanged && !tmproComponent.text.Equals(previousText))
            ResetAnimData();
        else
        {
            while (tmproComponent.text.Length > charAnimData.Count)
                AddNewAnimData();
        }
        previousText = tmproComponent.text;

        var textInfo = tmproComponent.textInfo;

        tmproComponent.ForceMeshUpdate();
        for (int i = 0; i < tmproComponent.text.Length; i++)
        {
            var charInfo = tmproComponent.textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            var charAnim = charAnimData[i];
            var vertexIndex = charInfo.vertexIndex;
            var materialIndex = charInfo.materialReferenceIndex;
            var vertices = textInfo.meshInfo[materialIndex].vertices;

            UpdateChar(charAnim);
            var matrix = Matrix4x4.TRS(charAnim.position, Quaternion.identity, Vector3.one);

            for (int j = vertexIndex; j < vertexIndex + 4; j++)
            {
                vertices[j] = matrix.MultiplyPoint3x4(vertices[j]);
            }


            tmproComponent.UpdateVertexData();

        }
        //tmproComponent.mesh.vertices = vertices;
    }
}
