using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

using System;

public class NetworkPlayerCtrl : NetworkBehaviour {

    public Transform otherPlayerM, otherPlayerF;
    public static bool isMale;
    public LocalPlayerCtrl localCtrl;
    public LocalPlayerCtrl myLocalPlayer;

    public static NetworkPlayerCtrl currentNetworkPlayer;
    public static NetworkPlayerCtrl networkPlayerAuthority;

    public static FaceController currentFaceController;


    [SyncVar]
	public string ViewerEmmotion;
    [SyncVar]
	public string PresenterEmotion;

    void Start() {

        if (!isLocalPlayer)
            currentNetworkPlayer = this;
        else
            this.enabled = false;

        myLocalPlayer = GetComponent<LocalPlayerCtrl>();
        if (isLocalPlayer)
            this.enabled = false;
    }



    public void SetExpression(string Expression) {


        if (isServer)
            CmdExpressionPresentor(Expression);
        else
            CmdExpressionViewer(Expression);
    }

    [Command]
    public void CmdExpressionViewer(string Expression) {
		ViewerEmmotion = Expression;
        ExpresionAction();
    }

    [Command]
    public void CmdExpressionPresentor(string Expression) {
		PresenterEmotion = Expression;
        ExpresionAction();
    }

    public void ExpresionAction() {
        if (currentFaceController == null )
            return;

        if (isServer)
            currentFaceController.StartChangeFace(ViewerEmmotion, .3f);
        else
            currentFaceController.StartChangeFace(PresenterEmotion, .3f);

    }


    void Update() {
        if (isLocalPlayer)
            return;


        if (( localCtrl == null || myLocalPlayer == localCtrl ) && isServer) {
            FindLocalPlayer();

        }



        if (isMale) {
            SetMale();
        }
        else {
            SetFemale();
        }



    }






    void FindLocalPlayer() {
        LocalPlayerCtrl[] playersInScene = FindObjectsOfType<LocalPlayerCtrl>();
        foreach (LocalPlayerCtrl player in playersInScene) {
            if (player != myLocalPlayer)
                localCtrl = player;
        }
    }


    void SetMale() {
        otherPlayerM.gameObject.SetActive(true);
        otherPlayerF.gameObject.SetActive(false);
        currentFaceController = otherPlayerM.GetComponentInChildren<FaceController>();
    }

    void SetFemale() {
        otherPlayerF.gameObject.SetActive(true);
        otherPlayerM.gameObject.SetActive(false);
        currentFaceController = otherPlayerF.GetComponentInChildren<FaceController>();
    }



    void OnDisable() {
        otherPlayerF.gameObject.SetActive(false);
        otherPlayerM.gameObject.SetActive(false);
    }
}
