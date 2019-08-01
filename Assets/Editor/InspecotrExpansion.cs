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
    int previewNumber = 0;

    private void OnEnable()
    {
        if(previewRenderUtility!=null)
            previewRenderUtility.Cleanup();
        expansion = target as StageGenerator;
        previewSetup();
    }

    private void OnDisable()
    {
        if(previewRenderUtility!=null)
            previewRenderUtility.Cleanup();
        previewRenderUtility = null;
        previewObject = null;
    }


    bool foldout = true;
    int cnt = 0;
    float a;

    //インスペクタの描画
    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        expansion.size= EditorGUILayout.Vector3IntField("Size", expansion.size);

        //builtsSetting
        if (foldout= EditorGUILayout.Foldout(foldout, ""))
        {
            //リストが足りてなかったら足す
            if (expansion.builtsSetting.Count!= (int)StageGenerator.Built.MAX)
            {
                expansion.builtsSetting = new List<float>();
                for (int i = expansion.builtsSetting.Count; i < (int)StageGenerator.Built.MAX; i++)
                {
                    expansion.builtsSetting.Add(1.0f);
                }
            }
            //リストの表示
            float s = 0.0f;
            for (int i = 0; i < expansion.builtsSetting.Count; s += expansion.builtsSetting[i], i++) ;
                for (var i =0;i< expansion.builtsSetting.Count;i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(((StageGenerator.Built)i).ToString(),GUILayout.MaxWidth(80));
                expansion.builtsSetting[i] = EditorGUILayout.FloatField(expansion.builtsSetting[i]);
                EditorGUILayout.LabelField((expansion.builtsSetting[i]/s*100).ToString("F1")+"%",GUILayout.MaxWidth(80));

                if (GUILayout.Button("preview",GUILayout.MaxWidth(100)))
                {
                    previewNumber = i;
                    changeObject();
                }
                EditorGUILayout.EndHorizontal();
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
        var previewCamera = previewRenderUtility.camera;
        previewCamera.transform.position = previewObject.transform.position + new Vector3(2.5f, 2.5f, -5);
        previewCamera.transform.LookAt(previewObject.transform);
        //FieldOfView を 30 にするとちょうどいい見た目になる
        previewRenderUtility.cameraFieldOfView = 30f;
        //必要に応じて nearClipPlane と farClipPlane を設定
        previewRenderUtility.camera.nearClipPlane = 0.3f;
        previewRenderUtility.camera.farClipPlane = 1000;
        //previewLayer のみ表示する
        previewRenderUtility.camera.cullingMask = 1 << previewLayer;
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
