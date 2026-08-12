using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{

    Animator anim;
    bool closeItemFoundUI;
    bool enteringPin;
    public bool isDoorOpen;
    public bool canInteract = true;

    [SerializeField] string documentText;


    [Header("UI Components")]
    [SerializeField] GameObject itemFoundUI;
    [SerializeField] Image SpriteHandler;
    [SerializeField] Sprite UvLight;
    [SerializeField] TMP_Text docTxt;
    [SerializeField] GameObject enterPin;
    [SerializeField] GameObject FadeIn;
    [SerializeField] GameObject loadingTxt;



    [SerializeField] LevelManager lvlManager;
    [SerializeField] InventoryManger inventorySC;
    [SerializeField] PlayerMovement Player;


    AudioSource audioSrc;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        // inventorySC = GetComponent<InventoryManger>();
    }


    void Update()
    {
        if (closeItemFoundUI && Input.GetKeyDown(KeyCode.Escape) || enteringPin && Input.GetKeyDown(KeyCode.Escape))
        {
            itemFoundUI.SetActive(false);
            Time.timeScale = 1f;
            Player.enabled = true;
            if(enteringPin) enterPin.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            canInteract = false;
            closeItemFoundUI = false;
            enteringPin = false;
        }
    }


    public void Door(bool isOpen, float time)
    {
        if (isOpen)
        {
            //close the door
        }
        else
        {
            audioSrc = GetComponent<AudioSource>();
            canInteract = false;
            anim.SetBool("isOpen", true);
            audioSrc.Play();
            StartCoroutine(StartTimer("isOpen", time, false));
        }
    }

    public void ReadingDoc()
    {
        itemFoundUI.SetActive(true);
        docTxt.text = documentText;
        Time.timeScale = 0f;
        closeItemFoundUI = true;
    }

    public void ShipCabinet(bool isOpen)
    {
        Debug.Log("Reached here" + isOpen);
        if (isOpen)
        {
            // mbLight.SetActive(false);
            isDoorOpen = false;
            anim.SetBool("isOpen", false);
        }
        else
        {
            isDoorOpen = true;
            anim.SetBool("isOpen", true);
        }
    }
    public void LeverOn()
    {
        anim.SetBool("isOn", true);
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        for (int i = 0; i < drones.Length; i++)
        {
            drones[i].GetComponent<DroneAI>().enabled = false;
        }
        canInteract = false;
    }
    public void OpenCrate(string item)
    {
        //Show Ui- Update Inventory and Intansiate the obj into slot

        itemFoundUI.SetActive(true);
        SpriteHandler.sprite = UvLight;

        Time.timeScale = 0f;
        
        inventorySC.AddItem(item,true);
        closeItemFoundUI = true;
        lvlManager.uvPickedUp = true;
    }

    public void onInputChanged()
    {
        Debug.Log("onInputChanged called");
        TMP_InputField input = enterPin.transform.GetChild(0).GetComponent<TMP_InputField>();
        Debug.Log(input.text);
        if (input.text == "3678")
        {
            Debug.Log("Door Opene " + input.text);
            Player.enabled = true;
            Time.timeScale = 1f;
            enterPin.SetActive(false);
            gameObject.GetComponent<ItemId>().ItemName = "Door";
        }

    }
    public void LockedDoor()
    {
        enteringPin = true;

        enterPin.SetActive(true);
        TMP_InputField input = enterPin.transform.GetChild(0).GetComponent<TMP_InputField>();
        Player.enabled = false;
        Time.timeScale = 0f;

        EventSystem.current.SetSelectedGameObject(input.gameObject);
        input.Select();
        input.ActivateInputField();

    }

    public void EntranceDoor()
    {
        //Play Fade in;
        FadeIn.GetComponent<Animator>().SetBool("fadeIn",true);

        if(SceneManager.GetActiveScene().buildIndex == 0) StartCoroutine(loadScene(1));
        else StartCoroutine(loadScene(0));


    } 


    IEnumerator loadScene(int index)
    {
        yield return new WaitForSeconds(1.2f);
        loadingTxt.SetActive(true);
        SceneManager.LoadSceneAsync(index);
    }

    IEnumerator StartTimer(string triggerName, float duration, bool value)
    {
        yield return new WaitForSeconds(duration);

        anim.SetBool(triggerName, value);
        canInteract = true;
        if (triggerName == "isOpen")
        {
            yield return new WaitForSeconds(0.4f);
            audioSrc.Play();
        }
    }


}
