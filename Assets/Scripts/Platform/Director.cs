﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{

    public List<GameObject> startingPlatforms; //MUST be populated
    public List<GameObject> floatingPlatforms;
    public List<GameObject> groundedPlatforms;
    public float platformBudget; //used to determine jump distance? and platform size;
    private List<RenderedPlatform> platforms; 
    public float waterHeight;
    public float jumpVelocity;
    public float xVelocity;
    public float gravity;
    public float xBufferDistance;
    public float yBufferDistance;

    private float distanceGenerated;

    private float LOOKAHEAD_DISTANCE = 5; //distance to end of run used to trigger new run generation
    private const float MIN_SPACING = 1f;
    private const float MAX_SPACING = 2;
    private const int RUN_LENGTH = 10; //length of run to generate 
    private const float JUMP_SCALE = 1;


    static Director instance;
    public static Director GetInstance()
    {
        return GameObject.FindGameObjectWithTag("Director").GetComponent<Director>();
    }

    //Interface instance used for sorting platformPrefabs
    class LengthCompare :  IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            float length1 = x.GetComponent<Renderer>().bounds.extents.x * x.transform.lossyScale.x;
            float length2 = y.GetComponent<Renderer>().bounds.extents.x * y.transform.lossyScale.x;

            return length1.CompareTo(length2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        distanceGenerated = CameraMovement.CameraRect().center.x;
        platforms = new List<RenderedPlatform>();
        foreach (GameObject platform in startingPlatforms)
        {
            platforms.Add(new RenderedPlatform(platform));
        }

        groundedPlatforms.Sort(new LengthCompare());
    }

    struct RenderedPlatform
    {
        public GameObject obj;
        public float width;
        public float height;
        public Vector3 upperLeft;
        public Vector3 upperRight;

        private RenderedPlatform(GameObject obj, float width, float height, Vector3 upperLeft, Vector3 upperRight)
        {
            this.obj = obj;
            this.width = width;
            this.height = height;
            this.upperLeft = upperLeft;
            this.upperRight = upperRight;
        }

        //create struct from already rendered gameobj
        public RenderedPlatform(GameObject obj)
        {
            this.obj = obj;
            this.width = obj.transform.lossyScale.x * obj.GetComponent<Renderer>().bounds.extents.x * 2;
            this.height = obj.transform.lossyScale.y * obj.GetComponent<Renderer>().bounds.extents.y * 2;
            this.upperLeft = obj.transform.position + new Vector3(-(this.width / 2), (this.height / 2), 0);
            this.upperRight = obj.transform.position + new Vector3((this.width / 2), (this.height / 2), 0);
        }

        //create sprite from prefab and leftSide position
        public RenderedPlatform(GameObject prefab, Vector3 leftSide)
        {
            this.width = prefab.transform.lossyScale.x * prefab.GetComponent<Renderer>().bounds.extents.x * 2;
            this.height = prefab.transform.lossyScale.y * prefab.GetComponent<Renderer>().bounds.extents.y * 2;
            this.obj = Instantiate(prefab, leftSide + new Vector3(width / 2, 0, 0), Quaternion.identity);
            this.upperLeft = obj.transform.position + new Vector3(-(this.width / 2), (this.height / 2), 0);
            this.upperRight = obj.transform.position + new Vector3((this.width / 2), (this.height / 2), 0);
        }

        public static RenderedPlatform FromUpperLeft(GameObject prefab, Vector3 upperLeft)
        {
            float width = prefab.transform.lossyScale.x * prefab.GetComponent<Renderer>().bounds.extents.x * 2;
            float height = prefab.transform.lossyScale.y * prefab.GetComponent<Renderer>().bounds.extents.y * 2;
            GameObject obj = Instantiate(prefab, upperLeft + new Vector3(width / 2, -(height / 2), 0), Quaternion.identity);
            Vector3 upperRight = obj.transform.position + new Vector3((width / 2), (height / 2), 0);
            return new RenderedPlatform(obj, width, height, upperLeft, upperRight);
        }
    }

    public IEnumerable<GameObject> GetPlatforms()
    {
        return platforms.Select(x => x.obj);
    }

    private float budgetWeight(float lowerBound, float upperBound)
    {
        return (upperBound - lowerBound) * Mathf.Pow(2, -platformBudget);
    }

    RenderedPlatform GenerateVariableHightPlatform(Rect cameraRect)
    {

        //determine width of platform from platform budget
        GameObject prefab = floatingPlatforms[(int)Mathf.Floor(Random.Range(0, floatingPlatforms.Count - 1))];

        //pick random height difference over interval (-inf, 0.5*v0^2/g)
        float yOffsetBound = 0.5f * Mathf.Pow(jumpVelocity, 2) /  gravity;

        float yOffset = Random.Range(0, yOffsetBound);

        float xOffsetBound = (
            Mathf.Sqrt(jumpVelocity * jumpVelocity * xVelocity * xVelocity - (2 * gravity * xVelocity * xVelocity * yOffset)) 
            + jumpVelocity * xVelocity) / gravity;
        float budgetWeight = this.budgetWeight(MIN_SPACING, xOffsetBound);
        float xOffset = Random.Range(MIN_SPACING + budgetWeight, xOffsetBound);

        if (platforms.Count > 0)
        {
            RenderedPlatform previous = platforms[platforms.Count - 1];
            float yPosition = Mathf.Clamp(previous.upperRight.y + yOffset,
                cameraRect.yMin + xBufferDistance, cameraRect.yMax - yBufferDistance);
            return RenderedPlatform.FromUpperLeft(prefab,
                new Vector3(previous.upperRight.x + xOffset, yPosition, 0));
        }
        else
        {
            return RenderedPlatform.FromUpperLeft(prefab,
                new Vector3(cameraRect.center.x + xOffset, cameraRect.center.y + yOffset, 0));
        }
    }

    RenderedPlatform GenerateStaticHeightPlatform(Rect cameraRect)
    {

        //determine width of platform from platform budget
        GameObject prefab = groundedPlatforms[(int)Mathf.Floor(Random.Range(0, groundedPlatforms.Count - 1))];

        //determine spacing
        float xOffsetBound = 2 * jumpVelocity * xVelocity / gravity;
        float budgetWeight = this.budgetWeight(MIN_SPACING, xOffsetBound);
        float xOffset = Random.Range(MIN_SPACING + budgetWeight, xOffsetBound);
        //Debug.Log("Interval: " + (MIN_SPACING + budgetWeight) + " " + xOffsetBound);

        if (platforms.Count > 0)
        {
            RenderedPlatform previous = platforms[platforms.Count - 1];
            return new RenderedPlatform(prefab, new Vector3(previous.upperRight.x + xOffset, waterHeight));
        }
        else
        {
            return new RenderedPlatform(prefab, new Vector3(cameraRect.center.x + xOffset, waterHeight));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get camera pose
        Rect cameraView = CameraMovement.CameraRect();

        //create batch of platforms whenever camera is within given distance of the end of the last run
        if (cameraView.xMax + LOOKAHEAD_DISTANCE > distanceGenerated)
        {
            //generate run of new platforms
            float targetDistance = distanceGenerated + RUN_LENGTH;
            while (distanceGenerated < targetDistance) 
            {
                RenderedPlatform platform = (Random.value > 0.65) ? GenerateStaticHeightPlatform(cameraView) :
                    (ClimateEvents.GetInstance().waterLevel ? GenerateStaticHeightPlatform(cameraView) : GenerateVariableHightPlatform(cameraView));
                distanceGenerated = platform.upperRight.x;
                platforms.Add(platform);
            }
        }


        //discard old platforms
        for (int i = 0; i < platforms.Count; i++)
        {
            if (platforms[i].obj == null)
            {
                platforms.RemoveAt(i);
                i--;
                continue;
            }

            if (platforms[i].obj.transform.position.x + (platforms[i].width / 2) < cameraView.xMin)
            {
                Destroy(platforms[i].obj);
                platforms.RemoveAt(i);
                i--;
            }
        }

    }

}
