using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CanEditMultipleObjects]
[CustomEditor(typeof(StageGenerator))]
public class InspecotrExpansion : Editor
{
    StageGenerator expansion;
    GameObject previewObject;
    PreviewRenderUtility previewRenderUtility;
    int previewNumber;

    private void OnEnable()
    {
        if(previewRenderUtility!=null)
            previewRenderUtility.Cleanup();
        expansion = target as StageGenerator;
        if (expansion.builtsProbability == null)
        {
            expansion.builtsProbability = new List<float>();
        }
        previewSetup();
        randomPutSize = expansion.randomPut.Count;
    }

    private void OnDisable()
    {
        if(previewRenderUtility!=null)
            previewRenderUtility.Cleanup();
        previewRenderUtility = null;
        if (previewObject != null)
        {
            DestroyImmediate(previewObject);
        }
        previewObject = null;
    }


    bool foldout = true;
    bool randomPutFold = true;
    int cnt = 0;
    float a;
    int randomPutSize = 0;

    //インスペクタの描画
    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        expansion.target = EditorGUILayout.ObjectField("Target",expansion.target, typeof(SavingObject), true) as SavingObject;
        expansion.targetCnt = EditorGUILayout.IntField("Target Count",expansion.targetCnt);

        expansion.size= EditorGUILayout.Vector3IntField("Size", expansion.size);


        expansion.spawner = EditorGUILayout.ObjectField("Player Spawner",expansion.spawner, typeof(GameObject), true) as GameObject;

        if (randomPutFold = EditorGUILayout.Foldout(randomPutFold, "Random Put"))
        {
            randomPutSize = EditorGUILayout.IntField("size", randomPutSize);
            if (randomPutSize < expansion.randomPut.Count && randomPutSize == 0)
            {

            }
            else if(Event.current.type==EventType.KeyUp&&Event.current.keyCode==KeyCode.Return)
            {
                if (randomPutSize > expansion.randomPut.Count)
                {
                    //expansion.randomPut.AddRange(new List<GameObject>(randomPutSize - expansion.randomPut.Count));
                    for (int i = 0; i < randomPutSize - expansion.randomPut.Count; i++)
                    {
                        expansion.randomPut.Add(null);

                    }
                }
                else if (randomPutSize < expansion.randomPut.Count)
                {
                    expansion.randomPut.RemoveRange(randomPutSize, expansion.randomPut.Count - randomPutSize);
                }
            }
            //Debug.Log(randomPutSize.ToString()+","+expansion.randomPut.Count.ToString());
            for (int i = 0; i < expansion.randomPut.Count; i++)
            {
                expansion.randomPut[i] = EditorGUILayout.ObjectField(i.ToString(), expansion.randomPut[i], typeof(GameObject), true) as GameObject;
            }
        }

        //builtsSetting
        if (foldout= EditorGUILayout.Foldout(foldout, ""))
        {
            //リストが足りてなかったら足す
            if (expansion.builtsProbability == null)
            {
                expansion.builtsProbability = new List<float>();
            }
            if (expansion.builtsProbability.Count!= (int)StageGenerator.Built.MAX)
            {
                expansion.builtsProbability = new List<float>();
                for (int i = expansion.builtsProbability.Count; i < (int)StageGenerator.Built.MAX; i++)
                {
                    expansion.builtsProbability.Add(1.0f);
                }
            }
            //リストの表示
            float s = 0.0f;
            for (int i = 0; i < expansion.builtsProbability.Count; s += expansion.builtsProbability[i], i++) ;
                for (var i =0;i< expansion.builtsProbability.Count;i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(((StageGenerator.Built)i).ToString(),GUILayout.MaxWidth(80));
                expansion.builtsProbability[i] = EditorGUILayout.FloatField(expansion.builtsProbability[i]);
                EditorGUILayout.LabelField((expansion.builtsProbability[i]/s*100).ToString("F1")+"%",GUILayout.MaxWidth(80));

                if (GUILayout.Button("preview",GUILayout.MaxWidth(100)))
                {
                    previewNumber = i;
                    changeObject();
                }
                EditorGUILayout.EndHorizontal();
                if (expansion.builtsMaterial.Count <= i)
                {
                    expansion.builtsMaterial.Add(null);
                }
                expansion.builtsMaterial[i]=EditorGUILayout.ObjectField(expansion.builtsMaterial[i], typeof(Material),true)as Material;
            }
        }


        EditorGUILayout.LabelField("test test");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("maji");
        EditorGUILayout.LabelField("manzi");
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(expansion, "stageGenerator");
        }
        serializedObject.ApplyModifiedProperties();
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }
    public override GUIContent GetPreviewTitle()
    {
        return new GUIContent(((StageGenerator.Built)previewNumber).ToString());
    }
    //previewの描画
    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        if (previewRenderUtility == null)
        {
            previewSetup();
        }
        previewRenderUtility.BeginPreview(r, background);
        previewObject.SetActive(true);

        previewRenderUtility.camera.Render();

        previewObject.SetActive(false);

        previewRenderUtility.EndAndDrawPreview(r);

        //Debug.Log(previewObject);

    }

    //previewの用意
    private void previewSetup()
    {
        previewObject = expansion.getBuiltObject((StageGenerator.Built)previewNumber);

        //previewObject.hideFlags = HideFlags.HideAndDontSave;
        previewObject.SetActive(true);
        var flags = BindingFlags.Static | BindingFlags.NonPublic;
        var propInfo = typeof(Camera).GetProperty("PreviewCullingLayer", flags);
        int previewLayer = (int)propInfo.GetValue(null, new object[0]);

        previewRenderUtility = new PreviewRenderUtility(true);
        previewRenderUtility.AddSingleGO(previewObject);
        //FieldOfView を 30 にするとちょうどいい見た目になる
        previewRenderUtility.cameraFieldOfView = 30f;
        //必要に応じて nearClipPlane と farClipPlane を設定
        previewRenderUtility.camera.nearClipPlane = 0.3f;
        previewRenderUtility.camera.farClipPlane = 1000;
        //previewLayer のみ表示する
        previewRenderUtility.camera.cullingMask = 1 << previewLayer;

        var previewCamera = previewRenderUtility.camera;
        previewCamera.transform.position = previewObject.transform.position + new Vector3(2.5f, 2.5f, -5);
        previewCamera.transform.LookAt(previewObject.transform);
        previewCamera.clearFlags = CameraClearFlags.Skybox;
        previewCamera.backgroundColor = new Color(0.9f, 0.9f, 0.9f);

        previewObject.layer = previewLayer;
        foreach (Transform transform in previewObject.transform)
        {
            transform.gameObject.layer = previewLayer;
        }
    }

    //previewを作り直す
    void changeObject()
    {
        if (previewRenderUtility != null)
            previewRenderUtility.Cleanup();
        previewRenderUtility = null;
        previewObject = null;
        previewSetup();
    }
}
