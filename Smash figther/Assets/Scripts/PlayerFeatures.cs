using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;
using UnityEditor;

public class PlayerFeatures : MonoBehaviour
{
    [Header("Inventory")]
    public int inventoryCapacity = 4;
    public int selectedItemRow;
    private GameObject throwablePrefab;
    public List<ItemGrab.ItemTypeEnum> inventory = new List<ItemGrab.ItemTypeEnum>();
    [HideInInspector]
    public ItemGrab.ItemTypeEnum selectedItemType;
    [HideInInspector]
    public ItemGrab item;
    
    [Header("Throw")]
    public double holdTime;
    private float returnTime;
    public float throwForce;
    [HideInInspector]
    public bool canGrabItem = false;
    [HideInInspector]
    public bool canThrowItem = false;
    private bool facingRight = true;
    private bool isHolding = false;
    private Vector2 throwEndPoint;
    public Vector3 throwOffset;
    private Vector2 throwRotation;
    private Vector2 throwStartPoint;
    private Transform throwTrans;
    private float _orientX;

    private void Start()
    {
        returnTime = 1000f;
        throwTrans = transform.GetChild(1);
        throwablePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ItemThrow.prefab");
    }

    private void Update()
    {
        ThrowUpdate();
    }

    public int SetSelectedItem(int row)
    {
        if (row == 1)
        {
            if (selectedItemRow < inventoryCapacity - 1 && selectedItemRow < inventory.Count -1)
            {
                selectedItemRow += row;
            }
        }
        else
        {
            if (selectedItemRow > 0f)
            {
                selectedItemRow += row;
            }
        }
        
        return selectedItemRow;
    }
    
    public void GrabItem()
    {
        if (inventory.Count < inventoryCapacity)
        {
            inventory.Add(item.itemType);
            //GameObject.Destroy(item.gameObject);
        }
    }
    
    #region Throw
    public void ThrowItem()
    {
        selectedItemType = inventory[selectedItemRow];
        var temp = throwablePrefab.GetComponent<ItemThrow>();
        temp.itemType = selectedItemType;
        var spawnedObject = Instantiate(temp, throwStartPoint, Quaternion.identity);
        spawnedObject.GetComponent<Rigidbody2D>().AddForce(RotationToVector(throwTrans.localEulerAngles.z +90) * throwForce, ForceMode2D.Impulse);
        
        if (inventory.Count > 1)
        {
            inventory.Remove(inventory[selectedItemRow]);
        }
        else
        {
            inventory.Clear();
        }

        if (selectedItemRow > 0f)
            SetSelectedItem(-1);
    }

    private void ThrowUpdate()
    {
        throwStartPoint = this.transform.position + throwOffset;
        throwTrans.position = throwStartPoint;
        Vector3 angle = throwTrans.transform.eulerAngles;
        Vector3 currentRotation = throwTrans.transform.localEulerAngles;

        Vector3 homeRotation;
        throwRotation.x = ReInput.players.GetPlayer(transform.GetComponent<PlayerController>().playerName).GetAxis("MoveXAim");
        throwRotation.y = ReInput.players.GetPlayer(transform.GetComponent<PlayerController>().playerName).GetAxis("MoveYAim");

        if (inventory.Count > 0)
        {
            canThrowItem = true;
        }
        else
        {
            canThrowItem = false;
        }
        
        if (isHolding)
        {
            if (throwRotation.x == 0f && throwRotation.y == 0f)
            {

                if (facingRight)
                {
                    homeRotation = new Vector3(0f, 0f, 45f);
                }
                else
                {
                    homeRotation = new Vector3(0f, 0f, 135f);
                }
            
                throwTrans.transform.localEulerAngles =
                    Vector3.Slerp(currentRotation, homeRotation, Time.deltaTime * returnTime);
            }
            else
            {
                throwTrans.transform.localEulerAngles =
                    new Vector3(0, 0,Mathf.Atan2(throwRotation.x, throwRotation.y) * -180 / Mathf.PI + 90f);
            }
        }
        else
        {
            if (facingRight)
            {
                homeRotation = new Vector3(0f, 0f, 45f);
            }
            else
            {
                homeRotation = new Vector3(0f, 0f, 135f);
            }
            
            throwTrans.transform.localEulerAngles =
                Vector3.Slerp(currentRotation, homeRotation, Time.deltaTime * returnTime);
        }
        
        if (ReInput.players.GetPlayer(transform.GetComponent<PlayerController>().playerName)
                .GetButtonTimePressed("Throw") >= holdTime)
        {
            isHolding = true;
        }
        else
        {
            isHolding = false;
        }
        
        if ((_orientX > 0.6f && !isHolding) || (throwRotation.x > 0.1f && isHolding))
        {
            throwOffset.x = 1;
            facingRight = true;
        }
        else if ((_orientX < -0.6f && !isHolding) || (throwRotation.x < -0.1f && isHolding))
        {
            throwOffset.x = -1;
            facingRight = false;
        }
    }
    
    Vector2 RotationToVector(float degrees) {

        Quaternion rotation = Quaternion.Euler(0, 0, degrees);
        Vector2 v = rotation * Vector3.down;

        return v;
    }
    
    public void GetOrient(float orient)
    {
        _orientX = orient;
    }
    #endregion

    #region Guizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (!Application.isPlaying)
        {
            Gizmos.DrawLine(throwStartPoint, throwEndPoint);
        }
        else
        {
            Gizmos.DrawCube( throwTrans.position, new Vector3(0.1f,0.1f,0.1f));
            Gizmos.DrawLine(throwStartPoint, throwEndPoint);
        }
    }
    #endregion
}
