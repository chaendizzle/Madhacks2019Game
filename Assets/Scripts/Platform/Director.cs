using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{

    public GameObject startingPlatform; //MUST be populated
    public List<GameObject> floatingPlatforms;
    public List<GameObject> groundedPlatforms;
    public float platformBudget; //used to determine jump distance? and platform size;
    private List<RenderedPlatform> platforms; 
    public float waterHeight;

    private float distanceGenerated;

    private float LOOKAHEAD_DISTANCE = CameraMovement.CameraRect().width; //distance to end of run used to trigger new run generation
    private const float MIN_SPACING = 1;
    private const float MAX_SPACING = 2;
    private const int RUN_LENGTH = 10; //length of run to generate 
    private const float JUMP_SCALE = 1;


    static Director instance;
    public static Director GetInstance()
    {
        if (Director.instance == null)
        {
            return new Director();
        }

        return instance;
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
        platforms.Add(new RenderedPlatform(startingPlatform));

        groundedPlatforms.Sort(new LengthCompare());
    }

    struct RenderedPlatform
    {
        public GameObject obj;
        public float width;
        public float height;
        public Vector3 upperLeft;
        public Vector3 upperRight;

        //create struct from already rendered gameobj
        public RenderedPlatform(GameObject obj)
        {
            this.obj = obj;
            this.width = obj.transform.lossyScale.x * obj.GetComponent<Renderer>().bounds.extents.x;
            this.height = obj.transform.lossyScale.y * obj.GetComponent<Renderer>().bounds.extents.y;
            this.upperLeft = obj.transform.position + new Vector3(-(this.width / 2), (this.height / 2), 0);
            this.upperRight = obj.transform.position + new Vector3((this.width / 2), (this.height / 2), 0);
        }

        //create sprite from prefab and leftSide position
        public RenderedPlatform(GameObject prefab, Vector3 leftSide)
        {
            this.width = prefab.transform.lossyScale.x * prefab.GetComponent<Renderer>().bounds.extents.x;
            this.height = prefab.transform.lossyScale.y * prefab.GetComponent<Renderer>().bounds.extents.y;
            this.obj = Instantiate(prefab, leftSide + new Vector3(width / 2, 0, 0), Quaternion.identity);
            this.upperLeft = obj.transform.position + new Vector3(-(this.width / 2), (this.height / 2), 0);
            this.upperRight = obj.transform.position + new Vector3((this.width / 2), (this.height / 2), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get camera pose
        Rect cameraView = CameraMovement.CameraRect();

        //create batch of platforms whenever camera is within given distance of the end of the last run
        if (cameraView.xMax - LOOKAHEAD_DISTANCE > distanceGenerated)
        {
            //generate run of new platforms
            float targetDistance = distanceGenerated + RUN_LENGTH;
            while (distanceGenerated < targetDistance) 
            {
                RenderedPlatform previous = platforms[platforms.Count - 1];

                //determine width of platform from platform budget
                GameObject prefab = groundedPlatforms[(int)Mathf.Floor(Random.Range(0, groundedPlatforms.Count - 1))];

                //determine spacing
                float xOffset = Random.Range(MIN_SPACING, MAX_SPACING);

                RenderedPlatform platform = new RenderedPlatform(prefab, new Vector3(previous.upperRight.x + xOffset, waterHeight));
                distanceGenerated = platform.upperRight.x;
                platforms.Add(platform);
            }
        }


        //discard old platforms
        for (int i = 0; i < platforms.Count; i++)
        {
            if (platforms[i].obj.transform.position.x + (platforms[i].width / 2) < cameraView.xMin)
            {
                Destroy(platforms[i].obj);
                platforms.RemoveAt(i);
                i--;
            }
        }

    }

}
