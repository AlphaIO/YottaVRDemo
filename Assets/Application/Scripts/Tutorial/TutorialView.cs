using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace YottaIO.View {
    public class TutorialView : Window {
        public GameObject Text;
        public System.Action Done;
        bool done;

		void OnEnable ()
		{
			#if UNITY_EDITOR
				Done.Invoke();
			#endif
		}

        void Update() {
            if (TutorialData.TutorialComplete && done == false) {
                //Text.SetActive(true);
                Debug.Log("done");
                if (Done != null)
                    Done.Invoke();

                done = true;
            }
        }
    }
}
