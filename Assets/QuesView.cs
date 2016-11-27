using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class QuesView : MonoBehaviour
{
	
	public SimpleSQL.SimpleSQLManager dbManager;
	public GameObject quesP;
	private GameObject content;

	public bool sentakuMode = false;
	public bool firstView = false;

	[SerializeField]
	List<int> senList;

	GameObject es;

	List<QuesData> dbData;

	void Awake ()
	{

		es = GameObject.Find ("EventSystem");
		content = GameObject.Find ("QuesContent");

		if (firstView) {
			SentakuQuesView ();
		}
	}

	public void TestQuesView (List<string> q)
	{

		foreach (Transform n in content.transform) {
			GameObject.Destroy (n.gameObject);
		}

		Color[] col = DrillColor.GetColorD (SceneData.nowColor);

		foreach (string d in q) {

			GameObject go = Instantiate (quesP);

			GameObject ba = go.transform.FindChild ("base").gameObject;
			GameObject obi = go.transform.FindChild ("obi").gameObject;
			GameObject zyun = go.transform.FindChild ("zyun").gameObject;
			GameObject ques = go.transform.FindChild ("quesText").gameObject;
			GameObject ans = go.transform.FindChild ("ansText").gameObject;
			GameObject exp = go.transform.FindChild ("expText").gameObject;
			GameObject dum = go.transform.FindChild ("dumText").gameObject;

			//Debug.Log (d);

			ba.GetComponent<Image> ().color = col [0];
			obi.GetComponent<Image> ().color = col [1];



			QuesArray qa = DbTextToQA.DbToQA (d);

			string sento = qa.Ques.Substring (0, QuesTextEdit.PerKeyCommon.Length);

			if (sento != QuesTextEdit.PerKeyCommon) {
				zyun.SetActive (false);
				ques.GetComponent<Text> ().text = "問題文：" + qa.Ques;
			} else {
				ques.GetComponent<Text> ().text = "問題文：" + qa.Ques.Substring (QuesTextEdit.PerKeyCommon.Length - 1);
			}

			string ansS = "";

			foreach (string s in qa.Ans) {
				ansS += (" " + s);
			}

			ans.GetComponent<Text> ().text = "正答：" + ansS;


			if (qa.Dummy.Length > 0) {
				string dumS = "";

				foreach (string s in qa.Dummy) {

					dumS += (" " + s);
				}

				dum.GetComponent<Text> ().text = "ダミー：" + dumS;
			} else {
				dum.GetComponent<Text> ().text = "ダミー：なし";
			}

			if (qa.Exp.Length > 0) {
				exp.GetComponent<Text> ().text = "解説：" + qa.Exp;
			} else {
				exp.GetComponent<Text> ().text = "解説：なし" + qa.Exp;

			}

			go.transform.SetParent (content.transform);
			go.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		}

	}

	public void SentakuQuesView ()
	{
		dbData = new List<QuesData> (from ps in dbManager.Table<QuesData> ()
		                             select ps);
		
		dbData = dbData.FindAll (s => s.DRILL_ID == SceneData.nowDrill);
		
		senList = new List<int> ();

		foreach (Transform n in content.transform) {
			GameObject.Destroy (n.gameObject);
		}

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

			string lastTime;
			if (d.LAST.Length > 5) {
				lastTime = DrillTime.GetLastTime (d.LAST);
			} else {
				lastTime = "なし";
			}

			num.GetComponent<Text> ().text = d.ID + "　最終：" + lastTime;

			Color[] col = DrillColor.GetColorD (SceneData.nowColor);
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
			} else if (d.LEVEL < 3) {
				level1.SetActive (true);
			} else if (d.LEVEL < 5) {
				level2.SetActive (true);
			} else if (d.LEVEL < 7) {
				level3.SetActive (true);
			} else {
				level4.SetActive (true);
			}

			QuesArray qa = DbTextToQA.DbToQA (d.TEXT);

			string sento = qa.Ques.Substring (0, QuesTextEdit.PerKeyCommon.Length);

			if (sento != QuesTextEdit.PerKeyCommon) {
				zyun.SetActive (false);
				ques.GetComponent<Text> ().text = qa.Ques;
			} else {
				ques.GetComponent<Text> ().text = qa.Ques.Substring (QuesTextEdit.PerKeyCommon.Length - 1);
			}


			string ansS = "";

			foreach (string s in qa.Ans) {
				ansS += (" " + s);
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
		} else {
			this.sentakuMode = false;
		}
	}

	public void DeleteSenB ()
	{

		if (senList.Count == 0) {
			return;
		} else {
			es.GetComponent<DbProcess> ().DeleteSelection (senList);
			SentakuQuesView ();
		}
	}
}
