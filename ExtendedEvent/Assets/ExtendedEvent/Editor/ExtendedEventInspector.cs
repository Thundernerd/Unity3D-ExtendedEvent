using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer( typeof( ExtendedEvent ) )]
public class ExtendedEventInspector : PropertyDrawer {

    private SerializedProperty property;
    private ReorderableList rList;

    private void InvokeReset( int index ) {
        var t = property.serializedObject.targetObject.GetType();
        var f = t.GetField( property.name );
        var evt = f.GetValue( property.serializedObject.targetObject ) as ExtendedEvent;
        evt.Objects[index].Reset();
    }

    private List<SerializedProperty> GetObjectsList() {
        var objects = property.FindPropertyRelative( "Objects" );
        var list = new List<SerializedProperty>();
        for ( int i = 0; i < objects.arraySize; i++ ) {
            list.Add( objects.GetArrayElementAtIndex( i ) );
        }
        return list;
    }

    private string[] GetList( SerializedProperty property ) {
        var pList = property.FindPropertyRelative( "List" );
        var list = new List<string>();
        for ( int i = 0; i < pList.arraySize; i++ ) {
            list.Add( pList.GetArrayElementAtIndex( i ).stringValue );
        }
        return list.ToArray();
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
        if ( rList == null ) return 80f;
        return rList.headerHeight + rList.footerHeight + ( rList.elementHeight * Mathf.Max( rList.list.Count, 1 ) ) + 7f;
    }

    public override void OnGUI( Rect position, SerializedProperty prop, GUIContent label ) {
        property = prop;
        property.serializedObject.Update();

        if ( rList == null ) {
            rList = new ReorderableList( GetObjectsList(), typeof( SerializedProperty ) );
            rList.draggable = false;
            rList.elementHeight *= 2;
            rList.drawHeaderCallback = DrawHeaderInternal;
            rList.drawElementCallback = DrawElementInternal;
            rList.onAddCallback = AddInternal;
            rList.onRemoveCallback = RemoveInternal;
        }

        if ( rList != null ) {
            rList.DoList( position );
        }
    }

    private void DrawHeaderInternal( Rect rect ) {
        EditorGUI.LabelField( rect, property.displayName );
    }

    private void AddInternal( ReorderableList list ) {
        var pList = property.FindPropertyRelative( "Objects" );
        pList.InsertArrayElementAtIndex( pList.arraySize );
        property.serializedObject.ApplyModifiedProperties();
        list.list = GetObjectsList();
    }

    private void RemoveInternal( ReorderableList list ) {
        var pList = property.FindPropertyRelative( "Objects" );
        pList.DeleteArrayElementAtIndex( list.index );
        property.serializedObject.ApplyModifiedProperties();
        list.list = GetObjectsList();
    }

    private void DrawElementInternal( Rect rect, int index, bool isActive, bool isFocused ) {
        var list = GetObjectsList();
        var property = list[index];

        rect.yMin += 3f;
        rect.yMax -= 7f;

        var rwidth = rect.width / 3;
        var rwidthTwo = rwidth * 2;

        EditorGUI.BeginChangeCheck();
        GUI.Box( new Rect( rect.x, rect.y, rwidth, rect.height / 2 ), "" );
        EditorGUI.PropertyField( new Rect( rect.x, rect.y, rwidth, rect.height / 2 ), property.FindPropertyRelative( "GameObject" ), GUIContent.none, false );
        if ( EditorGUI.EndChangeCheck() ) {
            property.serializedObject.ApplyModifiedProperties();
            InvokeReset( index );
        }

        EditorGUI.BeginChangeCheck();
        var pIndex = property.FindPropertyRelative( "Index" );
        pIndex.intValue = DropdownList( new Rect( rect.x + rwidth + 5, rect.y, rwidthTwo - 5, rect.height / 2 ), pIndex.intValue, GetList( property ) );
        if ( EditorGUI.EndChangeCheck() ) {
            property.serializedObject.ApplyModifiedProperties();
        }

        if ( property.FindPropertyRelative( "GameObject" ).objectReferenceValue != null ) {
            if ( pIndex.intValue != 0 ) {
                var tempRect = new Rect( rect.x, rect.y + rect.height / 2, rect.width, rect.height / 2 );
                var fields = property.FindPropertyRelative( "Fields" );
                if ( pIndex.intValue > fields.arraySize ) {
                    DrawMethod( tempRect, property, pIndex.intValue - 1 - fields.arraySize );
                } else {
                    DrawField( tempRect, property, pIndex.intValue - 1 );
                }
            }
        }
    }

