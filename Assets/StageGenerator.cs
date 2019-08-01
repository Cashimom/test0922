using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マップを自動で生成する
public class StageGenerator : MonoBehaviour
{
    [SerializeField] public Vector3Int size = new Vector3Int(5,5,5);

    private int[,,] map;

    private Vector3 blockSize;

    private Vector3 firstPos;

    public class BuiltSetting
    {
        float probability = 1.0f;
    }

    //[SerializeField] public List<float> builtSetting= new List<float>((int)Built.MAX);

    public List<float> builtsSetting;

    public enum Built:int
    {
        NULL,
        N1,
        N2,
        N3,
        MAX
    }

    // Start is called before the first frame update
    void Start()
    {
        firstPos = transform.position;
        //blockSize = size;
        blockSize.x = transform.localScale.x/size.x;
        blockSize.y = transform.localScale.y/size.y;
        blockSize.z = transform.localScale.z/size.z;
        map = new int[size.x, size.y,size.z];
        float sum = 0.0f;
        for(int i = 0; i < builtsSetting.Count; i++)
        {
            sum += builtsSetting[i];
        }
        for (int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    float value = Random.value;
                    float s = 0;
                    int i;
                    for (i = 0, s = builtsSetting[i] / sum; s < value; i++, s += builtsSetting[i] / sum);
                    map[x, y, z] = i;
                    if (map[x, y, z] != 0)
                    {
                        generateBuilt(this.vec3(x, y, z), (Built)map[x, y, z]);
                    }

                }
                
            }
        }
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

    void generateBuilt(Vector3 pos,Built built)
    {
        GameObject g=getBuiltObject(built);
        g.transform.parent = this.transform;
        g.transform.position += localPos(pos)+firstPos;

        g.transform.localScale = this.vec3(
             g.transform.localScale.x * blockSize.x,
             g.transform.localScale.y * blockSize.y,
             g.transform.localScale.z * blockSize.z);

    }

    public GameObject getBuiltObject(Built built)
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.transform.position = this.vec3(0, 0, 0);
        g.transform.Rotate(0, 0, 0);
        g.transform.localScale = this.vec3(1, 1, 1);
        g.name = "_StageObject";
        
        switch (built)
        {
            case Built.NULL:
                g.transform.localScale *= 0;
                break;
            case Built.N1:
                g.transform.localScale = this.vec3(0.8f, 0.05f, 0.8f);
                break;
            case Built.N2:
                g.transform.localScale = this.vec3(1,1,1);
                break;
            case Built.N3:
                g.transform.Rotate(0, 45, 0);
                g.transform.localScale = this.vec3(1/1.41f, 1 / 1.41f, 1 / 1.41f);
                break;
            default:
                g.transform.localScale = this.vec3(1,1,1);
                break;
        }
        return g;
    }
}
