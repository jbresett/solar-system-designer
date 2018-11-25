using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permission System.
/// 
/// Node-based permission system.  Uses '.' between nodes for more limited permissions.
/// Example: "edit" perm would mean the ability to edit anything.
///   "edit.planet" would have the ability to edit any planet.
///   "edit.planet.earth.mass" would have permission to just edit earth's mass.
/// 
/// Intitial permission are set by the ALEP based on the lession.
/// Permissions can be changed by the instructor in the case of a live session.
/// Permissions are checked by the software prior to any action that could require a permission. 
/// </summary>
public class Perm {
    
    /// <summary>
    /// Checks for user permission.
    /// </summary>
    /// <param name="perm">The permission to check for.</param>
    /// <param name="exact">Exact permission only. If true, will only the exact node exists.
    ///     If false, checks for all upper level nodes as well (e.g. "edit.gravity" would check for "edit" node as well).</param>
    /// <returns>True if the user has the permission set.</returns>
    static public bool Has(string perm, bool exact = false)
    {
        ExposedData exp = Main.Instance.Exposed;
        List<string> list = exp.Perms.getList();
        
        // Loop for checking upper level permission as well.
        while (true)
        {

            // Check Permission
            if (exp.Perms.getList().Contains(perm)) return true;

            // If exact (1st) match is not found and only checking for exact matches, return.
            if (exact) return false;

            // Remove last layer from permission to check next level up.
            int periodPos = perm.IndexOf('.');
            if (periodPos == -1) return false;
            perm = perm.Substring(0, perm.LastIndexOf("."));
        }
    }

    /// <summary>
    /// Adds a set of permission nodes.  
    /// </summary>
    /// <param name="perms">set of permission nodes.</param>
    /// <returns>True if any of the nodes were added, false if the user already had every node.</returns>
    static public bool Add(string[] perms)
    {
        // Turns true once a new node has been added (not already on the perm list).
        bool result = false;

        // Adds each node if it hasn't already been added, verifying if there are any changes.
        foreach(string perm in perms)
        {
            result |= Add(perm);
        }

        return result;
    }

    /// <summary>
    /// Adds a permission node. Should only be called after verifying caller is an instructor or admin.
    /// </summary>
    /// <param name="perm">Permission name. Use '.' to add specific layers.</param>
    /// <returns>True if the permission is added, false if it already exists.</returns>
    static public bool Add(string perm)
    {
        ExposedData exp = Main.Instance.Exposed;
        List<string> list = exp.Perms.getList();

        // Check if permission already exists.
        if (list.Contains(perm)) return false;

        // Add item and update value.
        list.Add(perm);
        exp.Perms.updateValue();

        return true;
    }

    /// <summary>
    /// Removes a permission node. Should only be called after verifying caller is an instructor.
    /// </summary>
    /// <param name="perm">Full Permission name.  Removing </param>
    /// <param name="remvoeChildren">Removes any children nodes that have the same parent.</param>
    /// <returns>True if any permission was removed, false if the node (or any children for removeChilden) did not exist to begin with.</returns>
    static public bool Remove(string perm, bool removeChildren = false)
    {
        ExposedData exp = Main.Instance.Exposed;
        List<string> list = exp.Perms.getList();

        // Gets initial size to later check for any changes.
        int initSize = list.Count;

        // Remove perms, and all children perms if removeChildren is true.
        list.Remove(perm);
        if (removeChildren) list.RemoveAll(val => val.StartsWith(perm + "."));

        // If list changed, update values.
        if (list.Count != initSize) exp.Perms.updateValue();

        // Returnsif the size has changed.
        return (list.Count != initSize);
    }

   
    /// <summary>
    /// Clears all current permmision nodes.
    /// </summary>
    /// <returns>True if cleared, false if nothing to clear.</returns>
    static public bool Clear()
    {
        ExposedData exp = Main.Instance.Exposed;
        List<string> list = exp.Perms.getList();

        if (list.Count == 0) return false;

        list.Clear();
        exp.Perms.updateValue();

        return true;
    }

}
