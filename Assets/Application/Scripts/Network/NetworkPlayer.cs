using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class NetworkPlayer : NetworkBehaviour {
    public GameObject p1, p2;
    // Use this for initialization


    void Update() {
        if (Input.GetKeyDown(KeyCode.W) && isLocalPlayer)
            transform.Translate(transform.forward);

    }


    public void Create(GameObject spawn) {

        spawn.GetComponent<PresenterController>().Create();
    }

    public override void OnStartLocalPlayer() {
        p1.SetActive(true);
        Create(p1);
        base.OnStartLocalPlayer();
    }


}
