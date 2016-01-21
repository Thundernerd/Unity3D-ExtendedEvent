using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public class ExtendedEvent {

    private static string GetTypeName( Type type ) {
        if ( type.Assembly.FullName.Contains( "Unity" ) ) {
            return type.Name;
        } else if ( type.IsEnum ) {
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

    public enum EMemberType {
        Field,
        Property,
        Method,
        None
    }

    [Serializable]
    public class Parameter {
        public string Name;
        public string DisplayName;
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
        [SerializeField]
        protected ParameterInfo info;

        public Parameter() { }

        public Parameter( ParameterInfo info ) {
            this.info = info;

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

            LoadDefaultValue();
            SetDisplayName();
        }

        public void Initialize() {
            LoadType();
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
                case "GameObject":
                    return ObjectValue;
                case "Enum":
                    return Enum.Parse( Type, EnumNames[EnumValue] );
            }

            return null;
        }

        public void LoadType() {
            Type = Type.GetType( string.Format( "{0},{1}", typeName, assemblyName ) );
        }

        public void LoadDefaultValue() {
            if ( info == null ) return;

            try {
                switch ( TypeName ) {
                    case "String":
                        StringValue = info.IsOptional ? (string)info.DefaultValue : "";
                        break;
                    case "Int32":
                        IntValue = info.IsOptional ? (int)info.DefaultValue : 0;
                        break;
                    case "Int64":
                        LongValue = info.IsOptional ? (long)info.DefaultValue : 0;
                        break;
                    case "Single":
                        FloatValue = info.IsOptional ? (float)info.DefaultValue : 0;
                        break;
                    case "Double":
                        DoubleValue = info.IsOptional ? (double)info.DefaultValue : 0;
                        break;
                    case "Boolean":
                        BoolValue = info.IsOptional ? (bool)info.DefaultValue : false;
                        break;
                    case "Vector2":
                        Vector2Value = info.IsOptional ? (Vector2)info.DefaultValue : Vector2.zero;
                        break;
                    case "Vector3":
                        Vector3Value = info.IsOptional ? (Vector3)info.DefaultValue : Vector3.zero;
                        break;
                    case "Vector4":
                        Vector4Value = info.IsOptional ? (Vector4)info.DefaultValue : Vector4.zero;
                        break;
                    case "Quaternion":
                        QuaternionValue = info.IsOptional ? (Quaternion)info.DefaultValue : new Quaternion();
                        break;
                    case "Bounds":
                        BoundsValue = info.IsOptional ? (Bounds)info.DefaultValue : new Bounds();
                        break;
                    case "Rect":
                        RectValue = info.IsOptional ? (Rect)info.DefaultValue : new Rect();
                        break;
                    case "Matrix4x4":
                        MatrixValue = info.IsOptional ? (Matrix4x4)info.DefaultValue : new Matrix4x4();
                        break;
                    case "AnimationCurve":
                        AnimationCurveValue = info.IsOptional ? (AnimationCurve)info.DefaultValue : new AnimationCurve();
                        break;
                    case "Object":
                    case "GameObject":
                        ObjectValue = info.IsOptional ? (UnityEngine.Object)info.DefaultValue : null;
                        break;
                    case "Enum":
                        EnumValue = info.IsOptional ? (int)info.DefaultValue : 0;
                        break;
                }
            } catch ( InvalidCastException ) {
                // Ignoring the default value for this parameter since it cannot be converted properly
            }
        }

        public void SetDisplayName() {
            var displayName = "";

            for ( int i = 0, j = 0; i < Name.Length; i++ ) {
                if ( i > 0 && char.IsUpper( Name[i] ) ) {
                    displayName += " " + Name.Substring( j, i - j );
                    j = i;
                }

                if ( i == Name.Length - 1 ) {
                    displayName += " " + Name.Substring( j, i - j + 1 );
                }
            }

            DisplayName = displayName.Trim();
            DisplayName = string.Format( "{0}{1}", char.ToUpper( DisplayName[0] ), DisplayName.Substring( 1 ) );
        }

        public override string ToString() {
            return string.Format( "{0} {1}", RepresentableType, Name );
        }
    }

    [Serializable]
    public class Member {
        public string Name;
        public string RepresentableType;
        public Type Type;
        public string TypeName;
        public EMemberType MemberType = EMemberType.None;

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

        // It's dirty to put it in here but Unity doesn't give me another choice
        // It wont save the object as it's derived type but as the base type
        public List<Parameter> Parameters;

        [SerializeField]
        protected string typeName;
        [SerializeField]
        protected string assemblyName;
        [SerializeField]
        protected string parentName;

        public Member() { }

        public Member( MemberInfo info, Type infoType, Type type, EMemberType memberType ) {
            Name = info.Name;
            Type = infoType;
            TypeName = infoType.Name;
            RepresentableType = GetTypeName( infoType );
            MemberType = memberType;

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

        public Member( MemberInfo info, Type infoType, Type type, UnityEngine.Object container )
            : this( info, infoType, type, EMemberType.Field ) {

            var finfo = info as FieldInfo;

            try {
                switch ( TypeName ) {
                    case "String":
                        StringValue = (string)finfo.GetValue( container );
                        break;
                    case "Int32":
                        IntValue = (int)finfo.GetValue( container );
                        break;
                    case "Int64":
                        LongValue = (long)finfo.GetValue( container );
                        break;
                    case "Single":
                        FloatValue = (float)finfo.GetValue( container );
                        break;
                    case "Double":
                        DoubleValue = (double)finfo.GetValue( container );
                        break;
                    case "Boolean":
                        BoolValue = (bool)finfo.GetValue( container );
                        break;
                    case "Vector2":
                        Vector2Value = (Vector2)finfo.GetValue( container );
                        break;
                    case "Vector3":
                        Vector3Value = (Vector3)finfo.GetValue( container );
                        break;
                    case "Vector4":
                        Vector4Value = (Vector4)finfo.GetValue( container );
                        break;
                    case "Quaternion":
                        QuaternionValue = (Quaternion)finfo.GetValue( container );
                        break;
                    case "Bounds":
                        BoundsValue = (Bounds)finfo.GetValue( container );
                        break;
                    case "Rect":
                        RectValue = (Rect)finfo.GetValue( container );
                        break;
                    case "Matrix4x4":
                        MatrixValue = (Matrix4x4)finfo.GetValue( container );
                        break;
                    case "AnimationCurve":
                        AnimationCurveValue = (AnimationCurve)finfo.GetValue( container );
                        break;
                    case "Object":
                    case "GameObject":
                        ObjectValue = (UnityEngine.Object)finfo.GetValue( container );
                        break;
                    case "Enum":
                        EnumValue = (int)finfo.GetValue( container );
                        break;
                }
            } catch ( InvalidCastException ) {
                // Ignoring the default value for this field since it cannot be converted properly
            }
        }

        public Member( MethodInfo info, Type infoType, Type type, ParameterInfo[] parameters )
            : this( info, infoType, type, EMemberType.Method ) {
            Parameters = new List<Parameter>();

            foreach ( var p in parameters ) {
                Parameters.Add( new Parameter( p ) );
            }
        }

        public void Initialize() {
            LoadType();

            if ( MemberType == EMemberType.Method ) {
                foreach ( var item in Parameters ) {
                    item.Initialize();
                }
            }
        }

        public void LoadType() {
            Type = Type.GetType( string.Format( "{0},{1}", typeName, assemblyName ) );
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
                case "GameObject":
                    return ObjectValue;
                case "Enum":
                    return Enum.Parse( Type, EnumNames[EnumValue] );
            }

            return null;
        }

        public void Invoke( GameObject item ) {
            switch ( MemberType ) {
                case EMemberType.Field:
                    InvokeField( item );
                    break;
                case EMemberType.Property:
                    InvokeProperty( item );
                    break;
                case EMemberType.Method:
                    InvokeMethod( item );
                    break;
            }
        }

        private void InvokeField( GameObject item ) {
            object value = GetValue();

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

        private void InvokeProperty( GameObject item ) {
            object value = GetValue();

            if ( parentName == "GameObject" ) {
                var type = typeof( GameObject );
                var property = type.GetProperty( Name, BindingFlags.Instance | BindingFlags.Public );
                property.SetValue( item, value, null );
            } else {
                var component = item.GetComponent( parentName );
                var componentType = component.GetType();
                var field = componentType.GetProperty( Name );
                field.SetValue( component, value, null );
            }
        }

        private void InvokeMethod( GameObject item ) {
            var parameters = new object[Parameters.Count];
            for ( int i = 0; i < Parameters.Count; i++ ) {
                var p = Parameters[i];
                parameters[i] = p.GetValue();
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

        public void CopyValue( Member other ) {
            if ( MemberType == EMemberType.Method ) {
                Parameters = other.Parameters;
            } else {
                switch ( TypeName ) {
                    case "String":
                        StringValue = other.StringValue;
                        break;
                    case "Int32":
                        IntValue = other.IntValue;
                        break;
                    case "Int64":
                        LongValue = other.LongValue;
                        break;
                    case "Single":
                        FloatValue = other.FloatValue;
                        break;
                    case "Double":
                        DoubleValue = other.DoubleValue;
                        break;
                    case "Boolean":
                        BoolValue = other.BoolValue;
                        break;
                    case "Vector2":
                        Vector2Value = other.Vector2Value;
                        break;
                    case "Vector3":
                        Vector3Value = other.Vector3Value;
                        break;
                    case "Vector4":
                        Vector4Value = other.Vector4Value;
                        break;
                    case "Quaternion":
                        QuaternionValue = other.QuaternionValue;
                        break;
                    case "Bounds":
                        BoundsValue = other.BoundsValue;
                        break;
                    case "Rect":
                        RectValue = other.RectValue;
                        break;
                    case "Matrix4x4":
                        MatrixValue = other.MatrixValue;
                        break;
                    case "AnimationCurve":
                        AnimationCurveValue = other.AnimationCurveValue;
                        break;
                    case "Object":
                    case "GameObject":
                        ObjectValue = other.ObjectValue;
                        break;
                    case "Enum":
                        EnumValue = other.EnumValue;
                        EnumNames = other.EnumNames;
                        break;
                }
            }
        }

        public override string ToString() {
            switch ( MemberType ) {
                case EMemberType.Field:
                    return string.Format( "{0}/Fields/{1} {2}", parentName, RepresentableType, Name );
                case EMemberType.Property:
                    return string.Format( "{0}/Properties/{1} {2}", parentName, RepresentableType, Name );
                case EMemberType.Method: {
                        var parameters = "";
                        foreach ( var item in Parameters ) {
                            parameters += string.Format( "{0}, ", item.ToString() );
                        }
                        parameters = parameters.TrimEnd( ',', ' ' );

                        return string.Format( "{2}/Methods/{0} ({1})", Name, parameters, parentName );
                    }
            }

            return base.ToString();
        }
    }

    [Serializable]
    public class GameObjectContainer {
        public GameObject GameObject;

        public List<Member> Members = new List<Member>();

        public GUIContent[] List = new GUIContent[0];
        public int Index = 0;

        public GameObjectContainer() { }

        public GameObjectContainer( GameObject gObj ) {
            Reset( gObj );
        }

        public void Initialize() {
            foreach ( var item in Members ) {
                item.Initialize();
            }

            if ( Index > 1 ) {
                var tempText = List[Index].text;
                var tempMember = Members[Index];

                Reset();

                for ( int i = 0; i < Members.Count; i++ ) {
                    var item = Members[i];
                    if ( item == null ) continue;
                    if ( item.ToString() == tempText ) {
                        Index = i;
                        item.CopyValue( tempMember );
                        break;
                    }
                }
            }
        }

        public void Reset( GameObject gObj ) {
            GameObject = gObj;
            Reset();
        }

        public void Reset() {
            List = new GUIContent[] { new GUIContent( "Nothing Selected" ), new GUIContent( "" ) };
            Index = 0;
            Members.Clear();
            // Add two null objects to fill in the blanks for "Nothing Selected" and ""
            Members.Add( null );
            Members.Add( null );

            if ( GameObject == null ) return;
            var tempList = new List<GUIContent>();

            // "Force add" GameObject item
            ReadObject( typeof( GameObject ), ref tempList );

            var components = GameObject.GetComponents<Component>();
            foreach ( var cmp in components ) {
                ReadObject( cmp.GetType(), ref tempList );
            }

            tempList.Insert( 0, new GUIContent( "" ) );
            tempList.Insert( 0, new GUIContent( "Nothing Selected" ) );
            List = tempList.ToArray();
        }

        private void ReadObject( Type type, ref List<GUIContent> tempList ) {
            var obsoleteType = typeof( ObsoleteAttribute );
            var objectType = typeof( UnityEngine.Object );
            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

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

            UnityEngine.Object obj = type == typeof( GameObject ) ? (UnityEngine.Object)GameObject : GameObject.GetComponent( type );
            foreach ( var f in fields ) {
                var field = new Member( f, f.FieldType, type, obj );
                tempList.Add( new GUIContent( field.ToString() ) );
                Members.Add( field );
            }

            foreach ( var p in properties ) {
                var property = new Member( p, p.PropertyType, type, EMemberType.Property );
                tempList.Add( new GUIContent( property.ToString() ) );
                Members.Add( property );
            }

            foreach ( var m in methods ) {
                var method = new Member( m, m.ReflectedType, type, m.GetParameters() );
                tempList.Add( new GUIContent( method.ToString() ) );
                Members.Add( method );
            }
        }

        public void Invoke() {
            if ( Index < 2 ) return;

            Members[Index].Invoke( GameObject );
        }

        public EMemberType Type {
            get {
                var splits = List[Index].text.Split( '/' );
                switch ( splits[1] ) {
                    case "Fields":
                        return EMemberType.Field;
                    case "Properties":
                        return EMemberType.Property;
                    case "Methods":
                        return EMemberType.Method;
                }

                return EMemberType.None;
            }
        }
    }

    public List<GameObjectContainer> Listeners = new List<GameObjectContainer>();

    public void Invoke() {
        foreach ( var item in Listeners ) {
            item.Invoke();
        }
    }

    public void Find( GameObject gameObject, string memberName, params object[] parameters ) {
        var container = new GameObjectContainer( gameObject );

        for ( int i = 0; i < container.Members.Count; i++ ) {
            var member = container.Members[i];

            if ( member == null ) continue;
            if ( member.Name != memberName ) continue;
            if ( member.Parameters.Count != parameters.Length ) continue;

            var foundMember = true;

            foreach ( var mParam in member.Parameters ) {
                var foundParameter = false;
                foreach ( var pParam in parameters ) {
                    var t = pParam.GetType();
                    if ( mParam.Type == t || t.IsSubclassOf( mParam.Type ) ) {
                        foundParameter = true;
                        break;
                    }
                }

                if ( !foundParameter ) {
                    foundMember = false;
                    break;
                }
            }

            if ( !foundMember ) continue;

            container.Index = i;

            foreach ( var mParam in member.Parameters ) {
                foreach ( var pParam in parameters ) {
                    if ( mParam.Type != pParam.GetType() ) continue;

                    switch ( mParam.TypeName ) {
                        case "String":
                            mParam.StringValue = (string)pParam;
                            break;
                        case "Int32":
                            mParam.IntValue = (int)pParam;
                            break;
                        case "Int64":
                            mParam.LongValue = (long)pParam;
                            break;
                        case "Single":
                            mParam.FloatValue = (float)pParam;
                            break;
                        case "Double":
                            mParam.DoubleValue = (double)pParam;
                            break;
                        case "Boolean":
                            mParam.BoolValue = (bool)pParam;
                            break;
                        case "Vector2":
                            mParam.Vector2Value = (Vector2)pParam;
                            break;
                        case "Vector3":
                            mParam.Vector3Value = (Vector3)pParam;
                            break;
                        case "Vector4":
                            mParam.Vector4Value = (Vector4)pParam;
                            break;
                        case "Quaternion":
                            mParam.QuaternionValue = (Quaternion)pParam;
                            break;
                        case "Bounds":
                            mParam.BoundsValue = (Bounds)pParam;
                            break;
                        case "Rect":
                            mParam.RectValue = (Rect)pParam;
                            break;
                        case "Matrix4x4":
                            mParam.MatrixValue = (Matrix4x4)pParam;
                            break;
                        case "AnimationCurve":
                            mParam.AnimationCurveValue = (AnimationCurve)pParam;
                            break;
                        case "Object":
                        case "GameObject":
                            mParam.ObjectValue = (UnityEngine.Object)pParam;
                            break;
                        case "Enum":
                            for ( int j = 0; j < mParam.EnumNames.Length; j++ ) {
                                if ( mParam.EnumNames[j] == pParam.ToString() ) {
                                    mParam.EnumValue = j;
                                    break;
                                }
                            }
                            break;
                        default:
                            Debug.LogErrorFormat( "The type {0} is not supported", mParam.Type.Name );
                            break;
                    }
                }
            }

            Listeners.Add( container );

            break;
        }
    }
}
