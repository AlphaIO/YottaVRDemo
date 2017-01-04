using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LocalPlayerCtrl : NetworkBehaviour {

    public Camera mainCamera;
    public GameObject Presenter;
    public GameObject Viewer;
    public LayerMask playerTwoMask;

    public static LocalPlayerCtrl thisLocalPlayer;

    [SyncVar]
    public bool isNetworkMale;

    [SyncVar]
    public bool tutorialIsFinished;
    public static bool SceneTutorialFinished;




    public override void OnStartLocalPlayer() {

        SceneTutorialFinished = false;
        mainCamera.enabled = true;

        if (isLocalPlayer) {
            thisLocalPlayer = this;
            NetworkPlayerCtrl.networkPlayerAuthority = GetComponent<NetworkPlayerCtrl>();
        }

        transform.name = "LOCAL PLAYER";
        mainCamera.name = "PlayerCamera";
        GetComponent<NetworkPlayerCtrl>().enabled = false;
        this.enabled = true;

        if (isServer) {

            Presenter.SetActive(true);
            Viewer.SetActive(false);
        }
        else {

            Presenter.SetActive(false);
            Viewer.SetActive(true);
            Invoke("TestThis", 5);
            print("Starting Invoke");


        }


    }



    void TestThis() {
        CmdSetTutorialState();
        print("Setting Bool");
    }

    [Command]
    public void CmdSetTutorialState() {
        tutorialIsFinished = true;
        SceneTutorialFinished = tutorialIsFinished;
    }



    void Update() {

        if (SceneTutorialFinished)
            mainCamera.cullingMask = playerTwoMask;
    }


    public void SetGender(bool isMale) {
        CmdSetGender(isMale);
    }
    [Command]
    public void CmdSetGender(bool isMale) {
        isNetworkMale = isMale;
        NetworkPlayerCtrl.isMale = isNetworkMale;
    }



}
