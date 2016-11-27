using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MasterSparkCodeCommand : MonoBehaviour {

    public MasterSparkCodeCommandType Input;
    public Sprite VisualArrow;
    public Sprite VisualAction;

	// Use this for initialization
	void Awake () {
        Input = (MasterSparkCodeCommandType)Random.Range(0, 5);
        MakeVisual();
    }

    public void MoveSelf(float direction)
    {
        var oldPosition = this.gameObject.transform.position;
        float time = 0;
        while (time < 1)
        {
            time += Time.timeScale / 0.5f;
            this.gameObject.transform.position = Vector3.Lerp(oldPosition, oldPosition + new Vector3(direction,0,0), time);
        }
    }

    public void DestroySelf()
    {
        // Add animation
        Destroy(this.gameObject);
    }

    void MakeVisual()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = VisualArrow;
        switch (Input)
        {
            case MasterSparkCodeCommandType.Up:
                this.gameObject.transform.Rotate(0, 0, 180);
                return;
            case MasterSparkCodeCommandType.Down:
                return;
            case MasterSparkCodeCommandType.Left:
                this.gameObject.transform.Rotate(0, 0, -90);
                return;
            case MasterSparkCodeCommandType.Right:
                this.gameObject.transform.Rotate(0, 0, 90);
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
