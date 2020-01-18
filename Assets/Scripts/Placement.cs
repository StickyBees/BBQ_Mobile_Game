using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    private GameObject customer;
    private Customer customerScript;

    private GameObject plate;
    private Plate plateScript;

    private void Awake()
    {
        customer = null;
        customerScript = null;

        plate = null;
        plateScript = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(customer != null && plate != null)
        {
            customer.GetComponent<Customer>().CheckOrderIsComplete(plate.GetComponent<Plate>().GetIngredients());
        }
    }

    public GameObject GetCustomer()
    {
        return customer;
    }

    public Customer GetCustomerScript()
    {
        return customerScript;
    }

    public void SetCustomer(GameObject newCustomer)
    {
        customer = newCustomer;
        if(customer)
        {
            customerScript = customer.GetComponent<Customer>();
        }
        else
        {
            customerScript = null;
        }
    }

    public GameObject GetPlate()
    {
        return plate;
    }

    public Plate GetPlateScript()
    {
        return plateScript;
    }

    public void SetPlate(GameObject newPlate)
    {
        plate = newPlate;
        if(plate)
        {
            plateScript = plate.GetComponent<Plate>();
        }
        else
        {
            plateScript = null;
        }
    }
}
