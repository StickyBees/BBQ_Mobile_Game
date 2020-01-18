using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variable Declaration and Initialisation
    public GameObject customerPrefab;
    public GameObject platePlacementOne;
    public GameObject platePlacementTwo;
    public GameObject platePlacementThree;
    public GameObject platePlacementFour;
    public GameObject platePlacementFive;
    public GameObject plateOne;
    public GameObject plateTwo;
    public GameObject plateThree;
    public GameObject breadPrefab;
    public GameObject cheesePrefab;
    public GameObject lettucePrefab;
    public GameObject saucePrefab;
    public GameObject waterPrefab;
    public GameObject endOfGameUI;
    public GameObject returnToMenuButton;
    public Text scoreText;
    public Text timerText;
    public Text endOfGameText;
    public float maxGameTime;
    public float customerInitialSpawnDelay;
    public float customerSpawnInterval;
    public float beginCustomerSpawnChance;
    public float middleCustomerSpawnChance;
    public float endCustomerSpawnChance;
    public int minPointsToWin;
    public int beginMaxNumOfCustomers;
    public int middleMaxNumOfCustomers;
    public int endMaxNumOfCustomers;
    public float beginTimeLimit;
    public float middleTimeLimit;
    public int dragLayer;
    public float gameTimePassed;
    public float customerSpawnTimePassed;
    public bool customerCanSpawn;
    public int score;
    public int numOfCustomers;
    public bool customerLimitReached;

    private float customerSpawnChance;
    private bool gameRunning;
    private GameObject objectToDrag;
    private List<Placement> placements;
    private Grill grillScript;

    private void Awake()
    {
        customerSpawnChance = beginCustomerSpawnChance;
        gameTimePassed = 0.0f;
        customerSpawnTimePassed = 0.0f;
        customerCanSpawn = false;
        score = 0;
        numOfCustomers = 0;
        customerLimitReached = false;
        gameRunning = true;
        objectToDrag = null;
        placements = new List<Placement>();
        placements.Add(platePlacementOne.GetComponent<Placement>());
        placements.Add(platePlacementTwo.GetComponent<Placement>());
        placements.Add(platePlacementThree.GetComponent<Placement>());
        placements.Add(platePlacementFour.GetComponent<Placement>());
        placements.Add(platePlacementFive.GetComponent<Placement>());
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!scoreText)
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        if (!timerText)
            timerText = GameObject.Find("TimerText").GetComponent<Text>();

        if (!endOfGameText)
            endOfGameText = GameObject.Find("EndGameText").GetComponent<Text>();

        endOfGameUI.SetActive(false);

        grillScript = GameObject.FindObjectOfType<Grill>();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
// Left in for marking //
#if UNITY_EDITOR
            if(Input.GetMouseButtonDown(0))
            {
                ClickDown();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ClickUp();
            }
            else
            {
                DragObject();
            }
#endif
// -- //
#if UNITY_ANDROID
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    ClickDown();
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ClickUp();
                }
                else
                {
                    DragObject();
                }
            }
