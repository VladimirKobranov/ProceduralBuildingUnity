using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLogoGenerator : MonoBehaviour
{
    // game objects
    public GameObject shopRack;
    public GameObject[] shopLogo;
    //params
    //public double logoPercentage;

    // Start is called before the first frame update
    void Start()
    {
        ////set percentage
        //logoPercentage = Random.Range(0, 100);
        //logoPercentage = logoPercentage / 100;

        GameObject gameObject0 = Instantiate(shopRack, transform.position, transform.rotation);
        gameObject0.transform.parent = transform;

        if (Random.value < 0.9)
        {
            GameObject gameObject1 = Instantiate(shopLogo[Random.Range(0, shopLogo.Length)], transform.position, transform.rotation);
            gameObject1.transform.parent = transform;
        }

    }
}
