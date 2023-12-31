using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    private PlayerFeatures _entity;
    private Player _rewiredPlayer;
    private float _orientX;
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
        _orientX = _rewiredPlayer.GetAxis("MoveX");

        if (_orientX != 0)
        {
            // _orientX = Mathf.Sign(_orientX);
            _entity.GetOrient(_orientX);
        }
        
        if (_rewiredPlayer.GetButtonDown("Grab") && _entity.canGrabItem)
        {
            _entity.GrabItem();
        }

        if (_rewiredPlayer.GetButtonDown("Drop"))
        {
            _entity.DropItem();
        }
        
        if (_rewiredPlayer.GetButtonUp("Throw") && _entity.canThrowItem)
        {
            _entity.ThrowItem();
        }
        
        if (_rewiredPlayer.GetButtonDown("Inventory Left"))
        {
            _entity.SetSelectedItem(-1);
        }
        
        if (_rewiredPlayer.GetButtonDown("Inventory Right"))
        {
            _entity.SetSelectedItem(1);
        }
    }
}
