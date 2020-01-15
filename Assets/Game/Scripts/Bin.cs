using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Plate":
                collision.gameObject.GetComponent<Plate>().ReturnPlate();
                break;

            case "Plate_Trigger":
                // Do nothing
                break;

            default:
                Destroy(collision.gameObject);
                break;
        }
    }
}
