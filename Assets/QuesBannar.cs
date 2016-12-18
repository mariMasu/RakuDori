using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class QuesBannar : MonoBehaviour
{

	public GameObject sele;
	public int id;
	public int jun;

	GameObject es;
	QuesSentakuMaster qv;

	void Awake ()
	{
		es = GameObject.Find ("EventSystem");
		qv = es.GetComponent<QuesSentakuMaster> ();

		sele.SetActive (false);
	}

	public void OnSentaku ()
	{
		if (qv.sentakuMode == true) {
			if (sele.activeSelf) {
				sele.SetActive (false);
			} else {
				sele.SetActive (true);

			}
			qv.SetSentakuId (this.id);
		}
	}

	public void QuesEditNext ()
	{
		es.GetComponent<PopupWindow> ().PopQuesEdit (id);
	}
}
