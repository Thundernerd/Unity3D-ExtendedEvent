#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace TNRD.ExtendedEvent {

    public class ParameterWizardValue {
        public ParameterInfo[] Parameters;
        public List<SerializedObject> Objects;
        public string Path;
    }
}

#endif