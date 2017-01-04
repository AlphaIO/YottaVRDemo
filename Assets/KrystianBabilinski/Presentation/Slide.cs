using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slide : MonoBehaviour {

    public string url;
    public MediaPlayerCtrl videoManager;
    private RawImage myImage;

    void Start() {
        myImage = GetComponent<RawImage>();
    }


    void OnEnable() {

        if (videoManager) {
            videoManager.Load(url);
            videoManager.Play();
        }
    }

    void OnDisable() {

        if (videoManager) {
            videoManager.Stop();
            

        }
    }
}
