using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MasterSparkCodeCommand : MonoBehaviour
{

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
  }

  public void MoveSelf(float direction) => transform.Translate(direction, 0f, 0f, Space.World);

  public void SetPressed()
  {
    CommandAnimator.enabled = true;
    CommandAnimator.SetTrigger("stateVictory");
  }

  public void DestroySelf() => Destroy(gameObject);

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
