using UnityEngine;
using System.Collections;
using UnityEditor;

public class MatrixWizard : FieldWizard {

    private enum EMatrixMode {
        Column,
        Row,
    }

    private EMatrixMode matrixMode;

    public override void WizardGUI() {
        try {
            matrixMode = (EMatrixMode)EditorGUILayout.EnumPopup( "Mode", matrixMode );

            EditorGUILayout.Space();
            EditorGUILayout.LabelField( label );

            switch ( matrixMode ) {
                case EMatrixMode.Column:
                    Member.MatrixValue = DrawColumns( Member.MatrixValue );
                    break;
                case EMatrixMode.Row:
                    Member.MatrixValue = DrawRows( Member.MatrixValue );
                    break;
            }
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }
    }

    private Matrix4x4 DrawColumns( Matrix4x4 matrix ) {
        matrix.SetColumn( 0, EditorGUILayout.Vector4Field( "", matrix.GetColumn( 0 ) ) );
        matrix.SetColumn( 1, EditorGUILayout.Vector4Field( "", matrix.GetColumn( 1 ) ) );
        matrix.SetColumn( 2, EditorGUILayout.Vector4Field( "", matrix.GetColumn( 2 ) ) );
        matrix.SetColumn( 3, EditorGUILayout.Vector4Field( "", matrix.GetColumn( 3 ) ) );
        return matrix;
    }

    private Matrix4x4 DrawRows( Matrix4x4 matrix ) {
        matrix.SetRow( 0, EditorGUILayout.Vector4Field( "", matrix.GetRow( 0 ) ) );
        matrix.SetRow( 1, EditorGUILayout.Vector4Field( "", matrix.GetRow( 1 ) ) );
        matrix.SetRow( 2, EditorGUILayout.Vector4Field( "", matrix.GetRow( 2 ) ) );
        matrix.SetRow( 3, EditorGUILayout.Vector4Field( "", matrix.GetRow( 3 ) ) );
        return matrix;
    }

}
