using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothRackGenerator : MonoBehaviour
{
    //game objects
    public GameObject clothRack;
    public GameObject[] towels;
    
    private int towelsNumber;
    //private int towelsTransform;
    private int towelsNumberCount;
    private double towelPercentage;

    void Start()
    {
        //towelsNumber = towels.Length;
        towelPercentage = Random.Range(0, 100);
        towelPercentage = towelPercentage / 100;

        //create rack
        GameObject gameObject0 = Instantiate(clothRack, transform.position, transform.rotation);
        gameObject0.transform.parent = transform;

        for (int i = 0; i < towels.Length; i++)
        {
            towelsNumber = Random.Range(0, towels.Length);

            switch (towelsNumber) // set lenght of towel array
            {
                case 0:
                    //towel1
                    towelsNumberCount = 2;
                    break;
                case 1:
                    //towel2
                    towelsNumberCount = 3;
                    break;
                case 2:
                    //towel3
                    towelsNumberCount = 4;
                    break;
                case 3:
                    //towel4
                    towelsNumberCount = 4;
                    break;
                case 4:
                    //towel5
                    towelsNumberCount = 5;
                    break;
                case 5:
                    //towel
                    towelsNumberCount = 7;
                    break;
            }

            for (int j = 0; j < towelsNumberCount; j++)
            {
                switch (towelsNumber) // spawn a random selected towel
                {
                    case 0:
                        //towel1
                        if (Random.value < towelPercentage)
                        {
                            GameObject gameObject1 = Instantiate(towels[0],transform.position, transform.rotation);
                            gameObject1.transform.parent = transform;
                            //gameObject1.transform.position = transform.position;
                            gameObject1.GetComponentInParent<Transform>().localPosition = gameObject1.transform.localPosition + new Vector3((float)(i * 0.017), 0, (float)(j * 0.25));
                        }
                        break;
                    case 1: 
                        //towel2
                        if (Random.value < towelPercentage)
                        { 
                        GameObject gameObject1 = Instantiate(towels[1], transform.position, transform.rotation);
                            gameObject1.transform.parent = transform;
                            //gameObject2.transform.position = transform.position;
                            gameObject1.GetComponentInParent<Transform>().localPosition = gameObject1.transform.localPosition + new Vector3((float)(i * 0.017), 0, (float)(j * 0.15));
                        }
                        break;
                    case 2:
                        //towel3
                        if(Random.value < towelPercentage)
                        {
                         GameObject gameObject1 = Instantiate(towels[2],transform.position, transform.rotation);
                            gameObject1.transform.parent = transform;
                            //gameObject3.transform.position = transform.position;
                            gameObject1.GetComponentInParent<Transform>().localPosition = gameObject1.transform.localPosition + new Vector3((float)(i * 0.017), 0, (float)(j * 0.125));
                        }        
                        break;
                    case 3:
                        //towel4
                        if (Random.value < towelPercentage)
                        {
                         GameObject gameObject1 = Instantiate(towels[3], transform.position, transform.rotation);
                            gameObject1.transform.parent = transform;
                            //gameObject4.transform.position = transform.position;
                            gameObject1.GetComponentInParent<Transform>().localPosition = gameObject1.transform.localPosition + new Vector3((float)(i * 0.017), 0, (float)(j * 0.12));
                        }
                        break;
                    case 4:
                        //towel5
                        if (Random.value < towelPercentage)
                        {
                           GameObject gameObject1 = Instantiate(towels[4], transform.position, transform.rotation);
                            gameObject1.transform.parent = transform;
                            //gameObject5.transform.position = transform.position;
                            gameObject1.GetComponentInParent<Transform>().localPosition = gameObject1.transform.localPosition + new Vector3((float)(i * 0.017), 0, (float)(j * 0.1));
                        }
                        break;
                    case 5:
                        //towel
                        if (Random.value < towelPercentage)
                        {
                           GameObject gameObject1 = Instantiate(towels[5], transform.position, transform.rotation);
                            gameObject1.transform.parent = transform;
                            //gameObject6.transform.position = transform.position;
                            gameObject1.GetComponentInParent<Transform>().localPosition = gameObject1.transform.localPosition + new Vector3((float)(i * 0.017), 0, (float)(j * 0.075));
                        }
                        break;
                }
            }
        }
    }
}
