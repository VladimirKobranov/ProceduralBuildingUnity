using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//日本‼
//ウラディミル製
//作業開始日2022年6月26日

public class Generator : MonoBehaviour
{
    // tiles and xyz numbers
    private readonly int xTile =1 ;
    private readonly int yTile = 1;
    private readonly int zTile =1;
    public int xNumberLenght;
    public int yNumberHeight;
    public int zNumberWidth;
    // accessoires percentages
    public double wallAccessoriesPercentage;
    public double roofAccessoriesPercentage;
    //random seed
    public int randomSeed;

    //bounding box
    private Collider boundingBoxCollider;
    private Vector3 boundingBoxSize;
    public GameObject pickedObject; // picked object in scene
    public bool usePickedObjectForSizes;

    //objects arrays
    public GameObject[] wallObjects;
    public GameObject[] wallCornerObjects;
    public GameObject[] firstFloorWallsObjects;
    public GameObject[] firstFloorCornersObjects;
    public GameObject[] roofObjects;
    public GameObject[] roofCornerObjects;
    public GameObject[] roofCupObjects;
    public GameObject[] wallAccessories;
    public GameObject[] roofAccessories;

    void Start()
    {
        // half tiles for centering
        int xTileHalf = xTile * xNumberLenght/ 2;
        int zTileHalf = zTile * zNumberWidth/ 2;
        double upGroundValue = 0.5; //to set it to the ground by half size of a object
        wallAccessoriesPercentage = wallAccessoriesPercentage / 100; // get percent
        roofAccessoriesPercentage = roofAccessoriesPercentage/100;

        //setup a random seed
        Random.InitState(randomSeed);

        //picked object in scene boolean
        if (usePickedObjectForSizes == true) 
        {
            boundingBoxCollider = pickedObject.GetComponent<Collider>();
            //picks a bounding box from test object
            boundingBoxSize = boundingBoxCollider.bounds.size;
            //convert bounds to int x y z
            xNumberLenght = Mathf.RoundToInt(boundingBoxSize[0]);
            yNumberHeight = Mathf.RoundToInt(boundingBoxSize[1]);
            zNumberWidth = Mathf.RoundToInt(boundingBoxSize[2]);
        }
        else
        {
            //use standart sizes
        }

        if(xNumberLenght < 2 || zNumberWidth < 2 || yNumberHeight < 3) // checks if sizes are not proper
        {
            //do not spawn
        }
        else //spawn
        {
            for (var i = 0; i < xNumberLenght; i++) //front & back facade
            {
                for (var j = 0; j < yNumberHeight - 1; j++)
                {
                    if (j * yTile == 0) //first floor
                    {
                        if (i * xTile == 0 || i * xTile == xNumberLenght - 1) //first floor corners
                        {
                            if (i * xTileHalf == 0) // back corners
                            {
                                //left
                                Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0)));
                                //right
                                Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0)));
                            }
                            else // front corners
                            {
                                //left
                                Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 0, 0)));
                                //right
                                Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0)));
                            }
                        }
                        else //first floor
                        {
                            //front
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0)));
                            //back
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0)));
                        }
                    }
                    else//main walls
                    {
                        if (i * xTile == 0 || i * xTile == xNumberLenght - 1) // main walls corners
                        {
                            if (i * xTile == 0) //left corners
                            {
                                // left corner
                                Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0)));
                                // left back corner
                                Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0))); 
                            }
                            else //right corners
                            {
                                // right corner
                                Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 0, 0)));
                                // right front corner
                                Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0))); 
                            }
                        }
                        else // main walls
                        {
                            //front
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0)));
                            //back
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0))); 
                                                                                                                                                                                                                                               
                            if (Random.value < wallAccessoriesPercentage)  // wall accessories with percentage
                            {
                                //spawn
                                //front Accessories
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0)));
                                //back Accessories
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0))); 
                            }
                            else
                            {
                                //null
                            }
                        }
                    }
                }
            }
            for (var i = 0; i < zNumberWidth; i++) // left && right facade　
            {
                for (int j = 0; j < yNumberHeight - 1; j++)
                {
                    if (i * zNumberWidth == 0) // removes first elements
                    {
                        //nothing
                    }
                    else
                    {
                        if (j * yTile == 0) // first floor
                        {
                            // left
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3(0 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0)));
                            //right
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3(xTile * xNumberLenght - 1 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.identity); 
                        }
                        else//main walls
                        {
                            // left
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3(0 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0)));
                            //right
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3(xTile * xNumberLenght - 1 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.identity); 

                            // wall accessories with percentage
                            if (Random.value < wallAccessoriesPercentage)  //spawn
                            {
                                // left Accessories
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3(0 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0)));
                                //right Accessories
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3(xTile * xNumberLenght - 1 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.identity); 
                            }
                            else
                            {
                                //null
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < xNumberLenght; i++) // roof 
            {
                for (int j = 0; j <= zNumberWidth; j++)
                {
                    if (i * xTile == 0 && j * zTile == 0 || i * xTile == xNumberLenght - 1 && j * zTile == zNumberWidth || i * xTile == 0 && j * zTile == zNumberWidth || i * xTile == xNumberLenght - 1 && j * zTile == 0) // roof corners
                    {
                        if (i * xTile == 0 && j * zTile == 0) //front left
                        {
                            Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0)));
                        }
                        else if (i * xTile == xNumberLenght - 1 && j * zTile == 0) //front right
                        {
                            Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 0, 0)));
                        }
                        else if (i * xTile == 0 && j * zTile == zNumberWidth) //back left
                        {
                            Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0)));
                        }
                        else if (i * xTile == xNumberLenght - 1 && j * zTile == zNumberWidth)
                        {
                            Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0)));
                        }
                    }
                    else //main roof
                    {
                        if (j * zTile == 0)
                        {
                            Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0))); // left roof
                        }
                        else if (j * zTile == zNumberWidth)
                        {
                            Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0))); // right roof
                        }
                        else if (i * xTile == 0)
                        {
                            Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, -180, 0)));  // back roof
                        }
                        else if (i * xTile == xNumberLenght - 1)
                        {
                            Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.identity);  // front roof
                        }
                        else
                        {
                            Instantiate(roofCupObjects[Random.Range(0, roofCupObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.identity); // roof cup
                        }

                    }

                    if (i*xTile == 0 || j*zTile == 0 || i*xTile == xNumberLenght-1 || j*zTile == zNumberWidth)
                    {
                        //not spawn
                    }
                    else if(Random.value < roofAccessoriesPercentage) // spawn
                    {
                        // roof accessories w random rotate by 90 degree
                        Instantiate(roofAccessories[Random.Range(0, roofAccessories.Length)], new Vector3(i * xTile - xTileHalf, (float)(yTile * (yNumberHeight - 1) + upGroundValue), j * zTile - zTileHalf), Quaternion.Euler(new Vector3(0,Random.Range(0,3)*90,0))); 
                    }
                    else
                    {
                        //not spawn
                    }
                }
            }
            Debug.Log("Sizes: " +"x: "+ xNumberLenght +", "+ "y: " + yNumberHeight + ", " + "z: " + zNumberWidth);
            Debug.Log("Done");
        }
    }
}
