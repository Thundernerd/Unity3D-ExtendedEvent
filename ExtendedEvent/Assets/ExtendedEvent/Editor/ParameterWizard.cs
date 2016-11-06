#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TNRD.ExtendedEvent {

    public class ParameterWizard : ScriptableWizard {

        public SerializedProperty Property;
        public string Path;
        public ParameterInfo[] Parameters;
        public Action<ParameterWizardValue> Callback;

        private List<FakeObject> fakeList;
        private List<SerializedObject> serializedList;

        protected override sealed bool DrawWizardGUI() {
            if ( fakeList == null ) {
                fakeList = new List<FakeObject>();
                serializedList = new List<SerializedObject>();

                for ( int i = 0; i < Parameters.Length; i++ ) {
                    fakeList.Add( CreateInstance<FakeObject>() );
                    serializedList.Add( new SerializedObject( fakeList[i] ) );
                    var type = Parameters[i].ParameterType;
                    var element = Property.GetArrayElementAtIndex( i );
                    var prop = Utilities.GetPropertyFromType( type, element );
                    Utilities.CopyValue( prop, serializedList[i].FindProperty( "Value" ), type );
                }
            }

            for ( int i = 0; i < serializedList.Count; i++ ) {
                var par = Parameters[i];
                var prop = serializedList[i].FindProperty( "Value" );
                var p = Utilities.GetPropertyFromType( par.ParameterType, prop );
                EditorGUILayout.PropertyField( p, new GUIContent( par.Name ), false );
            }

            return true;
        }

        void OnWizardCreate() {
            Callback( new ParameterWizardValue() {
                Parameters = Parameters,
                Objects = serializedList,
                Path = Path
            } );
        }
    }
}
#endif