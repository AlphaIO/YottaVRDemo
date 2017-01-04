using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class NetworkBasicControls : NetworkBehaviour {
    public GameObject ChildObject;
    Camera[] cam;
    RootMotion.FinalIK.FBBIKHeadEffector[] head;

    // Use this for initialization
    void Awake() {
        cam = GetComponentsInChildren<Camera>(true);
        head = GetComponentsInChildren<RootMotion.FinalIK.FBBIKHeadEffector>(true);

    }
    [ClientRpc]
    public void RpcTurnOnObject() {
        ChildObject.SetActive(true);
    }

    public void TurnOnCamera() {

        for (int i = 0; i < cam.Length; i++)
            cam[i].enabled = true;



        for (int i = 0; i < head.Length; i++)
            head[i].enabled = true;



    }
    // Update is called once per frame
    void Update() {

    }
}
