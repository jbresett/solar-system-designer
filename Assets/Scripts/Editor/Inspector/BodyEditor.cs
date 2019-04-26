using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Body))]
[CanEditMultipleObjects]
public class BodyEditor : Editor
{
    SerializedProperty typeProp;

    void OnEnable()
    {
        //typeProp = serializedObject.FindProperty("Type");
    }

    public override void OnInspectorGUI()
    {
        Body body = (Body)target;

        // Id
        GUI.enabled = false;
        EditorGUILayout.IntField("Id", body.Id);
        GUI.enabled = true;

        // Type
        body.Type = (BodyType)EditorGUILayout.EnumPopup("Type", body.Type);

        
        // Position
        v3d("Position", body.Position);
        body.Position = body.Position; // Set here to update Unity's Transform.

        // Velocity
        v3d("Velocity", body.Velocity);

        // Mass & Size
        body.Mass = EditorGUILayout.DoubleField("Mass", body.Mass);
        body.Diameter = EditorGUILayout.DoubleField("Diameter", body.Diameter);
        body.Rotation = EditorGUILayout.DoubleField("Rotation", body.Rotation);

        // Graphics
        body.Material = (BodyMaterial)EditorGUILayout.EnumPopup("Material", body.Material);

        // Make sure values are set.
        EditorUtility.SetDirty(body);
    }

    private void v3d(string name, Vector3d vector)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(name);
        EditorGUIUtility.labelWidth = 15;
        vector.x = EditorGUILayout.DoubleField("X", vector.x);
        vector.y = EditorGUILayout.DoubleField("Y", vector.y);
        vector.z = EditorGUILayout.DoubleField("Z", vector.z);
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 0;
    }
    
    
}