using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Presentation : YottaIO.View.Window {


    public GameObject[] slides;
    private static int slide;
    private int currentSlide = -1;
    private float WaitTime = 0;
    public float TimeToNextSlide { get; private set; }
    [SerializeField]
    Text CountDown;

    int[] Emotions = new int[3] { 0, 0, 0 };

    [SerializeField]
    Text[] EmotionText;

    bool tutorialDone;
    // Update is called once per frame
    void Update() {


        ShowSlide();



    }

    public void SetEmotionCountText(string emotion) {
        if (!tutorialDone)
            return;

        if (string.Equals(emotion, "Joy"))
            Emotions[0] += 1;

        if (string.Equals(emotion, "Surprise"))
            Emotions[1] += 1;


        if (string.Equals(emotion, "Contempt"))
            Emotions[2] += 1;

    }


    public void StartPresentation(float time) {
        StartCoroutine(PictureSlideShow(time));
        tutorialDone = true;
    }
    IEnumerator PictureSlideShow(float time) {
        WaitTime += Time.deltaTime;
        TimeToNextSlide = WaitTime / time;
        //     CountDown.text = TimeToNextSlide.ToString("F1");
        yield return new WaitForSeconds(time);
        for (int i = 0; i < Emotions.Length; i++) {
            Emotions[i] = 0;
        }
        slide++;
        WaitTime = 0;
        StartCoroutine(PictureSlideShow(time));
    }



    void ShowSlide() {
        if (currentSlide != slide) {
            for (int i = 0; i < slides.Length; i++) {

                if (i == slide) {

                    slides[i].SetActive(true);
                }
                else {
                    slides[i].SetActive(false);
                }

                if (i == slides.Length)
                    currentSlide = slide;

            }
        }

        for (int i = 0; i < Emotions.Length; i++) {
            EmotionText[i].text = Emotions[i].ToString();
        }


    }

    public void Next() {
        if (slide < slides.Length)
            slide++;
        else
            slide = 0;
    }

    public void Back() {

        if (slide > 0)
            slide--;
        else
            slide = slides.Length - 1;
    }
}
