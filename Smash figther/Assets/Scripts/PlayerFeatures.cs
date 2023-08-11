using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;

public class PlayerFeatures : MonoBehaviour
{
    public GameObject throwablePrefab;
    public List<ItemGrab.ItemTypeEnum> inventory = new List<ItemGrab.ItemTypeEnum>();
    public ItemGrab.ItemTypeEnum selectedItemType;
    public int selectedItemRow;
    public ItemGrab item;
    public bool facingRight = true;
    public bool canGrabItem = false;
    public bool canThrowItem = false;
    public bool isHolding = false;
    public double holdTime;

    public float returnTime;
    public float throwForce;
    public Vector2 throwEndPoint;
    public Vector3 throwOffset;
    public Vector2 throwStartPoint;
    public Transform throwTrans;
    private float _orientX;
    
    private void Update()
    {
        throwStartPoint = this.transform.position + throwOffset;
        
        Vector3 angle = throwTrans.transform.eulerAngles;
        Vector2 throwRotation;
        throwRotation.x = ReInput.players.GetPlayer(transform.GetComponent<PlayerController>().playerName).GetAxis("MoveXAim");
        throwRotation.y = ReInput.players.GetPlayer(transform.GetComponent<PlayerController>().playerName).GetAxis("MoveYAim");
        
        throwTrans.position = throwStartPoint;
        
        if (throwRotation.x == 0f && throwRotation.y == 0f)
        {
            Vector3 currentRotation = throwTrans.transform.localEulerAngles;
            Vector3 homeRotation;

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
        
        if (ReInput.players.GetPlayer(transform.GetComponent<PlayerController>().playerName)
            .GetButtonTimePressed("Throw") >= holdTime)
        {
            isHolding = true;
        }
        else
        {
            isHolding = false;
        }
            
        if (inventory.Count > 0)
        {
            canThrowItem = true;
        }
        else
        {
            canThrowItem = false;
        }

        if ((_orientX > 0.6f) || (throwRotation.x > 0.6f && isHolding))
        {
            throwOffset.x = 1;
            facingRight = true;
        }
        else if ((_orientX < -0.6f) || (throwRotation.x < -0.6f && isHolding))
        {
            throwOffset.x = -1;
            facingRight = false;
        }
    }

    public void GetOrient(float orient)
    {
        _orientX = orient;
    }
    
    Vector2 RotationToVector(float degrees) {

        Quaternion rotation = Quaternion.Euler(0, 0, degrees);
        Vector2 v = rotation * Vector3.down;

        return v;
    }
    
    public void GrabItem()
    {
        inventory.Add(item.itemType);
        //GameObject.Destroy(item.gameObject);
    }

    public void ThrowItem()
    {
        selectedItemType = inventory.First();
        selectedItemRow = 0;
        var temp = throwablePrefab.GetComponent<ItemThrow>();
        temp.itemType = selectedItemType;
        var spawnedObject = Instantiate(temp, throwStartPoint, Quaternion.identity);
        //spawnedObject.transform.rotation = throwTrans.rotation;
        var rotatedLine = Quaternion.AngleAxis(throwTrans.localEulerAngles.z, transform.up);
        spawnedObject.GetComponent<Rigidbody2D>().AddForce(RotationToVector(throwTrans.localEulerAngles.z +90) * throwForce, ForceMode2D.Impulse);
        
        if (inventory.Count > 1)
        {
            inventory.Remove(inventory[selectedItemRow]);
        }
        else
        {
            inventory.Clear();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (!Application.isPlaying)
        {
            Gizmos.DrawCube(throwTrans.position, new Vector3(0.1f,0.1f,0.1f));
            //Gizmos.DrawRay(throwPoint, throwRotation);
            Gizmos.DrawLine(throwStartPoint, throwEndPoint);
        }
        else
        {
            Gizmos.DrawCube( throwTrans.position, new Vector3(0.1f,0.1f,0.1f));
            //Gizmos.DrawRay(throwPoint, throwRotation);
            Gizmos.DrawLine(throwStartPoint, throwEndPoint);
        }
    }
}
