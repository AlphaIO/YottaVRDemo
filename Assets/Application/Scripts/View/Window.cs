using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace YottaIO.View {
    /// <summary>
    /// Root of the ui menus. Controls the animations
    /// </summary>
    public class Window : MonoBehaviour {
        private Coroutine HideCoroutine;
        protected float Alpha = 1;

		private Vector3 initialScale;

        void Awake() {
            Alpha = transform.GetAlpha();

        }

		void OnEnable ()
		{
			if (initialScale.x == 0 && initialScale.y == 0)
				initialScale = transform.localScale;
		}

        public void showWindow(float time = .5f) {
            if (HideCoroutine != null) {
                StopAllCoroutines();
                HideCoroutine = null;
            }

			gameObject.SetActive(true);
            StartCoroutine(Show(time));
        }

        public void hideWindow(float time = .5f) {
            if (HideCoroutine != null || !gameObject.activeInHierarchy) {
                return;
            }
            HideCoroutine = StartCoroutine(StartHideCoroutine(time));

        }

        IEnumerator StartHideCoroutine(float time = .5f) {
            yield return StartCoroutine(Hide(time));
            yield break;
        }

        public virtual IEnumerator Show(float time) {
			transform.DOScale(initialScale, time);
            yield return new WaitForSeconds(time);
            yield break;
        }
        public virtual IEnumerator Hide(float time) {

            transform.DOScale(Vector3.zero, time);
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
            yield break;
        }
    }
}

