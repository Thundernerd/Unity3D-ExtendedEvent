using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class ExtendedEvent {

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
