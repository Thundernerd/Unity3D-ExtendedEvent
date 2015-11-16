using UnityEditor;

public class RectWizard : FieldWizard {

    public override void WizardGUI() {
        try {
            Field.RectValue = EditorGUILayout.RectField( label, Field.RectValue );
        } catch ( System.NullReferenceException ) {
            ended = true;
            EditorGUILayout.HelpBox( "My parent window has lost focus, please close me", MessageType.Error );
            return;
        }
    }
}