#endif
            gameTimePassed += Time.deltaTime;

            if (gameTimePassed >= maxGameTime)
            {
                gameRunning = false;

                // End game
                endOfGameUI.SetActive(true);
                returnToMenuButton.SetActive(false);

                if (score >= minPointsToWin)
                {
                    // Player wins
                    endOfGameText.text = "You Win!";
                }
                else
                {
                    // Player loses
                    endOfGameText.text = "You Lose!";
                }

                Time.timeScale = 0;
            }
            else
            {
                float timerDisplay = maxGameTime - gameTimePassed;
                timerText.text = string.Format("{0:#00}:{1:00}", Mathf.Floor(timerDisplay / 60), Mathf.Floor(timerDisplay % 60));

                customerSpawnTimePassed += Time.deltaTime;

                if (customerCanSpawn)
                {
                    if (customerSpawnTimePassed >= customerSpawnInterval)
                    {
                        if (gameTimePassed <= beginTimeLimit)
                        {
                            if (numOfCustomers >= beginMaxNumOfCustomers)
                            {
                                customerLimitReached = true;
                            }
                            else
                            {
                                customerLimitReached = false;
                            }
                            customerSpawnChance = beginCustomerSpawnChance;
                        }
                        else if (gameTimePassed <= middleTimeLimit)
                        {
                            if (numOfCustomers >= middleMaxNumOfCustomers)
                            {
                                customerLimitReached = true;
                            }
                            else
                            {
                                customerLimitReached = false;
                            }
                            customerSpawnChance = middleCustomerSpawnChance;
                        }
                        else
                        {
                            if (numOfCustomers >= endMaxNumOfCustomers)
                            {
                                customerLimitReached = true;
                            }
                            else
                            {
                                customerLimitReached = false;
                            }
                            customerSpawnChance = endCustomerSpawnChance;
                        }

                        int randomResult = Random.Range(1, 100);
                        if (!customerLimitReached && randomResult <= customerSpawnChance)
                        {
                            SpawnCustomer();
                        }

                        customerSpawnTimePassed = 0.0f;
                    }
                }
                else
                {
                    if (customerSpawnTimePassed >= customerInitialSpawnDelay)
                    {
                        customerCanSpawn = true;
                        customerSpawnTimePassed = 0.0f;
                        SpawnCustomer();
                    }
                }
            }
        }
    }

    private void ClickDown()
    {
        var Hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Hit)
        {
            switch (Hit.tag)
            {
                case "BreadBasket":
                    // Spawn food
                    GameObject bread = Instantiate(breadPrefab, transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity);
                    SetDragObject(bread);
                    break;

                case "CheesePile":
                    GameObject cheese = Instantiate(cheesePrefab, transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity);
                    SetDragObject(cheese);
                    break;

                case "LettucePile":
                    GameObject lettuce = Instantiate(lettucePrefab, transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity);
                    SetDragObject(lettuce);
                    break;

                case "SaucePile":
                    GameObject sauce = Instantiate(saucePrefab, transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity);
                    SetDragObject(sauce);
                    break;

                case "Grill":
                    // Spawn sausage
                    grillScript.SpanwSausage();
                    break;

                case "WaterPile":
                    GameObject water = Instantiate(waterPrefab, transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity);
                    SetDragObject(water);
                    break;

                case "Bin":
                    // Do nothing
                    break;

                default:
                    if (Hit.gameObject.layer == dragLayer)
                    {
                        SetDragObject(Hit.gameObject);
                    }
                    break;
            }
        }
    }

    private void ClickUp()
    {
        if (objectToDrag)
        {
            switch (objectToDrag.tag)
            {
                case "Plate":
                    for (int i = 0; i < placements.Count; i++)
                    {
                        if (placements[i].GetPlate() == objectToDrag)
                        {
                            placements[i].SetPlate(null);
                        }
                    }
                    break;
            }
            objectToDrag.transform.position += new Vector3(0.0f, -0.1f, 0.0f);
            DropObjectToDrag();
        }
    }

    private bool SpawnCustomer()
    {
        bool customerSpawned = false;

        for(int i = 0; i < placements.Count && !customerSpawned; i++)
        {
            if(placements[i].GetCustomer() == null)
            {
                Vector2 spawnPos = placements[i].gameObject.transform.position + new Vector3(0, 1, 0);
                GameObject newCustomer = Instantiate(customerPrefab, spawnPos, Quaternion.identity);
                placements[i].SetCustomer(newCustomer);
                ++numOfCustomers;
                customerSpawned = true;
            }
        }

        return customerSpawned;
    }

    public void RemoveCustomer(GameObject customerToRemove)
    {
        for (int i = 0; i < placements.Count; i++)
        {
            if (placements[i].GetCustomer() == customerToRemove)
            {
                placements[i].SetCustomer(null);

                Destroy(customerToRemove);
                --numOfCustomers;
            }
        }
    }

    public void RemoveFood(GameObject customer)
    {
        for (int i = 0; i < placements.Count; i++)
        {
            if (placements[i].GetCustomer() == customer)
            {
                if (placements[i].GetPlate() != null)
                {
                    placements[i].GetPlateScript().ReturnPlate();
                    placements[i].SetPlate(null);
                }
            }
        }
    }

    public void IncrementPointsBy(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score: " + score;
    }

    private void SetDragObject(GameObject newObjectToDrag)
    {
        objectToDrag = newObjectToDrag;
        objectToDrag.GetComponent<Collider2D>().enabled = false;
        if(objectToDrag.transform.childCount > 0)
        {
            objectToDrag.transform.GetChild(0).gameObject.SetActive(false);
        }

        if(objectToDrag.tag == "Sausage")
        {
            objectToDrag.GetComponent<Sausage>().RemoveFromGrill();
        }
    }

    private void DragObject()
    {
        if (objectToDrag)
        {
            float oldZ = objectToDrag.transform.position.z;
            objectToDrag.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objectToDrag.transform.position = new Vector3(objectToDrag.transform.position.x, objectToDrag.transform.position.y, oldZ);
        }
    }

    private void DropObjectToDrag()
    {
        objectToDrag.GetComponent<Collider2D>().enabled = true;
        if (objectToDrag.transform.childCount > 0)
        {
            objectToDrag.transform.GetChild(0).gameObject.SetActive(true);
        }
        objectToDrag = null;
    }
}
