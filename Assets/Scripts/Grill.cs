using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : MonoBehaviour
{
    public GameObject sausagePrefab;
    public float secondsPerCookTick;
    public float cookTicksToBurn;

    private GameObject sausageCooking;
    private Sausage sausageCookingScript;
    private float timeBeingCooked;
    private int cookTick;

    private void Awake()
    {
        sausageCooking = null;
        sausageCookingScript = null;
        timeBeingCooked = 0.0f;
        cookTick = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(sausageCooking)
        {
            if(sausageCookingScript.GetBeingCooked())
            {
                timeBeingCooked += Time.deltaTime;

                if (timeBeingCooked >= secondsPerCookTick)
                {
                    timeBeingCooked = 0.0f;
                    ++cookTick;

                    if (cookTick == cookTicksToBurn - 1)
                    {
                        sausageCookingScript.SetCookState(Ingredient.COOKED_SAUSAGE);
                    }
                    else if (cookTick == cookTicksToBurn)
                    {
                        sausageCookingScript.SetCookState(Ingredient.BURNT_SAUSAGE);
                    }
                }
            }
            else
            {
                sausageCooking = null;
                sausageCookingScript = null;
                timeBeingCooked = 0.0f;
                cookTick = 0;
            }
        }
    }

    public void SpanwSausage()
    {
        sausageCooking = Instantiate(sausagePrefab, transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity);
        sausageCookingScript = sausageCooking.GetComponent<Sausage>();
    }
}
