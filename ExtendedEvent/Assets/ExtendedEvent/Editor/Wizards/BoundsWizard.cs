using UnityEditor;

public class BoundsWizard : FieldWizard {

    public override void WizardGUI() {
        try {
            if ( Member == null ) {
                Parameter.BoundsValue = EditorGUILayout.BoundsField( label, Parameter.BoundsValue );
            } else {
                Member.BoundsValue = EditorGUILayout.BoundsField( label, Member.BoundsValue );
            }
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }
    }
}