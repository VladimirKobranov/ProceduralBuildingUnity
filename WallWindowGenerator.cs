using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWindowGenerator : MonoBehaviour
{
    //game objects
    public GameObject wallObject;
    public GameObject wallWindowframe;
    public GameObject[] wallStone;
    public GameObject[] wallGlass;
    public GameObject wallWindowRod;
    //params
    private double wallWindowRodHeight = 0.4;
    public double wallRodPercent;

    void Start()
    {   //set random percent
        wallRodPercent = Random.Range(0, 100);
        
        //spawn objects
        GameObject gameObject1 = Instantiate(wallObject, transform.position, transform.rotation);
        GameObject gameObject2 = Instantiate(wallWindowframe, transform.position, transform.rotation);
        GameObject gameObject3 = Instantiate(wallStone[Random.Range(0, wallStone.Length)], transform.position, transform.rotation);
        GameObject gameObject4 = Instantiate(wallWindowRod, transform.position, transform.rotation);

        if(Random.value < wallRodPercent/100)
        {
            //spawn objects
            GameObject gameObject5 = Instantiate(wallWindowRod, transform.position + new Vector3(0, Random.Range(0, (float)wallWindowRodHeight), 0), transform.rotation);
            GameObject gameObject6 = Instantiate(wallGlass[0], transform.position, transform.rotation);
            GameObject gameObject7 = Instantiate(wallGlass[0], transform.position, transform.rotation);
            //parent objects
            gameObject5.transform.parent = transform;
            gameObject6.transform.parent = transform;
            gameObject7.transform.parent = transform;
        }
        else
        {
            //spawn objects
            GameObject gameObject8 = Instantiate(wallGlass[Random.Range(1,wallGlass.Length)], transform.position, transform.rotation);
            //parent objects
            gameObject8.transform.parent = transform;
        }
        //parent objects
        gameObject1.transform.parent = transform;
        gameObject2.transform.parent = transform;
        gameObject3.transform.parent = transform;
        gameObject4.transform.parent = transform;
    }

}
