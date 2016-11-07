using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class ExtendedEvent {

    [Serializable]
    public class Container {
        public UnityEngine.Object Target;
        public int Index;
        public MemberInfo Info = new MemberInfo();
        public List<Value> Values = new List<Value>();
    }

    [Serializable]
    public class Value {
        public AnimationCurve animationCurveValue;
        public bool boolValue;
        public Bounds boundsValue;
        public Color colorValue;
        public double doubleValue;
        public Enum enumValue;
        public float floatValue;
        public int intValue;
        public long longValue;
        public UnityEngine.Object objectReferenceValue;
        public Quaternion quaternionValue;
        public Rect rectValue;
        public string stringValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public Vector4 vector4Value;
    }

    [Serializable]
    public class MemberInfo {
        public int Type;
        public string DeclaringType;
        public string Name;
        public int Parameters;
        public string[] ParameterTypes;
    }

    private List<Action> callbacks = new List<Action>();

    [SerializeField]
    private List<Container> persistentCallbacks;

    public void AddListener( Action callback ) {
        callbacks.Add( callback );
    }

    public void RemoveAllListeners() {
        callbacks.Clear();
    }

    public void RemoveListener( Action callback ) {
        callbacks.Remove( callback );
    }

    public void Invoke() {

    }
}
