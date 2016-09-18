using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{

	public GameObject pop;

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
		ResetColor ();
	}

	public void Popup ()
	{
		pop.transform.position = new Vector3 (0, 0, 0);
	}

	public void Popdown ()
	{
		pop.transform.position = new Vector3 (Screen.width * 2, 0, 0);
	}

	public void BG ()
	{
		Debug.Log ("Black click!");
	}

	public void ResetInputField (int num)
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

	public void ResetColor ()
	{
		GameObject.Find ("sky").GetComponent<ButtonPoint> ().OnClick ();

	}

}