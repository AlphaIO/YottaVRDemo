using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace YottaIO.Tools {
    public class LoadingUI : MonoBehaviour {
        [SerializeField]
        MenuController Menu;
        [SerializeField]
        private Camera MenuCamera;
        [SerializeField]
        private View.TutorialView tutorialView;
        Coroutine Loader;
        public bool Done { get; private set; }


        public void Load(GameObject LoadObject, float time = 1) {
            if (Loader != null)
                return;

            Loader = StartCoroutine(StartLoad(LoadObject, time));



        }

        IEnumerator StartLoad(GameObject LoadObject, float time = 2) {

            Transitions.VRTransition.FadeScreen(time, this, 1, true);
            yield return new WaitForSeconds(time + .5f);
            Menu.Show(LoadObject, 0);
            Menu.ShowPopup(tutorialView.gameObject, 1);
            MenuCamera.enabled = false;
            Loader = null;
        }


    }
}
