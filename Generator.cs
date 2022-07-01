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

    void Start()
    {
        // half tiles for centering
        int xTileHalf = xTile * xNumberLenght/ 2;
        int zTileHalf = zTile * zNumberWidth/ 2;
        double upGroundValue = 0.5; //to set it to the ground by half size of a object
        wallAccessoriesPercentage = wallAccessoriesPercentage / 100; // get percent

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
                                Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0)));
                                Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0)));
                            }
                            else // front corners
                            {
                                Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 0, 0)));
                                Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0)));
                            }
                        }
                        else //first floor
                        {
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0))); //front
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0)));

                        }
                    }
                    else//main walls
                    {
                        if (i * xTile == 0 || i * xTile == xNumberLenght - 1) // main walls corners
                        {
                            if (i * xTile == 0) //left corners
                            {
                                Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0))); // left corner
                                Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0))); // left back corner
                            }
                            else //right corners
                            {
                                Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 0, 0))); // right corner
                                Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0))); // right front corner
                            }
                        }
                        else // main walls
                        {
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0))); //front
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0))); //back
                                                                                                                                                                                                                                               // wall accessories with percentage
                            if (Random.value < wallAccessoriesPercentage)  //spawn
                            {
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), 0 - zTileHalf), Quaternion.Euler(new Vector3(0, 90, 0))); //front Accessories
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3(i * xTile - xTileHalf, (float)(j * yTile + upGroundValue), zTile * zNumberWidth - zTileHalf), Quaternion.Euler(new Vector3(0, -90, 0))); //back Accessories
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
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3(0 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0))); // left
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3(xTile * xNumberLenght - 1 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.identity); //right
                        }
                        else//main walls
                        {
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3(0 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0))); // left
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3(xTile * xNumberLenght - 1 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.identity); //right

                            // wall accessories with percentage
                            if (Random.value < wallAccessoriesPercentage)  //spawn
                            {
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3(0 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.Euler(new Vector3(0, 180, 0))); // left Accessories
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3(xTile * xNumberLenght - 1 - xTileHalf, (float)(j * yTile + upGroundValue), i * zTile - zTileHalf), Quaternion.identity); //right Accessories
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
                }
            }
            Debug.Log("Sizes: " +"x: "+ xNumberLenght +", "+ "y: " + yNumberHeight + ", " + "z: " + zNumberWidth);
            Debug.Log("Done");
        }
    }
}