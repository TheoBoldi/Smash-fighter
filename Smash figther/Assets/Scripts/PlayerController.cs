using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    private PlayerFeatures _entity;
    private Player _rewiredPlayer;

    public string playerName = "Player1";
    
    // Start is called before the first frame update
    void Start()
    {
        _entity = GetComponentInChildren<PlayerFeatures>();
        _rewiredPlayer = ReInput.players.GetPlayer(playerName);
    }

    // Update is called once per frame
    void Update()
    {
        if (_rewiredPlayer.GetButtonDown("Grab") && _entity.canGrabItem)
        {
            _entity.GrabItem();
        }

        if (_rewiredPlayer.GetButtonDown("QuickThrow") && _entity.canThrowItem)
        {
            _entity.ThrowItem();
        }
    }
}
