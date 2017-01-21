using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineBanner : MonoBehaviour
{

	public Sprite banner1;
	public Sprite banner2;
	public Sprite banner3;
	public Sprite banner4;

	Sprite[] sprA = new Sprite[4];

	int i = 0;

	public float timeOut = 90;
	private float timeElapsed;

	void Start ()
	{

		sprA [0] = banner1;
		sprA [1] = banner2;
		sprA [2] = banner3;
		sprA [3] = banner4;

		i = UnityEngine.Random.Range (0, 4);
		this.GetComponent<Image> ().sprite = sprA [i];

	}

	void Update ()
	{
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= timeOut) {

			this.GetComponent<Image> ().sprite = sprA [i];

			if (i == 3) {
				i = 0;
			} else {
				i++;
			}

			timeElapsed = 0.0f;
		}
	}


}
