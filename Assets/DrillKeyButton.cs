using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrillKeyButton : MonoBehaviour {

	public GameObject baseOb;
	TempData temp;

	public GameObject input;
	public string defKey;
	public int myNum;

	void Awake() {
		temp = baseOb.GetComponent<TempData> ();
	}

	public void SetTemp(){
		switch (this.GetComponent<Dropdown>().value) {

		case 0:
			temp.SetTemp (defKey, myNum);
			InputOff ();
			break;
		case 1:
			temp.SetTemp ("　", myNum);
			InputOff ();
			break;
		case 2:
			temp.SetTemp (" ", myNum);
			InputOff ();
			break;
		case 3:
			temp.SetTemp ("\n", myNum);
			InputOff ();
			break;
		case 4:
			InputOn ();

			break;
		default:
			Debug.Log ("Incorrect data");
			break;
		}

	}

	public void SetCustom(){
		string t = input.GetComponent<InputField> ().text;
		temp.SetTemp (t, myNum);
	}

	public void InputOn(){
		input.GetComponent<InputField> ().text = null;
		input.SetActive (true);
	}

	public void InputOff(){
		input.SetActive (false);
	}
}