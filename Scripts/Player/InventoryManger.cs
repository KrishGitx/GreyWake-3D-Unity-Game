using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class InventoryManger : MonoBehaviour
{
    // List<string> Inventory = new List<string>();


    [SerializeField] GameObject[] Slots;
    [SerializeField] GameObject[] ItemList;

    [SerializeField] float waitForScroll;


     string[] Inventory = new string[4];
    int ActiveSlot = 0;

    public SpaceShip RepairScript;

    public TMP_Text pickupText;
    bool didHit;


    // Update is called once per frame
    void Update()
    {

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 1.2f))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.yellow);

            if (hit.collider.tag == "Item")
            {
                pickupText.gameObject.SetActive(true);
                Debug.Log("Press E to pick up");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    AddItem(hit.collider.GetComponent<ItemId>().ItemName);
                    Destroy(hit.collider.gameObject);
                    if (ActiveSlot == -1) ActiveSlot += 1;

                }
            }
            else if (hit.collider.tag == "RepairPart")
            {
                pickupText.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Inventory[ActiveSlot] == hit.collider.GetComponent<ItemId>().ItemName)
                    {
                        ItemId im = hit.collider.GetComponent<ItemId>();
                        if(im.RepairSystem == "NavSystem")
                        {
                        RepairScript.RepairNavigationSystem(hit.collider.GetComponent<ItemId>().ItemName);
                        }else if(im.RepairSystem == "LaunchBox")
                        {
                            RepairScript.RepairLaunch(hit.collider.GetComponent<ItemId>().ItemName);
                        }
                        Destroy(hit.collider.gameObject);
                        Inventory[ActiveSlot] = null;
                        Destroy(Slots[ActiveSlot].transform.GetChild(0).gameObject);
                    }
                    else
                    {
                        //Subtilte to show displya cable needed
                    }
                }
            }
            else if (hit.collider.tag == "Interactable")
            {
                if (hit.collider.GetComponent<Interaction>().canInteract)
                {
                    pickupText.gameObject.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        string item = hit.collider.GetComponent<ItemId>().ItemName;
                        switch (item)
                        {
                            case "Door":
                                hit.collider.GetComponent<Interaction>().Door(false,2f);
                                break;
                            case "Cabinet":
                                hit.collider.GetComponent<Interaction>().ShipCabinet(hit.collider.GetComponent<Interaction>().isDoorOpen);
                                break;
                            case "Crate":
                                hit.collider.GetComponent<Interaction>().OpenCrate("UVLight");
                                break;
                            case "Document":
                                hit.collider.GetComponent<Interaction>().ReadingDoc();
                                break;
                            case "Lever":
                                hit.collider.GetComponent<Interaction>().LeverOn();
                                break;
                            case "LockedDoor":
                                Debug.Log("Heresdfsd");
                                hit.collider.GetComponent<Interaction>().LockedDoor();
                                break;
                            case "EntranceDoor":
                                hit.collider.GetComponent<Interaction>().EntranceDoor();
                                break;
                        }

                    }
                }
            }
            else
            {
                pickupText.gameObject.SetActive(false);
            }
        }
        else
        {
            pickupText.gameObject.SetActive(false);
        }

        CheckForInput();


    }

    void CheckForInput()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActiveSlot = 0;
            EquipItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActiveSlot = 1;
            EquipItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActiveSlot = 2;
            EquipItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActiveSlot = 3;
            EquipItem();
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (waitForScroll > 0f)
        {
            waitForScroll -= Time.deltaTime;
        }

        if (scroll > 0f && waitForScroll <= 0f)
        {
            waitForScroll = 0.1f;
            ActiveSlot--;

            if (ActiveSlot <= 0) ActiveSlot = 3;
            EquipItem();
        }
        else if (scroll < 0f && waitForScroll <= 0f)
        {

            waitForScroll = 0.1f;
            ActiveSlot++;
            if (ActiveSlot > 3) ActiveSlot = 0;
            EquipItem();

        }

        if (Input.GetKeyDown(KeyCode.G) && ActiveSlot != 1 && Inventory[ActiveSlot] != null)
        {
            Slots[ActiveSlot].transform.GetChild(0).gameObject.SetActive(false);
            GameObject obj = Instantiate(Slots[ActiveSlot].transform.GetChild(0).gameObject, Slots[ActiveSlot].transform.position, Quaternion.identity);
            obj.SetActive(true);
            obj.GetComponent<Rigidbody>().isKinematic = false;

            Destroy(Slots[ActiveSlot].transform.GetChild(0).gameObject);

            Inventory[ActiveSlot] = null;
        }
    }

    void EquipItem()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (i == ActiveSlot) Slots[i].SetActive(true);
            else Slots[i].SetActive(false);
        }
    }

    public void AddItem(string item)
    {
        for (int i = 0; i < ItemList.Length; i++)
        {
            Debug.Log(ItemList[i].GetComponent<ItemId>().ItemName);
            if (item == ItemList[i].GetComponent<ItemId>().ItemName)
            {
                Debug.Log("Item Found!");
                for (int j = 0; j < 4; j++)
                {
                    if (Inventory[j] == null)
                    {
                        Inventory[j] = item;
                        Debug.Log("Invent: "+ Inventory[j]);
                        GameObject obj = Instantiate(ItemList[i], Slots[j].transform.position, Quaternion.identity, Slots[j].transform);
                        obj.layer = LayerMask.NameToLayer("Water");
                        break;
                    }
                }

            }
        }
    }

    public bool findItem(string item)
    {
        return Inventory.Contains(item);
    }



}