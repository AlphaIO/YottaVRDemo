using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

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

	private const float defaultFullWidth = 100.0f;
	private const float defaultMinWidth = 1.0f;
	private const float barHeight = 24f;

	private const float animSpeed = .4f;

    public void SetEmotion(string[] emotion) {
        //webAPI.AddState(emotion);
        //SlideShow.SetEmotionCountText(emotion);

		//ToDo This is temporary here
		//Will be changed soon
		float emotionLevelWidth = float.Parse (emotion [1]) / 10 * defaultFullWidth;

		var emotionBars = new Image[] {
			neutralBar, surpriseBar, smileBar, contemptBar, disgustBar, sadnessBar, angerBar
		};

		foreach ( var bar in emotionBars ) {
			if ( bar.rectTransform.sizeDelta.x == 0 ) continue;
			var targetBar = GetBarImage ( emotion [0] );

			var animVector = targetBar == bar 
				? new Vector2 (emotionLevelWidth, barHeight)
				: new Vector2 (defaultMinWidth, barHeight);
			bar.rectTransform.DOSizeDelta( animVector, animSpeed );
		}

		Debug.Log( emotion[0] + "   " + emotionLevelWidth );
		StartChangeFace (emotion[0], emotionLevelWidth, .4f);
    }

	Image GetBarImage ( string emotion ) {
		switch ( emotion )
		{
		case "neutral":		return neutralBar;
		case "smile":			return smileBar;
		case "surprise":		return surpriseBar;
		case "contempt":		return contemptBar;
		case "anger":			return angerBar;
		case "disgust":		return disgustBar;
		case "sadness":		return sadnessBar;
		}
		return null;
	}
}
