using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class QuesInputMaster : MonoBehaviour
{
	public SimpleSQL.SimpleSQLManager dbManager;
	public GameObject quesP;
	private GameObject content;

	public int inputPat = 0;
	public string sentaku;
	public string quesKey = "\n";
	public string ansKey = "!!";
	public string sepKey;
	public string expKey = "##";
	public string dummyKey;
	public string perKey;
	public string quesText;

	public GameObject keyText1;
	public GameObject senText;
	public GameObject keyText2;
	public GameObject patText;


	void Awake ()
	{

		content = GameObject.Find ("QuesContent");
		DrillKeyPatView ();

	}

	public void DrillKeyPatView ()
	{
		senText.GetComponent<Text> ().text = this.sentaku;

		if (this.inputPat == 0 || this.inputPat == 1) {

			string A2 = SpaceString (this.sepKey);
			string Q = SpaceString (this.quesKey);
			string D = SpaceString (this.dummyKey);
			string P = SpaceString (this.perKey);

			if (Statics.strNull (this.sentaku)) {
				keyText1.GetComponent<Text> ().text = "区切り\n\n正答、ダミー内の分割\n\nダミー選択肢\n\n正答の順序を守る";
				keyText2.GetComponent<Text> ().text = Q + "\n\n" + A2 + "\n\n" + D + "\n\n" + P;

			} else {

				keyText1.GetComponent<Text> ().text = "選択肢\n\n\n区切り\n\n正答、ダミー内の分割\n\nダミー選択肢\n\n正答の順序を守る";
				keyText2.GetComponent<Text> ().text = "\n\n\n" + Q + "\n\n" + A2 + "\n\n" + D + "\n\n" + P;

			}
			if (this.inputPat == 0) {
				patText.GetComponent<Text> ().text = "問→正\n→問...";
			} else {
				patText.GetComponent<Text> ().text = "全問\n→全正";
			}


		} else {
			string A1 = SpaceString (this.ansKey);
			string A2 = SpaceString (this.sepKey);
			string Q = SpaceString (this.quesKey);
			string E = SpaceString (this.expKey);
			string D = SpaceString (this.dummyKey);
			string P = SpaceString (this.perKey);

			if (Statics.strNull (this.sentaku)) {
				keyText1.GetComponent<Text> ().text = "問題の区切り\n\n正答の開始\n\n正答、ダミー内の分割\n\n解説の開始\n\nダミー選択肢\n\n正答の順序を守る";
				keyText2.GetComponent<Text> ().text = Q + "\n\n" + A1 + "\n\n" + A2 + "\n\n" + E + "\n\n" + D + "\n\n" + P;

			} else {

				keyText1.GetComponent<Text> ().text = "選択肢\n\n\n問題の区切り\n\n正答の開始\n\n正答、ダミー内の分割\n\n解説の開始\n\nダミー選択肢\n\n正答の順序を守る";
				keyText2.GetComponent<Text> ().text = "\n\n\n" + Q + "\n\n" + A1 + "\n\n" + A2 + "\n\n" + E + "\n\n" + D + "\n\n" + P;

			}

			if (this.inputPat == 2) {
				patText.GetComponent<Text> ().text = "問→正→\n解→問...";
			} else if (this.inputPat == 3) {
				patText.GetComponent<Text> ().text = "問→解→\n正→問...";
			} else if (this.inputPat == 4) {
				patText.GetComponent<Text> ().text = "全問→\n正→解→\n正...";
			} else if (this.inputPat == 5) {
				patText.GetComponent<Text> ().text = "全問→\n解→正→\n解...";
			}
		}
	}

	public void TestQuesView (List<string> q)
	{

		foreach (Transform n in content.transform) {
			GameObject.Destroy (n.gameObject);
		}

		Color[] col = DrillColor.GetColorD (Statics.nowColor);

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

	public void SetData (string data, string type)
	{

		switch (type) {


		case "InputPat":
			inputPat = int.Parse (data);

			break;
		case "Sentaku":
			sentaku = data;

			break;
		case "QuesKey":
			quesKey = data;

			break;
		case "AnsKey":
			ansKey = data;

			break;
		case "SepKey":
			sepKey = data;

			break;
		case "ExpKey":
			expKey = data;

			break;
		case "DummyKey":
			dummyKey = data;

			break;
		case "PerKey":
			perKey = data;

			break;
		case "QuesText":
			quesText = data;

			break;
		default:
			Debug.Log ("Incorrect data");
			break;
		}
	}

	public string GetDataByText (string type)
	{

		if (Statics.strNull (type)) {
			return "";

		} else {

			string r;

			switch (type) {

			case "InputPat":
				r = inputPat.ToString ();

				break;
			case "Sentaku":
				r = sentaku;

				break;
			case "QuesKey":
				r = quesKey;

				break;
			case "AnsKey":
				r = ansKey;

				break;
			case "SepKey":
				r = sepKey;

				break;
			case "ExpKey":
				r = expKey;

				break;
			case "DummyKey":
				r = dummyKey;

				break;
			case "PerKey":
				r = perKey;

				break;
			case "QuesText":
				r = quesText;

				break;
			default:
				Debug.Log ("Incorrect data");
				r = "";
				break;
			}

			return r;
		}
	}

	public string SpaceString (string s)
	{

		if (s == " ") {
			s = "半スペ";
			return s;
		} else if (s == "　") {
			s = "全スペ";
			return s;
		} else if (s == "\n") {
			s = "改行";
			return s;
		} else if (s == "\n\n") {
			s = "２改行";
			return s;
		} else if (s == "\n\n\n") {
			s = "３改行";
			return s;
		} else if (Statics.strNull (s)) {
			s = "なし";
			return s;
		} else {
			return s;
		}
	}

}
