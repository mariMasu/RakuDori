using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrillKeyState : MonoBehaviour
{

	public GameObject keyText1;
	public GameObject senText;
	public GameObject keyText2;
	public GameObject patText;
	public GameObject popKeyText;
	public GameObject senDrop;
	public GameObject senInput;
	public GameObject sepDrop;
	public GameObject sepInput;
	public GameObject dummyDrop;
	public GameObject dummyInput;
	public GameObject ansInput;
	public GameObject ansDrop;
	public GameObject quesInput;
	public GameObject quesDrop;
	public GameObject expDrop;
	public GameObject expInput;
	public GameObject perDrop;
	public GameObject perInput;

	public GameObject[] drops = new GameObject[7];
	public GameObject[] inputs = new GameObject[7];

	void Awake ()
	{
		DrillKeyPatView ();

		drops [0] = senDrop;
		drops [1] = quesDrop;
		drops [2] = ansDrop;
		drops [3] = sepDrop;
		drops [4] = expDrop;
		drops [5] = dummyDrop;
		drops [6] = perDrop;

		inputs [0] = senInput;
		inputs [1] = quesInput;
		inputs [2] = ansInput;
		inputs [3] = sepInput;
		inputs [4] = expInput;
		inputs [5] = dummyInput;
		inputs [6] = perInput;
	}

	public void DrillKeyPatView ()
	{
		SceneData sd = this.GetComponent<SceneData> ();


		if (sd.InputPat == 0 || sd.InputPat == 1) {
			
			string A2 = SpaceString (sd.SepKey);
			string Q = SpaceString (sd.QuesKey);
			string D = SpaceString (sd.DummyKey);
			string P = SpaceString (sd.PerKey);

			if (SceneData.strNull (sd.Sentaku)) {
				keyText1.GetComponent<Text> ().text = "区切り\n\n正答、ダミー内の分割\n\nダミー選択肢\n\n正答の順序を守る";
				keyText2.GetComponent<Text> ().text = Q + "\n\n" + A2 + "\n\n" + D + "\n\n" + P;

			} else {
				string S = sd.Sentaku;

				senText.GetComponent<Text> ().text = S;

				keyText1.GetComponent<Text> ().text = "選択肢\n\n\n区切り\n\n正答、ダミー内の分割\n\nダミー選択肢\n\n正答の順序を守る";
				keyText2.GetComponent<Text> ().text = "\n\n\n" + Q + "\n\n" + A2 + "\n\n" + D + "\n\n" + P;
			
			}
			if (sd.InputPat == 0) {
				patText.GetComponent<Text> ().text = "問→正\n→問...";
			} else {
				patText.GetComponent<Text> ().text = "全問\n→全正";
			}


		} else {
			string A1 = SpaceString (sd.AnsKey);
			string A2 = SpaceString (sd.SepKey);
			string Q = SpaceString (sd.QuesKey);
			string E = SpaceString (sd.ExpKey);
			string D = SpaceString (sd.DummyKey);
			string P = SpaceString (sd.PerKey);

			if (SceneData.strNull (sd.Sentaku)) {
				keyText1.GetComponent<Text> ().text = "問題の区切り\n\n正答の開始\n\n正答、ダミー内の分割\n\n解説の開始\n\nダミー選択肢\n\n正答の順序を守る";
				keyText2.GetComponent<Text> ().text = Q + "\n\n" + A1 + "\n\n" + A2 + "\n\n" + E + "\n\n" + D + "\n\n" + P;

			} else {
				string S = sd.Sentaku;

				senText.GetComponent<Text> ().text = S;

				keyText1.GetComponent<Text> ().text = "選択肢\n\n\n問題の区切り\n\n正答の開始\n\n正答、ダミー内の分割\n\n解説の開始\n\nダミー選択肢\n\n正答の順序を守る";
				keyText2.GetComponent<Text> ().text = "\n\n\n" + Q + "\n\n" + A1 + "\n\n" + A2 + "\n\n" + E + "\n\n" + D + "\n\n" + P;

			}

			if (sd.InputPat == 2) {
				patText.GetComponent<Text> ().text = "問→正→\n解→問...";
			} else if (sd.InputPat == 3) {
				patText.GetComponent<Text> ().text = "問→解→\n正→問...";
			} else if (sd.InputPat == 4) {
				patText.GetComponent<Text> ().text = "全問→\n正→解→\n正...";
			} else if (sd.InputPat == 5) {
				patText.GetComponent<Text> ().text = "全問→\n解→正→\n解...";
			}
		}
	}

	public void PopDrillKeyView ()
	{
		SceneData sd = this.GetComponent<SceneData> ();

		float gapX = ansInput.transform.position.x - ansDrop.transform.position.x;
		float gapY = senInput.transform.position.y - quesDrop.transform.position.y;
		gapY = (float)(gapY * 1.15);
		Vector3 quesPos = quesDrop.transform.position;

		if (sd.InputPat == 0 || sd.InputPat == 1) {
			quesDrop.GetComponent<Dropdown> ().options [0].text = "改行";

			ansDrop.transform.position = new Vector3 (Screen.width * 2, 0, 0);
			ansInput.transform.position = new Vector3 (Screen.width * 2, 0, 0);
			expDrop.transform.position = new Vector3 (Screen.width * 2, 0, 0);
			expInput.transform.position = new Vector3 (Screen.width * 2, 0, 0);

			sepDrop.transform.position = new Vector3 (quesPos.x, quesPos.y - gapY, 0);
			sepInput.transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY, 0);
			dummyDrop.transform.position = new Vector3 (quesPos.x, quesPos.y - gapY * 2, 0);
			dummyInput.transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY * 2, 0);
			perDrop.transform.position = new Vector3 (quesPos.x, quesPos.y - gapY * 3, 0);
			perInput.transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY * 3, 0);

			popKeyText.GetComponent<Text> ().text = "・選択式問題の選択肢設定\n\n\n・問題文の区切り\n\n・正答、ダミー内の分割\n\n・ダミー選択肢の開始\n\n・正答の順序を守る問題";
		} else {
			quesDrop.GetComponent<Dropdown> ().options [0].text = "@@(半角)";

			int m = 1;

			for (int i = 2; i < 7; i++) {
				drops [i].transform.position = new Vector3 (quesPos.x, quesPos.y - gapY * m, 0);
				inputs [i].transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY * m, 0);

				m++;
			}
