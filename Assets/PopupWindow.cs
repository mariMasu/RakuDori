using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{

	public GameObject pop;
	public GameObject input;

	[SerializeField]
	private string _text;

	[SerializeField]
	private int _colNum = 0;

	public string InputText {
		get {
			return _text;
		}
		set {
			_text = value;
		}
	}

	public int ColNum {
		get {
			return _colNum;
		}
		set {
			_colNum = value;
		}
	}

	public void DrillSentakuNext ()
	{

		Popdown ();
		InputText = input.GetComponent<InputField> ().text;

		this.GetComponent<DrillAdd> ().AddSimple ();

		ResetInputField ();
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

	public void ResetInputField ()
	{
		input.GetComponent<InputField> ().text = "";
	}

	public void ResetColor ()
	{
		GameObject.Find ("sky").GetComponent<ColorPoint> ().OnClick ();

	}

}