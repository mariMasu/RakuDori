using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

public class DrillNum : MonoBehaviour
{

	public int id = 0;
	public int color = 0;
	public string nameD = "";
	public int order = 0;

	public void DrillNext ()
	{
		GameObject es = GameObject.Find ("EventSystem");
		Statics.nowDrill = id;

		if (order == 0) {
			es.GetComponent<PopupWindow> ().PopDrillOrder ();
		} else {
			es.GetComponent<LoadButton> ().LoadDrillAns ();
		}
	}

}
