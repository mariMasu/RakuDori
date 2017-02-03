using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SetColor : MonoBehaviour
{

	public int typeInt = 0;

	// Use this for initialization
	void Start ()
	{
		setC ();
	}

	public void setC ()
	{
		Color[] col = DrillColor.GetColorD (Statics.nowColor);

		if (typeInt == 0) {
			GameObject ba = this.transform.FindChild ("base").gameObject;
			GameObject obi = this.transform.FindChild ("obi").gameObject;

			ba.GetComponent<Image> ().color = col [0];
			obi.GetComponent<Image> ().color = col [1];

		} else {
			this.GetComponent<Image> ().color = col [1];
		}
	}

}
