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
                Value = Utilities.GetPropertyValue( Property )
            } );
        }
    }
}
#endif