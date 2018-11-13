using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/MystiaServe/Customer Data")]
public class MystiaServeCustomerData : ScriptableObject
{
    [SerializeField]
    private Customer[] customers;
    public Customer[] Customers => customers;

    [System.Serializable]
    public class Customer
    {
        [SerializeField]
        private Sprite customerSprite;
        public Sprite CustomerSprite => customerSprite;
        [SerializeField]
        private Sprite servedSprite;
        public Sprite ServedSprite => servedSprite;
        [SerializeField]
        private Sprite failSprite;
        public Sprite FailSprite => failSprite;
        [SerializeField]
        private Sprite foodSprite;
        public Sprite FoodSprite => foodSprite;
    }
}
