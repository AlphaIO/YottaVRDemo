using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace YottaIO.Tools {
    /// <summary>
    /// lets us hide and show classes that derive from View.Window
    /// </summary>
    public class MenuController : MonoBehaviour {

        void Awake() {

        }
        public void ShowWindow(View.Window window, float time) {
            var AllViews = GetComponentsInChildren<View.Window>();
            int ViewCount = AllViews.Length;
            for (int i = 0; i < ViewCount; i++) {
                AllViews[i].hideWindow(time);
            }
            window.showWindow(time);
            window.gameObject.SetActive(true);

        }

        public void ShowWindow(View.Window window) {
            var AllViews = GetComponentsInChildren<View.Window>();
            int ViewCount = AllViews.Length;
            for (int i = 0; i < ViewCount; i++) {
                AllViews[i].hideWindow(1);
            }
            window.showWindow(1);
            window.gameObject.SetActive(true);

        }

        public void Show(GameObject ShowObject, float time) {
            var AllViews = GetComponentsInChildren<View.Window>();
            int ViewCount = AllViews.Length;
            for (int i = 0; i < ViewCount; i++) {
                AllViews[i].hideWindow(time);
            }
            if (ShowObject.GetComponent<View.Window>())
                ShowObject.GetComponent<View.Window>().showWindow(time);
            ShowObject.SetActive(true);
        }

        public void Show(GameObject ShowObject) {
            var AllViews = GetComponentsInChildren<View.Window>();
            int ViewCount = AllViews.Length;
            for (int i = 0; i < ViewCount; i++) {
                AllViews[i].hideWindow(1);
            }
            if (ShowObject.GetComponent<View.Window>())
                ShowObject.GetComponent<View.Window>().showWindow(1);
            ShowObject.SetActive(true);
        }


        public void ShowPopup(GameObject ShowObject, float time) {
            ShowObject.SetActive(true);
        }

        public void HidePopup(GameObject ShowObject, float time) {
            if (ShowObject.GetComponent<View.Window>())
                ShowObject.GetComponent<View.Window>().hideWindow(time);
        }

        public void HideAll() {
            var AllViews = GetComponentsInChildren<View.Window>();
            int ViewCount = AllViews.Length;
            for (int i = 0; i < ViewCount; i++) {
                AllViews[i].hideWindow();
            }

        }
        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit(); //this will quit our game. Note this will only work after building the game
#endif

        }
        public void Delete() {
            File.Delete(Application.persistentDataPath + "/playerinfo.dat");
        }

    }

}
