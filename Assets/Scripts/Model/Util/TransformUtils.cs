using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtils {

    /// <summary>
    /// Finds any child or grandchild with a given name.
    /// </summary>
    /// <param name="input">Top level transform.</param>
    /// <param name="name">child name.</param>
    /// <returns></returns>
    public static Transform FindDescendant(this Transform input, string name)
    {
        // Search Direct Children
        Transform result = input.Find(name);
        if (result != null) return result;

        // Recursivly serch grandchildren.
        foreach (Transform child in input)
        {
            result = FindDescendant(child, name);
            if (result != null) return result;
        }

        // No child Found.
        return null;
    }
}
