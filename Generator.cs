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
    [Min(0)]
    public int xNumberLenght;
    [Min(0)]
    public int yNumberHeight;
    [Min(0)]
    public int zNumberWidth;
    //half tiles 
    private double xTileHalf;
    private double zTileHalf;
    // accessoires percentages
    [Range(0, 100)]
    public double wallAccessoriesPercentage;
    [Range(0, 100)]
    public double firstFloorAccessoriesPercentage;
    [Range(0, 100)]
    public double roofAccessoriesPercentage;
    //random seed
    public int randomSeed;
    //brandmauer
    public bool brandmauer;
    //stairs
    public bool stairs;
    [Min(1)]
    public int stairsPosition;
    public bool randomStairsPosition;

    // Define possible states for enemy using an enum 
    public enum facadeSideStairsSelector { Front, Back, Left, Right };
    // The current state of stairs enum
    public facadeSideStairsSelector StairsSelectedFacade = facadeSideStairsSelector.Front;

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
    // stairs arrays 0-first floor, 1-main,2-last
    public GameObject[] stairsObjects;

    public void Awake()
    {
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

        //set random value for stairs if bool is true
        if(randomStairsPosition == true)
        {
            if(StairsSelectedFacade == facadeSideStairsSelector.Front || StairsSelectedFacade == facadeSideStairsSelector.Back)
            {
                stairsPosition = Random.Range(1, xNumberLenght - 1);
            }
            else
            {
                stairsPosition = Random.Range(1, zNumberWidth - 1);
            }
            
        }
    }

    public void facadeStairs()
    {
        if(stairs== true) // checks stairs bool
        {
            //switch on stairs enum
            switch (StairsSelectedFacade)
            {
                case facadeSideStairsSelector.Front:
                    for (var i = 0; i < xNumberLenght + 1; i++) //front & back facade
                    {
                        for (var j = 0; j < yNumberHeight; j++)
                        {

                            //stairs main facade
                            if (i * xTile > 0 && i * xTile < xNumberLenght && i * xTile == stairsPosition)
                            {
                                if (j * yTile == 1) //first floor stairs
                                {
                                   GameObject stairsObjects0 =  Instantiate(stairsObjects[0], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                                else if (j * yTile == yNumberHeight - 1)//last floor stairs
                                {
                                    GameObject stairsObjects0 =  Instantiate(stairsObjects[2], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                                else if (j * yTile != yNumberHeight - 1 && j * yTile != 0)//main stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[1], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                            }
                        }
                    }
                    Debug.Log("Stairs position: Front");
                    break;
                case facadeSideStairsSelector.Back:
                    for (var i = 0; i < xNumberLenght + 1; i++) //front & back facade
                    {
                        for (var j = 0; j < yNumberHeight; j++)
                        {
                            //stairs main facade
                            if (i * xTile > 0 && i * xTile < xNumberLenght && i * xTile == stairsPosition)
                            {
                                if (j * yTile == 1) //first floor stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[0], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                                else if (j * yTile == yNumberHeight - 1)//last floor stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[2], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                                else if (j * yTile != yNumberHeight - 1 && j * yTile != 0)//main stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[1], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                            }
                        }
                    }
                    Debug.Log("Stairs position: Back");
                    break;
                case facadeSideStairsSelector.Left:

                    for (var i = 0; i < zNumberWidth; i++) // left facade　
                    {
                        for (int j = 0; j < yNumberHeight; j++)
                        {

                            if (i * zTile == stairsPosition && brandmauer == false)
                            {
                                // left


                                if (j * yTile == 1) //first floor stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[0], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                                else if (j * yTile == yNumberHeight - 1)//last floor stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[2], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                                else if (j * yTile != yNumberHeight - 1 && j * yTile != 0)//main stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[1], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }


                                //right
                                // Instantiate(stairsObjects[0], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                            }
                        }
                    }

                    Debug.Log("Stairs position: Left");
                    break;
                case facadeSideStairsSelector.Right:

                    for (var i = 0; i < zNumberWidth; i++) // right facade　
                    {
                        for (int j = 0; j < yNumberHeight; j++)
                        {

                            if (i * zTile == stairsPosition && brandmauer == false)
                            {
                                // left


                                if (j * yTile == 1) //first floor stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[0], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                                else if (j * yTile == yNumberHeight - 1)//last floor stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[2], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0))); ;
                                    stairsObjects0.transform.parent = transform;
                                }
                                else if (j * yTile != yNumberHeight - 1 && j * yTile != 0)//main stairs
                                {
                                    GameObject stairsObjects0 = Instantiate(stairsObjects[1], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                                    stairsObjects0.transform.parent = transform;
                                }
                            }
                        }
                    }
                    Debug.Log("Stairs position: Right");
                    break;
            }
        }

    }

    public void makeBuilding()
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
                                    gameObject1.transform.parent = transform;
                                    //left
                                    GameObject gameObject2 = Instantiate(firstFloorCornerBrandmauer[Random.Range(0, firstFloorCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    gameObject2.transform.localScale = new Vector3(-1, 1, 1);
                                    gameObject2.transform.parent = transform;
                                }
                                else // front corners
                                {
                                    //right
                                    GameObject gameObject1 = Instantiate(firstFloorCornerBrandmauer[Random.Range(0, firstFloorCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    gameObject1.transform.localScale = new Vector3(-1, 1, 1);
                                    gameObject1.transform.parent = transform;

                                    //left
                                    GameObject gameObject2 = Instantiate(firstFloorCornerBrandmauer[Random.Range(0, firstFloorCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)((zTile * zNumberWidth) - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    gameObject2.transform.localScale = new Vector3(1, 1, 1);
                                    gameObject2.transform.parent = transform;
                                }
                            }
                            else
                            {
                                if (i * xTileHalf == 0) // back corners
                                {
                                    //left
                                    
                                    GameObject cornerObject0 = Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    cornerObject0.transform.parent = transform;
                                    //right

                                    GameObject cornerObject1 = Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                    cornerObject1.transform.parent = transform;
                                }
                                else // front corners
                                {
                                    //left
                                    GameObject cornerObject0 = Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                                    cornerObject0.transform.parent = transform;
                                    //right

                                    GameObject cornerObject1= Instantiate(firstFloorCornersObjects[Random.Range(0, firstFloorCornersObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)((zTile * zNumberWidth) - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    cornerObject1.transform.parent = transform;
                                }
                            }
                        }
                        else //first floor
                        {
                            //front
                            GameObject cornerObject0 = Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            cornerObject0.transform.parent = transform;
                            //back

                            GameObject cornerObject1 = Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            cornerObject1.transform.parent = transform;

                            if (Random.value < firstFloorAccessoriesPercentage) // first floor accessories percentage
                            {
                                //spawn
                                //front
                                GameObject firstFloorAccessories0 = Instantiate(firstFloorAccessories[Random.Range(0, firstFloorAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                firstFloorAccessories0.transform.parent = transform;
                                //back
                                GameObject firstFloorAccessories1 = Instantiate(firstFloorAccessories[Random.Range(0, firstFloorAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                firstFloorAccessories1.transform.parent = transform;
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
                                    GameObject cornerObject0 = Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    cornerObject0.transform.parent = transform;
                                    // left back corner
                                    GameObject cornerObject1 = Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                    cornerObject1.transform.parent = transform;
                                }
                                else //right corners
                                {
                                    // right corner
                                    GameObject cornerObject0 = Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                                    cornerObject0.transform.parent = transform;
                                    // right front corner
                                    GameObject cornerObject1 = Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    cornerObject1.transform.parent = transform;
                                }
                            }
                            else
                            {
                                if (i * xTile == 0) //left corners
                                {
                                    // left corner
                                    GameObject cornerObject0 = Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                                    cornerObject0.transform.parent = transform;
                                    // left back corner
                                    GameObject cornerObject1 = Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                    cornerObject1.transform.parent = transform;
                                }
                                else //right corners
                                {
                                    // right corner
                                    GameObject cornerObject0 = Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                                    cornerObject0.transform.parent = transform;
                                    // right front corner
                                    GameObject cornerObject1 = Instantiate(wallCornerObjects[Random.Range(0, wallCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                                    cornerObject1.transform.parent = transform;
                                }
                            }
                        }

                        else // main walls
                        {
                            //front
                            GameObject wallObjects0 = Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            wallObjects0.transform.parent = transform;
                            //back
                            GameObject wallObjects1 = Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            wallObjects1.transform.parent = transform;
                        }
                            if (Random.value < wallAccessoriesPercentage && i*xTile !=0 && i*xTile != xNumberLenght)  // wall accessories with percentage
                            {
                            //spawn
                            //front Accessories
                            GameObject wallAccessoriesObjects0 = Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(0 - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            wallAccessoriesObjects0.transform.parent = transform;
                            //back Accessories
                            GameObject wallAccessoriesObjects1 = Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(j * yTile + upGroundValue), (float)(zTile * zNumberWidth - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            wallAccessoriesObjects1.transform.parent = transform;
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
                            GameObject wallObjects0 = Instantiate(wallBrandmauer[Random.Range(0, wallBrandmauer.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                            wallObjects0.transform.parent = transform;
                            //right
                            GameObject wallObjects1 = Instantiate(wallBrandmauer[Random.Range(0, wallBrandmauer.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                            wallObjects1.transform.parent = transform;
                        }
                        else
                        {
                            // left
                            GameObject wallObjects0 =  Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                            wallObjects0.transform.parent = transform;
                            //right
                            GameObject wallObjects1 = Instantiate(firstFloorWallsObjects[Random.Range(0, firstFloorWallsObjects.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                            wallObjects1.transform.parent = transform;

                            if (Random.value < firstFloorAccessoriesPercentage) // first floor accessories percentage
                            {
                                //spawn
                                // left
                                GameObject wallAccessoriesObjects0 = Instantiate(firstFloorAccessories[Random.Range(0, firstFloorAccessories.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                wallAccessoriesObjects0.transform.parent = transform;
                                //right
                                GameObject wallAccessoriesObjects1 = Instantiate(firstFloorAccessories[Random.Range(0, firstFloorAccessories.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                                wallAccessoriesObjects1.transform.parent = transform;
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
                            GameObject wallObjects0 = Instantiate(wallBrandmauer[Random.Range(0, wallBrandmauer.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                            wallObjects0.transform.parent = transform;
                            //right
                            GameObject wallObjects1 = Instantiate(wallBrandmauer[Random.Range(0, wallBrandmauer.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                            wallObjects1.transform.parent = transform;
                        }
                        else
                        {
                            // left
                            GameObject wallObjects0 = Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                            wallObjects0.transform.parent = transform;
                            //right
                            GameObject wallObjects1 = Instantiate(wallObjects[Random.Range(0, wallObjects.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                            wallObjects1.transform.parent = transform;

                            // wall accessories with percentage
                            if (Random.value < wallAccessoriesPercentage)  //spawn
                            {
                                // left Accessories
                                GameObject wallAccessoriesObjects0 = Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3((float)(0 - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                                wallAccessoriesObjects0.transform.parent = transform;
                                //right Accessories
                                GameObject wallAccessoriesObjects1 = Instantiate(wallAccessories[Random.Range(0, wallAccessories.Length)], new Vector3((float)(xTile * xNumberLenght - xTileHalf), (float)(j * yTile + upGroundValue), (float)(i * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);
                                wallAccessoriesObjects1.transform.parent = transform;
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
                           GameObject roofObjects0 = Instantiate(roofCornerBrandmauer[Random.Range(0, roofCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                           roofObjects0.transform.parent = transform;
                        }
                        else if (i * xTile == xNumberLenght && j * zTile == 0) //front right
                        {
                            GameObject roofObjects0 = Instantiate(roofCornerBrandmauer[Random.Range(0, roofCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf - 0.5), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            roofObjects0.transform.parent = transform;
                        }
                        else if (i * xTile == 0 && j * zTile == zNumberWidth) //back left
                        {
                            GameObject roofObjects0 = Instantiate(roofCornerBrandmauer[Random.Range(0, roofCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf + 0.5), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            roofObjects0.transform.parent = transform;
                        }
                        else if (i * xTile == xNumberLenght && j * zTile == zNumberWidth)
                        {
                            GameObject roofObjects0 = Instantiate(roofCornerBrandmauer[Random.Range(0, roofCornerBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            roofObjects0.transform.parent = transform;
                        }
                    }
                    else
                    {
                        if (i * xTile == 0 && j * zTile == 0) //front left
                        {
                            GameObject roofObjects0 = Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            roofObjects0.transform.parent = transform;
                        }
                        else if (i * xTile == xNumberLenght && j * zTile == 0) //front right
                        {
                            GameObject roofObjects0 = Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                            roofObjects0.transform.parent = transform;
                        }
                        else if (i * xTile == 0 && j * zTile == zNumberWidth) //back left
                        {
                            GameObject roofObjects0 = Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                            roofObjects0.transform.parent = transform;
                        }
                        else if (i * xTile == xNumberLenght && j * zTile == zNumberWidth)
                        {
                            GameObject roofObjects0 = Instantiate(roofCornerObjects[Random.Range(0, roofCornerObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                            roofObjects0.transform.parent = transform;
                        }
                    }
                }
                else //main roof
                {
                    if (j * zTile == 0)
                    {
                        GameObject roofObjects0 = Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, 90, 0))); // left roof
                        roofObjects0.transform.parent = transform;
                    }
                    else if (j * zTile == zNumberWidth)
                    {
                        GameObject roofObjects0 = Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -90, 0))); // right roof
                        roofObjects0.transform.parent = transform;
                    }
                    else if (i * xTile == 0)
                    {
                        if (brandmauer == true)
                        {
                            GameObject roofObjects0 = Instantiate(roofBrandmauer[Random.Range(0, roofBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -180, 0)));  // back roof
                            roofObjects0.transform.parent = transform;
                        }
                        else
                        {
                            GameObject roofObjects0 = Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, -180, 0)));  // back roof
                            roofObjects0.transform.parent = transform;
                        }
                    }
                    else if (i * xTile == xNumberLenght)
                    {
                        if (brandmauer == true)
                        {
                            GameObject roofObjects0 = Instantiate(roofBrandmauer[Random.Range(0, roofBrandmauer.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);  // front roof
                            roofObjects0.transform.parent = transform;
                        }
                        else
                        {
                            GameObject roofObjects0 = Instantiate(roofObjects[Random.Range(0, roofObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity);  // front roof
                            roofObjects0.transform.parent = transform;
                        }
                    }
                    else
                    {
                        GameObject roofObjects0 = Instantiate(roofCupObjects[Random.Range(0, roofCupObjects.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.identity); // roof cup
                        roofObjects0.transform.parent = transform;
                    }
                }

                if (i * xTile == 0 || j * zTile == 0 || i * xTile == xNumberLenght || j * zTile == zNumberWidth)
                {
                    //not spawn
                }
                else if (Random.value < roofAccessoriesPercentage) // spawn
                {
                    // roof accessories w random rotate by 90 degree
                    GameObject roofObjects0 = Instantiate(roofAccessories[Random.Range(0, roofAccessories.Length)], new Vector3((float)(i * xTile - xTileHalf), (float)(yTile * (yNumberHeight - 1) + upGroundValue), (float)(j * zTile - zTileHalf)) + pickedObjectPosition.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 3) * 90, 0)));
                    roofObjects0.transform.parent = transform;
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

    void Start()
    {
        //calls building function
        makeBuilding();
        //calls stairs function
        facadeStairs();

        //hides boxes with "Boxes" tag
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Boxes");
        for(int i = 0; i < boxes.Length; i++)
        {
            boxes[i].SetActive(false);
        }

    }
}
