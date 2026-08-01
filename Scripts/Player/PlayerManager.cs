using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InventoryManger inventoryManger;
    bool isOn;

    public float maxUvDistance;
    public int uvDamage;
    public GameObject uvLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManger = gameObject.GetComponent<InventoryManger>();
    }

    // Update is called once per frame
    void Update()
    {
        // && inventoryManger.Inventory.Contains("UVLight")
        if(Input.GetKeyDown(KeyCode.F) && inventoryManger.findItem("UVLight"))
        {
            if (isOn)
            {
                uvLight.SetActive(false);
                isOn = false;
            }
            else
            {
                uvLight.SetActive(true);
                isOn =  true;
            }
        }

        if (isOn)
        {
            if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out RaycastHit hit,maxUvDistance))
            {
                if(hit.collider.tag == "Enemy")
                {
                    Debug.Log("Hitted");
                    SpiderAI spAi = hit.collider.gameObject.GetComponent<SpiderAI>();
                    spAi.Health -= uvDamage * Time.deltaTime;
                }
            }
        }
    }
}
