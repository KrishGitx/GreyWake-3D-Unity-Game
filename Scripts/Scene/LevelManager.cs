using UnityEngine;

public class LevelManager : MonoBehaviour
{


    //Releasing Spiders
    public bool uvPickedUp;
    public Transform[] spawnPoints;
    public GameObject spider;

    int spawnAmount = 5;
    int currSpwan;
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
            while(currSpwan <= spawnAmount)
            {
                int ranPos = UnityEngine.Random.Range(0,spawnPoints.Length);
                Instantiate(spider, spawnPoints[ranPos].position,Quaternion.identity);
                currSpwan++;
                // if(currSpwan == spawnAmount -1) spwaned = true;
            }
            
        }
    }
}
