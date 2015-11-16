using UnityEditor;

public class BoundsWizard : FieldWizard {

    public override void WizardGUI() {
        try {
            if ( Field != null ) {
                Field.BoundsValue = EditorGUILayout.BoundsField( label, Field.BoundsValue );
            } else if ( Property != null ) {
                Property.BoundsValue = EditorGUILayout.BoundsField( label, Property.BoundsValue );
            } else {
                Parameter.BoundsValue = EditorGUILayout.BoundsField( label, Parameter.BoundsValue );
            }
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }
    }
}