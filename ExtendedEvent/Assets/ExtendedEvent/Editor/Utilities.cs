#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace TNRD.ExtendedEvent {

    public class Utilities {

        public static SerializedProperty GetPropertyFromType( System.Type type, SerializedProperty property ) {
            if ( IsUnityObject( type ) ) {
                return property.FindPropertyRelative( "objectReferenceValue" );
            } else {
                if ( type == typeof( AnimationCurve ) ) {
                    return property.FindPropertyRelative( "animationCurveValue" );
                } else if ( type == typeof( bool ) ) {
                    return property.FindPropertyRelative( "boolValue" );
                } else if ( type == typeof( Bounds ) ) {
                    return property.FindPropertyRelative( "boundsValue" );
                } else if ( type == typeof( Color ) ) {
                    return property.FindPropertyRelative( "colorValue" );
                } else if ( type == typeof( double ) ) {
                    return property.FindPropertyRelative( "doubleValue" );
                } else if ( type.IsEnum ) {
                    return property.FindPropertyRelative( "enumValue" );
                } else if ( type == typeof( float ) ) {
                    return property.FindPropertyRelative( "floatValue" );
                } else if ( type == typeof( int ) ) {
                    return property.FindPropertyRelative( "intValue" );
                } else if ( type == typeof( long ) ) {
                    return property.FindPropertyRelative( "longValue" );
                } else if ( type == typeof( Quaternion ) ) {
                    return property.FindPropertyRelative( "quaternionValue" );
                } else if ( type == typeof( Rect ) ) {
                    return property.FindPropertyRelative( "rectValue" );
                } else if ( type == typeof( string ) ) {
                    return property.FindPropertyRelative( "stringValue" );
                } else if ( type == typeof( Vector2 ) ) {
                    return property.FindPropertyRelative( "vector2Value" );
                } else if ( type == typeof( Vector3 ) ) {
                    return property.FindPropertyRelative( "vector3Value" );
                } else if ( type == typeof( Vector4 ) ) {
                    return property.FindPropertyRelative( "vector4Value" );
                }
            }

            return null;
        }

        public static bool IsUnityObject( System.Type type ) {
            if ( type == null )
                return false;

            if ( type == typeof( Object ) )
                return true;

            return IsUnityObject( type.BaseType );
        }

        public static void CopyValue( SerializedProperty from, SerializedProperty element, System.Type type ) {
            if ( from == null ) return;

            var to = GetPropertyFromType( type, element );
            switch ( from.propertyType ) {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    to.intValue = from.intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    to.boolValue = from.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    to.floatValue = from.floatValue;
                    break;
                case SerializedPropertyType.String:
                    to.stringValue = from.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    to.colorValue = from.colorValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    to.objectReferenceValue = from.objectReferenceValue;
                    break;
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    break;
                case SerializedPropertyType.Vector2:
                    to.vector2Value = from.vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    to.vector3Value = from.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    to.vector4Value = from.vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    to.rectValue = from.rectValue;
                    break;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    to.animationCurveValue = from.animationCurveValue;
                    break;
                case SerializedPropertyType.Bounds:
                    to.boundsValue = from.boundsValue;
                    break;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Quaternion:
                    to.quaternionValue = from.quaternionValue;
                    break;
                default:
                    break;
            }
        }

        public static object GetPropertyValue( SerializedProperty property ) {
            switch ( property.propertyType ) {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    return property.intValue;
                case SerializedPropertyType.Boolean:
                    return property.boolValue;
                case SerializedPropertyType.Float:
                    return property.floatValue;
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Color:
                    return property.colorValue;
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue;
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    break;
                case SerializedPropertyType.Vector2:
                    return property.vector2Value;
                case SerializedPropertyType.Vector3:
                    return property.vector3Value;
                case SerializedPropertyType.Vector4:
                    return property.vector4Value;
                case SerializedPropertyType.Rect:
                    return property.rectValue;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    return property.animationCurveValue;
                case SerializedPropertyType.Bounds:
                    return property.boundsValue;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Quaternion:
                    return property.quaternionValue;
            }

            return null;
        }
    }
}

#endif