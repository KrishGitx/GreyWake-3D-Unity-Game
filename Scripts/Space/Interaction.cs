using System;
using System.Collections;
using TMPro;
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

    [SerializeField] GameObject credsScreen;



    [SerializeField] LevelManager lvlManager;
    [SerializeField] InventoryManger inventorySC;
    [SerializeField] PlayerMovement Player;


    [SerializeField] GameObject dialouge;

    public Camera mainCamera;
    public Camera cinematicCam;


    public Animator camShake;
    public Animator Ship;
    public GameObject PlayerObj;
    public GameObject Ramp;

    AudioSource audioSrc;

    public AudioSource Horn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        // inventorySC = GetComponent<InventoryManger>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && closeItemFoundUI)
        {

            itemFoundUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            Player.enabled = true;
            canInteract = false;
            closeItemFoundUI = false;
        }
        if (closeItemFoundUI && Input.GetKeyDown(KeyCode.Escape) || enteringPin && Input.GetKeyDown(KeyCode.Escape))
        {
            if (lvlManager.uvPickedUp) Horn.Play();
            itemFoundUI.SetActive(false);
            Time.timeScale = 1f;
            Player.enabled = true;
            if (enteringPin)
            {
                enterPin.SetActive(false);
                canInteract = true;
            }else
            {
                canInteract = false;
            }
            Cursor.lockState = CursorLockMode.Locked;
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

        inventorySC.AddItem(item, true);
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

        Cursor.lockState = CursorLockMode.None;

        EventSystem.current.SetSelectedGameObject(input.gameObject);
        input.Select();
        input.ActivateInputField();

    }

    public void EntranceDoor()
    {
        //Play Fade in;
        FadeIn.GetComponent<Animator>().SetBool("fadeIn", true);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(loadScene(2));
            PlayerPrefs.SetInt("Pos", 1);
            PlayerPrefs.Save();
        }
        else StartCoroutine(loadScene(1));


    }

    public void shipLever()
    {
        int repaired = PlayerPrefs.GetInt("Repaired");
        if (repaired == 1)
        {
            Debug.Log("HERE");
            FadeIn.GetComponent<Animator>().SetBool("fadeIn", true);
            // credsScreen.SetActive(true);
            Ramp.SetActive(false);
            PlayerObj.SetActive(false);
            FadeIn.GetComponent<Animator>().SetBool("fadeIn", false);

            mainCamera.enabled = false;
            cinematicCam.enabled = true;

            camShake.SetBool("shake", true);
            Ship.SetBool("startLaunch", true);

            Invoke("EndCreds", 3.5f);

        }
        else
        {
            dialouge.GetComponent<TMP_Text>().text = "I need to put NavChip inside the board to the left";
            Invoke("dialougefadeout", 2f);
        }
    }

    void EndCreds()
    {
        FadeIn.GetComponent<Animator>().SetBool("fadeIn", true);
        credsScreen.SetActive(true);
        SceneManager.LoadScene(0);
    }

    void dialougefadeout()
    {
        dialouge.GetComponent<TMP_Text>().text = "";
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
