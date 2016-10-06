using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer( typeof( ExtendedEvent ) )]
public class ExtendedEventPropertyDrawer : PropertyDrawer {

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
        return base.GetPropertyHeight( property, label );
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
        base.OnGUI( position, property, label );
    }
}
