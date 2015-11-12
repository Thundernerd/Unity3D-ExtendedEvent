using UnityEditor;

public class FieldWizard : ScriptableWizard {

    public SerializedProperty Property;
    protected bool ended = false;

    protected override bool DrawWizardGUI() {
        if ( Property == null ) return false;

        StartGUI();
        WizardGUI();
        return ended ? false : EndGUI();
    }


    private void StartGUI() {
        EditorGUI.BeginChangeCheck();
    }

    public virtual void WizardGUI() { }

    private bool EndGUI() {
        if ( EditorGUI.EndChangeCheck() ) {
            Property.serializedObject.ApplyModifiedProperties();
            return true;
        }

        return false;
    }
}