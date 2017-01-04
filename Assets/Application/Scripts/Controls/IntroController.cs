using UnityEngine;
using System.Collections;
using YottaIO.View;
using UnityEngine.Events;

namespace YottaIO.Tools {

    public class IntroController : MonoBehaviour {

        [SerializeField]
        private Window[] InfoPanels;
        [SerializeField]
        private MenuController UIController;
        [SerializeField]
        private float[] WaitTimes;
        [SerializeField]
        private MenuController Menu;
        [SerializeField]
        private YottaNetworkView YottaNetworkView;
        [SerializeField]
        private TutorialView Tutorial;
        [SerializeField]
        LoadingUI LoadingPanel;
        public GameObject MalePlayer, FemalePlayer;
        public GameObject Mirror;
        [SerializeField]
        Presentation PresentationView;
        public bool TutorialDone = false;
        public WebApiClient webAPI;
        void Awake() {
            if (InfoPanels.Length != WaitTimes.Length)
                return;

            Tutorial.Done += ShowNetwork;
            UIController.HideAll();
            UIController.ShowWindow(InfoPanels[0], 1);

            UnityEngine.VR.InputTracking.Recenter();
        }
        
		// Use this for initialization
        IEnumerator StartApplicationAction() {

            /*if (InfoPanels.Length != WaitTimes.Length)
                yield break;

            for (int i = 1; i < InfoPanels.Length; i++) {
                UIController.ShowWindow(InfoPanels[i], 1);
                webAPI.SendEvent(InfoPanels[i].name);
                yield return new WaitForSeconds(WaitTimes[i]);
            }*/

			MaleSelected ();
			yield return null;
        }

        public void StartApplication() {
            StartCoroutine(StartApplicationAction());
        }

        // Update is called once per frame
        void Update() {

        }

        public void ShowNetwork() 
		{
            var player = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject thing in player) {
                var playerscript = thing.GetComponent<PlayerScripts>();
                if (playerscript != null && playerscript.isLocalPlayer)
                    playerscript.ShowOtherPlayer();

            }
            Debug.Log("here");

            StartCoroutine(ShowSlides());
        }
        IEnumerator ShowSlides() {
            //yield return new WaitForSeconds(6);

			yield return new WaitForSeconds(2);
			Tutorial.gameObject.SetActive (false);

            //Mirror.SetActive(false);
            //Menu.Show(PresentationView.gameObject);
            //PresentationView.StartPresentation(10);
        }

        public void MaleSelected() {
            if (LocalPlayerCtrl.thisLocalPlayer)
                LocalPlayerCtrl.thisLocalPlayer.SetGender(true);
            LoadingPanel.Load(MalePlayer);
            Menu.Show(Mirror);

        }


        public void FemaleSelected() {
            if (LocalPlayerCtrl.thisLocalPlayer)
                LocalPlayerCtrl.thisLocalPlayer.SetGender(false);
            LoadingPanel.Load(FemalePlayer);
            Menu.Show(Mirror);


        }
    }
}
