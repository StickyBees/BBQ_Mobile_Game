using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CustomerType { COMMONER, INITIATE, CHAMPION };
public enum Ingredient { WATER, BREAD, UNCOOKED_SAUSAGE, COOKED_SAUSAGE, BURNT_SAUSAGE, CHEESE, LETTUCE, SAUCE };

public class Customer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject orderBubblePrefab;
    public GameObject modularBreadPrefab;
    public GameObject waterSkinPrefab;
    public GameObject modularSausagePrefab;
    public GameObject modularSaucePrefab;
    public GameObject modularCheesePrefab;
    public GameObject modularLettucePrefab;
    public Color cookedSausageColor;
    public Color burntSausageColor;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip leaveWithFoodClip;
    public AudioClip leaveWithoutFoodClip;

    private GameManager gameManager;
    private int pointsOnSuccessfulOrder;
    private int pointsPerSecondSpare;
    private float timeToCompleteOrder;
    private float timePassed;
    private CustomerType customerType;
    private List<Ingredient> order;
    private bool orderComplete;
    private Vector2 orderBubblePos;
    private Vector3 waterOffset = new Vector3(0.65f, 5.3f, 0.0f);
    private bool leaving;

    private void Awake()
    {
        #region Initialise values
        Array values = Enum.GetValues(typeof(CustomerType));
        customerType = (CustomerType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        switch(customerType)
        {
            case CustomerType.COMMONER:
                pointsOnSuccessfulOrder = 5;
                pointsPerSecondSpare = 1;
                timeToCompleteOrder = 16;
                spriteRenderer.color = new Color(255, 0, 0);
                break;

            case CustomerType.INITIATE:
                pointsOnSuccessfulOrder = 8;
                pointsPerSecondSpare = 2;
                timeToCompleteOrder = 13;
                spriteRenderer.color = new Color(0, 255, 0);
                break;

            case CustomerType.CHAMPION:
                pointsOnSuccessfulOrder = 12;
                pointsPerSecondSpare = 3;
                timeToCompleteOrder = 10;
                spriteRenderer.color = new Color(0, 0, 255);
                break;
        }

        order = new List<Ingredient>();
        orderComplete = false;
        orderBubblePos = transform.position + new Vector3(0.0f, 5.0f, 0.0f);
        leaving = false;
        #endregion

        #region Create Order
        GameObject orderBubble = Instantiate(orderBubblePrefab, orderBubblePos, Quaternion.identity);
        orderBubble.transform.SetParent(transform);


        GameObject bread = Instantiate(modularBreadPrefab, orderBubblePos, Quaternion.identity);
        bread.transform.SetParent(transform);
        order.Add(Ingredient.BREAD);

        GameObject sausage = Instantiate(modularSausagePrefab, orderBubblePos, Quaternion.identity);
        sausage.transform.SetParent(transform);


        if (UnityEngine.Random.Range(1, 100) <= 50)
        {
            // Ask for water
            order.Add(Ingredient.WATER);
            GameObject water = Instantiate(waterSkinPrefab, transform.position + waterOffset, Quaternion.identity);
            water.transform.SetParent(transform);
        }

        if (customerType == CustomerType.COMMONER)
        {
            if (UnityEngine.Random.Range(1, 100) <= 10)
            {
                // Ask for burnt sausage
                order.Add(Ingredient.BURNT_SAUSAGE);
                sausage.GetComponent<SpriteRenderer>().color = burntSausageColor;
            }
            else
            {
                // Ask for normal sausage
                order.Add(Ingredient.COOKED_SAUSAGE);
                sausage.GetComponent<SpriteRenderer>().color = cookedSausageColor;
            }
        }
        else
        {
            order.Add(Ingredient.COOKED_SAUSAGE);
            sausage.GetComponent<SpriteRenderer>().color = cookedSausageColor;
        }

        if (UnityEngine.Random.Range(1, 100) <= 60)
        {
            // Ask for sauce on sausage
            order.Add(Ingredient.SAUCE);
            GameObject sauce = Instantiate(modularSaucePrefab, orderBubblePos, Quaternion.identity);
            sauce.transform.SetParent(transform);
        }

        if (UnityEngine.Random.Range(1, 100) <= 70)
        {
            // Ask for extras
            int extraRandom = UnityEngine.Random.Range(1, 100);
            if (extraRandom <= 50)
            {
                // Ask for cheese & tomato
                order.Add(Ingredient.CHEESE);
                GameObject cheese = Instantiate(modularCheesePrefab, orderBubblePos, Quaternion.identity);
                cheese.transform.SetParent(transform);
            }
            else if (extraRandom <= 80)
            {
                // Ask for lettuce & tomato
                order.Add(Ingredient.LETTUCE);
                GameObject lettuce = Instantiate(modularLettucePrefab, orderBubblePos, Quaternion.identity);
                lettuce.transform.SetParent(transform);
            }
            else
            {
                // Ask for cheese, lettuce & tomato
                order.Add(Ingredient.CHEESE);
                GameObject cheese = Instantiate(modularCheesePrefab, orderBubblePos, Quaternion.identity);
                cheese.transform.SetParent(transform);

                order.Add(Ingredient.LETTUCE);
                GameObject lettuce = Instantiate(modularLettucePrefab, orderBubblePos, Quaternion.identity);
                lettuce.transform.SetParent(transform);
            }
        }
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!animator)
            animator = GetComponent<Animator>();

        if (!audioSource)
            audioSource = GetComponent<AudioSource>();

        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!leaving)
        {
            timePassed += Time.deltaTime;

            if (timePassed >= timeToCompleteOrder)
            {
                // Leave without order
                
                audioSource.clip = leaveWithoutFoodClip;
                Leave();
            }
            else
            {
                if (orderComplete)
                {
                    // Leave with order
                    int scoreToAdd = pointsOnSuccessfulOrder;
                    int timeLeft = (int)(timeToCompleteOrder - timePassed);
                    scoreToAdd += timeLeft * pointsPerSecondSpare;

                    gameManager.IncrementPointsBy(scoreToAdd);
                    audioSource.clip = leaveWithFoodClip;
                    RemoveFood();
                    Leave();
                }
            }
        }
    }

    public bool CheckOrderIsComplete(List<Ingredient> ingredientsOnPlate)
    {
        bool tempOrderComplete = true;
        if (ingredientsOnPlate.Count == order.Count)
        {
            foreach (Ingredient ingredient in order)
            {
                if (!ingredientsOnPlate.Contains(ingredient))
                {
                    tempOrderComplete = false;
                    break;
                }
            }
        }
        else
        {
            tempOrderComplete = false;
        }

        if(tempOrderComplete)
        {
            orderComplete = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Leave()
    {
        animator.SetTrigger("Leave");
        leaving = true;
        audioSource.Play();
    }

    public void RemoveCustomer()
    {
        gameManager.RemoveCustomer(gameObject);
    }

    public void RemoveFood()
    {
        gameManager.RemoveFood(gameObject);
    }

    //public bool IsOrderComplete()
    //{
    //    return orderComplete;
    //}
}
