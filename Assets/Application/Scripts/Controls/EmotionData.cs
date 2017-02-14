using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class EmotionData : MonoBehaviour {
    public enum EmotionEnum {
		neutral = -1, 
		smile = 10, 
		anger = 16, 
		contempt = 13, 
		disgust = 14, 
		surprise = 11, 
		fear = 15, 
		sadness = 12,

		big_wide_smile = 5,
		closed_smile = 3,
		eyebrows_only = 4,
		left_teeth = 0,
		mouth_only = 6,
		open_wide = 7,
		pursed_lips =8,
		right_teeth = 1,
		smile_and_eyebrow = 9,
		teethview = 2,
    }

	[SerializeField] private SkinnedMeshRenderer _faceRenderer;

	private EmotionEnum[] _emotionsList = (EmotionEnum[]) Enum.GetValues ( typeof ( EmotionEnum ) );

    public static EmotionEnum CurrentEmotionState;
	//Dictionary<EmotionEmum, CoreBlendshapeGroup> emotionDictionary = new Dictionary<EmotionEmum, CoreBlendshapeGroup>();

	//M3DCharacterManager m_CharacterManager;

    private Coroutine StopFaceCoroutine;
    //protected CoreBlendshapeGroup CurrentEmotion;

    IEnumerator Start() {
		/*CurrentEmotionState = EmotionEmum.neutral;

        m_CharacterManager = GetComponent<M3DCharacterManager>();
        if (m_CharacterManager == null) {
            Debug.LogError("Character Manager not found on this object.");
            yield break;
        }

        Init();*/
        yield break;
    }

    public void Init() {
        /*foreach (CoreBlendshapeGroup Emotion in m_CharacterManager.GetAllBlendshapeGroups()) 
		{
			try
			{
				EmotionEmum groupName = ParseEnum<EmotionEmum>(Emotion.groupName);
				emotionDictionary.Add(groupName, Emotion);
				//Debug.Log (groupName.ToString());
			}
			catch (Exception e)
			{
			}
        }

		//+1 is for EmotionEmum.neutral state, which does not defined in BlendShapes
		if (emotionDictionary.Count+1 != Enum.GetValues (typeof(EmotionEmum)).Length) {
			Debug.LogError ("Some BlendShapes emotions is missing");			
		}*/
    }

    private static T ParseEnum<T>(string value) {
        return (T) Enum.Parse(typeof(T), value, true);
    }

	public bool IsEmotion(string name) {
		/*CoreBlendshapeGroup To = null;

		try
		{
			return emotionDictionary.TryGetValue(ParseEnum<EmotionEmum>(name), out To);
		}
		catch (Exception e) {
			Debug.LogError ("Emotion " + name + " not found!");
			return false;
		}*/

		//remove
		return false;
	}

	public bool IsEmotion(EmotionEnum name) {
		/*CoreBlendshapeGroup To = null;
		return emotionDictionary.TryGetValue(name, out To);*/
		return false;
	}

	//-------------This is DEBUG-Only right now------------------
	public void StartChangeFace(int ToEmotionIndex, float emotionWeight, float speed) {
		try
		{
			StartChangeFace ((EmotionEnum)ToEmotionIndex, emotionWeight, speed);
		}
		catch (Exception e) {
			Debug.LogError ("Emotion index " + ToEmotionIndex + " not found!");
		}
	}
	//-----------------------------------------------------------

	public void StartChangeFace(string ToEmotionStr, float emotionWeight, float speed) {
		try
		{
			StartChangeFace (ParseEnum<EmotionEnum>(ToEmotionStr), emotionWeight, speed);
		}
		catch (Exception e) {
			Debug.LogError ("Emotion " + ToEmotionStr + " not found!");
		}
	}

	private void StartChangeFace(EmotionEnum ToEmotion, float emotionWeight, float speed) 
	{
		//Debug.Log ("-------" + ToEmotion.ToString() + "-------");

		/*
		if (ToEmotion == EmotionEnum.neutral) {
			StartResetFace (.4f);
			return;
		}
		*/

		ChangeFaceAnimate( ToEmotion, emotionWeight, speed );

        /*CoreBlendshapeGroup To = null;
        emotionDictionary.TryGetValue(ToEmotion, out To);
        if (To == null)
            return;

        if (StopFaceCoroutine != null || !gameObject.activeInHierarchy) {
            StopCoroutine(StopFaceCoroutine);
        }
        CurrentEmotion = To;
        StartCoroutine(ChangeToFace(To, speed));*/
    }

	void ChangeFaceAnimate ( EmotionEnum emotion, float weight, float speed ) {

		foreach ( var emo in _emotionsList ) {
			var indexBlendShape = ( int )emo;
			var targetWeight = emo == emotion ? weight : 0;

			targetWeight = emotion == EmotionEnum.neutral ? 0 : targetWeight;

			var currentWeight =  _faceRenderer.GetBlendShapeWeight( indexBlendShape );
			if ( currentWeight == targetWeight ) continue;

			DOTween.To( 
				()=> _faceRenderer.GetBlendShapeWeight( indexBlendShape )
				, x=> _faceRenderer.SetBlendShapeWeight( indexBlendShape, x ), targetWeight, speed );
		}

	}

    public void StartResetFace(float time = 2) {
        StartCoroutine(ResetFace(time));
    }

    IEnumerator ResetFace(float time = 2) {
        /*foreach (CoreBlendshapeGroup Emotion in m_CharacterManager.GetAllBlendshapeGroups()) {
            StartCoroutine(SetEmotionValueOff(Emotion, Emotion.groupValue, Time.time, time));
            yield return new WaitForEndOfFrame();
        }*/

		//remove
		yield return null;
    }

    /*IEnumerator ChangeToFace(CoreBlendshapeGroup To, float time = 2) {

        print("TurnOffAll");
        foreach (CoreBlendshapeGroup Emotion in m_CharacterManager.GetAllBlendshapeGroups()) {
            StartCoroutine(SetEmotionValueOff(Emotion, Emotion.groupValue, Time.time, time));
            yield return new WaitForEndOfFrame();
        }
        print("new Emotion");

        StopFaceCoroutine = StartCoroutine(SetEmotionValueOn(To, To.groupValue, Time.time, time));
        StopFaceCoroutine = null;


        yield return 0;
    }

    IEnumerator SetEmotionValueOff(CoreBlendshapeGroup data, float currentValue, float StartTime, float Speed = 2) {

        while (data.groupValue > 0) {

            float value = Mathf.Lerp(currentValue, 0, ( Time.time - StartTime ) / Speed);
            m_CharacterManager.SetBlendshapeGroupValue(data.groupName, value);
            yield return null;
        }
    }

    IEnumerator SetEmotionValueOn(CoreBlendshapeGroup data, float currentValue, float StartTime, float Speed = 2) {

        while (data.groupValue < 100) {

            float value = Mathf.Lerp(currentValue, 100, ( Time.time - StartTime ) / Speed);
            m_CharacterManager.SetBlendshapeGroupValue(data.groupName, value);

            yield return null;
        }

		CurrentEmotionState = ParseEnum<EmotionEmum>(data.groupName.ToLower());
    }*/
}

// 2560 x 1440
// 