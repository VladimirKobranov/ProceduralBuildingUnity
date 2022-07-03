using UnityEngine;

//日本‼
//ウラディミル製
//作業開始日2022年6月26日

public class Generator : MonoBehaviour
{
    // tiles and xyz numbers
    private readonly int xTile = 1;
    private readonly int yTile = 1;
    private readonly int zTile = 1;
    public int xNumberLenght;
    public int yNumberHeight;
    public int zNumberWidth;
    //half tiles 
    private double xTileHalf;
    private double zTileHalf;
    // accessoires percentages
    public double wallAccessoriesPercentage;
    public double firstFloorAccessoriesPercentage;
    public double roofAccessoriesPercentage;
    //random seed
    public int randomSeed;
    //brandmauer
    public bool brandmauer;

    private double upGroundValue;

    //bounding box
    private Collider boundingBoxCollider;
    private Vector3 boundingBoxSize;
    public GameObject pickedObject; // picked object in scene
    public bool usePickedObjectForSizes;
    public Transform pickedObjectPosition;
    public bool usePickedObjectPosition;

    //objects arrays
    public GameObject[] wallObjects;
    public GameObject[] wallCornerObjects;
    public GameObject[] firstFloorWallsObjects;
    public GameObject[] firstFloorCornersObjects;
    public GameObject[] roofObjects;
    public GameObject[] roofCornerObjects;
    public GameObject[] roofCupObjects;
    //brandmauer
    public GameObject[] wallBrandmauer;
    public GameObject[] roofBrandmauer;
    public GameObject[] roofCornerBrandmauer;
    public GameObject[] firstFloorCornerBrandmauer;
    //accessories
    public GameObject[] wallAccessories;
    public GameObject[] roofAccessories;
    public GameObject[] firstFloorAccessories;

