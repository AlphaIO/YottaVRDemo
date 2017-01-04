using UnityEngine;
using System.Collections;

public class WaterAnimator : MonoBehaviour {

   private Renderer water;

	// Use this for initialization
	void Start () {
        water = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        water.material.mainTextureOffset = new Vector2(Time.time / 100, 0);
 
	}
}
