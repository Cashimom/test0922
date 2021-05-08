using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerBehaviour : MonoBehaviour
{
    protected PlayerInput playerInput;

    private PlayerStatus _playerStatus;
    protected IStatusSystem playerStatus => _playerStatus;

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        _playerStatus = this.GetComponent<PlayerStatus>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