//
//			ansDrop.transform.position = new Vector3 (quesPos.x, quesPos.y - gapY, 0);
//			ansInput.transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY, 0);
//			sepDrop.transform.position = new Vector3 (quesPos.x, quesPos.y - gapY * 2, 0);
//			sepInput.transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY * 2, 0);
//			expDrop.transform.position = new Vector3 (quesPos.x, quesPos.y - gapY * 3, 0);
//			expInput.transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY * 3, 0);
//			dummyDrop.transform.position = new Vector3 (quesPos.x, quesPos.y - gapY * 4, 0);
//			dummyInput.transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY * 4, 0);
//			perDrop.transform.position = new Vector3 (quesPos.x, quesPos.y - gapY * 5, 0);
//			perInput.transform.position = new Vector3 (quesPos.x + gapX, quesPos.y - gapY * 5, 0);

			popKeyText.GetComponent<Text> ().text = "・選択式問題の選択肢設定\n\n\n・問題文の区切り\n\n・正答文の開始\n\n・正答、ダミー内の分割\n\n・解説文の開始\n\n・ダミー選択文の開始\n\n・正答の順序を守る問題";
		}
		quesDrop.GetComponent<Dropdown> ().value = 1;
		quesDrop.GetComponent<Dropdown> ().value = 0;
		DropViewReset ();
	}


	public void DropViewReset ()
	{

		foreach (GameObject d in drops) {
			d.GetComponent<Dropdown> ().value = 0;
		}

//		senDrop.GetComponent<Dropdown> ().value = 0;
//		quesDrop.GetComponent<Dropdown> ().value = 0;
//		ansDrop.GetComponent<Dropdown> ().value = 0;
//		dummyDrop.GetComponent<Dropdown> ().value = 0;
//		sepDrop.GetComponent<Dropdown> ().value = 0;
//		expDrop.GetComponent<Dropdown> ().value = 0;
//		perDrop.GetComponent<Dropdown> ().value = 0;

		SetStartTemp ();

	}

	public void SetStartTemp ()
	{

		foreach (GameObject d in drops) {
			d.GetComponent <DrillKeyButton> ().SetTemp ();
		}


//		senDrop.GetComponent <DrillKeyButton> ().SetTemp ();
//		quesDrop.GetComponent <DrillKeyButton> ().SetTemp ();
//		ansDrop.GetComponent<DrillKeyButton> ().SetTemp ();
//		dummyDrop.GetComponent<DrillKeyButton> ().SetTemp ();
//		sepDrop.GetComponent <DrillKeyButton> ().SetTemp ();
//		expDrop.GetComponent <DrillKeyButton> ().SetTemp ();
//		perDrop.GetComponent <DrillKeyButton> ().SetTemp ();
//
//		SceneData sd = this.GetComponent<SceneData> ();
//
//		if (sd.InputPat == 0) {
//			sepDrop.GetComponent<DrillKeyButton> ().NullTemp ();
//		} else {
//			sepDrop.GetComponent<DrillKeyButton> ().SetTemp ();
//		}

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
		} else if (SceneData.strNull (s)) {
			s = "なし";
			return s;
		} else {
			return s;
		}
	}

	public bool CustomCheck ()
	{
		foreach (GameObject i in inputs) {
			if (i.activeSelf && (i.GetComponent<InputField> ().text == "")) {
				return false;
			}
		}

		return true;
	}
}
