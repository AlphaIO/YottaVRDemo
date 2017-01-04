using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class VRInteractable : MonoBehaviour {

    public UnityEvent OnPointerEnter;
    public UnityEvent OnPointerClick;
    public UnityEvent OnPointerExit;

    public Image Radial;


    public void PointerEnter() {
        if (OnPointerEnter != null)
            OnPointerEnter.Invoke();
    }

    public void PointerClick() {
        PointerEnter();
        if (OnPointerClick != null)
            OnPointerClick.Invoke();
    }

    public void PointerExit() {
        Radial.fillAmount = 0;
        if (OnPointerExit != null)
            OnPointerExit.Invoke();
    }

    public  virtual void AutoClickProgress(float progress) {

        Radial.fillAmount = progress;
    }
}
