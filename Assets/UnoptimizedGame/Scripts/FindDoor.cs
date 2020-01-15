using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindDoor : MonoBehaviour
{
    public Text leText;
    public int RayRange;
    public float doorDestroyedMessageLife;

    private bool doorDestroyed;
    private float timeSinceDoorDestroyed;

    private void Awake()
    {
        doorDestroyed = false;
        timeSinceDoorDestroyed = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!leText)
            leText = GameObject.Find("LeText").GetComponent<Text>();

        leText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (doorDestroyed)
        {
            timeSinceDoorDestroyed += Time.deltaTime;
            if (timeSinceDoorDestroyed >= doorDestroyedMessageLife)
            {
                Destroy(leText.gameObject);
                Destroy(this);
            }
        }
        else
        {
            leText.text = "";

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, RayRange))
            {
                if (hit.transform.tag == "Door")
                {
                    leText.text = "Tap the Screen to Break Down";
// Left in for marking //
#if UNITY_EDITOR
                    if(Input.GetMouseButtonDown(0))
                    {
                        Destroy(hit.transform.gameObject);
                        leText.text = "Door is now destroyed. Good job!";
                        doorDestroyed = true;
                    }
#endif
// -- //
                    if (Input.touchCount > 0 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Began)
                    {
                        Destroy(hit.transform.gameObject);
                        leText.text = "Door is now destroyed. Good job!";
                        doorDestroyed = true;
                    }
                }
            }
        }
    }
}
