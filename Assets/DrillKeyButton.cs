using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class DrillKeyButton : MonoBehaviour
{

	public GameObject baseOb;
	TempData temp;

	public GameObject input;
	public string defKey;
	public string dataName;
	public int myNum;

	void Awake ()
	{
		temp = baseOb.GetComponent<TempData> ();
	}

	public void SetTemp ()
	{
		switch ((this.GetComponent<Dropdown> ().captionText).text) {

		case "設定する":
			InputOn ();
			break;
		case "設定しない":
			temp.SetTemp (null, myNum);
			InputOff ();
			break;
		case "スペース(半角)":
			temp.SetTemp (" ", myNum);
			InputOff ();
			break;
		case "スペース(全角)":
			temp.SetTemp ("　", myNum);
			InputOff ();
			break;
		case "改行":
			temp.SetTemp ("\n", myNum);
			InputOff ();
			break;
		case "２改行":
			temp.SetTemp ("\n\n", myNum);
			InputOff ();
			break;
		case "３改行":
			temp.SetTemp ("\n\n\n", myNum);
			InputOff ();
			break;
		case "カスタム":
			SceneData sd = GameObject.Find ("EventSystem").GetComponent<SceneData> ();
			input.GetComponent<InputField> ().text = sd.GetDataByText (dataName);

			InputOn ();
			break;
		case "なし":
			temp.SetTemp (null, myNum);
			InputOff ();

			break;
		default:
			temp.SetTemp (defKey, myNum);
			InputOff ();
			break;
		}

	}

	public void ChangeDropByText ()
	{
		SceneData sd = GameObject.Find ("EventSystem").GetComponent<SceneData> ();

		List<string> dropText = new List<string> ();
		List<Dropdown.OptionData> dropList = this.GetComponent<Dropdown> ().options;


		if (dataName == "Sentaku") {
			
			string senText = sd.Sentaku;

			if (SceneData.strNull (senText) == true) {
				this.GetComponent<Dropdown> ().value = 0;
				InputOff ();
			} else {
				this.GetComponent<Dropdown> ().value = 1;
				InputOn ();
				input.GetComponent<InputField> ().text = senText;
			}

			this.SetCustom ();

		} else {

			foreach (Dropdown.OptionData s in dropList) {
				dropText.Add (s.text);
			}

			string ds = SceneData.SpaceString (sd.GetDataByText (dataName));

			if (ds == "半スペ") {
				ds = "スペース(半角)";
			} else if (ds == "全スペ") {
				ds = "スペース(全角)";
			}

			int index = dropText.IndexOf (ds);

			if (index > -1) {
				this.GetComponent<Dropdown> ().value = index;
				this.SetTemp ();
			} else {
				this.GetComponent<Dropdown> ().value = dropText.IndexOf ("カスタム");
				this.SetTemp ();

				input.GetComponent<InputField> ().text = ds;
				SetCustom ();

			}
		}
	}

	public void NullTemp ()
	{
		temp.SetTemp (null, myNum);
		InputOff ();
	}

	public void SetCustom ()
	{
		string t = input.GetComponent<InputField> ().text;
		temp.SetTemp (t, myNum);
	}

	public void InputOn ()
	{
		input.GetComponent<InputField> ().text = null;
		input.SetActive (true);
	}

	public void InputOff ()
	{
		input.SetActive (false);
	}
}