using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerScripts : NetworkBehaviour {

    public Camera maincamera;
    public GameObject Presenter;
    public GameObject Viewer;
    NetworkClient myClient;
    public Transform otherPlayerM, otherPlayerF;
    public bool OtherIsViewer;
    public LayerMask playerTwoMask;
    public PlayerScripts Player2;


    void Start() {

    }
    [ClientRpc]
    public void RpcShowOtherPlayer() {
        Debug.Log("PRCShow");
        maincamera.cullingMask = playerTwoMask;
    }
    public void ShowOtherPlayer() {
        CmdShowOtherPlayer();
    }
    [Command]
    public void CmdShowOtherPlayer() {
        Debug.Log("CMDShow");
        maincamera.cullingMask = playerTwoMask;
        RpcShowOtherPlayer();
    }

    public override void OnStartLocalPlayer() {

        maincamera.enabled = true;
        transform.name = "LOCAL PLAYER";
        maincamera.name = "PlayerCamera";

        if (isServer) {

            Presenter.SetActive(true);
            var head = Presenter.GetComponentInChildren<RootMotion.FinalIK.FBBIKHeadEffector>();
            maincamera.transform.SetParent(head.transform.parent);
            maincamera.transform.localPosition = Vector3.zero;
            OtherIsViewer = true;
            //   maincamera.cullingMask = playerTwoMask;
        }
        else {
            var head = Viewer.GetComponentInChildren<RootMotion.FinalIK.FBBIKHeadEffector>(true);
            maincamera.transform.SetParent(head.transform.parent);
            maincamera.transform.localPosition = Vector3.zero;


            Viewer.SetActive(true);


            OtherIsViewer = false;
        }

        this.enabled = true;

        otherPlayerF.gameObject.SetActive(false);
        otherPlayerM.gameObject.SetActive(false);
        maincamera.transform.parent = null;

        base.OnStartLocalPlayer();
    }

    void Update() {
        if (Player2 == null && isLocalPlayer) {
            var otherplayer = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject thing in otherplayer) {
                if (thing != otherPlayerM && thing != otherPlayerF && thing != this.gameObject)
                    Player2 = thing.GetComponent<PlayerScripts>();
            }


        }

    }
    public void IsGirl(bool on) {
        CmdIsGirl(on);
    }

    [Command]
    public void CmdIsGirl(bool on) {
        if (OtherIsViewer)
            return;



        if (!isLocalPlayer) {

            otherPlayerM.gameObject.SetActive(!on);
            otherPlayerF.gameObject.SetActive(on);
            RpcIsGirl(on);
        }

        maincamera.cullingMask = playerTwoMask;
        Camera.main.cullingMask = playerTwoMask;
    }


    [ClientRpc]
    public void RpcIsGirl(bool on) {

        if (isLocalPlayer)
            return;
        otherPlayerM.gameObject.SetActive(!on);
        otherPlayerF.gameObject.SetActive(on);


        Camera.main.cullingMask = playerTwoMask;



    }




    //[Command]
    //public void CmdShowPresenter() {
    //    var myItem = Instantiate(Presenter);

    //    NetworkServer.Spawn(Presenter);
    //}

    //[Command]
    //public void CmdShowViewer() {
    //    var myItem = Instantiate(Viewer);
    //    NetworkServer.Spawn(Viewer);
    //}
    //[ClientRpc]
    //public void RpcShowViewer() {
    //    var myItem = Instantiate(Viewer);
    //    NetworkServer.Spawn(Viewer);
    //}

    //[ClientRpc]
    //public void RpcShowPresenter() {

    //    var myItem = Instantiate(Presenter);
    //    NetworkServer.Spawn(Presenter);
    //}

}
