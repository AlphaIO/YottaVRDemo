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

    // Update is called once per frame
    void Update() {
		/*if (NetworkPlayerCtrl.networkPlayerAuthority != null)
                NetworkPlayerCtrl.networkPlayerAuthority.SetExpression("Joy");*/
		
		if (Input.GetKeyDown (KeyCode.Alpha1))
			StartChangeFace (1, .4f);
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			StartChangeFace (2, .4f);
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			StartChangeFace (3, .4f);
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			StartChangeFace (4, .4f);
		else if (Input.GetKeyDown(KeyCode.Alpha5))
			StartChangeFace (5, .4f);
		else if (Input.GetKeyDown(KeyCode.Alpha6))
			StartChangeFace (6, .4f);
		else if (Input.GetKeyDown(KeyCode.Alpha7))
			StartChangeFace (7, .4f);
		else if (Input.GetKeyDown(KeyCode.Alpha8))
			StartChangeFace (8, .4f);
    }

}
