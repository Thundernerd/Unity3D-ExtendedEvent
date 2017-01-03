#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace TNRD.ExtendedEvent {

    public class PropertyWizard : ScriptableWizard {

        public Type Type;
        public SerializedProperty Property;
        public string Path;
        public Action<PropertyWizardValue> Callback;

        private FakeObject fakeObject;
        private SerializedObject serializedObject;

        protected override sealed bool DrawWizardGUI() {
            if ( fakeObject == null ) {
                fakeObject = CreateInstance<FakeObject>();
                serializedObject = new SerializedObject( fakeObject );
                Utilities.CopyValue( Property, serializedObject.FindProperty( "Value" ), Type );
            }

            var p = Utilities.GetPropertyFromType( Type, serializedObject.FindProperty( "Value" ) );
            EditorGUILayout.PropertyField( p, GUIContent.none, false );
            return true;
        }

        void OnWizardCreate() {
            Callback( new PropertyWizardValue() {
                Path = Property.propertyPath,
                Type = Type,
                Object = serializedObject
            } );
        }

        private void OnLostFocus() {
            Close();
        }
    }
}
#endif