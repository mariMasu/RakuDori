using UnityEngine;
using System.Collections;
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

	public void TagLevPopReset ()
	{
		GameObject go = GameObject.Find ("PopTagColor");

		go.GetComponent<TempData> ().ResetTemp ();
		GameObject col = go.transform.Find ("Scroll View/Viewport/Content/colSelect").gameObject;
		GameObject lev = go.transform.Find ("Scroll View/Viewport/Content/levSelect").gameObject;
		col.transform.position = new Vector3 (10000, 0, 0);
		lev.transform.position = new Vector3 (10000, 0, 0);

		Popup (1);
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
		} else {
			Debug.Log ("nonePop");
			pop = pop1;
		}

		pop.transform.position = new Vector3 (Screen.width * 2, 0, 0);
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

		Popup (1);

		go.transform.FindChild ("sky").GetComponent<ButtonPoint> ().OnClick ();

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
		GameObject go = GameObject.Find ("PopTagSort");
		go.GetComponent<TempData> ().temp [0] = "0";

		Popup (5);

		go.transform.FindChild ("none").GetComponent<ButtonPoint> ().OnClickTagSort ();
	}

	public void PopupCaution (string t)
	{
		GameObject pop = pop4;

		GameObject.Find ("CautionText").GetComponent<Text> ().text = t;
		pop.transform.position = GameObject.Find ("backGround").transform.position;
	}

	public void PopDrillOrder (int i)
	{
		GameObject parent = GameObject.Find ("PopAnsOrder");

		GameObject drop1 = parent.transform.Find ("OrderDrop1").gameObject;
		GameObject drop2 = parent.transform.Find ("DummyDrop1").gameObject;
		GameObject drop3 = parent.transform.Find ("DummyDrop2").gameObject;
		GameObject drop4 = parent.transform.Find ("OrderDrop2").gameObject;

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

	public void PopQuesEdit (int id)
	{
		QuesData qd = this.GetComponent<DbProcess> ().GetDbData (id);
		QuesArray data = DbTextToQA.DbToQA (qd.TEXT);

		GameObject parent = GameObject.Find ("PopQuesEdit");

		parent.GetComponent<TempData> ().temp [5] = id.ToString ();

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

}