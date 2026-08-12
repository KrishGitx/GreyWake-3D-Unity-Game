using UnityEngine;

public class LevelManager : MonoBehaviour
{


    //Releasing Spiders
    public bool uvPickedUp;
    public Transform[] spawnPoints;
    public GameObject spider;

    int spawnAmount = 5;

    bool spwaned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (uvPickedUp && !spwaned)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                Instantiate(spider, spawnPoints[i].position,Quaternion.identity);
                if(i == spawnAmount -1) spwaned = true;
            }
        }
    }
}
