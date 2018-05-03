using UnityEngine;
using UnityEditor;
using NextGenSprites.PropertiesCollections;

[CustomEditor(typeof(PropertiesCollectionProxyManager))]
public class PropertiesCollectionRemoteManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        var _target = (PropertiesCollectionProxyManager)target;

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PropCollections"), new GUIContent("Properties Collections"),true);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ReferenceId"));

        if(_target.transform.GetComponent<SpriteRenderer>() != null)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetThis"));

            if (_target.TargetThis)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SourceObject"));
            }
        }
        else
        {
            _target.TargetThis = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SourceObject"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
