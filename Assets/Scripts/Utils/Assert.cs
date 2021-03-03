using UnityEngine;
public class Assert
{
    public static void AssertAssigned(Object obj)
    {
        if (!obj)
        {
            Debug.Log("Error: object is not assigned or non-zero");
        }
    }
}