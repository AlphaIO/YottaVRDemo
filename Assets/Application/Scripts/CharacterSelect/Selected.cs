using UnityEngine;
using System.Collections;
using YottaIO.Tools;

public class Selected : MonoBehaviour {

    private Animator myAnimator;
    public static MonoBehaviour selected;
    IntroController IntroController;

    void Awake() {
        selected = this;
    }

    void Start() {
        myAnimator = GetComponent<Animator>();

    }

	public void OnSelect() {
        myAnimator.SetTrigger("Select");
    }


    public void SelectMale()
    {
        if (IntroController == null)
            IntroController = GameObject.FindGameObjectWithTag("MenuController").GetComponent<IntroController>();
            IntroController.MaleSelected();

            IntroController.FemaleSelected();
    }

    public void SelectFemale()
    {
        if (IntroController == null)
            IntroController = GameObject.FindGameObjectWithTag("MenuController").GetComponent<IntroController>();
        IntroController.FemaleSelected();
    }
}
