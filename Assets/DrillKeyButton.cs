using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrillKeyButton : MonoBehaviour
{

	public GameObject baseOb;
	TempData temp;

	public GameObject input;
	public string defKey;
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