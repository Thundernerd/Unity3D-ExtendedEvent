using System;
using UnityEditor;
using UnityEngine;

public class MethodWizard : ScriptableWizard {

    public ExtendedEvent.Member Method;

    private MatrixWizard.EMatrixMode matrixMode;

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
                    parameter.StringValue = EditorGUILayout.TextField( parameter.DisplayName, parameter.StringValue );
                    break;
                case "Int32":
                    parameter.IntValue = EditorGUILayout.IntField( parameter.DisplayName, parameter.IntValue );
                    break;
                case "Int64":
                    parameter.LongValue = EditorGUILayout.LongField( parameter.DisplayName, parameter.LongValue );
                    break;
                case "Single":
                    parameter.FloatValue = EditorGUILayout.FloatField( parameter.DisplayName, parameter.FloatValue );
                    break;
                case "Double":
                    parameter.DoubleValue = EditorGUILayout.DoubleField( parameter.DisplayName, parameter.DoubleValue );
                    break;
                case "Boolean":
                    parameter.BoolValue = EditorGUILayout.Toggle( parameter.DisplayName, parameter.BoolValue );
                    break;
                case "Vector2":
                    parameter.Vector2Value = EditorGUILayout.Vector2Field( parameter.DisplayName, parameter.Vector2Value );
                    break;
                case "Vector3":
                    parameter.Vector3Value = EditorGUILayout.Vector3Field( parameter.DisplayName, parameter.Vector3Value );
                    break;
                case "Vector4":
                    parameter.Vector4Value = EditorGUILayout.Vector4Field( parameter.DisplayName, parameter.Vector4Value );
                    break;
                case "Quaternion":
                    var v4 = new Vector4( parameter.QuaternionValue.x, parameter.QuaternionValue.y, parameter.QuaternionValue.z, parameter.QuaternionValue.w );
                    v4 = EditorGUILayout.Vector4Field( parameter.DisplayName, v4 );
                    parameter.QuaternionValue = new Quaternion( v4.x, v4.y, v4.z, v4.w );
                    break;
                case "Bounds":
                    parameter.BoundsValue = EditorGUILayout.BoundsField( parameter.DisplayName, parameter.BoundsValue );
                    break;
                case "Rect":
                    parameter.RectValue = EditorGUILayout.RectField( parameter.DisplayName, parameter.RectValue );
                    break;
                case "Matrix4x4":
                    matrixMode = (MatrixWizard.EMatrixMode)EditorGUILayout.EnumPopup( parameter.DisplayName, matrixMode );
                    switch ( matrixMode ) {
                        case MatrixWizard.EMatrixMode.Column:
                            parameter.MatrixValue = MatrixWizard.DrawColumns( parameter.MatrixValue );
                            break;
                        case MatrixWizard.EMatrixMode.Row:
                            parameter.MatrixValue = MatrixWizard.DrawRows( parameter.MatrixValue );
                            break;
                    }
                    break;
                case "AnimationCurve":
                    parameter.AnimationCurveValue = EditorGUILayout.CurveField( parameter.AnimationCurveValue );
                    break;
                case "Object":
                case "GameObject":
                    parameter.ObjectValue = EditorGUILayout.ObjectField( parameter.DisplayName, parameter.ObjectValue, parameter.Type, true );
                    break;
                case "Enum":
                    var enumValue = (Enum)Enum.Parse( parameter.Type, parameter.EnumNames[parameter.EnumValue] );
                    enumValue = EditorGUILayout.EnumPopup( parameter.DisplayName, enumValue );
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
