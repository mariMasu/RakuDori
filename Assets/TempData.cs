using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TempData : MonoBehaviour
{

	GameObject es;

	public string[] temp = new string[7];

	void Awake ()
	{
		es = GameObject.Find ("EventSystem");
	}

	public void DrillPatSend ()
	{

		QuesInputMaster sd = es.GetComponent<QuesInputMaster> ();

		if (temp [0] != "") {
			sd.SetData (temp [0], "InputPat");
		}

		if (sd.inputPat < 2) {
			sd.SetData ("\n", "QuesKey");
		} else {
			sd.SetData ("@@", "QuesKey");

		}
		es.GetComponent<QuesInputMaster> ().DrillKeyPatView ();
	}

	public void TagSend ()
	{

		if (Statics.strNull (temp [0])) {
			return;
		}

		QuesSentakuMaster qv = es.GetComponent<QuesSentakuMaster> ();
		qv.TagSenB (int.Parse (temp [0]));

		es.GetComponent<PopupWindow> ().Popdown (3);
		es.GetComponent<PopupWindow> ().Popdown (2);
	}

	public void SortSend ()
	{

		InputField input1 = this.transform.FindChild ("numInput1").GetComponent<InputField> ();
		InputField input2 = this.transform.FindChild ("numInput2").GetComponent<InputField> ();

		if (Statics.strNull (input1.text) || Statics.strNull (input2.text)) {
			return;
		}

		int nowJun = int.Parse (input1.text);
		int newJun = int.Parse (input2.text);

		if (nowJun == newJun) {
			return;
		}

		QuesSentakuMaster qv = es.GetComponent<QuesSentakuMaster> ();

		nowJun = JunModify (nowJun, qv.GetQuesCount ());
		newJun = JunModify (newJun, qv.GetQuesCount ());

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
			return c;
		}
		return i;
	}

	public void DrillEditSend ()
	{

		int col = int.Parse (temp [0]);
		GameObject input = es.GetComponent<PopupWindow> ().input1;
		string name = input.GetComponent<InputField> ().text;

		es.GetComponent<DbProcess> ().UpdateDrillNC (col, name);

		Statics.nowColor = col;
		es.GetComponent<QuesSentakuMaster> ().SentakuQuesView ();

		es.GetComponent<PopupWindow> ().Popdown (1);
	}

	public void TagSortSend ()
	{

		int tag = int.Parse (temp [0]);

		QuesSentakuMaster qv = es.GetComponent<QuesSentakuMaster> ();

		qv.sortTag = tag;

		qv.SentakuQuesView ();

		es.GetComponent<PopupWindow> ().Popdown (5);
	}

	public void DrillKeySend ()
	{

		if (KeyCheck () == false) {
			return;
		}

		QuesInputMaster sd = es.GetComponent<QuesInputMaster> ();

		sd.SetData (temp [0], "Sentaku");
		sd.SetData (temp [1], "QuesKey");
		sd.SetData (temp [2], "AnsKey");
		sd.SetData (temp [3], "SepKey");
		sd.SetData (temp [4], "ExpKey");
		sd.SetData (temp [5], "DummyKey");
		sd.SetData (temp [6], "PerKey");

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

			if (Statics.strNull (temp [i])) {
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
		int ansO = int.Parse (temp [0]);
		int dumU = int.Parse (temp [1]);
		int dumT = int.Parse (temp [2]);

		if (ansO == 0) {
			ansO = 1;
		}

		es.GetComponent<DbProcess> ().UpdateDrillOrder (ansO, dumU, dumT);

		es.GetComponent<LoadButton> ().LoadDrillAns ();


	}
		
}