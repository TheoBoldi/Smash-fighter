using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerFeatures _entity;
    public string playerName;
    public List<Image> highlightList;
    public List<Image> itemList;

    // Start is called before the first frame update
    void Start()
    {
        _entity = GameObject.Find(playerName).GetComponent<PlayerFeatures>();

        for (int i = 0; i < transform.childCount; i++)
        {
            highlightList.Add(transform.GetChild(i).GetChild(1).GetComponent<Image>());
            itemList.Add(transform.GetChild(i).GetChild(0).GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _entity.inventoryCapacity; i++)
        {
            if (i < _entity.inventory.Count)
            {
                itemList[i].gameObject.SetActive(true);
                switch (_entity.inventory[i])
                {
                    case ItemGrab.ItemTypeEnum.Fire:
                        itemList[i].color = Color.red;
                        break;
                    case ItemGrab.ItemTypeEnum.Water:
                        itemList[i].color = Color.cyan;
                        break;
                    case ItemGrab.ItemTypeEnum.Oil:
                        itemList[i].color = Color.black;
                        break;
                    case ItemGrab.ItemTypeEnum.Honey:
                        itemList[i].color = Color.yellow;
                        break;
                    case ItemGrab.ItemTypeEnum.Electric:
                        itemList[i].color = Color.magenta;
                        break;
                    case ItemGrab.ItemTypeEnum.Ice:
                        itemList[i].color = Color.blue;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                itemList[i].gameObject.SetActive(false);
            }

            for (int j = 0; j < highlightList.Count; j++)
            {
                if (_entity.selectedItemRow == i && _entity.inventory.Count != 0)
                {
                    highlightList[i].gameObject.SetActive(true);
                }
                else
                {
                    highlightList[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
