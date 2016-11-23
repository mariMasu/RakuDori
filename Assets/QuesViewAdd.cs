using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class QuesViewAdd : MonoBehaviour
{

	public GameObject quesP;
	private GameObject content;

	GameObject es;
	SceneData sd;


	void Start ()
	{

		es = GameObject.Find ("EventSystem");
		sd = es.GetComponent<SceneData> ();

		content = GameObject.Find ("QuesContent");
	}

	public void TestQuesView (List<string> q)
	{

		foreach (Transform n in content.transform) {
			GameObject.Destroy (n.gameObject);
		}

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

			Color[] col = DrillColor.GetColorD (sd.ColNum);
			ba.GetComponent<Image> ().color = col [0];
			obi.GetComponent<Image> ().color = col [1];



			QuesArray qa = DbTextToQA.DbToQA (d);

			string sento = qa.Ques.Substring (0, QuesTextEdit.PerKeyCommon.Length);

			if (sento != QuesTextEdit.PerKeyCommon) {
				zyun.SetActive (false);
			}

			ques.GetComponent<Text> ().text = "問題文：" + qa.Ques;



			string ansS = "";

			foreach (string s in qa.Ans) {
				ansS += (" " + s);
			}

			ans.GetComponent<Text> ().text = "正答：" + ansS;


			if (qa.Dummy.Length > 0) {
				string dumS = "";

				foreach (string s in qa.Dummy) {

					dumS += ("" + s);
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

}
