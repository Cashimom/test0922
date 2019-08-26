using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class title : MonoBehaviour
{

    [SerializeField] public string sceneName;
    [SerializeField] public TextMeshProUGUI tmp;

    private float cycle = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (sceneName == "")
        {
            Debug.LogError("sceneName is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        tmp.alpha = 0.5f+Mathf.Sin(Time.time*2)/2f;
        if (Input.anyKey)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
