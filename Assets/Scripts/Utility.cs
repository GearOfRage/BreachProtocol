using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static List<GameObject> GetChildren(GameObject parentObject)
    {
        List<GameObject> childrenList = new List<GameObject>();
        childrenList.Clear();

        // Check if the parent object exists.
        if (parentObject != null)
        {
            // Loop through all the child GameObjects.
            foreach (Transform child in parentObject.transform)
            {
                // Add each child GameObject to the list.
                childrenList.Add(child.gameObject);
            }
        }
        else
        {
            Debug.LogError("Parent GameObject is not assigned.");
            return null;
        }

        return childrenList;
    }
    
    public static void DestroyAllChildren(GameObject parentObject)
    {
        // Check if the parent object exists.
        if (parentObject != null)
        {
            // Loop through all the child GameObjects.
            foreach (Transform child in parentObject.transform)
            {
                // Destroy each child GameObject.
                Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogError("Parent GameObject is not assigned.");
        }
    }
}
