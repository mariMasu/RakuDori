using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class QuesInputMaster : MonoBehaviour
{
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

	public bool toHankaku = true;

	public GameObject keyText1;
	public GameObject senText;
	public GameObject keyText2;
	public GameObject patText;

	public GameObject placefolder;


	void Awake ()
	{

		content = GameObject.Find ("QuesContent");
		DrillKeyPatView ();

	}

	public void DrillKeyPatView ()
	{
		senText.GetComponent<Text> ().text = sentaku;

		if (this.inputPat == 0 || this.inputPat == 1) {

			string A2 = SpaceString (sepKey);
			string Q = SpaceString (quesKey);
			string D = SpaceString (dummyKey);
			string P = SpaceString (perKey);

			if (Statics.StrNull (this.sentaku)) {
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

			if (Statics.StrNull (this.sentaku)) {
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

			if (qa.Ques.Length > QuesTextEdit.PerKeyCommon.Length) {
				
				string sento = qa.Ques.Substring (0, QuesTextEdit.PerKeyCommon.Length);

				if (sento != QuesTextEdit.PerKeyCommon) {
					zyun.SetActive (false);
					ques.GetComponent<Text> ().text = "問題文：" + qa.Ques;
				} else {
					ques.GetComponent<Text> ().text = "問題文：" + qa.Ques.Substring (QuesTextEdit.PerKeyCommon.Length);

				}
			} else {
				zyun.SetActive (false);
				ques.GetComponent<Text> ().text = "問題文：" + qa.Ques;
			}

			string ansS = "";

			foreach (string s in qa.Ans) {
				ansS += ("/" + s);
			}

			ans.GetComponent<Text> ().text = "正答：" + ansS.Substring (1);


			if (qa.Dummy.Length > 0) {
				string dumS = "";

				foreach (string s in qa.Dummy) {

					dumS += ("/" + s);
				}

				dum.GetComponent<Text> ().text = "ダミー：" + dumS.Substring (1);
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

		SetPlaceholder ();

	}

	public string GetDataByText (string type)
	{

		if (Statics.StrNull (type)) {
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
		} else if (Statics.StrNull (s)) {
			s = "なし";
			return s;
		} else {
			return s;
		}
	}

	public void SetPlaceholder ()
	{

		string newPF = "(問題登録サンプル)\n";

		switch (inputPat) {

		case 0:

			if (Statics.StrNull (perKey) == true) {
				newPF += (quesKey + "A問題");
			} else {
				newPF += (perKey + quesKey + "A問題(答えの並び方も重要)");
			}
				
			if (Statics.StrNull (sepKey) == true) {
				newPF += (ansKey + "A問題の答え");
			} else {
				newPF += (ansKey + "A問題の答え1" + "A問題の答え2");
			}

			if (Statics.StrNull (expKey) == false) {
				newPF += (ansKey + "A問題の解説");
			}

			if (Statics.StrNull (dummyKey) == false) {
				if (Statics.StrNull (sepKey) == true) {
					newPF += (ansKey + "A問題の答えダミー");
				} else {
					newPF += (ansKey + "A問題の答えダミー1" + "A問題の答えダミー2" + "A問題の答えダミー3");
				}
			}

			if (Statics.StrNull (perKey) == true) {
				newPF += (quesKey + "B問題");
			} else {
				newPF += (perKey + quesKey + "B問題(答えの並び方も重要)");
			}

			if (Statics.StrNull (sepKey) == true) {
				newPF += (ansKey + "B問題の答え");
			} else {
				newPF += (ansKey + "B問題の答え1" + "B問題の答え2" + "B問題の答え3");
			}

			if (Statics.StrNull (expKey) == false) {
				newPF += (ansKey + "B問題の解説");
			}

			if (Statics.StrNull (dummyKey) == false) {
				if (Statics.StrNull (sepKey) == true) {
					newPF += (ansKey + "B問題の答えダミー");
				} else {
					newPF += (ansKey + "B問題の答えダミー1" + "B問題の答えダミー2");
				}
			}

			break;
		case 1:

			break;
		case 2:

			break;
		case 3:
			break;
		case 4:

			break;
		case 5:

			break;
		default:
			Debug.Log ("Incorrect data");
			break;
		}

		placefolder.GetComponent<Text> ().text = newPF;

	}

}
