using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExtendedEvent : ISerializationCallbackReceiver {

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

    public void OnBeforeSerialize() {
        
    }

    public void OnAfterDeserialize() {
        
    }
}
