﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class QuesSentakuMaster : MonoBehaviour
{
	
	public GameObject quesP;
	private GameObject content;

	public GameObject deleteDrillB;

	public bool sentakuMode = false;

	[SerializeField]
	List<int> senList;

	public int sortTag = 6;

	public List<QuesData> dbData;

	void Awake ()
	{

		content = GameObject.Find ("QuesContent");
		SentakuQuesView ();
	}


	public void SentakuQuesView ()
	{
		dbData = this.GetComponent<DbProcess> ().GetDbDataAll ();
		
		dbData = dbData.FindAll (s => s.DRILL_ID == Statics.nowDrill);

		if (sortTag != 6) {
			dbData = dbData.FindAll (s => s.TAG == sortTag);
		}

		dbData.Sort ((a, b) => a.JUN - b.JUN);

		senList = new List<int> ();

		foreach (Transform n in content.transform) {
			GameObject.Destroy (n.gameObject);
		}


		int i = 1;
		foreach (QuesData d in dbData) {

			GameObject go = Instantiate (quesP);

			GameObject ba = go.transform.FindChild ("base").gameObject;
			GameObject obi = go.transform.FindChild ("obi").gameObject;
			GameObject zyun = go.transform.FindChild ("zyun").gameObject;
			GameObject ques = go.transform.FindChild ("quesText").gameObject;
			GameObject ans = go.transform.FindChild ("ansText").gameObject;
			GameObject num = go.transform.FindChild ("numText").gameObject;
			GameObject tag = go.transform.FindChild ("tag").gameObject;

			GameObject level1 = go.transform.FindChild ("level1").gameObject;
			GameObject level2 = go.transform.FindChild ("level2").gameObject;
			GameObject level3 = go.transform.FindChild ("level3").gameObject;
			GameObject level4 = go.transform.FindChild ("level4").gameObject;

			//Debug.Log (d);

			go.GetComponent<QuesBannar> ().id = d.ID;
			go.GetComponent<QuesBannar> ().jun = i;

			string lastTime;
			if (d.LAST.Length > 5) {
				lastTime = TimeFunctions.GetLastString (d.LAST);
			} else {
				lastTime = "なし";
			}

			num.GetComponent<Text> ().text = i + "　最終：" + lastTime;

			i++;

			Color[] col = DrillColor.GetColorD (Statics.nowColor);
			ba.GetComponent<Image> ().color = col [0];
			obi.GetComponent<Image> ().color = col [1];

			if (d.TAG == 0) {
				tag.SetActive (false);
			} else {

				Color[] tCol = DrillColor.GetColorD (d.TAG);
				tag.GetComponent<Image> ().color = tCol [1];
			}

			level1.SetActive (false);
			level2.SetActive (false);
			level3.SetActive (false);
			level4.SetActive (false);

			if (d.LEVEL == 0) {
			} else if (d.LEVEL == 1) {
				level1.SetActive (true);
			} else if (d.LEVEL == 2) {
				level2.SetActive (true);
			} else if (d.LEVEL == 3) {
				level3.SetActive (true);
			} else {
				level4.SetActive (true);
			}

			QuesArray qa = DbTextToQA.DbToQA (d.TEXT);

			if (DbTextToQA.IsPer (qa.Ques)) {
				zyun.SetActive (true);
				ques.GetComponent<Text> ().text = qa.Ques.Substring (QuesTextEdit.PerKeyCommon.Length);
			} else {
				zyun.SetActive (false);
				ques.GetComponent<Text> ().text = qa.Ques;
			}
				
			string ansS = "";

			foreach (string s in qa.Ans) {
				ansS += ("/" + s);
			}

			ans.GetComponent<Text> ().text = ansS;

			go.transform.SetParent (content.transform);
			go.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		}

	}

	public void SetSentakuId (int i)
	{

		if (senList.Contains (i) == true) {
			senList.Remove (i);
		} else {
			senList.Add (i);
		}
	}


	public void SetSentakuMode (bool b)
	{

		if (b == true) {
			this.sentakuMode = true;

			foreach (Transform n in content.transform) {
				GameObject editB = n.gameObject.transform.FindChild ("editB").gameObject;
				editB.GetComponent<Button> ().enabled = false;
				editB.GetComponent<Image> ().enabled = false;
			}


			if (dbData.Count > 0) {
				deleteDrillB.SetActive (false);
			} else {
				deleteDrillB.SetActive (true);
			}

		} else {
			this.sentakuMode = false;
			foreach (Transform n in content.transform) {
				GameObject editB = n.gameObject.transform.FindChild ("editB").gameObject;
				editB.GetComponent<Image> ().enabled = true;
				editB.GetComponent<Button> ().enabled = true;	
			}
		}
	}

	public void DeleteSenB ()
	{

		if (senList.Count == 0) {
			return;
		} else {
			this.GetComponent<DbProcess> ().DeleteSelection (senList);
			SentakuQuesView ();
			this.GetComponent<PopupWindow> ().Popdown (2);
		}
	}

	public void TagSenB (int tagC)
	{

		foreach (int s in senList) {
			this.GetComponent<DbProcess> ().UpdateTag (s, tagC);

		}
		SentakuQuesView ();

	}

	public bool SentakuNull ()
	{
		if (senList.Count == 0) {
			return true;
		} else {
			return false;
		}
	}

	public void SentakuAll ()
	{
		foreach (Transform c in content.transform) {
			c.GetComponent<QuesBannar> ().OnSentaku ();
		}
	}

	public int GetQuesCount ()
	{
		return dbData.Count;
	}
}
