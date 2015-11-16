using System.Collections.Generic;
using UnityEditor;

public class FieldWizard : ScriptableWizard {

    public ExtendedEvent.Field Field;
    protected string label = "";
    protected bool ended = false;
    private bool initialized = false;

    protected virtual void Initialize() { }

    protected override sealed bool DrawWizardGUI() {
        if ( Field == null ) return false;

        if ( string.IsNullOrEmpty( label ) ) {
            DisplayName( Field.Name );
        }

        if ( !initialized ) {
            Initialize();
            initialized = true;
        }

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
            return true;
        }

        return false;
    }

    public void OnWizardCreate() { }

    protected void DisplayName( string item ) {
        var splits = new List<string>();
        var displayName = "";

        for ( int i = 0, j = 0; i < item.Length; i++ ) {
            if ( i > 0 && char.IsUpper( item[i] ) ) {
                displayName += " " + item.Substring( j, i - j );
                j = i;
            }

            if ( i == item.Length - 1 ) {
                displayName += " " + item.Substring( j, i - j + 1 );
            }
        }

        label = displayName.Trim();
        label = string.Format( "{0}{1}", char.ToUpper( label[0] ), label.Substring( 1 ) );
    }
}