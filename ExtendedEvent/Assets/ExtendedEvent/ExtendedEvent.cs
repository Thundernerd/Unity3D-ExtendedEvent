using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public class ExtendedEvent {

    private static string GetTypeName( Type type ) {
        if ( type.Assembly.FullName.Contains( "Unity" ) ) {
            return type.Name;
        } else {
            var code = Type.GetTypeCode( type );
            switch ( code ) {
                case TypeCode.Empty:
                    return "Empty";
                case TypeCode.Object:
                    return type.Name;
                case TypeCode.DBNull:
                    return "DBNull";
                case TypeCode.Boolean:
                    return "bool";
                case TypeCode.Char:
                    return "char";
                case TypeCode.SByte:
                    return "sbyte";
                case TypeCode.Byte:
                    return "byte";
                case TypeCode.Int16:
                    return "short";
                case TypeCode.UInt16:
                    return "ushort";
                case TypeCode.Int32:
                    return "int";
                case TypeCode.UInt32:
                    return "uint";
                case TypeCode.Int64:
                    return "long";
                case TypeCode.UInt64:
                    return "ulong";
                case TypeCode.Single:
                    return "float";
                case TypeCode.Double:
                    return "double";
                case TypeCode.Decimal:
                    return "decimal";
                case TypeCode.DateTime:
                    return "DateTime";
                case TypeCode.String:
                    return "string";
                default:
                    return "";
            }
        }
    }

    [Serializable]
    public class Field {
        public string Name;
        public string RepresentableType;
        public string NewValue;
        public string Type;
        public string Assembly;
        public UnityEngine.Object Object;

        [SerializeField]
        private string parentName;

        public Field() { }

        public Field( FieldInfo info, Type type ) {
            Name = info.Name;
            Assembly = info.FieldType.Assembly.FullName;
            Type = info.FieldType.FullName;
            RepresentableType = GetTypeName( info.FieldType );

            try {
                if ( info.FieldType.IsSubclassOf( typeof( Component ) ) ) {
                    NewValue = "";
                } else {
                    NewValue = Activator.CreateInstance( info.FieldType ).ToString();
                    Object = (UnityEngine.Object)Activator.CreateInstance( info.FieldType );
                }
            } catch ( Exception ) {
                // Catch 'm all just in case (bad habit, I am aware)
            }

            parentName = type.Name;
        }

        public void Invoke( GameObject item ) {
            var a = System.Reflection.Assembly.Load( Assembly );
            var t = a.GetType( Type );

            var component = item.GetComponent( parentName );
            var componentType = component.GetType();
            var field = componentType.GetField( Name );

            object value = null;

            if ( t == typeof( int ) ) {
                value = int.Parse( NewValue );
            } else if ( t == typeof( float ) ) {
                value = float.Parse( NewValue );
            } else if ( t == typeof( double ) ) {
                value = double.Parse( NewValue );
            } else if ( t == typeof( long ) ) {
                value = long.Parse( NewValue );
            } else if ( t == typeof( string ) ) {
                value = NewValue;
            } else if ( t == typeof( bool ) ) {
                value = bool.Parse( NewValue );
            } else if ( t == typeof( Vector2 ) ) {
                value = ExtendedEventConverter.Vec2( NewValue );
            } else if ( t == typeof( Vector3 ) ) {
                value = ExtendedEventConverter.Vec3( NewValue );
            } else if ( t == typeof( Vector4 ) ) {
                value = ExtendedEventConverter.Vec4( NewValue );
            } else if ( t == typeof( GameObject ) ) {
                value = GameObject.Find( NewValue );
            } else if ( t == typeof( Bounds ) ) {
                value = ExtendedEventConverter.Bounds( NewValue );
            } else if ( t == typeof( Rect ) ) {
                value = ExtendedEventConverter.Rect( NewValue );
            } else if ( t == typeof( Color ) ) {
                value = ExtendedEventConverter.Color( NewValue );
            } else if ( t == typeof( AnimationCurve ) ) {
                value = ExtendedEventConverter.Curve( NewValue );
            } else if ( t.IsSubclassOf( typeof( Enum ) ) ) {
                value = Enum.Parse( t, NewValue );
            } else if ( t.IsSubclassOf( typeof( UnityEngine.Object ) ) ) {
                value = Object;
            }

            field.SetValue( component, value );
        }

        public override string ToString() {
            return string.Format( "{0}/Fields/{1} {2}", parentName, RepresentableType, Name );
        }
    }

    [Serializable]
    public class Property {
        public string Name;
        public string RepresentableType;
        public string NewValue;
        public string Type;
        public string Assembly;
        public UnityEngine.Object Object;

        [SerializeField]
        private string parentName;

        public Property() { }

        public Property( PropertyInfo info, Type type ) {
            Name = info.Name;
            Assembly = info.PropertyType.Assembly.FullName;
            Type = info.PropertyType.FullName;
            RepresentableType = GetTypeName( info.PropertyType );

            try {
                if ( info.PropertyType.IsSubclassOf( typeof( Component ) ) ) {
                    NewValue = "";
                } else {
                    NewValue = Activator.CreateInstance( info.PropertyType ).ToString();
                    Object = (UnityEngine.Object)Activator.CreateInstance( info.PropertyType );
                }
            } catch ( Exception ) {
                // Catch 'm all just in case (bad habit, I am aware)
            }

            parentName = type.Name;
        }

        public void Invoke( GameObject item ) {
            var a = System.Reflection.Assembly.Load( Assembly );
            var t = a.GetType( Type );

            var component = item.GetComponent( parentName );
            var componentType = component.GetType();
            var field = componentType.GetProperty( Name );

            object value = null;

            if ( t == typeof( int ) ) {
                value = int.Parse( NewValue );
            } else if ( t == typeof( float ) ) {
                value = float.Parse( NewValue );
            } else if ( t == typeof( double ) ) {
                value = double.Parse( NewValue );
            } else if ( t == typeof( long ) ) {
                value = long.Parse( NewValue );
            } else if ( t == typeof( string ) ) {
                value = NewValue;
            } else if ( t == typeof( bool ) ) {
                value = bool.Parse( NewValue );
            } else if ( t == typeof( Vector2 ) ) {
                value = ExtendedEventConverter.Vec2( NewValue );
            } else if ( t == typeof( Vector3 ) ) {
                value = ExtendedEventConverter.Vec3( NewValue );
            } else if ( t == typeof( Vector4 ) ) {
                value = ExtendedEventConverter.Vec4( NewValue );
            } else if ( t == typeof( GameObject ) ) {
                value = GameObject.Find( NewValue );
            } else if ( t == typeof( Bounds ) ) {
                value = ExtendedEventConverter.Bounds( NewValue );
            } else if ( t == typeof( Rect ) ) {
                value = ExtendedEventConverter.Rect( NewValue );
            } else if ( t == typeof( Color ) ) {
                value = ExtendedEventConverter.Color( NewValue );
            } else if ( t == typeof( AnimationCurve ) ) {
                value = ExtendedEventConverter.Curve( NewValue );
            } else if ( t.IsSubclassOf( typeof( Enum ) ) ) {
                value = Enum.Parse( t, NewValue );
            } else if ( t.IsSubclassOf( typeof( UnityEngine.Object ) ) ) {
                value = Object;
            }

            field.SetValue( component, value, null );
        }

        public override string ToString() {
            return string.Format( "{0}/Properties/{1} {2}", parentName, RepresentableType, Name );
        }
    }

    [Serializable]
    public class Parameter {
        public string Name;
        public string RepresentableType;
        public string NewValue;
        public string Assembly;
        public string Type;
        public UnityEngine.Object Object;

        public Parameter() { }

        public Parameter( ParameterInfo info ) {
            Name = info.Name;
            Assembly = info.ParameterType.Assembly.FullName;
            Type = info.ParameterType.FullName;
            RepresentableType = GetTypeName( info.ParameterType );

            try {
                if ( info.ParameterType.IsSubclassOf( typeof( Component ) ) ) {
                    NewValue = "";
                } else {
                    NewValue = Activator.CreateInstance( info.ParameterType ).ToString();
                    Object = (UnityEngine.Object)Activator.CreateInstance( info.ParameterType );
                }
            } catch ( Exception ) {
                // Catch 'm all
            }
        }

        public string ToStringLong() {
            return string.Format( "{0} {1}", RepresentableType, Name );
        }

        public string ToStringShort() {
            return RepresentableType;
        }
    }

    [Serializable]
    public class Method {
        public string Name;
        public List<Parameter> Parameters;

        [SerializeField]
        private string parentName;

        public Method() { }

        public Method( MethodInfo info, Type type ) {
            Name = info.Name;
            Parameters = new List<Parameter>();

            var parameters = info.GetParameters();
            foreach ( var p in parameters ) {
                Parameters.Add( new Parameter( p ) );
            }

            parentName = type.Name;
        }

        public void Invoke( GameObject item ) {
            var component = item.GetComponent( parentName );
            var componentType = component.GetType();

            var parameters = new object[Parameters.Count];
            for ( int i = 0; i < Parameters.Count; i++ ) {
                var p = Parameters[i];
                var a = Assembly.Load( p.Assembly );
                var t = a.GetType( p.Type );

                if ( t == typeof( int ) ) {
                    parameters[i] = int.Parse( p.NewValue );
                } else if ( t == typeof( float ) ) {
                    parameters[i] = float.Parse( p.NewValue );
                } else if ( t == typeof( double ) ) {
                    parameters[i] = double.Parse( p.NewValue );
                } else if ( t == typeof( long ) ) {
                    parameters[i] = long.Parse( p.NewValue );
                } else if ( t == typeof( string ) ) {
                    parameters[i] = p.NewValue;
                } else if ( t == typeof( bool ) ) {
                    parameters[i] = bool.Parse( p.NewValue );
                } else if ( t == typeof( Vector2 ) ) {
                    parameters[i] = ExtendedEventConverter.Vec2( p.NewValue );
                } else if ( t == typeof( Vector3 ) ) {
                    parameters[i] = ExtendedEventConverter.Vec3( p.NewValue );
                } else if ( t == typeof( Vector4 ) ) {
                    parameters[i] = ExtendedEventConverter.Vec4( p.NewValue );
                } else if ( t == typeof( GameObject ) ) {
                    parameters[i] = GameObject.Find( p.NewValue );
                } else if ( t == typeof( Bounds ) ) {
                    parameters[i] = ExtendedEventConverter.Bounds( p.NewValue );
                } else if ( t == typeof( Rect ) ) {
                    parameters[i] = ExtendedEventConverter.Rect( p.NewValue );
                } else if ( t == typeof( Color ) ) {
                    parameters[i] = ExtendedEventConverter.Color( p.NewValue );
                } else if ( t == typeof( AnimationCurve ) ) {
                    parameters[i] = ExtendedEventConverter.Curve( p.NewValue );
                } else if ( t.IsSubclassOf( typeof( Enum ) ) ) {
                    parameters[i] = Enum.Parse( t, p.NewValue );
                } else if ( t.IsSubclassOf( typeof( UnityEngine.Object ) ) ) {
                    parameters[i] = p.Object;
                }
            }

            var mTypes = parameters.Select( p => p.GetType() );
            var method = componentType.GetMethod( Name, mTypes.ToArray() );

            method.Invoke( component, parameters );
        }

        public string ToStringShort() {
            var parameters = "";
            foreach ( var item in Parameters ) {
                parameters += string.Format( "{0}, ", item.ToStringShort() );
            }
            parameters = parameters.TrimEnd( ',', ' ' );

            return string.Format( "{2}/Methods/{0} ({1})", Name, parameters, parentName );
        }

        public override string ToString() {
            return string.Format( "{0}", Name );
        }
    }

    [Serializable]
    public class GObject {

        public GameObject GameObject;

        public List<Field> Fields = new List<Field>();
        public List<Property> Properties = new List<Property>();
        public List<Method> Methods = new List<Method>();

        public string[] List = { "Nothing" };
        public int Index = 0;

        public GObject() { }

        public GObject( GameObject gObj ) {
            Reset( gObj );
        }

        public void Reset( GameObject gObj ) {
            GameObject = gObj;
            Reset();
        }

        public void Reset() {
            Fields.Clear();
            Properties.Clear();
            Methods.Clear();
            Index = 0;

            var components = GameObject.GetComponents<Component>();
            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            foreach ( var cmp in components ) {
                var type = cmp.GetType();

                var fields = type.GetFields( flags ).ToList();
                fields.Sort( ( f1, f2 ) => f1.Name.CompareTo( f2.Name ) );

                var properties = type.GetProperties( flags ).Where( p => p.CanWrite ).ToList();
                if ( type.IsSubclassOf( typeof( Behaviour ) ) ) {
                    var t = typeof( Behaviour );
                    var temp = t.GetProperties( flags ).Where( p => p.CanWrite ).ToList();
                    properties.AddRange( temp );
                }
                {   // Bracketing it for no good reason
                    var t = typeof( Component );
                    var temp = t.GetProperties( flags ).Where( p => p.CanWrite ).ToList();
                    properties.AddRange( temp );
                }
                properties.Sort( ( p1, p2 ) => p1.Name.CompareTo( p2.Name ) );

                var methods = type.GetMethods( flags ).Where( m =>
                   !m.Name.StartsWith( "set_" ) && !m.Name.StartsWith( "get_" ) &&
                   !m.ContainsGenericParameters && !m.IsGenericMethod && !m.IsGenericMethodDefinition &&
                   !m.IsConstructor && !m.IsAbstract ).ToList();

                for ( int i = methods.Count - 1; i >= 0; i-- ) {
                    var pars = methods[i].GetParameters();
                    foreach ( var p in pars ) {
                        if ( p.ParameterType.IsGenericParameter || p.ParameterType.IsGenericType || p.ParameterType.IsGenericTypeDefinition ||
                            p.ParameterType.IsArray ) {
                            methods.RemoveAt( i );
                            break;
                        }
                    }
                }
                methods.Sort( ( m1, m2 ) => m1.Name.CompareTo( m2.Name ) );

                foreach ( var f in fields ) {
                    Fields.Add( new Field( f, type ) );
                }

                foreach ( var p in properties ) {
                    Properties.Add( new Property( p, type ) );
                }

                foreach ( var m in methods ) {
                    Methods.Add( new Method( m, type ) );
                }
            }

            List = new string[Fields.Count + Properties.Count + Methods.Count + 1];
            List[0] = "Nothing Selected";

            for ( int i = 0; i < Fields.Count; i++ ) {
                List[i + 1] = Fields[i].ToString();
            }
            for ( int i = 0, j = Fields.Count; i < Properties.Count; i++, j++ ) {
                List[j + 1] = Properties[i].ToString();
            }
            for ( int i = 0, j = Fields.Count + Properties.Count; i < Methods.Count; i++, j++ ) {
                List[j + 1] = Methods[i].ToStringShort();
            }
        }

        public void Invoke() {
            var i = Index - 1;

            if ( i >= Fields.Count + Properties.Count ) {
                Methods[i - Fields.Count - Properties.Count].Invoke( GameObject );
            } else if ( i >= Fields.Count ) {
                Properties[i - Fields.Count].Invoke( GameObject );
            } else {
                Fields[i].Invoke( GameObject );
            }
        }

        public bool IsMethod() {
            return Index - 1 >= Fields.Count;
        }
    }

    public List<GObject> Objects = new List<GObject>();

    public void Invoke() {
        foreach ( var item in Objects ) {
            item.Invoke();
        }
    }
}
