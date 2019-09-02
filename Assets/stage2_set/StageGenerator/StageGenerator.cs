using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

//マップを自動で生成する
public class StageGenerator : MonoBehaviour
{
    [SerializeField] public Vector3Int size = new Vector3Int(5,5,5);

    [SerializeField] public SavingObject target;

    [SerializeField] public int targetCnt = 3;

    [SerializeField] public GameObject spawner;

    [SerializeField] public List<GameObject> randomPut = new List<GameObject>();

    [SerializeField] public bool generateWhenStart = false;

    private int[,,] map;

    private Vector3 blockSize;

    private Vector3 firstPos;

    public class BuiltSetting
    {
        float probability = 1.0f;
    }

    //[SerializeField] public List<float> builtSetting= new List<float>((int)Built.MAX);

    public List<float> builtsProbability=new List<float>();

    public List<Material> builtsMaterial=new List<Material>();

    public enum Built:int
    {
        NULL,
        N1,
        N2,
        N3,
        MAX
    }

    Vector3 vec3(float x,float y,float z)
    {
        return new Vector3(x, y, z);
    }

    private void Awake()
    {
        firstPos = transform.position;
        //blockSize = size;
        blockSize.x = transform.localScale.x / size.x;
        blockSize.y = transform.localScale.y / size.y;
        blockSize.z = transform.localScale.z / size.z;
        map = new int[size.x, size.y, size.z];
    }

    // Start is called before the first frame update
    void Start()
    {
        if (generateWhenStart)
        {
            generate();
        }
    }

