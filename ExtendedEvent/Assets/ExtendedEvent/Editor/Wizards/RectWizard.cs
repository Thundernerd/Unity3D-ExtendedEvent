using UnityEditor;
using UnityEngine;

public class RectWizard : FieldWizard {

    public static RectWizard Create() {
        var wiz = ScriptableWizard.DisplayWizard<RectWizard>( "Rect Editor", "Close" );
        
        return wiz;
    }

    public override void WizardGUI() {
        Property.stringValue = EditorGUILayout.RectField( Rect( Property.stringValue ) ).ToString();
    }

    private Rect Rect( string value ) {
        value = value.Trim( '(', ')' );
        value = value.ToLower().Replace( "x:", "" ).Replace( "y:", "" ).Replace( "width:", "" ).Replace( "height:", "" );
        var splits = value.Split( ',' );
        return new Rect( float.Parse( splits[0] ), float.Parse( splits[1] ), float.Parse( splits[2] ), float.Parse( splits[3] ) );
    }

    private void OnWizardCreate() { }
}
