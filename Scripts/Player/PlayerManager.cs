using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    InventoryManger inventoryManger;
    bool isOn;

    public float maxUvDistance;
    public int uvDamage;
    public GameObject uvLight;

    public int Health = 100;
    public GameObject restartMenu;

    public Transform TowerPos;

    public GameObject dialouge;

    public Image healthEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManger = gameObject.GetComponent<InventoryManger>();

        int quality = PlayerPrefs.GetInt("QualityLevel", 2);
        QualitySettings.SetQualityLevel(quality);

        int pos = PlayerPrefs.GetInt("Pos");
        if (SceneManager.GetActiveScene().buildIndex == 0 && pos == 1)
        {
            transform.position = TowerPos.position;
            PlayerPrefs.SetInt("Pos", 0);
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Health > 0 && Health < 40)
        {
            Color color = healthEffect.color;
            color.a = 0.11f;
            healthEffect.color = color;
        }else if(Health >= 40 && Health<= 80)
        {
            Color color = healthEffect.color;
            color.a = 0.07f;
            healthEffect.color = color;
        }else if(Health > 80 && Health <= 99)
        {
            Color color = healthEffect.color;
            color.a = 0.04f;
            healthEffect.color = color;
        }
        if (Health <= 0)
        {
            healthEffect.enabled = false;
            Time.timeScale = 0f;
            restartMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            gameObject.GetComponent<PlayerManager>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.F) && inventoryManger.findItem("UVLight"))
        {
            if (isOn)
            {
                uvLight.SetActive(false);
                isOn = false;
            }
            else
            {
                uvLight.SetActive(true);
                isOn = true;
            }
        }

        if (isOn)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxUvDistance))
            {
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("Hitted");
                    SpiderAI spAi = hit.collider.gameObject.GetComponent<SpiderAI>();
                    spAi.Health -= uvDamage * Time.deltaTime;
                }
            }
        }
    }

}
