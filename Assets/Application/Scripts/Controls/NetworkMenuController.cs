using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
public class NetworkMenuController : NetworkBehaviour {

    public NetworkBasicControls Viewer, Presenter;

    public Canvas canvas;
    // Use this for initialization

    public GameObject StartPanel;

    void Start() {
        Viewer = GameObject.FindGameObjectWithTag("Viewer").GetComponent<NetworkBasicControls>();
        Presenter = GameObject.FindGameObjectWithTag("Presenter").GetComponent<NetworkBasicControls>();
        canvas = GetComponentInChildren<Canvas>(true);

    }
    [Command]
    public void CmdTurnOnPlayer() {


        Viewer.RpcTurnOnObject();


    }
    public override void OnStartLocalPlayer() {
        canvas.enabled = true;
        base.OnStartLocalPlayer();
    }


    [Command]
    public void CmdTurnOnPresenter() {
        Presenter.RpcTurnOnObject();
    }
    public void TurnOnPlayerCam() {
        canvas.enabled = false;
        Presenter.TurnOnCamera();
    }

    public void TurnOnPresenterCam() {
        StartPanel.SetActive(false);
        Viewer.TurnOnCamera();
    }


    public void TurnOffWaitingPanel() {
        canvas.enabled = false;
    }

    public void TurnOffStartPanel() {
        StartPanel.SetActive(false);
    }
}
