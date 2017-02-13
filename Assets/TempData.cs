using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class TempData : MonoBehaviour
{

	GameObject es;

	public string[] temp = new string[8];

	void Awake ()
	{
		es = GameObject.Find ("EventSystem");
	}

	public void ScrollReset ()
	{
		GameObject sv = this.transform.Find ("Scroll View").gameObject;
		sv.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;
	}

	public void DrillAddSend ()
	{
		string name = temp [1];

		if (Statics.StrNull (name)) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\nドリルの名前が未入力です");
			return;
		}

		List<DrillData> ddl = es.GetComponent<DbProcess> ().GetDbDrillDataAll ();
		foreach (DrillData dd in ddl) {
			if (dd.NAME == name) {
				es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n同名のドリルは作れません");
				return;
			}
		}

		int col = int.Parse (temp [0]);

		es.GetComponent<DrillSentakuMaster> ().AddSimple (col, name);

		es.GetComponent<PopupWindow> ().Popdown (1);

		es.GetComponent<PopupWindow> ().ResetInputField (1);
	}

	public void DrillPatSend ()
	{

		QuesInputMaster sd = es.GetComponent<QuesInputMaster> ();

		if (temp [0] != "") {
			sd.inputPat = int.Parse (temp [0]);
		}

		if (sd.inputPat < 2) {
			sd.quesKey = "\n";
		} else {
			sd.quesKey = "@@";

		}
		es.GetComponent<QuesInputMaster> ().DrillKeyPatView ();
	}

	public void TagSend ()
	{

		if (Statics.StrNull (temp [0])) {
			return;
		}

		QuesSentakuMaster qv = es.GetComponent<QuesSentakuMaster> ();
		qv.TagSenB (int.Parse (temp [0]));

		qv.SentakuQuesView ();

		es.GetComponent<PopupWindow> ().Popdown (3);
		es.GetComponent<PopupWindow> ().Popdown (2);
	}

	public void TagLevelSendQuesEdit ()
	{
		if (Statics.nowTag.ToString () == temp [0] && Statics.nowLevel.ToString () == temp [1]) {
			es.GetComponent<PopupWindow> ().Popdown (9);
		}

		QuesSentakuMaster mas = es.GetComponent<QuesSentakuMaster> ();


		if (Statics.nowTag.ToString () != temp [0]) {
			mas.nowQData.TAG = int.Parse (temp [0]);
		}

		if (Statics.nowLevel.ToString () != temp [1]) {
			mas.nowQData.LEVEL = int.Parse (temp [1]);
			mas.nowQData.REVIEW = 0;
		}

		es.GetComponent<DbProcess> ().UpdateQuesData (mas.nowQData);
		es.GetComponent<PopupWindow> ().Popdown (9);

		Statics.nowLevel = mas.nowQData.LEVEL;
		Statics.nowTag = mas.nowQData.TAG;

	}

	public void TagLevelSendDrillAns ()
	{

		if (Statics.nowTag.ToString () == temp [0] && Statics.nowLevel.ToString () == temp [1]) {
			es.GetComponent<PopupWindow> ().Popdown (1);
		}


		DrillAnsMaster mas = es.GetComponent<DrillAnsMaster> ();


		if (Statics.nowTag.ToString () != temp [0]) {
			mas.nowQData.TAG = int.Parse (temp [0]);
		}

		if (Statics.nowLevel.ToString () != temp [1]) {
			mas.nowQData.LEVEL = int.Parse (temp [1]);
			mas.nowQData.REVIEW = 1;

		}


		es.GetComponent<DbProcess> ().UpdateQuesData (mas.nowQData);
		mas.setTagLevActive ();
		es.GetComponent<PopupWindow> ().Popdown (1);

		Statics.nowLevel = mas.nowQData.LEVEL;
		Statics.nowTag = mas.nowQData.TAG;
	}

	public void SortSend ()
	{

		InputField input1 = this.transform.FindChild ("numInput1").GetComponent<InputField> ();
		InputField input2 = this.transform.FindChild ("numInput2").GetComponent<InputField> ();

		if (Statics.StrNull (input1.text) || Statics.StrNull (input2.text)) {
			return;
		}

		int nowJun = int.Parse (input1.text);
		int newJun = int.Parse (input2.text);

		if (nowJun == newJun || newJun < 0) {
			return;
		}

		QuesSentakuMaster qv = es.GetComponent<QuesSentakuMaster> ();
		int nowQuesCount = qv.GetQuesCount ();

		if (nowJun < 1 || nowJun > nowQuesCount) {

			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n" + nowJun + "番の問題は存在しません", 8);

			return;
		}

		nowJun = JunModify (nowJun, nowQuesCount);


		newJun = JunModify (newJun, nowQuesCount);

		es.GetComponent<DbProcess> ().UpdateQuesJun (nowJun - 1, newJun - 1);

		qv.SentakuQuesView ();

		es.GetComponent<PopupWindow> ().Popdown (4);
		input1.text = "";
		input2.text = "";
		ResetTemp ();

	}

	int JunModify (int i, int c)
	{
		if (i < 1) {
			return 1;
		} else if (i > c) {
			es.GetComponent<PopupWindow> ().PopupCaution (c + "番へ移動しました", 8);

			return c;
		}
		return i;
	}

	public void DrillEditSend ()
	{


		string name = temp [1];

		if (Statics.StrNull (name)) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\nドリルの名前が未入力です", 8);
			return;
		}

		if (Statics.nowName != name) {
			List<DrillData> ddl = es.GetComponent<DbProcess> ().GetDbDrillDataAll ();
			foreach (DrillData dd in ddl) {
				if (dd.NAME == name) {
					es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n同名のドリルが存在します", 8);
					return;
				}
			}
		}

		int col = int.Parse (temp [0]);

		es.GetComponent<DbProcess> ().UpdateDrillNC (col, name);

		Statics.nowColor = col;
		es.GetComponent<QuesSentakuMaster> ().SentakuQuesView ();

		es.GetComponent<PopupWindow> ().Popdown (1);
	}

	public void TagSortSend ()
	{

		int tag = int.Parse (temp [0]);

		QuesSentakuMaster qs = es.GetComponent<QuesSentakuMaster> ();

		qs.sortTag = tag;

		qs.SentakuQuesView ();

		es.GetComponent<PopupWindow> ().Popdown (5);
	}

	public void DrillKeySend ()
	{

		if (KeyCheck () == false) {
			return;
		}

		QuesInputMaster sd = es.GetComponent<QuesInputMaster> ();

		sd.sentaku = temp [0];
		sd.quesKey = temp [1];
		sd.ansKey = temp [2];
		sd.sepKey = temp [3];
		sd.expKey = temp [4];
		sd.dummyKey = temp [5];
		sd.perKey = temp [6];

		if (Statics.StrNull (temp [7]) == false) {
			sd.toHankaku = Statics.StringToBool (temp [7]);
		}

		es.GetComponent<QuesInputMaster> ().DrillKeyPatView ();
		es.GetComponent<PopDrillKey> ().DropViewReset ();
		es.GetComponent<PopupWindow> ().ResetInputField (4);
		es.GetComponent<PopupWindow> ().Popdown (2);
		ResetTemp ();
	}

	public void SetTemp (string data, int i)
	{

		temp [i] = data;
	}

	public void ResetTemp ()
	{
		for (int i = 0; i < 7; i++) {
			temp [i] = "";
		}
	}

	public bool KeyCheck ()
	{

		if (es.GetComponent<PopDrillKey> ().CustomCheck () == false) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n入力されていない\nカスタムがあります");
			return false;
		}

		HashSet<string> hs = new HashSet<string> ();
		//string[] used = {"###","$$$","%%%","&&&"};

		for (int i = 0; i < 7; i++) {

			if (Statics.StrNull (temp [i])) {
			} else {

				if ((hs.Add (temp [i]) == false)) {
					es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n複数の項目に同じキーは\n指定できません");
					return false;
				}

				//			if(-1 != UnityEditor.ArrayUtility.IndexOf(used,temp[i])){
				//				es.GetComponent<PopupWindow> ().PopupCaution ("###,$$$,%%%,&&&は使えません");
				//				return;
				//			}
			}
		}

		return true;
	}

	public void DrillOrderSend ()
	{
		int orderQ = int.Parse (temp [0]);
		int orderA = (int.Parse (temp [1]) - 1);
		int dumU = int.Parse (temp [2]);
		int dumT = int.Parse (temp [3]);

		int severe = int.Parse (temp [4]);

		if (orderQ == 0) {
			orderQ = 1;
		}

		es.GetComponent<DbProcess> ().UpdateDrillOrder (orderQ, orderA, dumU, dumT, severe);

	}

	public void AnsChooseSet ()
	{
		int i;
		int[] num = es.GetComponent<DrillSentakuMaster> ().nowNumQA;
		string s = "";


		if (temp [0] == "") {
			i = 0;
		} else {
			i = int.Parse (temp [0]);

		}

		if (i == 1 && (num [0] == 0)) {
			s = "要復習問題が存在しません";

		} else if (i == 3 && (num [1] == 0)) {
			s = "完了問題が存在しません";
		}
			
		if (s == "") {
			Statics.ansChoose = i;
			es.GetComponent<LoadButton> ().LoadDrillAnsSingle ();
		} else {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n" + s, 4);
		}
	}

	public void QuesEditSend ()
	{

		string p = temp [4];

		string q = temp [0];
		string a = temp [1];
		string e = temp [2];
		string d = temp [3];

		string sepkey = QuesTextEdit.SepKeyCommon;

		if (Statics.StrNull (q) || Statics.StrNull (a)) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n問題文か正答が空欄です", 8);
			return;
		}

		for (int i = 0; i < 4; i++) {
			string[] used = {
				QuesTextEdit.AnsKeyCommon,
				QuesTextEdit.DummyKeyCommon,
				QuesTextEdit.ExpKeyCommon,
				QuesTextEdit.PerKeyCommon
			};
			int f = 0;

			foreach (string s in used) {
				if (temp [i].IndexOf (s) > -1) {
					f++;
				}
			}

			if (f != 0) {
				es.GetComponent<PopupWindow> ().PopupCaution ("エラー\nテキスト内に" + QuesTextEdit.AnsKeyCommon + "、\n" + QuesTextEdit.ExpKeyCommon + "、" + QuesTextEdit.DummyKeyCommon + "、" + QuesTextEdit.PerKeyCommon + "は使えません", 8);
				return;
			}
		}




		q = QuesTextEdit.RemoveEnterZengo (q);

		if (p == "1") {
			q = (QuesTextEdit.PerKeyCommon + q);
		}


		a = QuesTextEdit.RemoveEnterAll (a);

		if (a.Length > sepkey.Length) {
			while (a.Substring (sepkey.Length) == sepkey) {
				a = a.Substring (sepkey.Length);
			}
			while (a.Substring (a.Length - sepkey.Length) == sepkey) {
				a = a.Substring (0, a.Length - sepkey.Length);
			}
		}

		a = (QuesTextEdit.AnsKeyCommon + a);

		if (Statics.StrNull (d) == false) {

			d = QuesTextEdit.RemoveEnterAll (d);

			if (d.Length > sepkey.Length) {
				while (d.Substring (sepkey.Length) == sepkey) {
					d = d.Substring (sepkey.Length);
				}
				while (a.Substring (a.Length - sepkey.Length) == sepkey) {
					d = a.Substring (0, a.Length - sepkey.Length);
				}
			}
			d = (QuesTextEdit.DummyKeyCommon + d);

		}

		if (Statics.StrNull (e) == false) {
			e = QuesTextEdit.RemoveEnterZengo (e);
			e = (QuesTextEdit.ExpKeyCommon + e);
		}
		QuesData qd = es.GetComponent<DbProcess> ().GetDbData (int.Parse (temp [5]));

		qd.TEXT = (q + a + d + e);

		es.GetComponent<DbProcess> ().UpdateQuesData (qd);

		es.GetComponent<QuesSentakuMaster> ().SentakuQuesView ();
		es.GetComponent<PopupWindow> ().Popdown (7);

	}

	public void QuesCopySend ()
	{
		if (Statics.StrNull (temp [0])) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\nコピー先のドリルを選択してください");
			return;
		}

		List<DrillData> ddl = es.GetComponent<DbProcess> ().GetDbDrillDataAll ();
		DrillData dd = ddl.Find (s => s.NAME == temp [0]);
		int drillId = dd.ID;

		bool tagCopy = Statics.StringToBool (temp [1]);
		bool levelCopy = Statics.StringToBool (temp [2]);

		QuesSentakuMaster qs = es.GetComponent<QuesSentakuMaster> ();
		qs.CopyQuesB (drillId, tagCopy, levelCopy);

		es.GetComponent<PopupWindow> ().Popdown (11);
		es.GetComponent<PopupWindow> ().Popdown (2);


	}

	public void ImportSend ()
	{

		if (Statics.StrNull (temp [0]) == true) {
			return;
		}

		string[] strA = DbTextToQA.ImportToText (temp [0]);

		if (strA.Length == 0) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\nコピーペーストに間違いが\nないか確かめてください");

		} else {
			es.GetComponent<QuesTextEdit> ().ImportQaTest (strA);

		}

	}

	public void DrillConfigSend ()
	{

		if (Statics.StrNull (temp [0]) == true) {
			return;
		}

		float keisu = float.Parse (temp [0]);

		if (keisu < 0 || keisu > 1) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n１〜０の範囲で入力してください");

		} else {
			
			PlayerPrefs.SetFloat ("NIGATE", keisu);
			PlayerPrefs.Save ();

			es.GetComponent<PopupWindow> ().Popdown (5);
		}
	}
}