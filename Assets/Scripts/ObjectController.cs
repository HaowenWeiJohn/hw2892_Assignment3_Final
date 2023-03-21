using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public Vector3 positionStartDrag;
    public Vector3 positionAfterDrag;

    public bool onDrag = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void onDragEnter()
    {
        onDrag = true;
        positionStartDrag = transform.position;
    }
    // object controller

    void onDragExit()
    {
        onDrag = false;
        positionAfterDrag = transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Untagged")
        {
            // overlapped with other object
            Debug.Log("Colliding with game items");
        }

    }

}
