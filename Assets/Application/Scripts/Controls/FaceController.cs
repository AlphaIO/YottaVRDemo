using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MORPH3D;
using MORPH3D.FOUNDATIONS;
using System.Collections.Generic;

using RootMotion.FinalIK;
public class FaceController : EmotionData {
    public WebApiClient webAPI;
    [SerializeField]
    Presentation SlideShow;

	public Image neutralBar;
	public Image surpriseBar;
	public Image smileBar;
	public Image contemptBar;
	public Image disgustBar;
	public Image sadnessBar;
	public Image angerBar;

	private const float defaultFullWidth = 140.0f;
	private const float defaultMinWidth = 1.0f;

    public void SetEmotion(string[] emotion) {
        //webAPI.AddState(emotion);
        //SlideShow.SetEmotionCountText(emotion);

		//ToDo This is temporary here
		//Will be changed soon
		neutralBar.rectTransform.sizeDelta = new Vector2 (defaultMinWidth, 24);
		surpriseBar.rectTransform.sizeDelta = new Vector2 (defaultMinWidth, 24);
		smileBar.rectTransform.sizeDelta = new Vector2 (defaultMinWidth, 24);
		contemptBar.rectTransform.sizeDelta = new Vector2 (defaultMinWidth, 24);
		disgustBar.rectTransform.sizeDelta = new Vector2 (defaultMinWidth, 24);
		sadnessBar.rectTransform.sizeDelta = new Vector2 (defaultMinWidth, 24);
		angerBar.rectTransform.sizeDelta = new Vector2 (defaultMinWidth, 24);

		float emotionLevelWidth = float.Parse (emotion [1]) / 10 * defaultFullWidth;

		switch (emotion[0])
		{
			case "neutral":
				neutralBar.rectTransform.sizeDelta = new Vector2 (emotionLevelWidth, 24);
				break;
			case "smile":
				smileBar.rectTransform.sizeDelta = new Vector2 (emotionLevelWidth, 24);
				break;
			case "surprise":
				surpriseBar.rectTransform.sizeDelta = new Vector2 (emotionLevelWidth, 24);
				break;
			case "contempt":
				contemptBar.rectTransform.sizeDelta = new Vector2 (emotionLevelWidth, 24);
				break;
			case "anger":
				angerBar.rectTransform.sizeDelta = new Vector2 (emotionLevelWidth, 24);
				break;
			case "disgust":
				disgustBar.rectTransform.sizeDelta = new Vector2 (emotionLevelWidth, 24);
				break;
			case "sadness":
				sadnessBar.rectTransform.sizeDelta = new Vector2 (emotionLevelWidth, 24);
				break;
		}

		StartChangeFace (emotion[0], .4f);
    }
}
