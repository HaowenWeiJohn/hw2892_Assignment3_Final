using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Presets
{
    public static int IdleState = 0;
    public static int PlacingObjectState = 1;
    public static int ManipulateObjectState = 2;
    public static int SavingState = 3;

    public static int NoButtonSelected = 0;
    public static int WallUnitObjectButtonSelected = 1;
    public static int TreeObjectButtonSelected = 2;
    public static int DuckObjectButtonSelected = 3;
    public static int RockObjectSelected = 4;
    public static int GolfObjectBallSelected = 5;
    public static int ReferenceObjectButtonSelected = 6;
    public static int AppleObjectButtonSelected = 7;
    public static int HoleObjectButtonSelected = 8;

    


    public static Color SelectedColor = Color.blue;
    public static Color UnselectedColor = Color.white;

    public static string AppleTag = "Apple";
    public static string DuckTag = "Duck";
    public static string HoleTag = "Hole";
    public static string RockTag = "Rock";
    public static string TreeTag = "Tree";
    public static string ReferenceObjectTag = "ReferenceObject";
    public static string WallUnitTag = "WallUnit";
    public static string GolfBallTag = "GolfBall";

    public static float ManipulateObjectRotationSpeed = 30.0f;

    public static int TransformationAction = 1;
    public static int AddItemAction = 2;
    public static int RemoveItemAction = 3;

    public static string SaveFileName = "/save.txt";

    public static Vector3 TransformationDifference(GameObject reference, GameObject target)
    {
        Vector3 positionDifference = target.transform.position - reference.transform.position;
        Debug.Log(positionDifference);
        return positionDifference;
    }

    public static Quaternion RotationDifference(GameObject reference, GameObject target)
    {
        Quaternion rotationDifference = target.transform.rotation * Quaternion.Inverse(reference.transform.rotation);
        Debug.Log(rotationDifference);
        return rotationDifference;
    }

    public static void WriteToFile(string tag, Vector3 transformation, Quaternion rotation)
    {
        string path = Application.persistentDataPath + Presets.SaveFileName;
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(tag);
        writer.WriteLine(transformation);
        writer.WriteLine(rotation);

        writer.Close();
    }

    public static void RemoveSaveFile()
    {
        File.Delete(Application.persistentDataPath + Presets.SaveFileName);
    }
}
