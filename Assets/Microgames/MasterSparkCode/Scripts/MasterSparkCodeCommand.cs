using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MasterSparkCodeCommand : MonoBehaviour {

    public MasterSparkCodeCommandType Input;
    public bool IsPressed = false;
    public Animator CommandAnimator;
    public Sprite VisualArrow;
    public Sprite VisualAction;
    public SpriteRenderer spriteRenderer;

    private void Update()
    {
        if (IsPressed)
            DestroySelf();
        if (MicrogameController.instance.getVictoryDetermined() && !MicrogameController.instance.getVictory())
            Destroy(gameObject);
    } 

    public void SetInput(MasterSparkCodeCommandType c)
    {
        Input = c;
        UpdateVisual();
    }

    public void ScaleSelf()
    {
        transform.localScale = new Vector3(1.25f, 1.25f, 1f);
        spriteRenderer.color = Color.white;
        //float time = 0;
        //while (time < 1)
        //{
        //	time += Time.deltaTime;
        //	var scaleStep = Mathf.Lerp(1.0f, 1.5f, time);
        //	gameObject.transform.localScale = new Vector3(scaleStep, scaleStep, 1.0f);

        //}
    }

    public void MoveSelf(float direction)
    {
        transform.Translate(direction, 0f, 0f, Space.World);
        //var oldposition = gameobject.transform.position;
        //float time = 0;
        //while (time < 1)
        //{
        //	time += time.deltatime;
        //	gameobject.transform.position = vector3.lerp(oldposition, oldposition + new vector3(direction, 0, 0), time);
        //}
    }

    public void SetPressed()
    {
        CommandAnimator.enabled = true;
        CommandAnimator.SetTrigger("stateVictory");
    }

    public void DestroySelf()
    {
        // Add animation
        Destroy(gameObject);
    }

    void UpdateVisual()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.GetComponent<SpriteRenderer>().sprite = VisualArrow;
        switch (Input)
        {
            case MasterSparkCodeCommandType.Up:
                gameObject.transform.Rotate(0, 0, 180, Space.World);
                return;
            case MasterSparkCodeCommandType.Down:
                return;
            case MasterSparkCodeCommandType.Left:
                gameObject.transform.Rotate(0, 0, -90, Space.World);
                return;
            case MasterSparkCodeCommandType.Right:
                gameObject.transform.Rotate(0, 0, 90, Space.World);
                return;
            case MasterSparkCodeCommandType.Action:
                gameObject.GetComponent<SpriteRenderer>().sprite = VisualAction;
                return;
            default:
                return;
        }
    }
}

public enum MasterSparkCodeCommandType { Up, Down, Left, Right, Action }
