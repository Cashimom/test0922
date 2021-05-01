using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] public GameSystem gameSystem;

    [SerializeField] public TriggerListenner listenner;

    [SerializeField] public List<GameObject> willActive;

    [SerializeField] public List<GameObject> willNotActive;

    [SerializeField] public Vector3 tutorialSpawnPosition;

    private Vector3 defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem.gameObject.SetActive(false);
        listenner.action = GameStart;
        defaultPosition= gameSystem.SpawnPosition;
        gameSystem.SpawnPosition = tutorialSpawnPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameStart(Collider other)
    {
        if (other.gameObject.tag == "Character" && other.gameObject.GetComponent<Character>() is PlayerController)
        {
            gameSystem.SpawnPosition = defaultPosition;
            gameSystem.gameObject.SetActive(true);
            foreach(var obj in willActive)
            {
                obj.SetActive(true);
            }
            foreach(var obj in willNotActive)
            {
                obj.SetActive(false);
            }
        }
    }

}