    public void Awake()
    {
        // get percent
        wallAccessoriesPercentage = wallAccessoriesPercentage / 100; //wall
        roofAccessoriesPercentage = roofAccessoriesPercentage / 100; //roof
        firstFloorAccessoriesPercentage = firstFloorAccessoriesPercentage / 100; //first floor

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

            upGroundValue = 0 - (float)(yNumberHeight) / 2 + 0.5; ; //to set it to the ground by half size of a object
        }
        else
        {
            //use standart sizes
            upGroundValue = 0.5; //to set it to the ground by half size of a object
        }
        // half tiles for centering
        xTileHalf = (float)(xTile * xNumberLenght) / 2;
        zTileHalf = (float)(zTile * zNumberWidth) / 2;
    }
    void Start()
    {
        if (usePickedObjectPosition == true) //set initial position from picked object
        {
            pickedObjectPosition.position = pickedObjectPosition.position;
        }
        else // use zero
        {
            pickedObjectPosition.position = Vector3.zero;
        }
        if (xNumberLenght < 2 || zNumberWidth < 2 || yNumberHeight < 3) // checks if sizes are not proper
        {
            //do not spawn
        }
        else //spawn
        {
            for (var i = 0; i < xNumberLenght + 1; i++) //front & back facade
            {
                for (var j = 0; j < yNumberHeight - 1; j++)
                {
                    if (j * yTile == 0) //first floor
                    {
                        if (i * xTile == 0 || i * xTile == xNumberLenght) //first floor corners
                        {
                            if (brandmauer == true)//checks brandmauer
                            {
                                if (i * xTileHalf == 0) // back corners
                                {
                                    //right
                                    GameObject gameObject1 = Instantiate(firstFloorCornerBrandmauer[Random.Range(0, firstFloorCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    gameObject1.transform.localScale = new Vector3(1, 1, 1);
                                    //left
                                    GameObject gameObject2 = Instantiate(firstFloorCornerBrandmauer[Random.Range(0, firstFloorCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    gameObject2.transform.localScale = new Vector3(-1, 1, 1);
                                }
                                else // front corners
                                {
                                    //right
                                    GameObject gameObject1 = Instantiate(firstFloorCornerBrandmauer[Random.Range(0, firstFloorCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    gameObject1.transform.localScale = new Vector3(-1, 1, 1);

                                    //left
                                    GameObject gameObject2 = Instantiate(firstFloorCornerBrandmauer[Random.Range(0, firstFloorCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)((zTile * zNumberWidth) - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    gameObject2.transform.localScale = new Vector3(1, 1, 1);
                                }
                            }
                            else
                            {
                                if (i * xTileHalf == 0) // back corners
                                {
                                    //left
                                    Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    //right
                                    Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                }
                                else // front corners
                                {
                                    //left
                                    Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                                    //right
                                    Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)((zTile * zNumberWidth) - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                }
                            }
                        }
                        else //first floor
                        {
                            //front
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            //back
                            Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));

                            if (Random.value < firstFloorAccessoriesPercentage) // first floor accessories percentage
                            {
                                //spawn
                                //front
                                Instantiate(firstFloorAccessories[Random.Range(0, firstFloorAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                //back
                                Instantiate(firstFloorAccessories[Random.Range(0, firstFloorAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            }
                            else
                            {
                                //not spawn
                            }
                        }
                    }
                    else//main walls
                    {
                        if (i * xTile == 0 || i * xTile == xNumberLenght) // main walls corners
                        {
                            if (brandmauer == true) //checks brandmauer
                            {
                                if (i * xTile == 0) //left corners
                                {
                                    // left corner
                                    Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    // left back corner
                                    Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                }
                                else //right corners
                                {
                                    // right corner
                                    Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                                    // right front corner
                                    Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                }
                            }
                            else
                            {
                                if (i * xTile == 0) //left corners
                                {
                                    // left corner
                                    Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    // left back corner
                                    Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                }
                                else //right corners
                                {
                                    // right corner
                                    Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                                    // right front corner
                                    Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                }
                            }
                        }
                        else // main walls
                        {
                            //front
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            //back
                            Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));

                            if (Random.value < wallAccessoriesPercentage)  // wall accessories with percentage
                            {
                                //spawn
                                //front Accessories
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                //back Accessories
                                Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
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
                            if (brandmauer == true) // checks brandmauer bool
                            {
                                // left
                                Instantiate(wallBrandmauer[Random.Range(0, wallBrandmauer.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                //right
                                Instantiate(wallBrandmauer[Random.Range(0, wallBrandmauer.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);

                            }
                            else
                            {
                                // left
                                Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                //right
                                Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);

                                if (Random.value < firstFloorAccessoriesPercentage) // first floor accessories percentage
                                {
                                    //spawn
                                    // left
                                    Instantiate(firstFloorAccessories[Random.Range(0, firstFloorAccessories.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                    //right
                                    Instantiate(firstFloorAccessories[Random.Range(0, firstFloorAccessories.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                                }
                                else
                                {
                                    //not spawn
                                }
                            }
                        }
                        else//main walls
                        {
                            if (brandmauer == true) // checks brandmauer bool
                            {
                                // left
                                Instantiate(wallBrandmauer[Random.Range(0, wallBrandmauer.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                //right
                                Instantiate(wallBrandmauer[Random.Range(0, wallBrandmauer.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);

                            }
                            else
                            {
                                // left
                                Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                //right
                                Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);

                                // wall accessories with percentage
                                if (Random.value < wallAccessoriesPercentage)  //spawn
                                {
                                    // left Accessories
                                    Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                    //right Accessories
                                    Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                                }
                                else
                                {
                                    //null
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < xNumberLenght + 1; i++) // roof 
            {
                for (int j = 0; j <= zNumberWidth; j++)
                {
                    if (i * xTile == 0 && j * zTile == 0 || i * xTile == xNumberLenght && j * zTile == zNumberWidth || i * xTile == 0 && j * zTile == zNumberWidth || i * xTile == xNumberLenght && j * zTile == 0) // roof corners
                    {
                        if (brandmauer == true)//checks brandmauer
                        {
                            if (i * xTile == 0 && j * zTile == 0) //front left
                            {
                                Instantiate(roofCornerBrandmauer[Random.Range(0, roofCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            }
                            else if (i * xTile == xNumberLenght && j * zTile == 0) //front right
                            {
                                Instantiate(roofCornerBrandmauer[Random.Range(0, roofCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf - 0.5), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            }
                            else if (i * xTile == 0 && j * zTile == zNumberWidth) //back left
                            {
                                Instantiate(roofCornerBrandmauer[Random.Range(0, roofCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf + 0.5), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            }
                            else if (i * xTile == xNumberLenght && j * zTile == zNumberWidth)
                            {
                                Instantiate(roofCornerBrandmauer[Random.Range(0, roofCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            }
                        }
                        else
                        {
                            if (i * xTile == 0 && j * zTile == 0) //front left
                            {
                                Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            }
                            else if (i * xTile == xNumberLenght && j * zTile == 0) //front right
                            {
                                Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                            }
                            else if (i * xTile == 0 && j * zTile == zNumberWidth) //back left
                            {
                                Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                            }
                            else if (i * xTile == xNumberLenght && j * zTile == zNumberWidth)
                            {
                                Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            }
                        }
                    }
                    else //main roof
                    {
                        if (j * zTile == 0)
                        {
                            Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0))); // left roof
                        }
                        else if (j * zTile == zNumberWidth)
                        {
                            Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0))); // right roof
                        }
                        else if (i * xTile == 0)
                        {
                            if (brandmauer == true)
                            {
                                Instantiate(roofBrandmauer[Random.Range(0, roofBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -180, 0)));  // back roof
                            }
                            else
                            {
                                Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -180, 0)));  // back roof

                            }
                        }
                        else if (i * xTile == xNumberLenght)
                        {
                            if (brandmauer == true)
                            {
                                Instantiate(roofBrandmauer[Random.Range(0, roofBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);  // front roof
                            }
                            else
                            {
                                Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);  // front roof

                            }
                        }
                        else
                        {
                            Instantiate(roofCupObjects[Random.Range(0, roofCupObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity); // roof cup
                        }
                    }

                    if (i * xTile == 0 || j * zTile == 0 || i * xTile == xNumberLenght || j * zTile == zNumberWidth)
                    {
                        //not spawn
                    }
                    else if (Random.value < roofAccessoriesPercentage) // spawn
                    {
                        // roof accessories w random rotate by 90 degree
                        Instantiate(roofAccessories[Random.Range(0, roofAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 3) * 90, 0)));
                    }
                    else
                    {
                        //not spawn
                    }
                }
            }
            Debug.Log("Sizes: " + "x: " + xNumberLenght + ", " + "y: " + yNumberHeight + ", " + "z: " + zNumberWidth);
            Debug.Log("Done");
        }
    }
}