    private void DrawMethod( Rect rect, SerializedProperty property, int index ) {
        var methods = property.FindPropertyRelative( "Methods" );
        var method = methods.GetArrayElementAtIndex( index );
        var parameters = method.FindPropertyRelative( "Parameters" );

        rect.yMin += 3f;
        rect.yMax += 3f;

        EditorGUI.BeginChangeCheck();

        if ( parameters.arraySize == 1 ) {
            var parameter = parameters.GetArrayElementAtIndex( 0 );
            var assembly = parameter.FindPropertyRelative( "Assembly" );
            var type = parameter.FindPropertyRelative( "Type" );
            var value = parameter.FindPropertyRelative( "NewValue" );
            var obj = parameter.FindPropertyRelative( "Object" );

            var a = System.Reflection.Assembly.Load( assembly.stringValue );
            var t = a.GetType( type.stringValue );

            if ( t.IsSubclassOf( typeof( UnityEngine.Object ) ) ) {
                obj.objectReferenceValue = EditorGUI.ObjectField( rect, obj.objectReferenceValue, t, true );
            } else {
                DrawProperty( t, value, rect );
            }

        } else if ( parameters.arraySize > 1 ) {
            if ( GUI.Button( rect, "..." ) ) {
                var wiz = ScriptableWizard.DisplayWizard<MethodWizard>( "Parameter Editor", "Close" );
                wiz.Property = parameters;
            }
        }

        if ( EditorGUI.EndChangeCheck() ) {
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    private void DrawField( Rect rect, SerializedProperty property, int index ) {
        var fields = property.FindPropertyRelative( "Fields" );
        var field = fields.GetArrayElementAtIndex( index );

        var value = field.FindPropertyRelative( "NewValue" );
        var type = field.FindPropertyRelative( "Type" );
        var assembly = field.FindPropertyRelative( "Assembly" );
        var obj = field.FindPropertyRelative( "Object" );

        var a = System.Reflection.Assembly.Load( assembly.stringValue );
        var t = a.GetType( type.stringValue );

        rect.yMin += 3f;
        rect.yMax += 3f;

        EditorGUI.BeginChangeCheck();

        if ( t.IsSubclassOf( typeof( UnityEngine.Object ) ) ) {
            obj.objectReferenceValue = EditorGUI.ObjectField( rect, obj.objectReferenceValue, t, true );
        } else {
            DrawProperty( t, value, rect );
        }

        if ( EditorGUI.EndChangeCheck() ) {
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    private void ShowWizard<T>( Rect rect, SerializedProperty property, string title, float width, float height ) where T : FieldWizard {
        if ( GUI.Button( rect, "..." ) ) {
            var wiz = ScriptableWizard.DisplayWizard<T>( title, "Close" );
            wiz.Property = property;
            wiz.minSize = new Vector2( width, height );
            wiz.maxSize = new Vector2( width, height );
        }
    }

    private void DrawProperty( Type t, SerializedProperty value, Rect rect ) {
        if ( t == typeof( int ) ) {
            value.stringValue = EditorGUI.IntField( rect, int.Parse( value.stringValue ) ).ToString();
        } else if ( t == typeof( float ) ) {
            value.stringValue = EditorGUI.FloatField( rect, float.Parse( value.stringValue ) ).ToString();
        } else if ( t == typeof( double ) ) {
            value.stringValue = EditorGUI.DoubleField( rect, double.Parse( value.stringValue ) ).ToString();
        } else if ( t == typeof( long ) ) {
            value.stringValue = EditorGUI.LongField( rect, long.Parse( value.stringValue ) ).ToString();
        } else if ( t == typeof( string ) ) {
            value.stringValue = EditorGUI.TextField( rect, value.stringValue );
        } else if ( t == typeof( bool ) ) {
            value.stringValue = EditorGUI.Toggle( rect, bool.Parse( value.stringValue ) ).ToString();
        } else if ( t == typeof( Vector2 ) ) {
            value.stringValue = EditorGUI.Vector2Field( rect, GUIContent.none, ExtendedEventConverter.Vec2( value.stringValue ) ).ToString();
        } else if ( t == typeof( Vector3 ) ) {
            value.stringValue = EditorGUI.Vector3Field( rect, GUIContent.none, ExtendedEventConverter.Vec3( value.stringValue ) ).ToString();
        } else if ( t == typeof( Vector4 ) ) {
            rect.y -= 16f;
            value.stringValue = EditorGUI.Vector4Field( rect, "", ExtendedEventConverter.Vec4( value.stringValue ) ).ToString();
        } else if ( t == typeof( GameObject ) ) {
            var objectFound = GameObject.Find( value.stringValue );
            objectFound = (GameObject)EditorGUI.ObjectField( rect, objectFound, typeof( GameObject ), true );
            if ( objectFound != null ) {
                value.stringValue = objectFound.name;
            }
        } else if ( t == typeof( Bounds ) ) {
            ShowWizard<BoundsWizard>( rect, value, "Bounds Editor", 405, 115 );
        } else if ( t == typeof( Rect ) ) {
            ShowWizard<RectWizard>( rect, value, "Rect Editor", 350, 115 );
        } else if ( t.BaseType == typeof( Enum ) ) {
            var eValue = (Enum)Enum.Parse( t, value.stringValue );
            value.stringValue = EditorGUI.EnumPopup( rect, eValue ).ToString();
        } else if ( t == typeof( AnimationCurve ) ) {
            var curve = EditorGUI.CurveField( rect, ExtendedEventConverter.Curve( value.stringValue ) );
            value.stringValue = "";
            for ( int i = 0; i < curve.keys.Length; i++ ) {
                var item = curve.keys[i];
                value.stringValue += string.Format( "{0}|{1}|{2}|{3}|{4};",
                    item.inTangent.ToString(), item.outTangent.ToString(),
                    item.tangentMode.ToString(), item.time.ToString(), item.value.ToString() );
            }
        } else if ( t == typeof( Color ) ) {
            value.stringValue = EditorGUI.ColorField( rect, ExtendedEventConverter.Color( value.stringValue ) ).ToString();
        } else {
            EditorGUI.HelpBox( rect, string.Format( "The type \"{0}\" is not supported", t.Name ), MessageType.Warning );
        }
    }

    #region MyRegion
    private static int dropdownHash = "extDropDown".GetHashCode();
    private static GUIStyle dropdownPopupStyle = new GUIStyle( EditorStyles.popup );
    private class DropdownCallbackInfo {
        private const string kMaskMenuChangedMessage = "MaskMenuChangedAyo";
        public static DropdownCallbackInfo instance;
        private readonly int controlID;
        private int selectedIndex;
        private object view;
        private MethodInfo method;

        public DropdownCallbackInfo( int controlID ) {
            this.controlID = controlID;
            var assembly = Assembly.GetAssembly( typeof( EditorGUI ) );
            Type t = assembly.GetType( "UnityEditor.GUIView" );
            var p = t.GetProperty( "current", BindingFlags.Static | BindingFlags.Public );
            view = p.GetValue( null, null );
            method = t.GetMethod( "SendEvent", BindingFlags.NonPublic | BindingFlags.Instance );
        }

        public static int GetSelectedValueForControl( int controlID, int index ) {
            Event current = Event.current;

            if ( current.type == EventType.ExecuteCommand && current.commandName == kMaskMenuChangedMessage ) {
                if ( instance == null ) {
                    Debug.LogError( "Mask menu has no instance" );
                    return index;
                } else if ( instance.controlID == controlID ) {
                    index = instance.selectedIndex;
                    GUI.changed = true;
                    instance = null;
                    GUIUtility.hotControl = GUIUtility.keyboardControl = 0;
                    current.Use();
                }
            }

            return index;
        }

        internal void SetMaskValueDelegate( object userData, string[] options, int selected ) {
            selectedIndex = selected;

            if ( view != null ) {
                method.Invoke( view, new object[] { EditorGUIUtility.CommandEvent( kMaskMenuChangedMessage ) } );
            }
        }
    }
    private static int DropdownList( Rect position, int current, string[] items ) {
        GUIContent[] contents = new GUIContent[items.Length];
        for ( int i = 0; i < items.Length; i++ ) {
            contents[i] = new GUIContent( items[i] );
        }
        return InternalDropdownList( position, current, contents, dropdownPopupStyle );
    }
    private static int InternalDropdownList( Rect position, int current, GUIContent[] items, GUIStyle style ) {
        int controlID = GUIUtility.GetControlID( dropdownHash, FocusType.Native, position );
        var mask = DropdownCallbackInfo.GetSelectedValueForControl( controlID, current );

        var evt = Event.current;
        if ( evt.type == EventType.Repaint ) {
            if ( current >= items.Length || current == -1 ) {
                style.Draw( position, new GUIContent( "-" ), controlID, false );
            } else {
                style.Draw( position, new GUIContent( items[current] ), controlID, false );
            }
        } else if ( evt.type == EventType.MouseDown && position.Contains( evt.mousePosition ) ) {
            DropdownCallbackInfo.instance = new DropdownCallbackInfo( controlID );
            GUIUtility.keyboardControl = GUIUtility.hotControl = 0;
            evt.Use();
            EditorUtility.DisplayCustomMenu( position, items, current,
                new EditorUtility.SelectMenuItemFunction( DropdownCallbackInfo.instance.SetMaskValueDelegate ), null );
        }

        return mask;
    }
    #endregion
}
