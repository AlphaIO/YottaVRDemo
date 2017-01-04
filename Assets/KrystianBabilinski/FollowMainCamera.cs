using UnityEngine;
using System.Collections;

public class FollowMainCamera : MonoBehaviour {

    public Transform PlayerCamera;
    public Vector3 offsetRotation;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (PlayerCamera)
            transform.localEulerAngles = PlayerCamera.transform.localEulerAngles;
    }
}
