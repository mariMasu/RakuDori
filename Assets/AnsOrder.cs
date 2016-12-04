using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnsOrder : MonoBehaviour
{

	public GameObject baseOb;
	TempData temp;

	public int tempNum;

	void Awake ()
	{
		temp = baseOb.GetComponent<TempData> ();
	}

	public void SetTemp ()
	{
		switch ((this.GetComponent<Dropdown> ().captionText).text) {

		case "ランダム":
			temp.SetTemp ("1", tempNum);
			break;
		case "昇順":
			temp.SetTemp ("2", tempNum);
			break;
		case "降順":
			temp.SetTemp ("3", tempNum);
			break;
		case "ダミーなし":
			temp.SetTemp ("0", tempNum);
			break;
		case "このドリルから":
			temp.SetTemp ("1", tempNum);
			break;
		case "全ドリルから":
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
