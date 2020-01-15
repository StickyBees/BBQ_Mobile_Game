using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sausage : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color cookedColor;
    public Color burntColor;

    private Ingredient cookState;
    private bool beingCooked;

    private void Awake()
    {
        cookState = Ingredient.UNCOOKED_SAUSAGE;
        beingCooked = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Ingredient GetCookState()
    {
        return cookState;
    }

    public void SetCookState(Ingredient newCookState)
    {
        switch(newCookState)
        {
            case Ingredient.COOKED_SAUSAGE:
                cookState = newCookState;
                spriteRenderer.color = cookedColor;
                break;

            case Ingredient.BURNT_SAUSAGE:
                cookState = newCookState;
                spriteRenderer.color = burntColor;
                break;
        }
    }

    public bool GetBeingCooked()
    {
        return beingCooked;
    }

    public void RemoveFromGrill()
    {
        beingCooked = false;
    }
}
