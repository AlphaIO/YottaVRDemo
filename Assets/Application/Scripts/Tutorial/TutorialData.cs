using UnityEngine;
using System.Collections;

public static class TutorialData {

    private static int completedEmotions;
    private static int totalEmotions;


    public static void AddEmotion() {
        totalEmotions++;
    }

    public static void EmotionComplete() {
        completedEmotions++;
    }

    public static bool TutorialComplete {

        get { return completedEmotions >= totalEmotions; }
    }




}
