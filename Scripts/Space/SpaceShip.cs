using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public GameObject NavCable;

    public GameObject Player;

    public Camera navCinematicCamera;
    public Camera mainCamera;

    public void RepairSpaceShip(string item)
    {
        if (item != "NavChip")
            return;

        NavCable.SetActive(true);

        Debug.Log("NavCable Repaired");

        Player.SetActive(false);
        mainCamera.enabled = false;
        navCinematicCamera.enabled = true;
        PlayerPrefs.SetInt("Repaired",1);
        PlayerPrefs.Save();

        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(2f);

        Player.SetActive(true);
        mainCamera.enabled = true;
        navCinematicCamera.enabled = false;
    }
}