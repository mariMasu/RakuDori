using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrillKeyState : MonoBehaviour {

	public GameObject text1;
	public GameObject text2;
	public GameObject text3;
	public GameObject sepDrop;
	public GameObject sepInput;
	public GameObject dummyDrop;
	public GameObject dummyInput;
	public GameObject ansInput;
	public GameObject ansDrop;
	public GameObject quesInput;
	public GameObject quesDrop;

	void Awake() {
		DrillKeyPatView ();
		PopDrillKeyView ();
	}

	public void DrillKeyPatView(){
		SceneData sd = this.GetComponent<SceneData>();

		if (sd.InputPat == 0) {
			text1.GetComponent<Text> ().text = "問題\u3000\u3000\u3000\u3000ダミー\n\n回答";
			text2.GetComponent<Text> ().text = "問題\n↓\n回答";
		} else {
			text1.GetComponent<Text> ().text = "問題\u3000\u3000\u3000\u3000ダミー\n\n回答\u3000\u3000\u3000\u3000回答区";
			text2.GetComponent<Text> ().text = "全問題\n↓\n全回答";
		}

	}

	public void PopDrillKeyView(){
		SceneData sd = this.GetComponent<SceneData>();

		if (sd.InputPat == 0) {
			sepDrop.transform.position = new Vector3 (Screen.width * 2, 0, 0);
			sepInput.transform.position = new Vector3 (Screen.width * 2, 0, 0);
			dummyDrop.transform.position = new Vector3 (-330, -256, 0);
			dummyInput.transform.position = new Vector3 (553, -256, 0);

			text3.GetComponent<Text> ().text = "・問題文の開始\n\n・回答文の開始\n\n・ダミー回答の開始";
		}else{
			sepDrop.transform.position = new Vector3 (-330, -256, 0);
			sepInput.transform.position = new Vector3 (553, -256, 0);
			dummyDrop.transform.position = new Vector3 (-330, -680, 0);
			dummyInput.transform.position = new Vector3 (553, -680, 0);

			text3.GetComponent<Text> ().text = "・問題文の開始\n\n・回答文の開始\n\n・回答内の分割\n\n・ダミー回答の開始";
		}
	}


	public void DropViewReset(){
		quesDrop.GetComponent<Dropdown> ().value = 0;
		ansDrop.GetComponent<Dropdown> ().value = 0;
		dummyDrop.GetComponent<Dropdown> ().value = 0;
		sepDrop.GetComponent<Dropdown> ().value = 0;

	}
}
