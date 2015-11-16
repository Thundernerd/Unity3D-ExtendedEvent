using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer( typeof( ExtendedEvent ) )]
public class ExtendedEventPropertyDrawer : PropertyDrawer {

    private ReorderableList rList;
    private ExtendedEvent eEvent;
    private string header = "";
    private ExtendedEvent.GameObjectContainer listener;

    private void RestoreState( SerializedProperty property ) {
        if ( rList == null || eEvent == null ) {
            header = property.name;

            var target = property.serializedObject.targetObject;
            var type = target.GetType();
            var field = type.GetField( property.name );
            eEvent = field.GetValue( target ) as ExtendedEvent;
            foreach ( var item in eEvent.Listeners ) {
                item.Initialize();
            }

            rList = new ReorderableList( eEvent.Listeners, typeof( ExtendedEvent.GameObjectContainer ) );
            rList.draggable = false;
            rList.elementHeight *= 2;
            rList.drawHeaderCallback = DrawHeaderInternal;
            rList.drawElementCallback = DrawElementInternal;
            rList.onAddCallback = AddInternal;
            rList.onRemoveCallback = RemoveInternal;
        }
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
        if ( rList == null ) return 80f;
        return rList.headerHeight + rList.footerHeight + ( rList.elementHeight * Mathf.Max( rList.list.Count, 1 ) ) + 7f;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
        RestoreState( property );
        rList.DoList( position );
    }

    private void DrawHeaderInternal( Rect rect ) {
        EditorGUI.LabelField( rect, header );
    }

    private void AddInternal( ReorderableList list ) {
        eEvent.Listeners.Add( new ExtendedEvent.GameObjectContainer() );
    }

    private void RemoveInternal( ReorderableList list ) {
        eEvent.Listeners.RemoveAt( list.index );
    }

    private void DrawElementInternal( Rect rect, int index, bool isActive, bool isFocused ) {
        listener = eEvent.Listeners[index];

        rect.yMin += 3f;
        rect.yMax -= 7f;

        var thirdWidth = rect.width / 3;
        var halfHeight = rect.height / 2;

        var gameObjectRect = new Rect( rect.x, rect.y, thirdWidth, halfHeight );
        var dropdownRect = new Rect( rect.x + thirdWidth, rect.y, thirdWidth * 2, halfHeight );
        var bottomRect = new Rect( rect.x, rect.y + halfHeight, rect.width, halfHeight );

        EditorGUI.BeginChangeCheck();
        listener.GameObject = (GameObject)EditorGUI.ObjectField( gameObjectRect, listener.GameObject, typeof( GameObject ), true );
        if ( EditorGUI.EndChangeCheck() ) {
            listener.Reset();
        }

        listener.Index = DropdownList( dropdownRect, listener.Index, listener.List );
        var i = listener.Index;

        if ( i > 1 && listener.GameObject != null ) {
            switch ( listener.Type ) {
                case 0:
                    var field = listener.CurrentField;
                    DrawField( field, bottomRect );
                    break;
                case 1:
                    var property = listener.CurrentProperty;
                    DrawProperty( property, bottomRect );
                    break;
                case 2:
                    var method = listener.CurrentMethod;
                    DrawMethod( method, bottomRect );
                    break;
            }
        }
    }

    private void DrawField( ExtendedEvent.Field field, Rect rect ) {
        rect.yMax += 3;
        rect.yMin += 3;

        switch ( field.TypeName ) {
            case "String":
                field.StringValue = EditorGUI.TextField( rect, field.StringValue );
                break;
            case "Int32":
                field.IntValue = EditorGUI.IntField( rect, field.IntValue );
                break;
            case "Int64":
                field.LongValue = EditorGUI.LongField( rect, field.LongValue );
                break;
            case "Single":
                field.FloatValue = EditorGUI.FloatField( rect, field.FloatValue );
                break;
            case "Double":
                field.DoubleValue = EditorGUI.DoubleField( rect, field.DoubleValue );
                break;
            case "Boolean":
                field.BoolValue = EditorGUI.Toggle( rect, field.BoolValue );
                break;
            case "Vector2":
                field.Vector2Value = EditorGUI.Vector2Field( rect, "", field.Vector2Value );
                break;
            case "Vector3":
                field.Vector3Value = EditorGUI.Vector3Field( rect, "", field.Vector3Value );
                break;
            case "Vector4":
                rect.y -= 16f;
                field.Vector4Value = EditorGUI.Vector4Field( rect, "", field.Vector4Value );
                break;
            case "Quaternion":
                rect.y -= 16f;
                var v4 = new Vector4( field.QuaternionValue.x, field.QuaternionValue.y, field.QuaternionValue.z, field.QuaternionValue.w );
                v4 = EditorGUI.Vector4Field( rect, "", v4 );
                field.QuaternionValue = new Quaternion( v4.x, v4.y, v4.z, v4.w );
                break;
            case "Bounds":
                ShowWizard<BoundsWizard>( rect, field, "Bounds Editor", 405, 130 );
                break;
            case "Rect":
                ShowWizard<RectWizard>( rect, field, "Rect Editor", 350, 130 );
                break;
            case "AnimationCurve":
                field.AnimationCurveValue = EditorGUI.CurveField( rect, field.AnimationCurveValue );
                break;
            case "Object":
                field.ObjectValue = EditorGUI.ObjectField( rect, field.ObjectValue, field.Type, true );
                break;
            case "Enum":
                var enumValue = (Enum)Enum.Parse( field.Type, field.EnumNames[field.EnumValue] );
                enumValue = EditorGUI.EnumPopup( rect, enumValue );
                for ( int i = 0; i < field.EnumNames.Length; i++ ) {
                    if ( field.EnumNames[i] == enumValue.ToString() ) {
                        field.EnumValue = i;
                        break;
                    }
                }
                break;
            default:
                EditorGUI.HelpBox( rect, string.Format( "The type {0} is not supported", field.RepresentableType ), MessageType.Warning );
                break;
        }
    }

    private void DrawProperty( ExtendedEvent.Property property, Rect rect ) {

    }

    private void DrawMethod( ExtendedEvent.Method method, Rect rect ) {

    }

    private void ShowWizard<T>( Rect rect, ExtendedEvent.Field field, string title, float width, float height ) where T : FieldWizard {
        if ( GUI.Button( rect, "..." ) ) {
            var wiz = ScriptableWizard.DisplayWizard<T>( title, "Close" );
            wiz.Field = field;
            wiz.minSize = new Vector2( width, height );
            wiz.maxSize = new Vector2( width, height );
        }
    }


    #region DropDown 
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
    private static int DropdownList( Rect position, int current, GUIContent[] items ) {
        int controlID = GUIUtility.GetControlID( dropdownHash, FocusType.Native, position );
        var mask = DropdownCallbackInfo.GetSelectedValueForControl( controlID, current );

        var evt = Event.current;
        if ( evt.type == EventType.Repaint ) {
            if ( current >= items.Length || current == -1 ) {
                EditorStyles.popup.Draw( position, new GUIContent( "-" ), controlID, false );
            } else {
                EditorStyles.popup.Draw( position, new GUIContent( items[current] ), controlID, false );
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
