using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{

	public GameObject pop1;
	public GameObject pop2;
	public GameObject pop3;
	public GameObject pop4;

	public GameObject input1;
	public GameObject input2;
	public GameObject input3;
	public GameObject input4;


	SceneData data;

	void Awake() {
		data = this.GetComponent<SceneData> ();
	}

	public void DrillAddNext ()
	{

		Popdown ();
		data.SetData (input1.GetComponent<InputField> ().text, "DrillName");

		this.GetComponent<DrillAdd> ().AddSimple ();

		ResetInputField (1);
		DrillAddReset ();
	}

	public void DrillAddReset ()
	{
		GameObject.Find ("sky").GetComponent<ButtonPoint> ().OnClick ();

	}

	public void DrillPatReset(){

		if (data.InputPat == 0) {
			GameObject.Find ("patternB0").GetComponent<ButtonPoint> ().OnClick ();
		} else {
			GameObject.Find ("patternB1").GetComponent<ButtonPoint> ().OnClick ();
		}

		Popdown (1);
		
	}

	public void Popup (int num = 1)
	{
		GameObject pop;

		if (num == 2) {
			pop = pop2;
		} else if (num == 3) {
			pop = pop3;
		} else if (num == 4) {
			pop = pop4;
		} else {
			pop = pop1;
		}

		pop.transform.position = GameObject.Find ("backGround").transform.position;
	}

	public void Popdown (int num = 1)
	{
		GameObject pop;

		if (num == 2) {
			pop = pop2;
		} else if (num == 3) {
			pop = pop3;
		} else if (num == 4) {
			pop = pop4;
		} else {
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
		} else {
		}
	}

}