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
	public int orderQ = 0;
	public int orderA = 0;
	public bool existQ = false;

	public void DrillEdit ()
	{
		GameObject es = GameObject.Find ("EventSystem");
		Statics.nowDrill = id;
		Statics.nowColor = color;

		es.GetComponent<LoadButton> ().LoadQuesSentaku ();
	}

	public void DrillNext ()
	{
		if (existQ) {

			GameObject es = GameObject.Find ("EventSystem");
			Statics.nowDrill = id;
			Statics.nowColor = color;

			if (orderQ == 0) {
				es.GetComponent<PopupWindow> ().PopDrillOrder (2);
			} else {
				es.GetComponent<LoadButton> ().LoadDrillAnsSingle ();
			}
		}
	}


}
