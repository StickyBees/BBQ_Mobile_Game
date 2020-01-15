using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTrigger : MonoBehaviour
{
    public PolygonCollider2D plateTrigger;
    public BoxCollider2D placementOneCollider;
    public BoxCollider2D placementTwoCollider;
    public BoxCollider2D placementThreeCollider;
    public BoxCollider2D placementFourCollider;
    public BoxCollider2D placementFiveCollider;

    private Plate plateScript;
    private List<string> ingredientTags;

    private void Awake()
    {
        plateScript = GetComponentInParent<Plate>();
        ingredientTags = new List<string>();
        ingredientTags.Add("Water");
        ingredientTags.Add("Sausage");
        ingredientTags.Add("Cheese");
        ingredientTags.Add("Lettuce");
        ingredientTags.Add("Sauce");
        ingredientTags.Add("Bread");
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!plateTrigger)
        {
            plateTrigger = GetComponent<PolygonCollider2D>();
        }

        Physics2D.IgnoreCollision(plateTrigger, placementOneCollider);
        Physics2D.IgnoreCollision(plateTrigger, placementTwoCollider);
        Physics2D.IgnoreCollision(plateTrigger, placementThreeCollider);
        Physics2D.IgnoreCollision(plateTrigger, placementFourCollider);
        Physics2D.IgnoreCollision(plateTrigger, placementFiveCollider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(ingredientTags.Contains(collision.tag))
            plateScript.PlaceIngredient(collision.gameObject);
    }
}
