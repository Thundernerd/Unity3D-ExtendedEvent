using UnityEditor;
using UnityEngine;

public class MatrixWizard : FieldWizard {

    public enum EMatrixMode {
        Column,
        Row,
    }

    private EMatrixMode matrixMode;

    public override void WizardGUI() {
        try {
            matrixMode = (EMatrixMode)EditorGUILayout.EnumPopup( "Mode", matrixMode );

            if ( Member == null ) {
                switch ( matrixMode ) {
                    case EMatrixMode.Column:
                        Parameter.MatrixValue = DrawColumns( Parameter.MatrixValue );
                        break;
                    case EMatrixMode.Row:
                        Parameter.MatrixValue = DrawRows( Parameter.MatrixValue );
                        break;
                }
            } else {
                switch ( matrixMode ) {
                    case EMatrixMode.Column:
                        Member.MatrixValue = DrawColumns( Member.MatrixValue );
                        break;
                    case EMatrixMode.Row:
                        Member.MatrixValue = DrawRows( Member.MatrixValue );
                        break;
                }
            }
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }
    }

    public static Matrix4x4 DrawColumns( Matrix4x4 matrix ) {
        matrix.SetColumn( 0, EditorGUI.Vector4Field( GetRekt(), "", matrix.GetColumn( 0 ) ) );
        matrix.SetColumn( 1, EditorGUI.Vector4Field( GetRekt(), "", matrix.GetColumn( 1 ) ) );
        matrix.SetColumn( 2, EditorGUI.Vector4Field( GetRekt(), "", matrix.GetColumn( 2 ) ) );
        matrix.SetColumn( 3, EditorGUI.Vector4Field( GetRekt(), "", matrix.GetColumn( 3 ) ) );
        return matrix;
    }

    public static Matrix4x4 DrawRows( Matrix4x4 matrix ) {
        matrix.SetRow( 0, EditorGUI.Vector4Field( GetRekt(), "", matrix.GetRow( 0 ) ) );
        matrix.SetRow( 1, EditorGUI.Vector4Field( GetRekt(), "", matrix.GetRow( 1 ) ) );
        matrix.SetRow( 2, EditorGUI.Vector4Field( GetRekt(), "", matrix.GetRow( 2 ) ) );
        matrix.SetRow( 3, EditorGUI.Vector4Field( GetRekt(), "", matrix.GetRow( 3 ) ) );
        return matrix;
    }

    private const float xOffset = 16f;

    // Yes, yes I did.
    private static Rect GetRekt() {
        var rect = EditorGUILayout.GetControlRect();
        rect.x += xOffset;
        rect.y -= EditorGUIUtility.singleLineHeight;
        rect.width -= xOffset;
        return rect;
    }
}
