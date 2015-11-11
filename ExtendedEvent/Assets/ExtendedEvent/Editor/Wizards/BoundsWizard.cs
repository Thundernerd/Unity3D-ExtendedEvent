using UnityEditor;
using UnityEngine;

public class BoundsWizard : FieldWizard {

    public static BoundsWizard Create() {
        var wiz = ScriptableWizard.DisplayWizard<BoundsWizard>( "Bounds Editor", "Close" );
        wiz.maxSize = new Vector2( 405, 115 );
        wiz.minSize = new Vector2( 405, 115 );
        return wiz;
    }

    public override void WizardGUI() {
        Property.stringValue = EditorGUILayout.BoundsField( Bounds( Property.stringValue ) ).ToString();
    }

    private Vector3 Vec3( string value ) {
        value = value.Trim( '(', ')' );
        var splits = value.Split( ',' );
        return new Vector3( float.Parse( splits[0] ), float.Parse( splits[1] ), float.Parse( splits[2] ) );
    }
    private Bounds Bounds( string value ) {
        value = value.Replace( ", Extents: ", "|" );
        value = value.Replace( "Center: ", "" );
        value = value.Trim( ' ' );
        var splits = value.Split( '|' );
        return new Bounds( Vec3( splits[0] ), Vec3( splits[1] ) );
    }

    private void OnWizardCreate() { }
}