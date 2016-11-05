#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace TNRD.ExtendedEvent {

    public class PropertyWizard : ScriptableWizard {

        public SerializedProperty Property;
        public Action<PropertyWizardValue> Callback;

        protected override sealed bool DrawWizardGUI() {
            EditorGUILayout.PropertyField( Property, GUIContent.none );
            return true;
        }

        void OnWizardCreate() {
            Callback( new PropertyWizardValue() {
                Path = Property.propertyPath,
                Type = Property.propertyType,
                Value = GetPropertyValue( Property )
            } );
        }

        private object GetPropertyValue( SerializedProperty property ) {
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