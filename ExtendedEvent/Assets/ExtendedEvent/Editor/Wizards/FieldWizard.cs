using System.Collections.Generic;
using UnityEditor;

public class FieldWizard : ScriptableWizard {

    public ExtendedEvent.Field Field;
    public ExtendedEvent.Property Property;
    public ExtendedEvent.Parameter Parameter;

    protected string label = "";
    protected bool ended = false;
    private bool initialized = false;

    protected virtual void Initialize() { }

    protected override sealed bool DrawWizardGUI() {
        if ( Field == null && Property == null && Parameter == null ) return false;

        if ( string.IsNullOrEmpty( label ) ) {
            if ( Field != null ) {
                DisplayName( Field.Name );
            } else if ( Property != null ) {
                DisplayName( Property.Name );
            } else if (Parameter != null ) {
                DisplayName( Parameter.Name );
            }
        }

        if ( !initialized ) {
            Initialize();
            initialized = true;
        }
        
        WizardGUI();
        return true;
    }

    public virtual void WizardGUI() { }

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