using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class FbBtnAnimation : MonoBehaviour 
{
	private Tweener alphaTween;
	private Tweener scaleTween;

	public bool disableAnimation = false;

	void OnEnable ()
	{
		scaleTween.Kill (true);
		alphaTween.Kill (true);

		Image img = GetComponent <Image> ();
		Color initialColor = img.color;
		Vector3 initialScale = transform.localScale;

		if (!disableAnimation) {
			img.SetAlpha (0.5f);
			transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
			scaleTween = transform.DOScale (initialScale, 0.25f);
			alphaTween = img.DOColor (initialColor, 0.25f);
		}
	}
}
