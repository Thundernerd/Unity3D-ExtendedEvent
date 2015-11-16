using UnityEditor;

public class BoundsWizard : FieldWizard {

    public override void WizardGUI() {
        try {
            Field.BoundsValue = EditorGUILayout.BoundsField( label, Field.BoundsValue );
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }
    }
}