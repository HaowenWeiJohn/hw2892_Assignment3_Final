using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollider : MonoBehaviour
{
    // Start is called before the first frame update

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

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Colliding");
    }



}
