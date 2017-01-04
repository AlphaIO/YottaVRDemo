using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Transitions {

    public static class VRTransition {
        /// You can call the switch scene function using TransitionActions.ChangeScne(float,Level)

        static string levelName;
        static float time;
        static AsyncOperation async; // used for level loading.
        static GameObject faderSphere;

        static bool changeScenes;

        delegate void LoadScene();
        static event LoadScene onLoad;

        public delegate void Loading();
        public static event Loading onLoading;

        public delegate void Complete();
        public static event Complete onCompleteFade;

        static public void QuitApp() {
            Debug.Log("QuitApp");
            Application.Quit();

        }

        /// <summary>
        /// Action to fade scene smoothly
        /// </summary>
        /// <param name="fadeTime"></param>
        /// <param name="level"></param>
        /// <param name="instance"> just pass "this"</param>
        public static void ChangeScene(float fadeTime, string level, MonoBehaviour instance) {
            time = fadeTime;
            levelName = level;
            instance.StopAllCoroutines();
            instance.StartCoroutine(DoFadeOutThenLoadLevel(time, levelName));
        }

        /// <summary>
        /// Use to fade the screen out without changing scene
        /// </summary>
        /// <param name="time"></param>
        /// <param name="instance">just use "this"</param>
        public static void FadeScreen(float time, MonoBehaviour instance, int direction = 1, bool pingPong = false) {
            instance.StartCoroutine(DoFadeOut(instance, time, direction, pingPong));
        }



        private static IEnumerator DoFadeOutThenLoadLevel(float fadeTime, string level) {
            GameObject fadeObject = null;
            //Debug.Log("Doing fade out");
            if (onLoading != null)
                onLoading();

            async = SceneManager.LoadSceneAsync(level);
            async.allowSceneActivation = false;
            while (!async.isDone && async.progress < .9f) {
                Debug.Log("loading" + async.progress);
                yield return null;


            }
            Debug.Log("loaded");

            if (faderSphere) {
                fadeObject = Object.Instantiate(faderSphere, Camera.main.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject; // Instanciate the Orb at the camera position
            }
            else {
                fadeObject = SetFaderObject();
            }
            fadeObject.SetActive(false);
            Material faderMaterial = fadeObject.GetComponent<Renderer>().material;
            Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            faderMaterial.color = color;
            fadeObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            float elapsedTime = Time.deltaTime;


            while (elapsedTime < fadeTime) {
                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeTime);
                faderMaterial.color = color;

            }
            if (onLoad != null)
                onLoad();
            yield return new WaitForSeconds(fadeTime);
            async.allowSceneActivation = true;

        }

        static IEnumerator DoFadeOut(MonoBehaviour instance, float time, int direction = 1, bool pingPong = false) {
            GameObject fadeObject = null;
            if (faderSphere) {
                fadeObject = Object.Instantiate(faderSphere, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject; // Instanciate the Orb at the camera position
            }
            else {
                fadeObject = SetFaderObject();
            }
            fadeObject.SetActive(false);
            Material faderMaterial = fadeObject.GetComponent<Renderer>().material;
            Color color = new Color(0.0f, 0.0f, 0.0f, ( 1 - direction ));
            faderMaterial.color = color;
            fadeObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            float elapsedTime = Time.deltaTime;


            while (elapsedTime < time) {
                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Abs(Mathf.Clamp01(elapsedTime / time) - ( 1 - direction ));
                faderMaterial.color = color;

            }

            yield return new WaitForSeconds(.2f);
            if (pingPong) {
                if (onCompleteFade != null)
                    onCompleteFade();
                FadeScreen(1, instance, -( 1 - direction ));
                Object.Destroy(fadeObject);
                yield break;
            }
            else {
                if (onCompleteFade != null)
                    onCompleteFade();
                Object.Destroy(fadeObject);
            }


        }

        private static GameObject SetFaderObject() {


            faderSphere = Resources.Load("FaderSphere", typeof(GameObject)) as GameObject;
            faderSphere.SetActive(false);

            faderSphere.transform.localScale = new Vector3(100, 100, 100);
            return Object.Instantiate(faderSphere, Camera.main.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject; // Instanciate the Orb at the camera position
        }

    }
}

