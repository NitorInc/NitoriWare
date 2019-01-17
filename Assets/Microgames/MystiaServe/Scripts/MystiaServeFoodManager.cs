using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MystiaServeFoodManager : MonoBehaviour
{
    [SerializeField]
    private Transform foodContainer;
    [SerializeField]
    private GameObject foodPrefab;
    [SerializeField]
    private MystiaServeFood tray;
    [SerializeField]
    private bool ignoreSiblingCollisions;

    private MystiaServeFood[] foods;

    public void createFood(MystiaServeCustomer[] customers)
    {
        foods = new MystiaServeFood[customers.Length];
        for (int i = 0; i < foods.Length; i++)
        {
            var newFood = Instantiate(foodPrefab, foodContainer).GetComponent<MystiaServeFood>();
            newFood.Customer = customers[i];
            foods[i] = newFood;
        }
    }

    public MystiaServeCustomer serveNextCustomer()
    {
        var food = foods.FirstOrDefault(a => a.OnTray);
        food.Customer.serve();
        food.serve();
        return food.Customer;
    }

    public void launchFoods()
    {
        if (ignoreSiblingCollisions)
        {
            var colliders = foods.Select(a => a.GetComponent<Collider2D>()).ToArray();
            var trayCollider = tray.GetComponent<Collider2D>();
            for (int i = 0; i < foods.Length; i++)
            {
                for (int j = i + 1; j < foods.Length; j++)
                {
                    Physics2D.IgnoreCollision(colliders[i], colliders[j]);
                }
                Physics2D.IgnoreCollision(colliders[i], trayCollider);
            }
        }

        foreach (var food in foods.Where(a => a.OnTray))
        {
            food.launch();
        }
        tray.launch();
    }
}
