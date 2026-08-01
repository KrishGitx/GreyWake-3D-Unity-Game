using System.Collections;
using System.Linq;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{

    int LaunchParts;
    int NavigationSystemParts;
    int EngineParts;

    public GameObject[] NavSystemParts; // 0-Battery 1-Cables 2-Scrap
    public GameObject[] LaunchBoxPos;
   

    public GameObject Player; 

    public Camera navCinematicCamera;
    public Camera mainCamera;
  
    public void RepairLaunch(string item)
    {
        if (item == "Cable")
        {
            LaunchParts++;
            LaunchParts++;
            // GameObject obj = Instantiate(LaunchBoxParts[1], LaunchBoxPos[1].transform.position, Quaternion.identity, LaunchBoxPos[1].transform);
            // obj.transform.Rotate(-90f,-90f,0f);
            // obj.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
        }
        else if (item == "LQScrap")
        {
            LaunchParts++;
        }
        else if (item == "Battery")
        {
            LaunchParts++;
            // NavSystemParts[1].SetActive(true);
            // GameObject obj = Instantiate(LaunchBoxParts[0], LaunchBoxPos[0].transform.position, Quaternion.identity, LaunchBoxPos[0].transform);
            // obj.transform.Rotate(-90f,0,0);
            // obj.transform.localScale = new Vector3(0.03f,0.03f,0.03f);


        }

        Debug.Log(item + " Got Repaied");
        if (LaunchParts >= 3)
        {
            Debug.Log("LaunchPort Is Ready");
        }

    }

    public void RepairNavigationSystem(string item)
    {
        if(item == "NavigationChip")
        {
            NavigationSystemParts++;
        }else if(item == "DisplayPlug")
        {
            NavigationSystemParts++;
        }else if (item == "NavCable")
        {
            NavigationSystemParts++;
            NavSystemParts[1].SetActive(true);
            Debug.Log("WIRE ADDED");
            Player.SetActive(false);
            mainCamera.enabled = false;
            navCinematicCamera.enabled = true;
            StartCoroutine(time());
        }
    }

    public void RepairEngine(string item)
    {
        if(item == "Fuel")
        {
            EngineParts++;
        }else if(item == "Fluid")
        {
            EngineParts++;
        }else if (item == "SparkPlug")
        {
            EngineParts++;
        }
    }

    IEnumerator time()
    {
        yield return new WaitForSeconds(2f);
        Player.SetActive(true);
        mainCamera.enabled = true;
        navCinematicCamera.enabled = false;
    }
}
