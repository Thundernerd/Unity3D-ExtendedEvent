using System;
using UnityEngine;

public class ExtendedEventConverter {

    public static Vector2 Vec2( string value ) {
        if ( string.IsNullOrEmpty( value ) ) return new Vector2();
        value = value.Trim( '(', ')' );
        var splits = value.Split( ',' );
        return new Vector2( float.Parse( splits[0] ), float.Parse( splits[1] ) );
    }
    public static Vector3 Vec3( string value ) {
        if ( string.IsNullOrEmpty( value ) ) return new Vector3();
        value = value.Trim( '(', ')' );
        var splits = value.Split( ',' );
        return new Vector3( float.Parse( splits[0] ), float.Parse( splits[1] ), float.Parse( splits[2] ) );
    }
    public static Vector4 Vec4( string value ) {
        if ( string.IsNullOrEmpty( value ) ) return new Vector4();
        value = value.Trim( '(', ')' );
        var splits = value.Split( ',' );
        return new Vector4( float.Parse( splits[0] ), float.Parse( splits[1] ), float.Parse( splits[2] ), float.Parse( splits[3] ) );
    }
    public static Bounds Bounds( string value ) {
        if ( string.IsNullOrEmpty( value ) ) return new Bounds();
        value = value.Replace( ", Extents: ", "|" );
        value = value.Replace( "Center: ", "" );
        value = value.Trim( ' ' );
        var splits = value.Split( '|' );
        return new Bounds( Vec3( splits[0] ), Vec3( splits[1] ) );
    }
    public static Rect Rect( string value ) {
        if ( string.IsNullOrEmpty( value ) ) return new Rect();
        value = value.Trim( '(', ')' );
        value = value.ToLower().Replace( "x:", "" ).Replace( "y:", "" ).Replace( "width:", "" ).Replace( "height:", "" );
        var splits = value.Split( ',' );
        return new Rect( float.Parse( splits[0] ), float.Parse( splits[1] ), float.Parse( splits[2] ), float.Parse( splits[3] ) );
    }
    public static AnimationCurve Curve( string value ) {
        if ( string.IsNullOrEmpty( value ) || value == "UnityEngine.AnimationCurve" ) return new AnimationCurve();
        var splits = value.Split( new[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
        var keys = new Keyframe[splits.Length];
        for ( int i = 0; i < splits.Length; i++ ) {
            var s = splits[i].Split( '|' );
            var k = new Keyframe() {
                inTangent = float.Parse( s[0] ),
                outTangent = float.Parse( s[1] ),
                tangentMode = int.Parse( s[2] ),
                time = float.Parse( s[3] ),
                value = float.Parse( s[4] )
            };
            keys[i] = k;
        }
        return new AnimationCurve( keys );
    }
    public static Color Color( string value ) {
        if ( string.IsNullOrEmpty( value ) ) return new Color( 1, 1, 1, 1 );
        value = value.Replace( "RGBA", "" );
        value = value.Trim( '(', ')' );
        var splits = value.Split( ',' );
        return new Color( float.Parse( splits[0] ), float.Parse( splits[1] ), float.Parse( splits[2] ), float.Parse( splits[3] ) );
    }
}
