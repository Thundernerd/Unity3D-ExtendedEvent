#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TNRD.ExtendedEvent {

    [CustomPropertyDrawer( typeof( global::ExtendedEvent ), true )]
    public class ExtendedEventPropertyDrawer : PropertyDrawer {

        private static Dictionary<System.Type, Members> AllMembers = new Dictionary<System.Type, Members>();

        private class State {
            public ReorderableList reorderableList;
            public int lastSelectedIndex;
        }

        private class Members {
            public List<MemberInfo> Infos = new List<MemberInfo>();
            public string[] Labels;

            public Members( Object target ) {
                if ( target == null ) {
                    return;
                }

                if ( target is GameObject ) {

                } else {
                    var type = target.GetType();
                    if ( AllMembers.ContainsKey( type ) ) {
                        Infos = AllMembers[type].Infos;
                        Labels = AllMembers[type].Labels;
                        return;
                    }

                    var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
                    var fields = type.GetFields( flags )
                        .Where( f => f.GetCustomAttributes( typeof( System.ObsoleteAttribute ), true ).Length == 0 )
                        .ToList();
                    var properties = type.GetProperties( flags )
                        .Where( p => p.CanWrite )
                        .Where( p => p.GetCustomAttributes( typeof( System.ObsoleteAttribute ), true ).Length == 0 )
                        .ToList();
                    var methods = type.GetMethods( flags )
                        .Where( m => !m.Name.StartsWith( "get_" ) && !m.Name.StartsWith( "set_" ) )
                        .Where( m => m.GetCustomAttributes( typeof( System.ObsoleteAttribute ), true ).Length == 0 )
                        .ToList();

                    var labels = new List<string>();

                    foreach ( var item in fields ) {
                        Infos.Add( item );
                        labels.Add( string.Format( "Fields/{0} {1}",
                            GetTypeName( item.FieldType ),
                            ObjectNames.NicifyVariableName( item.Name ) ) );
                    }

                    foreach ( var item in properties ) {
                        Infos.Add( item );
                        labels.Add( string.Format( "Properties/{0} {1}",
                            GetTypeName( item.PropertyType ),
                            ObjectNames.NicifyVariableName( item.Name ) ) );
                    }

                    foreach ( var item in methods ) {
                        Infos.Add( item );
                        labels.Add( string.Format( "Methods/{0}({1})",
                            item.Name,
                            GetParameters( item ) ) );
                    }

                    Labels = labels.ToArray();

                    AllMembers.Add( type, this );
                }
            }

            private string GetTypeName( System.Type type ) {
                if ( type.IsArray )
                    return GetTypeName( type.GetElementType() ) + "[]";

                switch ( type.Name ) {
                    case "Boolean":
                        return "bool";
                    case "String":
                        return "string";
                    case "Int16":
                        return "short";
                    case "Int32":
                        return "int";
                    case "Int64":
                        return "long";
                    case "UInt16":
                        return "ushort";
                    case "UInt32":
                        return "uint";
                    case "UInt64":
                        return "ulong";
                    case "Single":
                        return "float";
                }

                return type.Name;
            }

            private string GetParameters( MethodInfo info ) {
                var p = "";
                var parameters = info.GetParameters();
                foreach ( var item in parameters ) {
                    p += GetTypeName( item.ParameterType ) + ", ";
                }
                return p.Trim( ' ', ',' );
            }
        }

        private Dictionary<string, State> states = new Dictionary<string, State>();
        private List<Members> members = new List<Members>();

        private SerializedProperty prop;
        private string text;

        private SerializedProperty listenersArray;
        private ReorderableList reorderableList;
        private int lastSelectedIndex;

        private PropertyWizardValue propertyWizardValue;
        private ParameterWizardValue parameterWizardValue;

        private State GetState( SerializedProperty property ) {
            State state = null;
            states.TryGetValue( property.propertyPath, out state );

            if ( state == null ) {
                state = new State();
                var prop = property.FindPropertyRelative( "persistentCallbacks" );

                state.reorderableList = new ReorderableList( property.serializedObject, prop, false, true, true, true );
                state.reorderableList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate( DrawEventHeader );
                state.reorderableList.drawElementCallback = new ReorderableList.ElementCallbackDelegate( DrawEventListener );
                state.reorderableList.onSelectCallback = new ReorderableList.SelectCallbackDelegate( SelectEventListener );
                state.reorderableList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate( EndDragChild );
                state.reorderableList.onAddCallback = new ReorderableList.AddCallbackDelegate( AddEventListener );
                state.reorderableList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate( RemoveButton );
                state.reorderableList.elementHeight = 43f;

                states[property.propertyPath] = state;
            }

            return state;
        }

        private State RestoreState( SerializedProperty property ) {
            var state = GetState( property );
            listenersArray = state.reorderableList.serializedProperty;
            reorderableList = state.reorderableList;
            lastSelectedIndex = state.lastSelectedIndex;
            reorderableList.index = state.lastSelectedIndex;
            return state;
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
            RestoreState( property );
            if ( reorderableList != null )
                return reorderableList.GetHeight();
            return 0;
        }

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
            if ( propertyWizardValue != null ) {
                Utilities.SetPropertyValue( property, propertyWizardValue );
                propertyWizardValue = null;
            } else if ( parameterWizardValue != null ) {
                var p = property.serializedObject.FindProperty( parameterWizardValue.Path );
                for ( int i = 0; i < parameterWizardValue.Objects.Count; i++ ) {
                    var element = p.GetArrayElementAtIndex( i );
                    var value = parameterWizardValue.Objects[i].FindProperty( "Value" );
                    var type = parameterWizardValue.Parameters[i].ParameterType;
                    Utilities.CopyValue( Utilities.GetPropertyFromType( type, value ), element, type );
                }
                parameterWizardValue = null;
            }

            prop = property;
            text = label.text;

            var state = RestoreState( property );
            OnGUI( position );
            state.lastSelectedIndex = lastSelectedIndex;
        }

        private void OnGUI( Rect position ) {
            if ( listenersArray == null || !listenersArray.isArray )
                return;
            if ( reorderableList == null )
                return;

            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            reorderableList.DoList( position );
            EditorGUI.indentLevel = indentLevel;
        }

        protected virtual void DrawEventHeader( Rect headerRect ) {
            headerRect.height = 16f;
            GUI.Label( headerRect, text );
        }

        private Members InsertOrUpdateMembers( SerializedProperty property, int index ) {
            if ( property.objectReferenceValue == null )
                return null;
            var mems = new Members( property.objectReferenceValue );
            if ( members.Count > index )
                members[index] = mems;
            else
                members.Insert( index, mems );
            return mems;
        }

        private void DrawEventListener( Rect rect, int index, bool isactive, bool isfocused ) {
            var element = listenersArray.GetArrayElementAtIndex( index );
            ++rect.y;

            var targetProperty = element.FindPropertyRelative( "Target" );
            var indexProperty = element.FindPropertyRelative( "Index" );
            var infoProperty = element.FindPropertyRelative( "Info" );
            var valuesProperty = element.FindPropertyRelative( "Values" );

            var targetRect = new Rect( rect );
            targetRect.width *= 0.3f;
            targetRect.height = 16;
            targetRect.y += 2f;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField( targetRect, targetProperty, GUIContent.none );
            var members = InsertOrUpdateMembers( targetProperty, index );
            if ( EditorGUI.EndChangeCheck() ) {
                indexProperty.intValue = -1;
            }

            var memberRect = new Rect( rect );
            memberRect.xMin = targetRect.xMax + 5;
            memberRect.height = 16;
            memberRect.y += 2;

            EditorGUI.BeginDisabledGroup( targetProperty.objectReferenceValue == null );
            EditorGUI.BeginChangeCheck();
            if ( targetProperty.objectReferenceValue == null ) {
                indexProperty.intValue = EditorGUI.Popup( memberRect, indexProperty.intValue, new string[0] );
            } else {
                indexProperty.intValue = EditorGUI.Popup( memberRect, indexProperty.intValue, members.Labels );
            }
            if ( EditorGUI.EndChangeCheck() ) {
                UpdateInfo( infoProperty, indexProperty.intValue, members );
                valuesProperty.ClearArray();
            }
            EditorGUI.EndDisabledGroup();

            var valueRect = new Rect( rect );
            valueRect.height = 16f;
            valueRect.y += EditorGUIUtility.singleLineHeight + 4f;

            EditorGUI.BeginDisabledGroup( targetProperty.objectReferenceValue == null || indexProperty.intValue == -1 );
            if ( targetProperty.objectReferenceValue != null && indexProperty.intValue != -1 ) {
                var info = members.Infos[indexProperty.intValue];
                if ( info is FieldInfo ) {
                    var type = ( (FieldInfo)info ).FieldType;
                    if ( valuesProperty.arraySize == 0 ) {
                        valuesProperty.InsertArrayElementAtIndex( 0 );
                    }
                    DrawMember( valueRect, type, valuesProperty.GetArrayElementAtIndex( 0 ) );
                } else if ( info is PropertyInfo ) {
                    var type = ( (PropertyInfo)info ).PropertyType;
                    if ( valuesProperty.arraySize == 0 ) {
                        valuesProperty.InsertArrayElementAtIndex( 0 );
                    }
                    DrawMember( valueRect, type, valuesProperty.GetArrayElementAtIndex( 0 ) );
                } else if ( info is MethodInfo ) {
                    var parameters = ( (MethodInfo)info ).GetParameters();
                    if ( parameters.Length > 1 ) {
                        if ( valuesProperty.arraySize == 0 ) {
                            for ( int i = 0; i < parameters.Length; i++ ) {
                                valuesProperty.InsertArrayElementAtIndex( i );
                            }
                        }

                        if ( GUI.Button( valueRect, "..." ) ) {
                            var wizard = ScriptableWizard.DisplayWizard<ParameterWizard>( "Parameters", "Close" );
                            wizard.Parameters = parameters;
                            wizard.Property = valuesProperty.Copy();
                            wizard.Path = valuesProperty.propertyPath;
                            wizard.Callback = OnParameterWizardClose;
                        }
                    } else if ( parameters.Length == 1 ) {
                        if ( valuesProperty.arraySize == 0 ) {
                            valuesProperty.InsertArrayElementAtIndex( 0 );
                        }

                        DrawMember( valueRect, parameters[0].ParameterType, valuesProperty.GetArrayElementAtIndex( 0 ) );
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void UpdateInfo( SerializedProperty property, int index, Members members ) {
            var typeProperty = property.FindPropertyRelative( "Type" );
            var nameProperty = property.FindPropertyRelative( "Name" );
            var countProperty = property.FindPropertyRelative( "Parameters" );
            var paramsProperty = property.FindPropertyRelative( "ParameterTypes" );
            paramsProperty.ClearArray();

            var mem = members.Infos[index];
            if ( mem is FieldInfo ) {
                typeProperty.intValue = 1;
                nameProperty.stringValue = mem.Name;
                countProperty.intValue = 0;
            } else if ( mem is PropertyInfo ) {
                typeProperty.intValue = 2;
                nameProperty.stringValue = mem.Name;
                countProperty.intValue = 0;
            } else if ( mem is MethodInfo ) {
                typeProperty.intValue = 3;
                nameProperty.stringValue = mem.Name;
                var method = (MethodInfo)mem;
                var parameters = method.GetParameters();
                countProperty.intValue = parameters.Length;
                for ( int i = 0; i < parameters.Length; i++ ) {
                    var par = parameters[i];
                    paramsProperty.InsertArrayElementAtIndex( i );
                    paramsProperty.GetArrayElementAtIndex( i ).stringValue = par.ParameterType.FullName;
                }
            }
        }

        private void DrawMember( Rect rect, System.Type type, SerializedProperty property ) {
            if ( type == typeof( Rect ) || type == typeof( Bounds ) ) {
                if ( GUI.Button( rect, "..." ) ) {
                    var wizard = ScriptableWizard.DisplayWizard<PropertyWizard>( "", "Close" );
                    wizard.Property = Utilities.GetPropertyFromType( type, property ).Copy();
                    wizard.Callback = OnPropertyWizardClose;
                }
            } else if ( type.IsEnum ) {

            } else {
                var prop = Utilities.GetPropertyFromType( type, property );
                if ( prop != null ) {
                    if ( type == typeof( Quaternion ) ) {
                        var euler = prop.quaternionValue.eulerAngles;
                        EditorGUI.Vector3Field( rect, "", euler );
                        prop.quaternionValue = Quaternion.Euler( euler );
                    } else if ( type == typeof( Vector4 ) ) {
                        EditorGUI.Vector4Field( rect, "", prop.vector4Value );
                    } else {
                        EditorGUI.PropertyField( rect, prop, GUIContent.none, false );
                    }
                } else {
                    EditorGUI.HelpBox( rect, "Type not supported", MessageType.Warning );
                }
            }
        }

        private void RemoveButton( ReorderableList list ) {
            ReorderableList.defaultBehaviours.DoRemoveButton( list );
            lastSelectedIndex = list.index;
        }

        private void AddEventListener( ReorderableList list ) {
            if ( listenersArray.hasMultipleDifferentValues ) {
                foreach ( Object targetObject in listenersArray.serializedObject.targetObjects ) {
                    var serializedObject = new SerializedObject( targetObject );
                    ++serializedObject.FindProperty( listenersArray.propertyPath ).arraySize;
                    serializedObject.ApplyModifiedProperties();
                }
                listenersArray.serializedObject.SetIsDifferentCacheDirty();
                listenersArray.serializedObject.Update();
                list.index = list.serializedProperty.arraySize - 1;
            } else
                ReorderableList.defaultBehaviours.DoAddButton( list );

            lastSelectedIndex = list.index;

            var element = listenersArray.GetArrayElementAtIndex( list.index );
            element.FindPropertyRelative( "Target" ).objectReferenceValue = null;
            element.FindPropertyRelative( "Index" ).intValue = -1;
            element.FindPropertyRelative( "Values" ).ClearArray();
        }

        private void SelectEventListener( ReorderableList list ) {
            lastSelectedIndex = list.index;
        }

        private void EndDragChild( ReorderableList list ) {
            lastSelectedIndex = list.index;
        }

        private void OnPropertyWizardClose( PropertyWizardValue value ) {
            propertyWizardValue = value;
        }

        private void OnParameterWizardClose( ParameterWizardValue value ) {
            parameterWizardValue = value;
        }
    }
}
#endif