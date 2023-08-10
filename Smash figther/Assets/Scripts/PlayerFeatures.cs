using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFeatures : MonoBehaviour
{
    public GameObject throwablePrefab;
    public List<ItemGrab.ItemTypeEnum> inventory = new List<ItemGrab.ItemTypeEnum>();
    public ItemGrab.ItemTypeEnum selectedItemType;
    public int selectedItemRow;
    public ItemGrab item;
    public bool canGrabItem = false;
    public bool canThrowItem = false;

    private void Update()
    {
        if (inventory.Count > 0)
        {
            canThrowItem = true;
        }
        else
        {
            canThrowItem = false;
        }
    }

    public void GrabItem()
    {
        inventory.Add(item.itemType);
        GameObject.Destroy(item.gameObject);
    }

    public void ThrowItem()
    {
        selectedItemType = inventory.First();
        selectedItemRow = 0;
        var temp = throwablePrefab.GetComponent<ItemThrow>();
        temp.itemType = selectedItemType;
        Instantiate(temp, this.transform.position, Quaternion.identity);

        if (inventory.Count > 1)
        {
            inventory.Remove(inventory[selectedItemRow]);
        }
        else
        {
            inventory.Clear();
        }
    }
}
