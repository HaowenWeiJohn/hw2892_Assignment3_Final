//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class PlaceObjectManager : MonoBehaviour
//{

//    //public Button WallUnitObjectButton;
//    //public Button TemberOjbectButton;
//    //public Button TemberOjbectButton;
//    //public Button TemberOjbectButton;
//    //public Button TemberOjbectButton;
//    //public Button TemberOjbectButton;
//    //public Button TemberOjbectButton;
//    //public Button TemberOjbectButton;
//    //public Button TemberOjbectButton;

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;
using  UnityEngine.EventSystems;
using System.IO;

public class PlaceObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject Apple;
    [SerializeField] private GameObject Tree;
    [SerializeField] private GameObject GolfBall;
    [SerializeField] private GameObject Hole;
    [SerializeField] private GameObject Rock;
    [SerializeField] private GameObject WallUnit;
    [SerializeField] private GameObject ReferenceObject;

    // place object buttons
    [SerializeField] private Button WallUnitObjectButton;
    [SerializeField] private Button TreeObjectButton;
    [SerializeField] private Button AppleObjectButton;
    [SerializeField] private Button RockObjectButton;
    [SerializeField] private Button GolfBallObjectButton;
    [SerializeField] private Button HoleObjectButton;
    [SerializeField] private Button ReferenceObjectButton;

    [SerializeField] private Button RotateClockWiseButton;
    [SerializeField] private Button RotateCounterClockWiseButton;
    [SerializeField] private Button SaveManipulationButton;
    [SerializeField] private Button RemoveObjectButton;

    [SerializeField] private Button SaveButton;
    [SerializeField] private Button ResetButton;

    [SerializeField] private GameObject ObjectButtons;
    [SerializeField] private GameObject ManipulateObjectButtons;
    [SerializeField] private GameObject ReduUndoButtons;
    [SerializeField] private GameObject SaveResetButtons;



    private int gameState = Presets.IdleState;
    
    private GameObject currentSelectedObject;
    private int currentSelectionState = 0;
    


    //private GameObject placedObject;
    private ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();


    // object list
    public List<GameObject> placedObjectsList = new List<GameObject>();
    
    public GameObject manipulatingObject = null;


    //void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
    //    Debug.Log("Button held down");
    //}

    //void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    //{
    //    Debug.Log("Button held up");
    //}


    /// <summary>
    /// </summary>
    /// 

    //////////////////// manipulate object
    ///

    private GameObject golfBall = null;
    private GameObject hole = null;
    private GameObject referenceObject = null;


    //public TextAsset save;
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        connectButtonFunctions();
        IdleStateGUI();
    }


    void Update()
    {
        //Debug.Log(gameState);
        // only try placing object if one-finger touch
        if (Input.touchCount == 1)
        {
            //get touch
            Touch touch = Input.GetTouch(0);

            // only try placing if the touch is a tap
            // (as opposed to a drag/flick)


            // state: idel state

            if (gameState == Presets.IdleState)
            {
                // place object button pressed, we go to place object state

                // select object, we go to manipulate object state
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    selectObject();
                }
            }
            else if(gameState == Presets.PlacingObjectState)
            {
                if (currentSelectionState != 0)
                {
                    // check if selecting buttons
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (touch.phase == TouchPhase.Ended)
                        {
                            TryPlaceObject(touch);
                        }
                    }
                }
            }
            else if(gameState == Presets.ManipulateObjectState)
            {
                // do nothing
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    moveObjectWithTouch(touch);
                }

            }


        }
    }


    void FixedUpdate()
    {
        // rotate object
        if (RotateClockWiseButton.GetComponent<ButtonState>().buttonPressed)
        {
            manipulatingObject.transform.RotateAround(manipulatingObject.transform.position, Vector3.up, Presets.ManipulateObjectRotationSpeed * Time.deltaTime);
        }
        else if (RotateCounterClockWiseButton.GetComponent<ButtonState>().buttonPressed) {
            manipulatingObject.transform.RotateAround(manipulatingObject.transform.position, Vector3.up, -Presets.ManipulateObjectRotationSpeed * Time.deltaTime);
        }
    }

    void selectObject()
    {
        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (touch.phase == TouchPhase.Ended) // select object if end phase
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.tag);
                if (
                    hit.collider.tag == Presets.AppleTag ||
                    hit.collider.tag == Presets.HoleTag ||
                    hit.collider.tag == Presets.RockTag ||
                    hit.collider.tag == Presets.TreeTag ||
                    hit.collider.tag == Presets.ReferenceObjectTag ||
                    hit.collider.tag == Presets.WallUnitTag
                    )
                {
                    manipulatingObject = hit.collider.gameObject;
                    
                    // state transition
                    setGameState(Presets.ManipulateObjectState);
                    ManipulatingObjectStateGUI();
                    
                    Debug.Log("Selected");
                }
            }
        }
    }


    // game state transition

    void IdleStateGUI()
    {
        resetSelectionButtons();
        SaveResetButtons.SetActive(true);
        ManipulateObjectButtons.SetActive(false);
        ObjectButtons.SetActive(true);
    }

    void ManipulatingObjectStateGUI()
    {
        SaveResetButtons.SetActive(false);
        ManipulateObjectButtons.SetActive(true);
        ObjectButtons.SetActive(false);
    }


    //void IdleStateToPlaceingObjectState()
    //{
    //    setGameState(Presets.PlacingObjectState);
    //}

    //void IdleStateToManipulatingObjectState()
    //{
    //    setGameState(Presets.ManipulatingObjectState);
    //}


    //void ManipulatingObjectStateToIdleState()
    //{
    //    setGameState(Presets.IdleState);
    //}



    void connectButtonFunctions()
    {
        WallUnitObjectButton.onClick.AddListener(WallUnitObjectButtonClicked);
        TreeObjectButton.onClick.AddListener(TreeObjectButtonClicked);
        AppleObjectButton.onClick.AddListener(AppleObjectButtonClicked);
        RockObjectButton.onClick.AddListener(RockObjectButtonClicked);
        GolfBallObjectButton.onClick.AddListener(GolfBallObjectButtonClicked);
        HoleObjectButton.onClick.AddListener(HoleObjectButtonClicked);
        ReferenceObjectButton.onClick.AddListener(ReferenceObjectButtonClicked);

        // manipulation state
        SaveManipulationButton.onClick.AddListener(SaveManipulationButtonClicked);
        RemoveObjectButton.onClick.AddListener(RemoveObjectButtonClicked);

        // save reset
        ResetButton.onClick.AddListener(ResetButtonClicked);
        SaveButton.onClick.AddListener(saveButtonClicked);
    }


    void setGameState(int state)
    {
        gameState = state;
    }

    void resetSelectionState()
    {
        
        currentSelectionState = Presets.NoButtonSelected;
        currentSelectedObject = null;

        setGameState(Presets.IdleState);
    }
    void resetSelectionButtons()
    {
        setButtonColor(WallUnitObjectButton, Presets.UnselectedColor);
        setButtonColor(TreeObjectButton, Presets.UnselectedColor);
        setButtonColor(AppleObjectButton, Presets.UnselectedColor);
        setButtonColor(RockObjectButton, Presets.UnselectedColor);
        setButtonColor(GolfBallObjectButton, Presets.UnselectedColor);
        setButtonColor(HoleObjectButton, Presets.UnselectedColor);
        setButtonColor(ReferenceObjectButton, Presets.UnselectedColor);

    }

    void setButtonColor(Button button, Color color)
    {
        button.GetComponent<Image>().color = color;
    }

    void setSelectionState(int selectionState)
    {
        currentSelectionState = selectionState;
    }

    void selectButton(int selectionState, Button selectedButton, GameObject selectedObject)
    {


        setSelectionState(selectionState);
        resetSelectionButtons();
        setButtonColor(selectedButton, Presets.SelectedColor);
        currentSelectedObject = selectedObject;


        setGameState(Presets.PlacingObjectState);
    }

    void WallUnitObjectButtonClicked()
    {
        if (currentSelectionState == Presets.WallUnitObjectButtonSelected)
        {
            resetSelectionState();
            setButtonColor(WallUnitObjectButton, Presets.UnselectedColor);
        }
        else {
            selectButton(Presets.WallUnitObjectButtonSelected, WallUnitObjectButton, WallUnit);
            //// change selection state
            //setSelectionState(Presets.WallUnitObjectButtonSelected);
            //// update the selection button color
            //resetSelectionButtons();
            //setButtonColor(WallUnitObjectButton, Presets.SelectedColor);
        }
    }

    void TreeObjectButtonClicked()
    {
        if (currentSelectionState == Presets.TreeObjectButtonSelected)
        {
            resetSelectionState();
            setButtonColor(TreeObjectButton, Presets.UnselectedColor);
        }
        else
        {
            selectButton(Presets.TreeObjectButtonSelected, TreeObjectButton, Tree);
            //// change selection state
            //setSelectionState(Presets.TreeObjectButtonSelected);
            //// update the selection button color
            //resetSelectionButtons();
            //setButtonColor(TreeObjectButton, Presets.SelectedColor);
        }
    }

    void AppleObjectButtonClicked()
    {
        if (currentSelectionState == Presets.AppleObjectButtonSelected)
        {
            resetSelectionState();
            setButtonColor(AppleObjectButton, Presets.UnselectedColor);
        }
        else
        {
            selectButton(Presets.AppleObjectButtonSelected, AppleObjectButton, Apple);
            //// change selection state
            //setSelectionState(Presets.DuckObjectButtonSelected);
            //// update the selection button color
            //resetSelectionButtons();
            //setButtonColor(DuckObjectButton, Presets.SelectedColor);
        }
    }

    void RockObjectButtonClicked()
    {
        if (currentSelectionState == Presets.RockObjectSelected)
        {
            resetSelectionState();
            setButtonColor(RockObjectButton, Presets.UnselectedColor);
        }
        else
        {
            selectButton(Presets.RockObjectSelected, RockObjectButton, Rock);
            //// change selection state
            //setSelectionState(Presets.RockObjectSelected);
            //// update the selection button color
            //resetSelectionButtons();
            //setButtonColor(RockObjectButton, Presets.SelectedColor);
        }
    }

    void GolfBallObjectButtonClicked()
    {
        if (currentSelectionState == Presets.GolfObjectBallSelected)
        {
            resetSelectionState();
            setButtonColor(GolfBallObjectButton, Presets.UnselectedColor);
        }
        else
        {
            selectButton(Presets.GolfObjectBallSelected, GolfBallObjectButton, GolfBall);
            //// change selection state
            //setSelectionState(Presets.GolfObjectBallSelected);
            //// update the selection button color
            //resetSelectionButtons();
            //setButtonColor(GolfBallObjectButton, Presets.SelectedColor);
        }
    }


    void HoleObjectButtonClicked()
    {
        if (currentSelectionState == Presets.HoleObjectButtonSelected)
        {
            resetSelectionState();
            setButtonColor(HoleObjectButton, Presets.UnselectedColor);
        }
        else
        {
            selectButton(Presets.HoleObjectButtonSelected, HoleObjectButton, Hole);
            //// change selection state
            //setSelectionState(Presets.GolfObjectBallSelected);
            //// update the selection button color
            //resetSelectionButtons();
            //setButtonColor(GolfBallObjectButton, Presets.SelectedColor);
        }
    }


    void ReferenceObjectButtonClicked()
    {
        if (currentSelectionState == Presets.ReferenceObjectButtonSelected)
        {
            resetSelectionState();
            setButtonColor(ReferenceObjectButton, Presets.UnselectedColor);
        }
        else
        {
            selectButton(Presets.ReferenceObjectButtonSelected, ReferenceObjectButton, ReferenceObject);
            //// change selection state
            //setSelectionState(Presets.ReferenceObjectButtonSelected);
            //// update the selection button color
            //resetSelectionButtons();
            //setButtonColor(ReferenceObjectButton, Presets.SelectedColor);
        }
    }


    void SaveManipulationButtonClicked()
    {

        IdleStateGUI();
        setGameState(Presets.IdleState);

    }

    void RemoveObjectButtonClicked()
    {
        placedObjectsList.Remove(manipulatingObject);
        Destroy(manipulatingObject);
        manipulatingObject = null;
        IdleStateGUI();
        setGameState(Presets.IdleState);
    }



    void ResetButtonClicked()
    {
        foreach (GameObject placedObject in placedObjectsList){
            Destroy(placedObject);
        }
        placedObjectsList = new List<GameObject>();
        setGameState(Presets.IdleState);
        IdleStateGUI();
    }










    // helper function to place duck
    void TryPlaceObject(Touch touch)
    {
        // get touch position (2D, on screen)
        Vector2 touchPos = touch.position;

        // raycasts to worldspace (3D)
        // and check if the 3D space is on the ground






        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit hitCollider;

        //if (touch.phase == TouchPhase.Ended) // select object if end phase
        //{
        //    if (Physics.Raycast(ray, out hitCollider))
        //    {
        //        Debug.Log(hitCollider.collider.tag);
        //        if (
        //            hitCollider.collider.tag == Presets.AppleTag ||
        //            hitCollider.collider.tag == Presets.HoleTag ||
        //            hitCollider.collider.tag == Presets.RockTag ||
        //            hitCollider.collider.tag == Presets.TreeTag ||
        //            hitCollider.collider.tag == Presets.ReferenceObjectTag ||
        //            hitCollider.collider.tag == Presets.WallUnitTag
        //            )
        //        {
        //            manipulatingObject = hitCollider.collider.gameObject;

        //            // state transition
        //            setGameState(Presets.ManipulateObjectState);
        //            ManipulatingObjectStateGUI();

        //            Debug.Log("Selected");
        //        }
        //    }
        //}

        // select block and hit block




        //if (Physics.Raycast(ray, out hitCollider)){
        //    Debug.Log("Touch Hit Object");
        //    if (currentSelectedObject.tag == Presets.WallUnitTag && hitCollider.collider.tag==Presets.WallUnitTag ) {
        //        // stack wall unit
        //        Debug.Log("touch hit object");
        //    }
        //}


        if (raycastManager
            .Raycast(touchPos,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0].pose;

            // ball hole reference object

            if (currentSelectedObject.tag == Presets.HoleTag)
            {
                if (hole == null)
                {
                    hole = Instantiate(currentSelectedObject, hit.position, Quaternion.identity);
                    placedObjectsList.Add(hole);
                }
                else
                {
                    hole.transform.position = hit.position;
                }
            }

            else if (currentSelectedObject.tag == Presets.GolfBallTag)
            {
                if (golfBall == null)
                {
                    golfBall = Instantiate(currentSelectedObject, hit.position, Quaternion.identity);
                    placedObjectsList.Add(golfBall);
                }
                else
                {
                    golfBall.transform.position = hit.position;
                }
            }

            else if (currentSelectedObject.tag == Presets.ReferenceObjectTag)
            {
                if (referenceObject == null)
                {
                    referenceObject = Instantiate(currentSelectedObject, hit.position, Quaternion.identity);
                    placedObjectsList.Add(referenceObject);
                }
                else
                {
                    referenceObject.transform.position = hit.position;
                }
            }

            else
            {

                GameObject placedObject = Instantiate(currentSelectedObject, hit.position, Quaternion.identity);
                placedObjectsList.Add(placedObject);
            }

        }
    }


    void moveObjectWithTouch(Touch touch)
    {
        // get touch position (2D, on screen)
        Vector2 touchPos = touch.position;

        // raycasts to worldspace (3D)
        // and check if the 3D space is on the ground
        if (raycastManager
            .Raycast(touchPos,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0].pose;
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
            {
                if (!RotateClockWiseButton.GetComponent<ButtonState>().buttonPressed && !RotateCounterClockWiseButton.GetComponent<ButtonState>().buttonPressed) {
                    manipulatingObject.transform.position = hit.position;
                }

                
            }
        }

    }

    void saveButtonClicked()
    {
        if (referenceObject !=null && golfBall!=null && hole!=null) {
            positionToText();
        }
        else{
            Debug.Log("you must have at least one reference object, golf ball, and hole");
        }
    }

    void positionToText()
    {
        // 
        Presets.RemoveSaveFile();

        foreach (GameObject item in placedObjectsList)
        {
            if (item.tag != Presets.ReferenceObjectTag)
            {
                // save referece object
                //transformationDifference(referenceObject, item);
                //rotationDifference(referenceObject, item);
                Presets.WriteToFile(item.tag, 
                    Presets.TransformationDifference(referenceObject, item),
                    Presets.RotationDifference(referenceObject, item));
            }
            else {
                // save reference object
                Debug.Log("Reference Object does not need to be saved");
            }
        }
    }

    //Vector3 transformationDifference(GameObject reference, GameObject target) {
    //    Vector3 positionDifference = target.transform.position - reference.transform.position;
    //    Debug.Log(positionDifference);
    //    return positionDifference;
    //}

    //Quaternion rotationDifference(GameObject reference, GameObject target)
    //{
    //    Quaternion rotationDifference = target.transform.rotation * Quaternion.Inverse(reference.transform.rotation);
    //    Debug.Log(rotationDifference);
    //    return rotationDifference;
    //}

    //void writeToFile(string tag, Vector3 transformation, Quaternion rotation)
    //{
    //    string path = Application.persistentDataPath + Presets.SaveFileName;
    //    StreamWriter writer = new StreamWriter(path, true);
    //    writer.WriteLine(tag);
    //    writer.WriteLine(transformation);
    //    writer.WriteLine(rotation);

    //    writer.Close();
    //}

    //void removeSaveFile()
    //{
    //    File.Delete(Application.persistentDataPath + Presets.SaveFileName);
    //}

}