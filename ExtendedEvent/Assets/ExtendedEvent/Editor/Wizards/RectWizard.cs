using UnityEditor;

public class RectWizard : FieldWizard {

    public override void WizardGUI() {
        try {
            if ( Field != null ) {
                Field.RectValue = EditorGUILayout.RectField( label, Field.RectValue );
            } else if ( Property != null ) {
                Property.RectValue = EditorGUILayout.RectField( label, Property.RectValue );
            } else {
                Parameter.RectValue = EditorGUILayout.RectField( label, Parameter.RectValue );
            }
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }
    }
}
