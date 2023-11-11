using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ItemThrow : MonoBehaviour
{
    public ItemGrab.ItemTypeEnum itemType;
    [HideInInspector]
    public bool isDroped = false;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _renderer;
    private GameObject grabPrefab;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.AddForce(Vector2.right);
        _renderer = GetComponent<SpriteRenderer>();
        grabPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ItemGrab.prefab");

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
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && isDroped)
        {
            var temp = grabPrefab.GetComponent<ItemGrab>();
            temp.itemType = itemType;
            var spawnedObject = Instantiate(temp, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Player") && !isDroped)
        {
           //Destroy(this.gameObject);
        }
    }
}
