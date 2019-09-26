using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    [SerializeField] private Menu menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _returnToTitle()
    {
        menu.Exit();
        SceneManager.LoadSceneAsync("Title");
    }
}
