using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CustomPlayerController : MonoBehaviour
{
    private PlayerController _entity;
    private Player _rewiredPlayer;

    public string playerName = "Player1";
    
    // Start is called before the first frame update
    void Start()
    {
        _entity = GetComponentInChildren<PlayerController>();
        _rewiredPlayer = ReInput.players.GetPlayer(playerName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
