using UnityEngine;
using System.Collections;
using MORPH3D;
using MORPH3D.FOUNDATIONS;
using System.Collections.Generic;
using System;

public class EmotionData : MonoBehaviour {
    public enum EmotionEmum {
		neutral = 1, 
		smile = 2, 
		anger = 3, 
		contempt = 4, 
		disgust = 5, 
		surprise = 6, 
		fear = 7, 
		sadness = 8
    }

    public static EmotionEmum CurrentEmotionState;
	Dictionary<EmotionEmum, CoreBlendshapeGroup> emotionDictionary = new Dictionary<EmotionEmum, CoreBlendshapeGroup>();

	M3DCharacterManager m_CharacterManager;

    private Coroutine StopFaceCoroutine;
    protected CoreBlendshapeGroup CurrentEmotion;

    IEnumerator Start() {
		CurrentEmotionState = EmotionEmum.neutral;

        m_CharacterManager = GetComponent<M3DCharacterManager>();
        if (m_CharacterManager == null) {
            Debug.LogError("Character Manager not found on this object.");
            yield break;
        }

        Init();
        yield break;
    }

    public void Init() {
        foreach (CoreBlendshapeGroup Emotion in m_CharacterManager.GetAllBlendshapeGroups()) 
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
		}
    }

    private static T ParseEnum<T>(string value) {
        return (T) Enum.Parse(typeof(T), value, true);
    }

	public bool IsEmotion(string name) {
		CoreBlendshapeGroup To = null;

		try
		{
			return emotionDictionary.TryGetValue(ParseEnum<EmotionEmum>(name), out To);
		}
		catch (Exception e) {
			Debug.LogError ("Emotion " + name + " not found!");
			return false;
		}
	}

	public bool IsEmotion(EmotionEmum name) {
		CoreBlendshapeGroup To = null;
		return emotionDictionary.TryGetValue(name, out To);
	}

	//-------------This is DEBUG-Only right now------------------
	public void StartChangeFace(int ToEmotionIndex, float speed) {
		try
		{
			StartChangeFace ((EmotionEmum)ToEmotionIndex, speed);
		}
		catch (Exception e) {
			Debug.LogError ("Emotion index " + ToEmotionIndex + " not found!");
		}
	}
	//-----------------------------------------------------------

	public void StartChangeFace(string ToEmotionStr, float speed) {
		try
		{
			StartChangeFace (ParseEnum<EmotionEmum>(ToEmotionStr), speed);
		}
		catch (Exception e) {
			Debug.LogError ("Emotion " + ToEmotionStr + " not found!");
		}
	}

	private void StartChangeFace(EmotionEmum ToEmotion, float speed) 
	{
		if (ToEmotion == EmotionEmum.neutral) {
			StartResetFace (.4f);
			return;
		}

        CoreBlendshapeGroup To = null;
        emotionDictionary.TryGetValue(ToEmotion, out To);
        if (To == null)
            return;

        if (StopFaceCoroutine != null || !gameObject.activeInHierarchy) {
            StopCoroutine(StopFaceCoroutine);
        }
        CurrentEmotion = To;
        StartCoroutine(ChangeToFace(To, speed));
    }

    public void StartResetFace(float time = 2) {
        StartCoroutine(ResetFace(time));
    }

    IEnumerator ResetFace(float time = 2) {
        foreach (CoreBlendshapeGroup Emotion in m_CharacterManager.GetAllBlendshapeGroups()) {
            StartCoroutine(SetEmotionValueOff(Emotion, Emotion.groupValue, Time.time, time));
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ChangeToFace(CoreBlendshapeGroup To, float time = 2) {

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
        CurrentEmotionState = ParseEnum<EmotionEmum>(data.groupName.ToUpper());
    }
}
