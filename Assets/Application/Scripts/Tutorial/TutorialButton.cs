using UnityEngine;
using System.Collections;
using YottaIO.Tools;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour {

    [SerializeField]
    private EmotionData.EmotionEmum assignedEmotion;
    private Toggle checkMark;
    private bool isComplete;

    void Start() {
        TutorialData.AddEmotion();
        checkMark = GetComponent<Toggle>();
        checkMark.isOn = false;
    }

    void Update() {


        if (FaceController.CurrentEmotionState == assignedEmotion && isComplete != true) {

            TutorialData.EmotionComplete();
            checkMark.isOn = true;
            isComplete = true;
            return;

        }

    }

}
