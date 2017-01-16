using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SimpleToTemp : MonoBehaviour
{
	
	public GameObject baseOb;
	TempData temp;

	public GameObject connectOb;


	public int tempNum;

	void Awake ()
	{
		temp = baseOb.GetComponent<TempData> ();
	}

	public void SetTempInputField ()
	{
		string text = this.GetComponent<InputField> ().text;

		if (Statics.StrNull (text)) {
			temp.temp [tempNum] = "";
		} else {
			temp.temp [tempNum] = text;

		}

	}

	public void SetTempToggle ()
	{
		bool b = this.GetComponent<Toggle> ().isOn;

		if (b == true) {
			temp.temp [tempNum] = "1";
		} else {
			temp.temp [tempNum] = "0";

		}

	}

	public void SetTempDrillName ()
	{
		string name = this.GetComponent<Dropdown> ().captionText.text;

		temp.temp [tempNum] = name;
	}

	public void SetTempAnsOrder ()
	{
		Dropdown dd = this.GetComponent<Dropdown> ();

		switch (dd.captionText.text) {

		case "ランダム":
			temp.SetTemp ("1", tempNum);
			break;
		case "abc順":
			temp.SetTemp ("2", tempNum);
			break;
		case "登録順":
			temp.SetTemp ("3", tempNum);
			break;
		case "昇順":
			temp.SetTemp ("2", tempNum);
			break;
		case "降順":
			temp.SetTemp ("3", tempNum);
			break;
		case "ダミーなし":

			if (connectOb != null) {
				connectOb.SetActive (false);
			}

			temp.SetTemp ("0", tempNum);
			break;
		case "このドリルから":
			if (connectOb != null) {
				connectOb.SetActive (true);
			}

			temp.SetTemp ("1", tempNum);
			break;
		case "全ドリルから":
			if (connectOb != null) {
				connectOb.SetActive (true);
			}

			temp.SetTemp ("2", tempNum);
			break;
		case "タグ問わず":
			temp.SetTemp ("0", tempNum);
			break;
		case "タグなし":
			temp.SetTemp ("1", tempNum);
			break;
		case "ピンク":
			temp.SetTemp ("2", tempNum);
			break;
		case "イエロー":
			temp.SetTemp ("3", tempNum);
			break;
		case "グリーン":
			temp.SetTemp ("4", tempNum);
			break;
		case "オレンジ":
			temp.SetTemp ("5", tempNum);
			break;
		case "ブルー":
			temp.SetTemp ("6", tempNum);
			break;
		default:
			temp.SetTemp ("0", tempNum);
			break;
		}

	}
}
