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
    }

    [Serializable]
    public class MemberInfo {
        public int Type;
        public string Name;
        public int Parameters;
        public string[] ParameterTypes;
    }

    private List<Action> callbacks = new List<Action>();

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