    public List<SavingObject> generate()
    {
        var targetList = new List<SavingObject>();
        firstPos = transform.position;
        //blockSize = size;
        blockSize.x = transform.localScale.x / size.x;
        blockSize.y = transform.localScale.y / size.y;
        blockSize.z = transform.localScale.z / size.z;
        map = new int[size.x, size.y, size.z];
        int sw = 2;
        Vector3 flg = vec3(0, 0, 0);

        Func<Vector3Int> randomPosition = ()=>  new Vector3Int(Random.Range(sw, size.x - sw), Random.Range(sw, size.y - sw - 1), Random.Range(sw, size.z - sw)) ;

        for (int jjj = 0; jjj < targetCnt; jjj++)
        {
            var mainStreet = randomPosition();
            targetList.Add(Instantiate(target, localPos(mainStreet)+firstPos, Quaternion.Euler(0, 0, 0)));
            if (!(Mathf.Abs(mainStreet.x-size.x/2) >= Mathf.Abs(mainStreet.z - size.z / 2)))
            {
                flg = vec3(1, 0, 0);
                for (int i = sw; i < size.x - sw; i++)
                {
                    float center = (-Mathf.Abs(((float)mainStreet.x - (float)i)) / (float)size.x + 1.3f);
                    int streetWidth = Mathf.FloorToInt(center);
                    map[i, mainStreet.y, mainStreet.z] = -1;
                    map[i, mainStreet.y + 1, mainStreet.z] = -1;
                    map[i, mainStreet.y - 1, mainStreet.z] = (int)StageGenerator.Built.N2;
                    map[i, mainStreet.y - 1, mainStreet.z - 1- streetWidth] = (int)StageGenerator.Built.N2;
                    map[i, mainStreet.y - 1, mainStreet.z + 1+ streetWidth] = (int)StageGenerator.Built.N2;
                    map[i, mainStreet.y - 1, mainStreet.z - 1] = (int)StageGenerator.Built.N2;
                    map[i, mainStreet.y - 1, mainStreet.z + 1] = (int)StageGenerator.Built.N2;
                    map[i, mainStreet.y , mainStreet.z - 1 - streetWidth] = Random.Range(2, 4);
                    map[i, mainStreet.y , mainStreet.z + 1 + streetWidth] = Random.Range(2, 4);
                    map[i, mainStreet.y + 1, mainStreet.z - 1- streetWidth] = Random.Range(2, 4);
                    map[i, mainStreet.y + 1, mainStreet.z + 1+ streetWidth] = Random.Range(2, 4);
                    map[i, mainStreet.y + 2, mainStreet.z - 1- streetWidth] = Random.Range(2, 4);
                    map[i, mainStreet.y + 2, mainStreet.z + 1+ streetWidth] = Random.Range(2, 4);
                    map[i, mainStreet.y + 3, mainStreet.z - 1- streetWidth] = Random.Range(2, 4);
                    map[i, mainStreet.y + 3, mainStreet.z + 1+ streetWidth] = Random.Range(2, 4);
                }
            }
            else
            {
                flg = vec3(0, 0, 1);
                for (int i = sw; i < size.z - sw; i++)
                {
                    float center = (-Mathf.Abs(((float)mainStreet.z - (float)i)) / (float)size.z + 1.3f);
                    int streetWidth = Mathf.FloorToInt(center);
                    map[ mainStreet.x, mainStreet.y,i] = -1;
                    map[ mainStreet.x, mainStreet.y + 1,i] = -1;
                    map[ mainStreet.x, mainStreet.y - 1,i] = (int)StageGenerator.Built.N2;
                    map[ mainStreet.x - 1 - streetWidth, mainStreet.y - 1,i] = (int)StageGenerator.Built.N2;
                    map[ mainStreet.x + 1 + streetWidth, mainStreet.y - 1,i] = (int)StageGenerator.Built.N2;
                    map[ mainStreet.x - 1, mainStreet.y - 1,i] = (int)StageGenerator.Built.N2;
                    map[ mainStreet.x + 1, mainStreet.y - 1,i] = (int)StageGenerator.Built.N2;
                    map[ mainStreet.x - 1 - streetWidth, mainStreet.y,i] = Random.Range(2, 4);
                    map[ mainStreet.x + 1 + streetWidth, mainStreet.y,i] = Random.Range(2, 4);
                    map[ mainStreet.x - 1 - streetWidth, mainStreet.y + 1,i] = Random.Range(2, 4);
                    map[ mainStreet.x + 1 + streetWidth, mainStreet.y + 1,i] = Random.Range(2, 4);
                    map[ mainStreet.x - 1 - streetWidth, mainStreet.y + 2,i] = Random.Range(2, 4);
                    map[ mainStreet.x + 1 + streetWidth, mainStreet.y + 2,i] = Random.Range(2, 4);
                    map[ mainStreet.x - 1 - streetWidth, mainStreet.y + 3,i] = Random.Range(2, 4);
                    map[ mainStreet.x + 1 + streetWidth, mainStreet.y + 3,i] = Random.Range(2, 4);
                }
            }
        }

        if (spawner != null)
        {
            randomPut.Add(spawner);
        }
        foreach (var gameObject in randomPut)
        {
            if (gameObject == null) continue;
            Vector3Int pos = randomPosition();
            gameObject.transform.position = localPos(pos) + firstPos;
            map[pos.x, pos.y, pos.z] = -1;
        }

        generateBuilt(vec3(-1, (float)(size.y - 1) / 2f, (float)(size.z - 1) / 2f), Built.N2)
            .transform.localScale = vec3(blockSize.x / transform.localScale.x, 1.1f, 1.1f);
        generateBuilt(vec3(size.x, (float)(size.y - 1) / 2f, (float)(size.z - 1) / 2f), Built.N2)
            .transform.localScale = vec3(blockSize.x / transform.localScale.x, 1.1f, 1.1f);

        generateBuilt(vec3((float)(size.x - 1) / 2f, (float)(size.y - 1) / 2f, -1), Built.N2)
            .transform.localScale = vec3(1.1f, 1.1f, blockSize.z / transform.localScale.z);
        generateBuilt(vec3((float)(size.x - 1) / 2f, (float)(size.y - 1) / 2f, size.z), Built.N2)
            .transform.localScale = vec3(1.1f, 1.1f, blockSize.z / transform.localScale.z);

        generateBuilt(vec3((float)(size.x - 1) / 2f, -1, (float)(size.z - 1) / 2f), Built.N2)
            .transform.localScale = vec3(1.1f, blockSize.y / transform.localScale.y, 1.1f);
        generateBuilt(vec3((float)(size.x - 1) / 2f, size.y, (float)(size.z - 1) / 2f), Built.N2)
            .transform.localScale = vec3(1.1f, blockSize.y / transform.localScale.y, 1.1f);

        float sum = 0.0f;
        for (int i = 0; i < builtsProbability.Count; i++)
        {
            sum += builtsProbability[i];
        }
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    if (map[x, y, z] == 0)
                    {
                        float value = Random.value;
                        float s = 0;
                        int i;
                        for (i = 0, s = builtsProbability[i] / sum; s < value; i++, s += builtsProbability[i] / sum) ;
                        map[x, y, z] = i;
                    }

                    if (map[x, y, z] != 0 && map[x, y, z] != -1)
                    {
                        generateBuilt(vec3(x, y, z), (Built)map[x, y, z]);
                    }

                }

            }
        }



        return targetList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 localPos(Vector3 pos)
    {
        return new Vector3(transform.localScale.x/size.x*(pos.x),
            transform.localScale.y / size.y * (pos.y),
            transform.localScale.z / size.z * (pos.z));
    }

    GameObject generateBuilt(Vector3 pos,Built built)
    {
        GameObject g=getBuiltObject(built);
        g.transform.parent = this.transform;
        g.transform.position += localPos(pos)+firstPos;

        g.transform.localScale = vec3(
             g.transform.localScale.x * blockSize.x,
             g.transform.localScale.y * blockSize.y,
             g.transform.localScale.z * blockSize.z);
        return g;
    }

    public GameObject getBuiltObject(Built built)
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.transform.position = vec3(0, 0, 0);
        g.transform.Rotate(0, 0, 0);
        g.transform.localScale = vec3(1, 1, 1);
        if(builtsMaterial!=null&&builtsMaterial.Count > (int)built && builtsMaterial[(int)built]!=null)
            g.GetComponent<Renderer>().material = builtsMaterial[(int)built];
        g.name = "_StageObject";
        
        switch (built)
        {
            case Built.NULL:
                g.transform.localScale *= 0;
                break;
            case Built.N1:
                g.transform.localScale = vec3(0.8f, 0.05f, 0.8f);
                break;
            case Built.N2:
                g.transform.localScale = vec3(1,1,1);
                break;
            case Built.N3:
                g.transform.Rotate(0, 45, 0);
                g.transform.localScale = vec3(1/1.41f, 1 / 1.41f, 1 / 1.41f);
                break;
            default:
                g.transform.localScale = vec3(1,1,1);
                break;
        }
        return g;
    }
}
