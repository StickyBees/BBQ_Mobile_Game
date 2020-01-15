using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Vector3 plateRespawnPosition;
    public GameObject modularBread;
    public GameObject modularCheese;
    public GameObject modularLettuce;
    public GameObject modularSauce;
    public GameObject modularSausage;
    public GameObject order_Water;

    private List<Ingredient> ingredientsOnPlate;
    private GameManager gameManager;
    private Vector3 waterOffset;

    private void Awake()
    {
        ingredientsOnPlate = new List<Ingredient>();
        waterOffset = new Vector3(0.5f, 0.0f, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public List<Ingredient> GetIngredients()
    {
        return ingredientsOnPlate;
    }

    public void ReturnPlate()
    {
        foreach (Transform child in transform)
        {
            if(child.tag != "Plate_Trigger")
            {
                Destroy(child.gameObject);
            }
        }
        transform.localPosition = plateRespawnPosition;
        ingredientsOnPlate.Clear();
    }

    private bool PlateContains(Ingredient ingredientIn)
    {
        foreach(Ingredient ingredient in ingredientsOnPlate)
        {
            if(ingredient == ingredientIn)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlatePlacement")
        {
            Placement placement = collision.GetComponent<Placement>();
            if(placement.GetPlate() == null)
            {
                placement.SetPlate(gameObject);
                transform.position = collision.transform.position;
            }
        }
    }

    public void PlaceIngredient(GameObject ingredientToPlace)
    {
        switch(ingredientToPlace.tag)
        {
            case "Bread":
                if(!PlateContains(Ingredient.BREAD))
                {
                    ingredientsOnPlate.Add(Ingredient.BREAD);
                    ingredientToPlace.tag = "Placed";
                    Destroy(ingredientToPlace);
                    GameObject bread = Instantiate(modularBread, transform);
                }
                break;

            case "Cheese":
                if(PlateContains(Ingredient.BREAD) && !PlateContains(Ingredient.CHEESE))
                {
                    ingredientsOnPlate.Add(Ingredient.CHEESE);
                    ingredientToPlace.tag = "Placed";
                    Destroy(ingredientToPlace);
                    GameObject cheese = Instantiate(modularCheese, transform);
                }
                break;

            case "Lettuce":
                if (PlateContains(Ingredient.BREAD) && !PlateContains(Ingredient.LETTUCE))
                {
                    ingredientsOnPlate.Add(Ingredient.LETTUCE);
                    ingredientToPlace.tag = "Placed";
                    Destroy(ingredientToPlace);
                    GameObject lettuce = Instantiate(modularLettuce, transform);
                }
                break;

            case "Sauce":
                if ((PlateContains(Ingredient.COOKED_SAUSAGE) || PlateContains(Ingredient.BURNT_SAUSAGE)) && !PlateContains(Ingredient.SAUCE))
                {
                    ingredientsOnPlate.Add(Ingredient.SAUCE);
                    ingredientToPlace.tag = "Placed";
                    Destroy(ingredientToPlace);
                    GameObject sauce = Instantiate(modularSauce, transform);
                }
                break;

            case "Sausage":
                if (PlateContains(Ingredient.BREAD) 
                    && !PlateContains(Ingredient.COOKED_SAUSAGE) 
                    && !PlateContains(Ingredient.BURNT_SAUSAGE) 
                    && !PlateContains(Ingredient.UNCOOKED_SAUSAGE))
                {
                    GameObject sausage = Instantiate(modularSausage, transform);
                    sausage.GetComponent<SpriteRenderer>().color = ingredientToPlace.GetComponent<SpriteRenderer>().color;

                    switch (ingredientToPlace.GetComponent<Sausage>().GetCookState())
                    {
                        case Ingredient.COOKED_SAUSAGE:
                            ingredientsOnPlate.Add(Ingredient.COOKED_SAUSAGE);

                            break;

                        case Ingredient.BURNT_SAUSAGE:
                            ingredientsOnPlate.Add(Ingredient.BURNT_SAUSAGE);
                            break;

                        case Ingredient.UNCOOKED_SAUSAGE:
                            ingredientsOnPlate.Add(Ingredient.UNCOOKED_SAUSAGE);
                            break;
                    }
                    ingredientToPlace.tag = "Placed";
                    Destroy(ingredientToPlace);
                }
                break;

            case "Water":
                if(!PlateContains(Ingredient.WATER))
                {
                    ingredientsOnPlate.Add(Ingredient.WATER);
                    ingredientToPlace.tag = "Placed";
                    Destroy(ingredientToPlace);
                    GameObject water = Instantiate(order_Water, transform.position + waterOffset, Quaternion.identity);
                    water.transform.SetParent(transform);
                }
                break;
        }
    }
}
