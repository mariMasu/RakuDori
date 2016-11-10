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
	public GameObject input5;
	public GameObject input6;
	public GameObject input7;

	SceneData data;

	void Awake ()
	{
		data = this.GetComponent<SceneData> ();
	}

	public void DrillAddNext ()
	{

		Popdown ();
		data.SetData (input1.GetComponent<InputField> ().text, "DrillName");

		this.GetComponent<DrillAdd> ().AddSimple ();

		ResetInputField (1);


	}

	public void DrillAddReset ()
	{
		GameObject.Find ("sky").GetComponent<ButtonPoint> ().OnClick ();

	}

	public void DrillPatReset ()
	{

		switch (data.InputPat) {

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
			input5.GetComponent<InputField> ().text = "";
			input6.GetComponent<InputField> ().text = "";
			input7.GetComponent<InputField> ().text = "";
		} else {
		}
	}

	public void PopupCaution (string t)
	{
		GameObject pop = pop4;

		GameObject.Find ("CautionText").GetComponent<Text> ().text = t;
		pop.transform.position = GameObject.Find ("backGround").transform.position;
	}

}