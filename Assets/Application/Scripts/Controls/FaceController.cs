using UnityEngine;
using System.Collections;
using MORPH3D;
using MORPH3D.FOUNDATIONS;
using System.Collections.Generic;

using RootMotion.FinalIK;
public class FaceController : EmotionData {
    public WebApiClient webAPI;
    [SerializeField]
    Presentation SlideShow;

    public void SetEmotion(string emotion) {
        webAPI.AddState(emotion);
        SlideShow.SetEmotionCountText(emotion);
		StartChangeFace (emotion, .4f);
    }

    public void SetFace(string feeling) {
        SetEmotion(feeling);
    }
}
