using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SetColor : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

		GameObject ba = this.transform.FindChild ("base").gameObject;
		GameObject obi = this.transform.FindChild ("obi").gameObject;

		Color[] col = DrillColor.GetColorD (SceneData.nowColor);
		ba.GetComponent<Image> ().color = col [0];
		obi.GetComponent<Image> ().color = col [1];
	
	}

}
