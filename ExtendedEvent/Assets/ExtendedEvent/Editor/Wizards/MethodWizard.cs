using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class MethodWizard : ScriptableWizard {

    public ExtendedEvent.Method Method;

    protected override bool DrawWizardGUI() {
        if ( Method == null ) return false;

        WizardGUI();
        return true;
    }

    public void WizardGUI() {
        int size = 0;

        try {
            size = Method.Parameters.Count;
        } catch ( NullReferenceException ) {
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }

        for ( int i = 0; i < size; i++ ) {
            var parameter = Method.Parameters[i];
            switch ( parameter.TypeName ) {
                case "String":
                    parameter.StringValue = EditorGUILayout.TextField( parameter.Name, parameter.StringValue );
                    break;
                case "Int32":
                    parameter.IntValue = EditorGUILayout.IntField( parameter.Name, parameter.IntValue );
                    break;
                case "Int64":
                    parameter.LongValue = EditorGUILayout.LongField( parameter.Name, parameter.LongValue );
                    break;
                case "Single":
                    parameter.FloatValue = EditorGUILayout.FloatField( parameter.Name, parameter.FloatValue );
                    break;
                case "Double":
                    parameter.DoubleValue = EditorGUILayout.DoubleField( parameter.Name, parameter.DoubleValue );
                    break;
                case "Boolean":
                    parameter.BoolValue = EditorGUILayout.Toggle( parameter.Name, parameter.BoolValue );
                    break;
                case "Vector2":
                    parameter.Vector2Value = EditorGUILayout.Vector2Field( parameter.Name, parameter.Vector2Value );
                    break;
                case "Vector3":
                    parameter.Vector3Value = EditorGUILayout.Vector3Field( parameter.Name, parameter.Vector3Value );
                    break;
                case "Vector4":
                    parameter.Vector4Value = EditorGUILayout.Vector4Field( parameter.Name, parameter.Vector4Value );
                    break;
                case "Quaternion":
                    var v4 = new Vector4( parameter.QuaternionValue.x, parameter.QuaternionValue.y, parameter.QuaternionValue.z, parameter.QuaternionValue.w );
                    v4 = EditorGUILayout.Vector4Field( parameter.Name, v4 );
                    parameter.QuaternionValue = new Quaternion( v4.x, v4.y, v4.z, v4.w );
                    break;
                case "Bounds":
                    parameter.BoundsValue = EditorGUILayout.BoundsField( parameter.Name, parameter.BoundsValue );
                    break;
                case "Rect":
                    parameter.RectValue = EditorGUILayout.RectField( parameter.Name, parameter.RectValue );
                    break;
                case "Matrix4x4":

                    break;
                case "AnimationCurve":
                    parameter.AnimationCurveValue = EditorGUILayout.CurveField( parameter.AnimationCurveValue );
                    break;
                case "Object":
                    parameter.ObjectValue = EditorGUILayout.ObjectField( parameter.Name, parameter.ObjectValue, parameter.Type, true );
                    break;
                case "Enum":
                    var enumValue = (Enum)Enum.Parse( parameter.Type, parameter.EnumNames[parameter.EnumValue] );
                    enumValue = EditorGUILayout.EnumPopup( parameter.Name, enumValue );
                    for ( int j = 0; j < parameter.EnumNames.Length; j++ ) {
                        if ( parameter.EnumNames[j] == enumValue.ToString() ) {
                            parameter.EnumValue = j;
                            break;
                        }
                    }
                    break;
                default:
                    EditorGUILayout.HelpBox( string.Format( "The type {0} is not supported", parameter.RepresentableType ), MessageType.Warning );
                    break;
            }
        }
    }

    private void OnWizardCreate() { }

}
