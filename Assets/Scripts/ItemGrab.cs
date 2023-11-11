using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    public enum ItemTypeEnum
    {
        Fire,
        Water,
        Oil,
        Honey,
        Electric,
        Ice
    }

    public ItemTypeEnum itemType;
    private PlayerFeatures _playerFeatures;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        
        switch (itemType)
        {
            case ItemGrab.ItemTypeEnum.Fire:
                _renderer.color = Color.red;
                break;
            case ItemGrab.ItemTypeEnum.Water:
                _renderer.color = Color.cyan;
                break;
            case ItemGrab.ItemTypeEnum.Oil:
                _renderer.color = Color.black;
                break;
            case ItemGrab.ItemTypeEnum.Honey:
                _renderer.color = Color.yellow;
                break;
            case ItemGrab.ItemTypeEnum.Electric:
                _renderer.color = Color.magenta;
                break;
            case ItemGrab.ItemTypeEnum.Ice:
                _renderer.color = Color.blue;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerFeatures = col.GetComponentInParent<PlayerFeatures>();
            _playerFeatures.canGrabItem = true;
            _playerFeatures.item = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerFeatures.canGrabItem = false;
            _playerFeatures.item = null;
            _playerFeatures = null;
        }
    }
}
