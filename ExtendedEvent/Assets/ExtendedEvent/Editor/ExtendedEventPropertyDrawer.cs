using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer( typeof( ExtendedEvent ) )]
public class ExtendedEventPropertyDrawer : PropertyDrawer {

    private ReorderableList rList;
    private ExtendedEvent eEvent;
    private string header = "";
    private ExtendedEvent.GameObjectContainer current;

    private void RestoreState( SerializedProperty property ) {
        if ( rList == null || eEvent == null ) {
            header = property.name;

            var target = property.serializedObject.targetObject;
            var type = target.GetType();
            var field = type.GetField( property.name );
            eEvent = field.GetValue( target ) as ExtendedEvent;

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
        current = eEvent.Listeners[index];

        rect.yMin += 3f;
        rect.yMax -= 7f;

        var thirdWidth = rect.width / 3;
        var halfHeight = rect.height / 2;

        var gameObjectRect = new Rect( rect.x, rect.y, thirdWidth, halfHeight );
        var dropdownRect = new Rect( rect.x + thirdWidth, rect.y, thirdWidth * 2, halfHeight );

        EditorGUI.BeginChangeCheck();
        current.GameObject = (GameObject)EditorGUI.ObjectField( gameObjectRect, current.GameObject, typeof( GameObject ), true );
        if ( EditorGUI.EndChangeCheck() ) {
            current.Reset();
        }

        current.Index = DropdownList( dropdownRect, current.Index, current.List );
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
