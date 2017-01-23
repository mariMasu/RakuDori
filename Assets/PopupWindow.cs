using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{

	public GameObject pop1;
	public GameObject pop2;
	public GameObject pop3;
	public GameObject pop4;
	public GameObject pop5;
	public GameObject pop6;
	public GameObject pop7;
	public GameObject pop8;
	public GameObject pop9;
	public GameObject pop10;
	public GameObject pop11;
	public GameObject pop12;



	public GameObject input1;
	public GameObject input2;
	public GameObject input3;
	public GameObject input4;
	public GameObject input5;
	public GameObject input6;
	public GameObject input7;


	void Awake ()
	{
	}

	public void DrillAddNext ()
	{

		Popdown ();

		int col = int.Parse (GameObject.Find ("PopDrillAdd").GetComponent<TempData> ().temp [0]);
		string name = input1.GetComponent<InputField> ().text;

		this.GetComponent<DrillSentakuMaster> ().AddSimple (col, name);

		ResetInputField (1);
	}

	public void DrillAddReset ()
	{
		GameObject.Find ("sky").GetComponent<ButtonPoint> ().OnClick ();

	}

	public void TagPopReset ()
	{
		QuesSentakuMaster qv = this.GetComponent<QuesSentakuMaster> ();

		if (qv.SentakuNull ()) {
			return;
		}

		GameObject go = GameObject.Find ("PopTagColor");
		go.transform.FindChild ("none").GetComponent<ButtonPoint> ().OnClick ();
		Popup (3);
	}

	public void TagLevPopReset (int i = 1)
	{
		GameObject go = GameObject.Find ("PopTagLevel");
		go.GetComponent<TempData> ().ResetTemp ();

		go.transform.Find ("Scroll View").gameObject.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;

		GameObject content = go.transform.Find ("Scroll View/Viewport/Content").gameObject;

		//GameObject col = content.transform.Find ("colSelect").gameObject;
		GameObject lev = content.transform.Find ("levSelect").gameObject;

		switch (Statics.nowTag) {

		case 0:
			content.transform.Find ("none").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 1:
			content.transform.Find ("pink").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 2:
			content.transform.Find ("yellow").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 3:
			content.transform.Find ("green").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 4:
			content.transform.Find ("orange").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 5:
			content.transform.Find ("blue").GetComponent<ButtonPoint> ().OnClick ();
			break;
		default:
			Debug.Log ("Incorrect data");
			break;
		}

		switch (Statics.nowLevel) {

		case 0:
			go.GetComponent<TempData> ().temp [1] = "0";
			lev.GetComponent<RectTransform> ().localPosition = new Vector3 (10000, 0, 0);
			//lev.transform.position = new Vector3 (10000, 0, 0);
			break;
		case 1:
			content.transform.Find ("level1").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 2:
			content.transform.Find ("level2").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 3:
			content.transform.Find ("level3").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 4:
			content.transform.Find ("level4").GetComponent<ButtonPoint> ().OnClick ();
			break;
		case 5:
			content.transform.Find ("level5").GetComponent<ButtonPoint> ().OnClick ();
			break;
		default:
			Debug.Log ("Incorrect data");
			break;
		}

		Popup (i);
	}

	public void DrillPatReset ()
	{
		QuesInputMaster data = this.GetComponent<QuesInputMaster> ();

		switch (data.inputPat) {

		case 0:
			GameObject.Find ("patternB0").GetComponent<ButtonPoint> ().OnClick ();

			break;
		case 1:
			GameObject.Find ("patternB1").GetComponent<ButtonPoint> ().OnClick ();

			break;
		case 2:
			GameObject.Find ("patternB2").GetComponent<ButtonPoint> ().OnClick ();

			break;
		case 3:
			GameObject.Find ("patternB3").GetComponent<ButtonPoint> ().OnClick ();

			break;
		case 4:
			GameObject.Find ("patternB4").GetComponent<ButtonPoint> ().OnClick ();

			break;
		case 5:
			GameObject.Find ("patternB5").GetComponent<ButtonPoint> ().OnClick ();

			break;
		default:
			Debug.Log ("Incorrect data");
			break;
		}

		Popdown (1);
		
	}

	public void Popup (int num = 1)
	{
		GameObject pop;

		if (num == 1) {
			pop = pop1;
		} else if (num == 2) {
			pop = pop2;
		} else if (num == 3) {
			pop = pop3;
		} else if (num == 4) {
			pop = pop4;
		} else if (num == 5) {
			pop = pop5;
		} else if (num == 6) {
			pop = pop6;
		} else if (num == 7) {
			pop = pop7;
		} else if (num == 8) {
			pop = pop8;
		} else if (num == 9) {
			pop = pop9;
		} else if (num == 10) {
			pop = pop10;
		} else if (num == 11) {
			pop = pop11;
		} else if (num == 12) {
			pop = pop12;
		} else {
			Debug.Log ("nonePop");
			pop = pop1;
		}

		pop.transform.position = GameObject.Find ("mainBackGround").transform.position;
	}

	public void Popdown (int num = 1)
	{
		GameObject pop;

		if (num == 1) {
			pop = pop1;
		} else if (num == 2) {
			pop = pop2;
		} else if (num == 3) {
			pop = pop3;
		} else if (num == 4) {
			pop = pop4;
		} else if (num == 5) {
			pop = pop5;
		} else if (num == 6) {
			pop = pop6;
		} else if (num == 7) {
			pop = pop7;
		} else if (num == 8) {
			pop = pop8;
		} else if (num == 9) {
			pop = pop9;
		} else if (num == 10) {
			pop = pop10;
		} else if (num == 11) {
			pop = pop11;
		} else if (num == 12) {
			pop = pop12;
		} else {
			Debug.Log ("nonePop");
			pop = pop1;
		}

		pop.transform.position = new Vector3 (10000, 0, 0);
	}

	public void BG ()
	{
		Debug.Log ("Black click!");
	}

	public void ResetInputField (int num = 1)
	{
		input1.GetComponent<InputField> ().text = "";

		if (num == 2) {
			input2.GetComponent<InputField> ().text = "";
		} else if (num == 3) {
			input2.GetComponent<InputField> ().text = "";
			input3.GetComponent<InputField> ().text = "";
		} else if (num == 4) {
			input2.GetComponent<InputField> ().text = "";
			input3.GetComponent<InputField> ().text = "";
			input4.GetComponent<InputField> ().text = "";
			input5.GetComponent<InputField> ().text = "";
			input6.GetComponent<InputField> ().text = "";
			input7.GetComponent<InputField> ().text = "";
		} else {
		}
	}

	public void PopDrillEdit ()
	{

		GameObject go = GameObject.Find ("PopDrillEdit");
		input1.GetComponent<InputField> ().text = Statics.nowName;

		int num = Statics.nowColor;
		if (num == 0) {
			go.transform.FindChild ("sky").GetComponent<ButtonPoint> ().OnClick ();
		} else if (num == 1) {
			go.transform.FindChild ("pink").GetComponent<ButtonPoint> ().OnClick ();
		} else if (num == 2) {
			go.transform.FindChild ("yellow").GetComponent<ButtonPoint> ().OnClick ();
		} else if (num == 3) {
			go.transform.FindChild ("green").GetComponent<ButtonPoint> ().OnClick ();
		} else if (num == 4) {
			go.transform.FindChild ("orange").GetComponent<ButtonPoint> ().OnClick ();
		} else {
			go.transform.FindChild ("blue").GetComponent<ButtonPoint> ().OnClick ();

		}

		Popup (1);

	}

	public void PopSort ()
	{
		QuesSentakuMaster qv = this.GetComponent<QuesSentakuMaster> ();
		if (qv.GetQuesCount () == 0) {
			return;
		}

		Popup (4);
	}

	public void PopTagSort ()
	{

		QuesSentakuMaster qv = this.GetComponent<QuesSentakuMaster> ();
		if (qv.GetPlainQuesCount () == 0) {
			return;
		}

		GameObject go = GameObject.Find ("PopTagSort");
		go.GetComponent<TempData> ().temp [0] = "0";

		Popup (5);

		go.transform.FindChild ("none").GetComponent<ButtonPoint> ().OnClickTagSort ();
	}

	public void PopupCaution (string t, int i = 4)
	{

		GameObject.Find ("CautionText").GetComponent<Text> ().text = t;
		Popup (i);
	}

	public void PopDrillOrder (int i)
	{
		GameObject parent = GameObject.Find ("PopAnsOrder");

		GameObject drop1 = parent.transform.Find ("OrderDrop1").gameObject;
		GameObject drop2 = parent.transform.Find ("OrderDrop2").gameObject;
		GameObject drop3 = parent.transform.Find ("DummyDrop1").gameObject;
		GameObject drop4 = parent.transform.Find ("DummyDrop2").gameObject;

		drop1.GetComponent<Dropdown> ().value = 1;
		drop2.GetComponent<Dropdown> ().value = 1;
		drop3.GetComponent<Dropdown> ().value = 1;
		drop4.GetComponent<Dropdown> ().value = 1;

		drop1.GetComponent<Dropdown> ().value = 0;
		drop2.GetComponent<Dropdown> ().value = 0;
		drop3.GetComponent<Dropdown> ().value = 0;
		drop4.GetComponent<Dropdown> ().value = 0;

		Popup (i);
	}

	public void PopDrillOrderSet (int i)
	{

		DrillData dd = this.GetComponent<DbProcess> ().GetDbDrillData (Statics.nowDrill);

		GameObject parent = GameObject.Find ("PopAnsOrder");

		GameObject drop1 = parent.transform.Find ("OrderDrop1").gameObject;
		GameObject drop2 = parent.transform.Find ("OrderDrop2").gameObject;
		GameObject drop3 = parent.transform.Find ("DummyDrop1").gameObject;
		GameObject drop4 = parent.transform.Find ("DummyDrop2").gameObject;

		drop1.GetComponent<Dropdown> ().value = 1;
		drop2.GetComponent<Dropdown> ().value = 1;
		drop3.GetComponent<Dropdown> ().value = 1;
		drop4.GetComponent<Dropdown> ().value = 1;

		if (dd.QUES_ORDER == 0) {
			drop1.GetComponent<Dropdown> ().value = 0;
		} else {
			drop1.GetComponent<Dropdown> ().value = (dd.QUES_ORDER - 1);
		}
		drop2.GetComponent<Dropdown> ().value = dd.ANS_ORDER;
		drop3.GetComponent<Dropdown> ().value = dd.DUMMY_USE;
		drop4.GetComponent<Dropdown> ().value = dd.DUMMY_TAG;

		Popup (i);
	}

	public void PopQuesEdit (QuesData qd)
	{
		QuesArray data = DbTextToQA.DbToQA (qd.TEXT);

		Statics.nowTag = qd.TAG;
		Statics.nowLevel = qd.LEVEL;

		GameObject parent = GameObject.Find ("PopQuesEdit");

		parent.GetComponent<TempData> ().temp [5] = qd.ID.ToString ();

		GameObject toggle = parent.transform.Find ("toggle").gameObject;
		GameObject qText = parent.transform.Find ("qText").gameObject;
		GameObject aText = parent.transform.Find ("aText").gameObject;
		GameObject eText = parent.transform.Find ("eText").gameObject;
		GameObject dText = parent.transform.Find ("dText").gameObject;

		if (DbTextToQA.IsPer (data.Ques)) {
			toggle.GetComponent<Toggle> ().isOn = true;
			data.Ques = data.Ques.Substring (QuesTextEdit.PerKeyCommon.Length);
		} else {
			toggle.GetComponent<Toggle> ().isOn = false;
		}


		qText.GetComponent<InputField> ().text = data.Ques;
		eText.GetComponent<InputField> ().text = data.Exp;


		string tempText = "";
		string sepkey = QuesTextEdit.SepKeyCommon;

		foreach (string s in data.Ans) {
			tempText += (sepkey + s);
		}
		aText.GetComponent<InputField> ().text = tempText.Substring (sepkey.Length);

		tempText = "";
		foreach (string s in data.Dummy) {
			tempText += (sepkey + s);
		}
		if (tempText.Length > sepkey.Length) {
			dText.GetComponent<InputField> ().text = tempText.Substring (sepkey.Length);
		}

		toggle.GetComponent<SimpleToTemp> ().SetTempToggle ();
		qText.GetComponent<SimpleToTemp> ().SetTempInputField ();
		aText.GetComponent<SimpleToTemp> ().SetTempInputField ();
		dText.GetComponent<SimpleToTemp> ().SetTempInputField ();
		eText.GetComponent<SimpleToTemp> ().SetTempInputField ();


		Popup (7);
	}

	public void PopImport ()
	{
		GameObject parent = GameObject.Find ("PopImport");
		GameObject text = parent.transform.Find ("importText").gameObject;

		text.GetComponent<InputField> ().text = "";
		Popup (5);
	}

	public void PopExport ()
	{
		GameObject parent = GameObject.Find ("PopExport");
		GameObject text = parent.transform.Find ("exportText").gameObject;

		List<QuesData> qdl = this.GetComponent<DbProcess> ().GetDbDataDrillId (Statics.nowDrill);
		string exText = "";

		foreach (QuesData qd in qdl) {
			exText += ("@@@" + qd.TEXT);
		}

		text.GetComponent<InputField> ().text = exText;
		Popup (10);
	}

	public void PopCopyQues ()
	{
		QuesSentakuMaster qv = this.GetComponent<QuesSentakuMaster> ();

		if (qv.SentakuNull ()) {
			return;
		}


		GameObject parent = GameObject.Find ("PopCopyQues");
		GameObject drop = parent.transform.Find ("drillNameDrop").gameObject;
		GameObject toggle1 = parent.transform.Find ("toggle1").gameObject;
		GameObject toggle2 = parent.transform.Find ("toggle2").gameObject;


		List<DrillData> ddl = this.GetComponent<DbProcess> ().GetDbDrillDataAll ();

		drop.GetComponent<Dropdown> ().options.Clear ();
		drop.GetComponent<Dropdown> ().options.Add (new Dropdown.OptionData { text = "ドリルを選択" });


		foreach (DrillData dd in ddl) {
			drop.GetComponent<Dropdown> ().options.Add (new Dropdown.OptionData { text = dd.NAME });
		}

		drop.GetComponent<Dropdown> ().value = 0;
		drop.GetComponent<Dropdown> ().RefreshShownValue ();

		toggle1.GetComponent<Toggle> ().isOn = false;
		toggle2.GetComponent<Toggle> ().isOn = false;


		Popup (11);

	}

	public void PopHelp (string url)
	{

		WebViewObject webViewObject;

		webViewObject = (new GameObject ("WebViewObject")).AddComponent<WebViewObject> ();
		webViewObject.Init ((msg) => {
			Debug.Log (msg);
		});
		webViewObject.LoadURL (url);
		webViewObject.SetMargins (0, 0, 0, 0);
		webViewObject.SetVisibility (true);
	}

}