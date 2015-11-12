using System.Reflection;
using UnityEditor;
using UnityEngine;

public class MethodWizard : ScriptableWizard {

    public SerializedProperty Property;
    private bool ended = false;

    protected override bool DrawWizardGUI() {
        if ( Property == null ) return false;

        StartGUI();
        WizardGUI();
        return ended ? false : EndGUI();
    }


    private void StartGUI() {
        EditorGUI.BeginChangeCheck();
    }

    public void WizardGUI() {
        int size = 0;

        try {
            size = Property.arraySize;
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }        

        for ( int i = 0; i < size; i++ ) {
            var parameter = Property.GetArrayElementAtIndex( i );
            var name = parameter.FindPropertyRelative( "Name" );
            var assembly = parameter.FindPropertyRelative( "Assembly" );
            var type = parameter.FindPropertyRelative( "Type" );
            var value = parameter.FindPropertyRelative( "NewValue" );
            var obj = parameter.FindPropertyRelative( "Object" );

            var a = Assembly.Load( assembly.stringValue );
            var t = a.GetType( type.stringValue );

            if ( t == typeof( int ) ) {
                value.stringValue = EditorGUILayout.IntField( name.stringValue, int.Parse( value.stringValue ) ).ToString();
            } else if ( t == typeof( float ) ) {
                value.stringValue = EditorGUILayout.FloatField( name.stringValue, float.Parse( value.stringValue ) ).ToString();
            } else if ( t == typeof( double ) ) {
                value.stringValue = EditorGUILayout.DoubleField( name.stringValue, double.Parse( value.stringValue ) ).ToString();
            } else if ( t == typeof( long ) ) {
                value.stringValue = EditorGUILayout.LongField( name.stringValue, long.Parse( value.stringValue ) ).ToString();
            } else if ( t == typeof( string ) ) {
                value.stringValue = EditorGUILayout.TextField( name.stringValue, value.stringValue );
            } else if ( t == typeof( bool ) ) {
                value.stringValue = EditorGUILayout.Toggle( name.stringValue, bool.Parse( value.stringValue ) ).ToString();
            } else if ( t == typeof( Vector2 ) ) {
                value.stringValue = EditorGUILayout.Vector2Field( name.stringValue, ExtendedEventConverter.Vec2( value.stringValue ) ).ToString();
            } else if ( t == typeof( Vector3 ) ) {
                value.stringValue = EditorGUILayout.Vector3Field( name.stringValue, ExtendedEventConverter.Vec3( value.stringValue ) ).ToString();
            } else if ( t == typeof( Vector4 ) ) {
                value.stringValue = EditorGUILayout.Vector4Field( name.stringValue, ExtendedEventConverter.Vec4( value.stringValue ) ).ToString();
            } else if ( t == typeof( GameObject ) ) {
                var objectFound = GameObject.Find( value.stringValue );
                objectFound = (GameObject)EditorGUILayout.ObjectField( name.stringValue, objectFound, typeof( GameObject ), true );
                if ( objectFound != null ) {
                    value.stringValue = objectFound.name;
                }
            } else if ( t == typeof( Bounds ) ) {
                value.stringValue = EditorGUILayout.BoundsField( name.stringValue, ExtendedEventConverter.Bounds( value.stringValue ) ).ToString();
            } else if ( t == typeof( Rect ) ) {
                value.stringValue = EditorGUILayout.RectField( name.stringValue, ExtendedEventConverter.Rect( value.stringValue ) ).ToString();
            } else if ( t == typeof( AnimationCurve ) ) {
                var curve = EditorGUILayout.CurveField( name.stringValue, ExtendedEventConverter.Curve( value.stringValue ) );
                value.stringValue = "";
                for ( int j = 0; j < curve.keys.Length; j++ ) {
                    var item = curve.keys[j];
                    value.stringValue += string.Format( "{0}|{1}|{2}|{3}|{4};",
                        item.inTangent.ToString(), item.outTangent.ToString(),
                        item.tangentMode.ToString(), item.time.ToString(), item.value.ToString() );
                }
            } else if ( t == typeof( Color ) ) {
                value.stringValue = EditorGUILayout.ColorField(name.stringValue, ExtendedEventConverter.Color( value.stringValue ) ).ToString();
            } else if ( t.IsSubclassOf( typeof( System.Enum ) ) ) {
                var eValue = (System.Enum)System.Enum.Parse( t, value.stringValue );
                value.stringValue = EditorGUILayout.EnumPopup( name.stringValue, eValue ).ToString();
            } else if ( t.IsSubclassOf( typeof( Object ) ) ) {
                obj.objectReferenceValue = EditorGUILayout.ObjectField( name.stringValue, obj.objectReferenceValue, t, true );
            } else {
                EditorGUILayout.HelpBox( string.Format( "The field \"{0}\" with type \"{1}\" is not supported", name.stringValue, t.Name ), MessageType.Warning );
            }
        }
    }

    private bool EndGUI() {
        if ( EditorGUI.EndChangeCheck() ) {
            Property.serializedObject.ApplyModifiedProperties();
            return true;
        }

        return false;
    }

    private void OnWizardCreate() { }

}
