#if UNITY_EDITOR
using UnityEditor;

namespace TNRD.ExtendedEvent {

    public class PropertyWizardValue {
        public SerializedPropertyType Type;
        public string Path;
        public object Value;
    }
}
#endif