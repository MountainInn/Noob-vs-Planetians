using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Volume))]
public class VolumeDrawer : PropertyDrawer
{
    GUILayoutOption
        width = GUILayout.Width(10),
        width2 = GUILayout.Width(50),
        width3 = GUILayout.Width(100)
        ;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        
        EditorGUI.BeginProperty(position, label, property);

        position.width = 150;
        EditorGUI.LabelField(position, label);

        position.x += position.width;
        position.width = 75;
        EditorGUI.PropertyField(position,
                                property.FindPropertyRelative("_current"),
                                new GUIContent(""));

        position.x += position.width;
        position.width = 20;        
        EditorGUI.LabelField(position, "/");

        position.x += position.width;
        position.width = 75;        
        EditorGUI.PropertyField(position,
                                property.FindPropertyRelative("_maximum"),
                                new GUIContent(""));

        EditorGUI.EndProperty();
    }
}
