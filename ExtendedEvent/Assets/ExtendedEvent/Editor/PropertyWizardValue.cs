#if UNITY_EDITOR
using System;
using UnityEditor;

namespace TNRD.ExtendedEvent {

    public class PropertyWizardValue {
        public Type Type;
        public SerializedObject Object;
        public string Path;
    }
}
#endif