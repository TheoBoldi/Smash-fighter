using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CustomPlayerController : MonoBehaviour
{
    private PlayerEntity _entity;
    private Player _rewiredPlayer;

    public string playerName = "Player1";
    
    // Start is called before the first frame update
    void Start()
    {
        _entity = GetComponentInChildren<PlayerEntity>();
        _rewiredPlayer = ReInput.players.GetPlayer(playerName);
    }

    // Update is called once per frame
    void Update()
    {
        // float dirX = _rewiredPlayer.GetAxis("MoveX");
        // float dirY = _rewiredPlayer.GetAxis("MoveY");
        //
        // Vector2 moveDir = new Vector2(dirX, dirY);
        // moveDir.Normalize();
        //
        // _entity.Move(moveDir);
        // Debug.Log(moveDir);
        
        float dirX = _rewiredPlayer.GetAxis("MoveX");
        if (dirX != 0f)
        {
            Debug.Log(dirX);
            dirX = Mathf.Sign(dirX);
        }
        
        _entity.Move(dirX);
        
        if (_rewiredPlayer.GetButtonDown("Jump") && _entity.IsOnground())
        {
            _entity.Jump();
        }
    }
}
