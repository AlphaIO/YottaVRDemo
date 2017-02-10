using UnityEngine;
using System.Collections;

public class HeadRotation : MonoBehaviour {

	public GameObject characterNeck;
	public GameObject userCamera;

	void LateUpdate ()
	{
		characterNeck.transform.rotation = userCamera.transform.rotation;
	}
}
