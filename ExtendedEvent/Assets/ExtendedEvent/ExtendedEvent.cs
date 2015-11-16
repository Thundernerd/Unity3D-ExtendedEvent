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
        public Type Type;
        public string TypeName;

        public string StringValue;
        public int IntValue;
        public float FloatValue;
        public double DoubleValue;
        public long LongValue;
        public bool BoolValue;
        public Vector2 Vector2Value;
        public Vector3 Vector3Value;
        public Vector4 Vector4Value;
        public Quaternion QuaternionValue;
        public Bounds BoundsValue;
        public Rect RectValue;
        public AnimationCurve AnimationCurveValue;
        public Color ColorValue;
        public UnityEngine.Object ObjectValue;

        public string[] StringValues;
        public int[] IntValues;
        public long[] LongValues;
        public float[] FloatValues;
        public double[] DoubleValues;
        public bool[] BoolValues;
        public Vector2[] Vector2Values;
        public Vector3[] Vector3Values;
        public Vector4[] Vector4Values;
        public Quaternion[] QuaternionValues;
        public Bounds[] BoundsValues;
        public Rect[] RectValues;
        public AnimationCurve[] AnimationCurveValues;
        public Color[] ColorValues;
        public UnityEngine.Object[] ObjectValues;

        public int EnumValue;
        public string[] EnumNames;

        public bool IsArray;

        [SerializeField]
        private string parentName;

        public Field() { }

        public Field( FieldInfo info, Type type ) {
            Name = info.Name;
            Type = info.FieldType;
            TypeName = info.FieldType.Name.Replace( "[]", "" );
            IsArray = info.FieldType.IsArray;

            if ( IsArray ) {
                string fullName = info.FieldType.FullName.Substring( 0, info.FieldType.FullName.Length - 2 );
                var elementType = Type.GetType( string.Format( "{0},{1}", fullName, info.FieldType.Assembly.GetName().Name ) );
                RepresentableType = GetTypeName( elementType ) + "[]";
            } else {
                RepresentableType = GetTypeName( info.FieldType );
            }

            if ( info.FieldType.IsSubclassOf( typeof( UnityEngine.Object ) ) ) {
                TypeName = "Object";
            } else if ( info.FieldType.IsEnum ) {
                TypeName = "Enum";
                EnumNames = Enum.GetNames( info.FieldType );
            }

            parentName = type.Name;
        }

        public void Invoke( GameObject item ) {
            object value = null;

            switch ( TypeName ) {
                case "String":
                    value = IsArray ? (object)StringValues : StringValue;
                    break;
                case "Int32":
                    value = IsArray ? (object)IntValues : IntValue;
                    break;
                case "Int64":
                    value = IsArray ? (object)LongValues : LongValue;
                    break;
                case "Single":
                    value = IsArray ? (object)FloatValue : FloatValue;
                    break;
                case "Double":
                    value = IsArray ? (object)DoubleValues : DoubleValue;
                    break;
                case "Boolean":
                    value = IsArray ? (object)BoolValues : BoolValue;
                    break;
                case "Vector2":
                    value = IsArray ? (object)Vector2Values : Vector2Value;
                    break;
                case "Vector3":
                    value = IsArray ? (object)Vector3Values : Vector3Value;
                    break;
                case "Vector4":
                    value = IsArray ? (object)Vector4Values : Vector4Value;
                    break;
                case "Quaternion":
                    value = IsArray ? (object)QuaternionValues : QuaternionValue;
                    break;
                case "Bounds":
                    value = IsArray ? (object)BoundsValues : BoundsValue;
                    break;
                case "Rect":
                    value = IsArray ? (object)RectValues : RectValue;
                    break;
                case "AnimationCurve":
                    value = IsArray ? (object)AnimationCurveValues : AnimationCurveValue;
                    break;
                case "Object":
                    value = IsArray ? (object)ObjectValues : ObjectValue;
                    break;
                case "Enum":
                    value = Enum.Parse( Type, EnumNames[EnumValue] );
                    break;
            }

            if ( parentName == "GameObject" ) {
                var type = typeof( GameObject );
                var field = type.GetField( Name );
                field.SetValue( item, value );
            } else {
                var component = item.GetComponent( parentName );
                var componentType = component.GetType();
                var field = componentType.GetField( Name );
                field.SetValue( component, value );
            }
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
            } else if ( t == typeof( Quaternion ) ) {
                value = ExtendedEventConverter.Quat( NewValue );
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

            if ( parentName == "GameObject" ) {
                var type = typeof( GameObject );
                var field = type.GetProperty( Name, BindingFlags.Instance | BindingFlags.Public );
                field.SetValue( item, value, null );
            } else {
                var component = item.GetComponent( parentName );
                var componentType = component.GetType();
                var field = componentType.GetProperty( Name );
                field.SetValue( component, value, null );
            }
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
                // Catch 'm all just in case (bad habit, I am aware)
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
                } else if ( t == typeof( Quaternion ) ) {
                    parameters[i] = ExtendedEventConverter.Quat( p.NewValue );
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

            if ( parentName == "GameObject" ) {
                var type = typeof( GameObject );
                var method = type.GetMethod( Name, mTypes.ToArray() );
                method.Invoke( item, parameters );
            } else {
                var component = item.GetComponent( parentName );
                var componentType = component.GetType();
                var method = componentType.GetMethod( Name, mTypes.ToArray() );
                method.Invoke( component, parameters );
            }
        }

        public override string ToString() {
            var parameters = "";
            foreach ( var item in Parameters ) {
                parameters += string.Format( "{0}, ", item.ToStringShort() );
            }
            parameters = parameters.TrimEnd( ',', ' ' );

            return string.Format( "{2}/Methods/{0} ({1})", Name, parameters, parentName );
        }
    }

    [Serializable]
    public class GameObjectContainer {
        public GameObject GameObject;

        public List<Field> Fields = new List<Field>();
        public List<Property> Properties = new List<Property>();
        public List<Method> Methods = new List<Method>();

        public GUIContent[] List = new GUIContent[0];
        public List<int> Indeces = new List<int>();
        public int Index = 0;

        public GameObjectContainer() { }

        public GameObjectContainer( GameObject gObj ) {
            Reset( gObj );
        }

        public void Reset( GameObject gObj ) {
            GameObject = gObj;
            Reset();
        }

        public void Reset() {
            var obsoleteType = typeof( ObsoleteAttribute );
            var objectType = typeof( UnityEngine.Object );

            Fields.Clear();
            Properties.Clear();
            Methods.Clear();

            Indeces.Clear();
            Indeces.Add( 0 );
            Indeces.Add( 0 );

            Index = 0;

            List = new GUIContent[] { new GUIContent( "Nothing Selected" ), new GUIContent( "" ) };
            if ( GameObject == null ) return;

            var tempList = new List<GUIContent>();
            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            {
                // "Force add" GameObject item
                var gType = typeof( GameObject );
                var gFields = gType.GetFields( flags ).Where( m => m.GetCustomAttributes( obsoleteType, false ).Length == 0 ).ToList();
                var gProperties = gType.GetProperties( flags ).Where( p => p.GetCustomAttributes( obsoleteType, false ).Length == 0 && p.CanWrite ).ToList();
                var gMethods = gType.GetMethods( flags ).Where( m => m.GetCustomAttributes( obsoleteType, false ).Length == 0 &&
                                                                !m.Name.StartsWith( "get_" ) && !m.Name.StartsWith( "set_" ) && !m.ContainsGenericParameters &&
                                                                !m.IsAbstract && !m.IsConstructor && !m.IsGenericMethod && !m.IsGenericMethodDefinition && !m.IsVirtual ).ToList();

                {
                    var tFields = objectType.GetFields( flags ).Where( f => f.GetCustomAttributes( obsoleteType, false ).Length == 0 ).ToList();
                    gFields.AddRange( tFields );

                    var tProperties = objectType.GetProperties( flags ).Where( p => p.GetCustomAttributes( obsoleteType, false ).Length == 0 && p.CanWrite ).ToList();
                    gProperties.AddRange( tProperties );

                    var tMethods = objectType.GetMethods( flags ).Where( m => m.GetCustomAttributes( obsoleteType, false ).Length == 0 &&
                                                                 !m.Name.StartsWith( "get_" ) && !m.Name.StartsWith( "set_" ) && !m.ContainsGenericParameters &&
                                                                 !m.IsAbstract && !m.IsConstructor && !m.IsGenericMethod && !m.IsGenericMethodDefinition && !m.IsVirtual ).ToList();
                    gMethods.AddRange( tMethods );
                }

                // Filter methods for properties that we can't handle
                for ( int i = gMethods.Count - 1; i >= 0; i-- ) {
                    var pars = gMethods[i].GetParameters();
                    foreach ( var p in pars ) {
                        if ( p.ParameterType.IsGenericParameter ||
                                p.ParameterType.IsGenericType ||
                                p.ParameterType.IsGenericTypeDefinition ||
                                p.ParameterType.IsArray ||
                                ( !p.ParameterType.IsValueType && !p.ParameterType.IsSubclassOf( objectType ) && p.ParameterType.Name.ToLower() != "string" ) ) {
                            gMethods.RemoveAt( i );
                            break;
                        }
                    }
                }

                gFields.Sort( ( f1, f2 ) => f1.Name.CompareTo( f2.Name ) );
                gProperties.Sort( ( p1, p2 ) => p1.Name.CompareTo( p2.Name ) );
                gMethods.Sort( ( m1, m2 ) => m1.Name.CompareTo( m2.Name ) );

                foreach ( var f in gFields ) {
                    var field = new Field( f, gType );
                    Indeces.Add( Fields.Count );
                    tempList.Add( new GUIContent( field.ToString() ) );
                    Fields.Add( field );
                }

                foreach ( var p in gProperties ) {
                    var property = new Property( p, gType );
                    Indeces.Add( Properties.Count );
                    tempList.Add( new GUIContent( property.ToString() ) );
                    Properties.Add( property );
                }

                foreach ( var m in gMethods ) {
                    var method = new Method( m, gType );
                    Indeces.Add( Methods.Count );
                    tempList.Add( new GUIContent( method.ToString() ) );
                    Methods.Add( method );
                }
            }


            var components = GameObject.GetComponents<Component>();
            foreach ( var cmp in components ) {
                var type = cmp.GetType();

                var fields = type.GetFields( flags ).Where( f => f.GetCustomAttributes( obsoleteType, false ).Length == 0 ).ToList();
                var properties = type.GetProperties( flags ).Where( p => p.GetCustomAttributes( obsoleteType, false ).Length == 0 && p.CanWrite ).ToList();
                var methods = type.GetMethods( flags ).Where( m => m.GetCustomAttributes( obsoleteType, false ).Length == 0 &&
                                            !m.Name.StartsWith( "get_" ) && !m.Name.StartsWith( "set_" ) && !m.ContainsGenericParameters &&
                                            !m.IsAbstract && !m.IsConstructor && !m.IsGenericMethod && !m.IsGenericMethodDefinition && !m.IsVirtual ).ToList();


                // Add extra properties from subclasses
                if ( type.IsSubclassOf( typeof( Behaviour ) ) ) {
                    var t = typeof( Behaviour );
                    var temp = t.GetProperties( flags ).Where( p => p.GetCustomAttributes( obsoleteType, false ).Length == 0 && p.CanWrite ).ToList();
                    properties.AddRange( temp );
                }
                {   // Bracketing it for no good reason
                    var t = typeof( Component );
                    var temp = t.GetProperties( flags ).Where( p => p.GetCustomAttributes( obsoleteType, false ).Length == 0 && p.CanWrite ).ToList();
                    properties.AddRange( temp );
                }
                {
                    var tFields = objectType.GetFields( flags ).Where( f => f.GetCustomAttributes( obsoleteType, false ).Length == 0 ).ToList();
                    fields.AddRange( tFields );

                    var tProperties = objectType.GetProperties( flags ).Where( p => p.GetCustomAttributes( obsoleteType, false ).Length == 0 && p.CanWrite ).ToList();
                    properties.AddRange( tProperties );

                    var tMethods = objectType.GetMethods( flags ).Where( m => m.GetCustomAttributes( obsoleteType, false ).Length == 0 &&
                                                                 !m.Name.StartsWith( "get_" ) && !m.Name.StartsWith( "set_" ) && !m.ContainsGenericParameters &&
                                                                 !m.IsAbstract && !m.IsConstructor && !m.IsGenericMethod && !m.IsGenericMethodDefinition && !m.IsVirtual ).ToList();
                    methods.AddRange( tMethods );
                }

                // Filter methods for properties that we can't handle
                for ( int i = methods.Count - 1; i >= 0; i-- ) {
                    var pars = methods[i].GetParameters();
                    foreach ( var p in pars ) {
                        if ( p.ParameterType.IsGenericParameter ||
                                p.ParameterType.IsGenericType ||
                                p.ParameterType.IsGenericTypeDefinition ||
                                p.ParameterType.IsArray ||
                                ( !p.ParameterType.IsValueType && !p.ParameterType.IsSubclassOf( objectType ) && p.ParameterType.Name.ToLower() != "string" ) ) {
                            methods.RemoveAt( i );
                            break;
                        }
                    }
                }

                fields.Sort( ( f1, f2 ) => f1.Name.CompareTo( f2.Name ) );
                properties.Sort( ( p1, p2 ) => p1.Name.CompareTo( p2.Name ) );
                methods.Sort( ( m1, m2 ) => m1.Name.CompareTo( m2.Name ) );

                foreach ( var f in fields ) {
                    var field = new Field( f, type );
                    Indeces.Add( Fields.Count );
                    tempList.Add( new GUIContent( field.ToString() ) );
                    Fields.Add( field );
                }

                foreach ( var p in properties ) {
                    var property = new Property( p, type );
                    Indeces.Add( Properties.Count );
                    tempList.Add( new GUIContent( property.ToString() ) );
                    Properties.Add( property );
                }

                foreach ( var m in methods ) {
                    var method = new Method( m, type );
                    Indeces.Add( Methods.Count );
                    tempList.Add( new GUIContent( method.ToString() ) );
                    Methods.Add( method );
                }
            }

            tempList.Insert( 0, new GUIContent( "" ) );
            tempList.Insert( 0, new GUIContent( "Nothing Selected" ) );
            List = tempList.ToArray();
        }

        public void Invoke() {
            if ( Index < 2 ) return;

            var splits = List[Index].text.Split( '/' );
            switch ( splits[1] ) {
                case "Fields":
                    Fields[Indeces[Index]].Invoke( GameObject );
                    break;
                case "Properties":
                    Properties[Indeces[Index]].Invoke( GameObject );
                    break;
                case "Methods":
                    Methods[Indeces[Index]].Invoke( GameObject );
                    break;
            }
        }

        public int Type {
            get {
                var splits = List[Index].text.Split( '/' );
                switch ( splits[1] ) {
                    case "Fields":
                        return 0;
                    case "Properties":
                        return 1;
                    case "Methods":
                        return 2;
                }
                return -1;
            }
        }

        public Field CurrentField {
            get {
                return Fields[Indeces[Index]];
            }
        }

        public Property CurrentProperty {
            get {
                return Properties[Indeces[Index]];
            }
        }

        public Method CurrentMethod {
            get {
                return Methods[Indeces[Index]];
            }
        }
    }

    public List<GameObjectContainer> Listeners = new List<GameObjectContainer>();

    public void Invoke() {
        foreach ( var item in Listeners ) {
            item.Invoke();
        }
    }
}
