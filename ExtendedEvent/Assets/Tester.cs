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

    public void doit( Transform trans, Rect rect, Bounds b ) {
        Debug.LogError( trans );
        Debug.LogError( rect );
        Debug.LogError( b );
    }

    public void Doti( int a, Matrix4x4 b, float c, Matrix4x4 d, double e ) {

    }
}
