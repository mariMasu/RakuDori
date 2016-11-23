using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TempData : MonoBehaviour
{

	GameObject es;
	SceneData sd;
	[SerializeField]
	private string[] temp = new string[7];

	void Awake ()
	{
		es = GameObject.Find ("EventSystem");
		sd = es.GetComponent<SceneData> ();
	}

	public void DrillAddSend ()
	{

		if (temp [0] != "") {
			sd.SetData (temp [0], "ColNum");
		}

	}

	public void DrillPatSend ()
	{

		if (temp [0] != "") {
			sd.SetData (temp [0], "InputPat");
		}

		if (sd.InputPat < 2) {
			sd.SetData ("\n", "QuesKey");
		} else {
			sd.SetData ("@@", "QuesKey");

		}
		es.GetComponent<DrillKeyTextDrops> ().DrillKeyPatView ();
	}

	public void DrillKeySend ()
	{

		if (KeyCheck () == false) {
			return;
		}

		sd.SetData (temp [0], "Sentaku");
		sd.SetData (temp [1], "QuesKey");
		sd.SetData (temp [2], "AnsKey");
		sd.SetData (temp [3], "SepKey");
		sd.SetData (temp [4], "ExpKey");
		sd.SetData (temp [5], "DummyKey");
		sd.SetData (temp [6], "PerKey");

		es.GetComponent<DrillKeyTextDrops> ().DrillKeyPatView ();
		es.GetComponent<DrillKeyTextDrops> ().DropViewReset ();
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
			temp [i] = null;
		}
	}

	public bool KeyCheck ()
	{

		if (es.GetComponent<DrillKeyTextDrops> ().CustomCheck () == false) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n入力されていない\nカスタムがあります");
			return false;
		}

		HashSet<string> hs = new HashSet<string> ();
		//string[] used = {"###","$$$","%%%","&&&"};

		for (int i = 0; i < 7; i++) {

			if (SceneData.strNull (temp [i])) {
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
}