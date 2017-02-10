using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class PresenterController : MonoBehaviour {
    public GameObject Presenter;
    [SerializeField]
    Camera Cam;
    [SerializeField]
    FaceController Face;
    [SerializeField]
    RootMotion.FinalIK.FBBIKHeadEffector Head;
    [SerializeField]
    RootMotion.FinalIK.FullBodyBipedIK FullBody;
    RootMotion.FinalIK.BipedIK BipedIK;
    // Use this for initialization
    void Start() {




    }
    void turnOff() {

        Cam.enabled = false;
        Face.enabled = false;
 
    }
    public void GetComponents() {
        Cam = gameObject.GetComponentInChildren<Camera>(true);
        Face = gameObject.GetComponentInChildren<FaceController>(true);
        Head = gameObject.GetComponentInChildren<RootMotion.FinalIK.FBBIKHeadEffector>(true);
        FullBody = gameObject.GetComponentInChildren<RootMotion.FinalIK.FullBodyBipedIK>(true);
        BipedIK = gameObject.GetComponentInChildren<RootMotion.FinalIK.BipedIK>(true);
        turnOff();
    }



    // Update is called once per frame
    void Update() {

    }

    void TurnOn() {
        Presenter.SetActive(true);
        FullBody.enabled = true;
        BipedIK.enabled = true;
        Cam.enabled = true;
        Face.enabled = true;
        Head.enabled = true;


        Debug.Log("done");
    }

    public void Create() {
        Presenter.SetActive(true);
        if (Head == null) {
            GetComponents();
        }
        TurnOn();



    }








}
