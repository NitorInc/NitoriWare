using UnityEngine;
using System.Collections;

public class SetAnimationParameter : MonoBehaviour
{
  public Animator animator;

  public FloatValue[] floatValues;
  public IntValue[] intValues;
  public BoolValue[] boolValues;


  [System.Serializable]
  public struct FloatValue
  {
    public string name;
    public float value;
  }

  [System.Serializable]
  public struct IntValue
  {
    public string name;
    public int value;
  }

  [System.Serializable]
  public struct BoolValue
  {
    public string name;
    public bool value;
  }


  void Start() => setParameters();


  public void setParameters()
  {
    for (int i = 0; i < floatValues.Length; i++)
    {
      animator.SetFloat(floatValues[i].name, floatValues[i].value);
    }

    for (int i = 0; i < intValues.Length; i++)
    {
      animator.SetInteger(intValues[i].name, intValues[i].value);
    }

    for (int i = 0; i < boolValues.Length; i++)
    {
      animator.SetBool(boolValues[i].name, boolValues[i].value);
    }
  }

}
