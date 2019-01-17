using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaServeCustomer : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawnXRange;
    [SerializeField]
    private SpriteRenderer customerRenderer;
    [SerializeField]
    private SpriteRenderer foodRenderer;
    [SerializeField]
    private Animator rigAnimator;

    private MystiaServeCustomerData.Customer data;
    public MystiaServeCustomerData.Customer Data => data;

	void Awake ()
    {
        var x = MathHelper.randomRangeFromVector(spawnXRange);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}

    public void setData(MystiaServeCustomerData.Customer data)
    {
        this.data = data;
        customerRenderer.sprite = data.CustomerSprite;
    }

    public void serve()
    {
        foodRenderer.gameObject.SetActive(true);
        foodRenderer.sprite = data.FoodSprite;
        customerRenderer.sprite = data.ServedSprite;
        rigAnimator.SetTrigger("Serve");
        enabled = false;
    }
    
    void Update()
    {
        if (MicrogameController.instance.getVictoryDetermined() && !MicrogameController.instance.getVictory())
        {
            customerRenderer.sprite = data.FailSprite;
            rigAnimator.SetTrigger("Fail");
            enabled = false;
        }
    }
}
