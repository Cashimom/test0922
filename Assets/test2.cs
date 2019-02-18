using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class test2 : MonoBehaviour
{
    class OBJ_LIKE_FORMAT
    {
        //まずインスタンスを生成して、vtxとtriにOBJ構造を入れる。
        //インスタンスからExport_Unity_Mash_Formatメソッドを呼び出すと、
        //戻り値には、UnityMesh形式に変換されたvtxとtriが返ってくる

        public List<Vector3> vtx;
        public List<int> tri;

        public OBJ_LIKE_FORMAT()
        {
            vtx = new List<Vector3>();
            tri = new List<int>();
        }

        public OBJ_LIKE_FORMAT Export_Unity_Mash_Format()
        {
            if (vtx.Count == 0 || tri.Count == 0)
            {
                //Debug.Log("List の個数がゼロ");
                return null;
            }

            OBJ_LIKE_FORMAT obj = new OBJ_LIKE_FORMAT();

            for (int i = 0; i < tri.Count; i++)
            {
                obj.vtx.Add(vtx[tri[i]]);
            }
            obj.tri = Enumerable.Range(0, obj.vtx.Count).ToList();

            return obj;
        }
    }

    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {

        //変換のためのインスタンスを生成する
        OBJ_LIKE_FORMAT obj = new OBJ_LIKE_FORMAT();

        //a...正三角形の一辺の長さ b = たて正四面体
        float a = 2;
        float b = Mathf.Sqrt(3f) * a / 2f;
        float h = Mathf.Sqrt(a * a - (b / 3 * 2) * (b / 3 * 2));
        //OBJフォーマットでデータを入れる
        //頂点座標配列
        obj.vtx.AddRange(new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,-h,(b / 3 * 2)),
            new Vector3(-a/2f,-h,-b / 3),
            new Vector3(a/2f,-h,-b / 3)
        });

        //頂点を結ぶ順番配列(面を構成)
        obj.tri.AddRange(new int[]
        {
            2,1,0,
            0,1,3,
            3,2,0,
            1,2,3
        });

        //UnityMesh形式に変換する
        OBJ_LIKE_FORMAT unityMesh = obj.Export_Unity_Mash_Format();

        //Listを配列に変換して加えればOK.
        Mesh mesh = new Mesh();
        mesh.vertices = unityMesh.vtx.ToArray();
        mesh.triangles = unityMesh.tri.ToArray();
        mesh.name = "abc";

        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Mathf.Sin(Time.time)*8,0);
    }
}
