using UnityEditor;

public class RectWizard : FieldWizard {

    public override void WizardGUI() {
        try {
            if ( Member == null ) {
                Parameter.RectValue = EditorGUILayout.RectField( label, Parameter.RectValue );
            } else {
                Member.RectValue = EditorGUILayout.RectField( label, Member.RectValue );
            }
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }
    }
}
