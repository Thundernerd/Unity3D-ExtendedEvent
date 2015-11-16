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
    public class MemberBase {
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
        public Matrix4x4 MatrixValue;
        public AnimationCurve AnimationCurveValue;
        public Color ColorValue;
        public UnityEngine.Object ObjectValue;

        public int EnumValue;
        public string[] EnumNames;

        [SerializeField]
        protected string typeName;
        [SerializeField]
        protected string assemblyName;
        [SerializeField]
        protected string parentName;

        public MemberBase() { }

        public MemberBase( string name, Type infoType, Type type ) {
            Name = name;
            Type = infoType;
            TypeName = infoType.Name;
            RepresentableType = GetTypeName( infoType );

            typeName = infoType.FullName;
            assemblyName = infoType.Assembly.GetName().Name;

            if ( Type.IsSubclassOf( typeof( UnityEngine.Object ) ) ) {
                TypeName = "Object";
            } else if ( Type.IsEnum ) {
                TypeName = "Enum";
                EnumNames = Enum.GetNames( Type );
            }

            parentName = type.Name;
        }

        public object GetValue() {
            switch ( TypeName ) {
                case "String":
                    return StringValue;
                case "Int32":
                    return IntValue;
                case "Int64":
                    return LongValue;
                case "Single":
                    return FloatValue;
                case "Double":
                    return DoubleValue;
                case "Boolean":
                    return BoolValue;
                case "Vector2":
                    return Vector2Value;
                case "Vector3":
                    return Vector3Value;
                case "Vector4":
                    return Vector4Value;
                case "Quaternion":
                    return QuaternionValue;
                case "Bounds":
                    return BoundsValue;
                case "Rect":
                    return RectValue;
                case "Matrix4x4":
                    return MatrixValue;
                case "AnimationCurve":
                    return AnimationCurveValue;
                case "Object":
                    return ObjectValue;
                case "Enum":
                    return Enum.Parse( Type, EnumNames[EnumValue] );
            }

            return null;
        }

        public void Initialize() {
            LoadType();
        }

        public void LoadType() {
            Type = Type.GetType( string.Format( "{0},{1}", typeName, assemblyName ) );
        }
    }

    [Serializable]
    public class Field : MemberBase {
        //public string Name;
        //public string RepresentableType;
        //public Type Type;
        //public string TypeName;

        //public string StringValue;
        //public int IntValue;
        //public float FloatValue;
        //public double DoubleValue;
        //public long LongValue;
        //public bool BoolValue;
        //public Vector2 Vector2Value;
        //public Vector3 Vector3Value;
        //public Vector4 Vector4Value;
        //public Quaternion QuaternionValue;
        //public Bounds BoundsValue;
        //public Rect RectValue;
        //public Matrix4x4 MatrixValue;
        //public AnimationCurve AnimationCurveValue;
        //public Color ColorValue;
        //public UnityEngine.Object ObjectValue;

        //public int EnumValue;
        //public string[] EnumNames;

        //[SerializeField]
        //private string typeName;
        //[SerializeField]
        //private string assemblyName;
        //[SerializeField]
        //private string parentName;

        public Field() { }

        public Field( FieldInfo info, Type type )
            : base( info.Name, info.FieldType, type ) {
            //Name = info.Name;
            //Type = info.FieldType;
            //TypeName = info.FieldType.Name;
            //RepresentableType = GetTypeName( Type );

            //typeName = info.FieldType.FullName;
            //assemblyName = info.FieldType.Assembly.GetName().Name;


            //if ( Type.IsSubclassOf( typeof( UnityEngine.Object ) ) ) {
            //    TypeName = "Object";
            //} else if ( Type.IsEnum ) {
            //    TypeName = "Enum";
            //    EnumNames = Enum.GetNames( Type );
            //}

            //parentName = type.Name;
        }

        public void Initialize() {
            LoadType();
        }

        public void Invoke( GameObject item ) {
            object value = GetValue();

            //switch ( TypeName ) {
            //    case "String":
            //        value = StringValue;
            //        break;
            //    case "Int32":
            //        value = IntValue;
            //        break;
            //    case "Int64":
            //        value = LongValue;
            //        break;
            //    case "Single":
            //        value = FloatValue;
            //        break;
            //    case "Double":
            //        value = DoubleValue;
            //        break;
            //    case "Boolean":
            //        value = BoolValue;
            //        break;
            //    case "Vector2":
            //        value = Vector2Value;
            //        break;
            //    case "Vector3":
            //        value = Vector3Value;
            //        break;
            //    case "Vector4":
            //        value = Vector4Value;
            //        break;
            //    case "Quaternion":
            //        value = QuaternionValue;
            //        break;
            //    case "Bounds":
            //        value = BoundsValue;
            //        break;
            //    case "Rect":
            //        value = RectValue;
            //        break;
            //    case "Matrix4x4":
            //        value = MatrixValue;
            //        break;
            //    case "AnimationCurve":
            //        value = AnimationCurveValue;
            //        break;
            //    case "Object":
            //        value = ObjectValue;
            //        break;
            //    case "Enum":
            //        value = Enum.Parse( Type, EnumNames[EnumValue] );
            //        break;
            //}

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

        //public void LoadType() {
        //    Type = Type.GetType( string.Format( "{0},{1}", typeName, assemblyName ) );
        //}

        public override string ToString() {
            return string.Format( "{0}/Fields/{1} {2}", parentName, RepresentableType, Name );
        }
    }

    [Serializable]
    public class Property : MemberBase {
        //public string Name;
        //public string RepresentableType;
        //public Type Type;
        //public string TypeName;

        //public string StringValue;
        //public int IntValue;
        //public float FloatValue;
        //public double DoubleValue;
        //public long LongValue;
        //public bool BoolValue;
        //public Vector2 Vector2Value;
        //public Vector3 Vector3Value;
        //public Vector4 Vector4Value;
        //public Quaternion QuaternionValue;
        //public Bounds BoundsValue;
        //public Rect RectValue;
        //public Matrix4x4 MatrixValue;
        //public AnimationCurve AnimationCurveValue;
        //public Color ColorValue;
        //public UnityEngine.Object ObjectValue;

        //public int EnumValue;
        //public string[] EnumNames;

        //[SerializeField]
        //private string typeName;
        //[SerializeField]
        //private string assemblyName;
        //[SerializeField]
        //private string parentName;

        public Property() { }

        public Property( PropertyInfo info, Type type )
            : base( info.Name, info.PropertyType, type ) {
            //Name = info.Name;
            //Type = info.PropertyType;
            //TypeName = info.PropertyType.Name;
            //RepresentableType = GetTypeName( Type );

            //typeName = info.PropertyType.FullName;
            //assemblyName = info.PropertyType.Assembly.GetName().Name;

            //if ( Type.IsSubclassOf( typeof( Component ) ) ) {
            //    TypeName = "Object";
            //} else if ( Type.IsEnum ) {
            //    TypeName = "Enum";
            //    EnumNames = Enum.GetNames( Type );
            //}

            //parentName = type.Name;
        }

        public void Initialize() {
            LoadType();
        }

        public void Invoke( GameObject item ) {
            object value = GetValue();

            //switch ( TypeName ) {
            //    case "String":
            //        value = StringValue;
            //        break;
            //    case "Int32":
            //        value = IntValue;
            //        break;
            //    case "Int64":
            //        value = LongValue;
            //        break;
            //    case "Single":
            //        value = FloatValue;
            //        break;
            //    case "Double":
            //        value = DoubleValue;
            //        break;
            //    case "Boolean":
            //        value = BoolValue;
            //        break;
            //    case "Vector2":
            //        value = Vector2Value;
            //        break;
            //    case "Vector3":
            //        value = Vector3Value;
            //        break;
            //    case "Vector4":
            //        value = Vector4Value;
            //        break;
            //    case "Quaternion":
            //        value = QuaternionValue;
            //        break;
            //    case "Bounds":
            //        value = BoundsValue;
            //        break;
            //    case "Rect":
            //        value = RectValue;
            //        break;
            //    case "Matrix4x4":
            //        value = MatrixValue;
            //        break;
            //    case "AnimationCurve":
            //        value = AnimationCurveValue;
            //        break;
            //    case "Object":
            //        value = ObjectValue;
            //        break;
            //    case "Enum":
            //        value = Enum.Parse( Type, EnumNames[EnumValue] );
            //        break;
            //}

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

        //public void LoadType() {
        //    Type = Type.GetType( string.Format( "{0},{1}", typeName, assemblyName ) );
        //}

        public override string ToString() {
            return string.Format( "{0}/Properties/{1} {2}", parentName, RepresentableType, Name );
        }
    }

    [Serializable]
    public class Parameter : MemberBase {
        //public string Name;
        //public string RepresentableType;
        //public Type Type;
        //public string TypeName;

        //public string StringValue;
        //public int IntValue;
        //public float FloatValue;
        //public double DoubleValue;
        //public long LongValue;
        //public bool BoolValue;
        //public Vector2 Vector2Value;
        //public Vector3 Vector3Value;
        //public Vector4 Vector4Value;
        //public Quaternion QuaternionValue;
        //public Bounds BoundsValue;
        //public Rect RectValue;
        //public Matrix4x4 MatrixValue;
        //public AnimationCurve AnimationCurveValue;
        //public Color ColorValue;
        //public UnityEngine.Object ObjectValue;

        //public int EnumValue;
        //public string[] EnumNames;

        //[SerializeField]
        //private string typeName;
        //[SerializeField]
        //private string assemblyName;

        public Parameter() { }

        public Parameter( ParameterInfo info )
            : base( info.Name, info.ParameterType, info.ParameterType ) {
            Name = info.Name;
            Type = info.ParameterType;
            TypeName = info.ParameterType.Name;
            RepresentableType = GetTypeName( Type );

            typeName = info.ParameterType.FullName;
            assemblyName = info.ParameterType.Assembly.GetName().Name;

            if ( Type.IsSubclassOf( typeof( Component ) ) ) {
                TypeName = "Object";
            } else if ( Type.IsEnum ) {
                TypeName = "Enum";
                EnumNames = Enum.GetNames( Type );
            }
        }

        //public void Initialize() {
        //    LoadType();
        //}

        //public void LoadType() {
        //    Type = Type.GetType( string.Format( "{0},{1}", typeName, assemblyName ) );
        //}

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

        public void Initialize() {
            foreach ( var item in Parameters ) {
                item.Initialize();
            }
        }

        public void Invoke( GameObject item ) {
            var parameters = new object[Parameters.Count];
            for ( int i = 0; i < Parameters.Count; i++ ) {
                var p = Parameters[i];

                parameters[i] = p.GetValue();

                //switch ( p.TypeName ) {
                //    case "String":
                //        parameters[i] = p.StringValue;
                //        break;
                //    case "Int32":
                //        parameters[i] = p.IntValue;
                //        break;
                //    case "Int64":
                //        parameters[i] = p.LongValue;
                //        break;
                //    case "Single":
                //        parameters[i] = p.FloatValue;
                //        break;
                //    case "Double":
                //        parameters[i] = p.DoubleValue;
                //        break;
                //    case "Boolean":
                //        parameters[i] = p.BoolValue;
                //        break;
                //    case "Vector2":
                //        parameters[i] = p.Vector2Value;
                //        break;
                //    case "Vector3":
                //        parameters[i] = p.Vector3Value;
                //        break;
                //    case "Vector4":
                //        parameters[i] = p.Vector4Value;
                //        break;
                //    case "Quaternion":
                //        parameters[i] = p.QuaternionValue;
                //        break;
                //    case "Bounds":
                //        parameters[i] = p.BoundsValue;
                //        break;
                //    case "Rect":
                //        parameters[i] = p.RectValue;
                //        break;
                //    case "Matrix4x4":
                //        parameters[i] = p.MatrixValue;
                //        break;
                //    case "AnimationCurve":
                //        parameters[i] = p.AnimationCurveValue;
                //        break;
                //    case "Object":
                //        parameters[i] = p.ObjectValue;
                //        break;
                //    case "Enum":
                //        parameters[i] = Enum.Parse( p.Type, p.EnumNames[p.EnumValue] );
                //        break;
                //}
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

        public void Initialize() {
            foreach ( var item in Fields ) {
                item.Initialize();
            }
            foreach ( var item in Properties ) {
                item.Initialize();
            }
            foreach ( var item in Methods ) {
                item.Initialize();
            }
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
