using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public Vector3 positionStartDrag;
    public Vector3 positionAfterDrag;

    public bool onDrag = false;

    public bool intersectWithOtherObject = false;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onDragEnter()
    {
        onDrag = true;
        positionStartDrag = transform.position;
    }
    // object controller

    public void onDragExit()
    {
        onDrag = false;
        positionAfterDrag = transform.position;

        if (intersectWithOtherObject)
        {
            Debug.Log("Object intersect");
            transform.position = positionStartDrag;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Untagged")
        {
            intersectWithOtherObject = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Untagged")
        {
            intersectWithOtherObject = false;
        }
    }


}
