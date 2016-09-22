using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TempData : MonoBehaviour {

	GameObject es;
	SceneData sd;

	private string[] temp = new string[5];

	void Awake() {
		es = GameObject.Find ("EventSystem");
		sd = es.GetComponent<SceneData>();
	}

	public void DrillAddSend() {

		if (temp [0] != null) {
			sd.SetData (temp [0], "ColNum");
		}

	}

	public void DrillPatSend() {

		if (temp [0] != null) {
			sd.SetData (temp [0], "InputPat");
		}
		es.GetComponent<DrillKeyState> ().DrillKeyPatView ();
		es.GetComponent<DrillKeyState> ().PopDrillKeyView ();
		es.GetComponent<PopupWindow> ().Popdown (1);
	}

	public void DrillKeySend() {

		HashSet<string> hs = new HashSet<string>();

		for (int i = 0; i < 4; i++) {
			if ((hs.Add (temp [i]) == false)) {
				Debug.Log ("zyuuhukuKey");
				return;
			}
		}

			sd.SetData (temp [0], "QuesKey");
			sd.SetData (temp [1], "AnsKey1");
			sd.SetData (temp [2], "DummyKey");
			sd.SetData (temp [3], "AnsKey2");

		es.GetComponent<DrillKeyState> ().DrillKeyPatView ();
		es.GetComponent<DrillKeyState> ().DropViewReset ();
		es.GetComponent<PopupWindow> ().ResetInputField (4);
		es.GetComponent<PopupWindow> ().Popdown (2);
	}

	public void SetTemp (string data, int i)
	{

		temp [i] = data;
	}

	public void ResetTemp(){
		for (int i = 0; i < 5; i++) {
			temp [i] = null;
		}
	}
}