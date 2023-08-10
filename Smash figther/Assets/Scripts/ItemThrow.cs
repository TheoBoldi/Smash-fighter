using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemThrow : MonoBehaviour
{
    public ItemGrab.ItemTypeEnum itemType;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.AddForce(Vector2.right);
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
}
