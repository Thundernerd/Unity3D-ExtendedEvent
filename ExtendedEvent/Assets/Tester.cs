using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {

    public int AnInt;
    public Rect rect;
    public Bounds boundsAboundsBounds;
    public ExtendedEvent Event;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.Space ) ) {
            Event.Invoke();
        }
    }
}
