using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MasterSparkCodeCommand : MonoBehaviour {

    public MasterSparkCodeCommandType Input;
    public bool IsPressed = false;
    public Animator CommandAnimator;
    public Sprite VisualArrow;
    public Sprite VisualAction;

	// Use this for initialization
	void Awake () {
        SetInput((MasterSparkCodeCommandType)Random.Range(0, 5));
    }

    private void Update()
    {
        if (IsPressed)
            DestroySelf();
    } 

    public void SetInput(MasterSparkCodeCommandType c)
    {
        Input = c;
        UpdateVisual();
    }

    public void ScaleSelf()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            var scaleStep = Mathf.Lerp(1.0f, 1.5f, time);
            this.gameObject.transform.localScale = new Vector3(scaleStep, scaleStep, 1.0f);

        }
    }

    public void MoveSelf(float direction)
    {
        var oldPosition = this.gameObject.transform.position;
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            this.gameObject.transform.position = Vector3.Lerp(oldPosition, oldPosition + new Vector3(direction,0,0), time);
        }
    }

    public void SetPressed()
    {
        CommandAnimator.SetTrigger("stateVictory");
    }

    public void DestroySelf()
    {
        // Add animation
        Destroy(this.gameObject);
    }

    void UpdateVisual()
    {
        this.gameObject.transform.rotation = Quaternion.identity;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = VisualArrow;
        switch (Input)
        {
            case MasterSparkCodeCommandType.Up:
                this.gameObject.transform.Rotate(0, 0, 180, Space.World);
                return;
            case MasterSparkCodeCommandType.Down:
                return;
            case MasterSparkCodeCommandType.Left:
                this.gameObject.transform.Rotate(0, 0, -90, Space.World);
                return;
            case MasterSparkCodeCommandType.Right:
                this.gameObject.transform.Rotate(0, 0, 90, Space.World);
                return;
            case MasterSparkCodeCommandType.Action:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = VisualAction;
                return;
            default:
                return;
        }
    }
}

public enum MasterSparkCodeCommandType { Up, Down, Left, Right, Action }
