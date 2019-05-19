using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MystiaServeCustomerManager : MonoBehaviour
{
    [SerializeField]
    private MystiaServeCustomerData customerData;
    [SerializeField]
    private Transform customerContainer;
    public Transform CustomerContainer => customerContainer;

    private MystiaServeCustomer[] customers;
    public MystiaServeCustomer[] Customers => customers;

    void Awake()
    {
        var customerPool = new List<MystiaServeCustomerData.Customer>(customerData.Customers);
        customers = new MystiaServeCustomer[customerContainer.childCount];
        for (int i = 0; i < customerContainer.childCount; i++)
        {
            var newCustomer = customerContainer.GetChild(i).GetComponent<MystiaServeCustomer>();
            var chosenData = customerPool[Random.Range(0, customerPool.Count)];
            newCustomer.setData(chosenData);
            customerPool.Remove(chosenData);
            customers[i] = newCustomer;
        }
    }

    public void reverseCustomerPositions()
    {
        foreach (var customer in customers)
        {
            var trans = customer.transform;
            trans.position = new Vector3(-trans.position.x, trans.position.y, trans.position.z);
        }
    }
}
