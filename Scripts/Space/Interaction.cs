using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{

    Animator anim;
    bool closeItemFoundUI;
    public bool isDoorOpen;
    public bool canInteract = true;

    [SerializeField] string documentText;
    

    [Header("UI Components")]
    [SerializeField] GameObject itemFoundUI;
    [SerializeField] Image SpriteHandler;
    [SerializeField] Sprite UvLight;
    [SerializeField] TMP_Text docTxt;
    
    

    [SerializeField] LevelManager lvlManager;
    [SerializeField] InventoryManger inventorySC;


     AudioSource audioSrc;
    //GameObeject
    // [Header("Ship Objcets")]
    // public GameObject mbLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }


    void Update()
    {
        if (closeItemFoundUI && Input.GetKeyDown(KeyCode.Escape))
        {
             itemFoundUI.SetActive(false);
             Time.timeScale = 1f;
             canInteract = false;
             closeItemFoundUI = false;
        }
    }


    public void Door(bool isOpen,float time)
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
            // mbLight.SetActive(true);
            anim.SetBool("isOpen", true);
        }
    }

    public void OpenCrate(string item)
    {
        //Show Ui- Update Inventory and Intansiate the obj into slot

        itemFoundUI.SetActive(true);
        SpriteHandler.sprite = UvLight;

        Time.timeScale = 0f;

        inventorySC.AddItem(item);
        Debug.Log("ADDED "+item+"  to inventory");
        closeItemFoundUI = true;
    }

    IEnumerator StartTimer(string triggerName, float duration, bool value)
    {
        yield return new WaitForSeconds(duration);

        anim.SetBool(triggerName, value);
        canInteract = true;
        if(triggerName == "isOpen") 
        {
            yield return new WaitForSeconds(0.4f);
            audioSrc.Play();
        }
    }
}
