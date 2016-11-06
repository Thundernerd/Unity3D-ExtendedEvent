#if UNITY_EDITOR
using UnityEngine;

namespace TNRD.ExtendedEvent {

    public class FakeObject : ScriptableObject {
        public global::ExtendedEvent.Value Value = new global::ExtendedEvent.Value();
    }
}

#endif