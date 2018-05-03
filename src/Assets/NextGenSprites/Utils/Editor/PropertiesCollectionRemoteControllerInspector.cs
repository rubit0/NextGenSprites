using UnityEngine;
using UnityEditor;
using NextGenSprites.PropertiesCollections;

[CustomEditor(typeof(PropertiesCollectionProxyController))]
public class PropertiesCollectionRemoteControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        var _target = (PropertiesCollectionProxyController)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("FindManagerByReference"), new GUIContent("Find Manager by Id"));
        EditorGUILayout.Space();

        if (_target.FindManagerByReference)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ManagerReferenceId"), new GUIContent("Id Reference Name"));
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CollectionManager"), new GUIContent("Manager"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
