using UnityEngine;
using System.Collections;

public class ButtonPoint : MonoBehaviour
{

	public GameObject select;
	public GameObject baseOb;
	TempData temp;

	public string data;
	public int tempNum;

	void Awake ()
	{
		temp = baseOb.GetComponent<TempData> ();
	}

	public void OnClick ()
	{
		//Debug.Log (this.name + "click");

		select.transform.position = this.transform.position;

		temp.SetTemp (data, tempNum);

	}

	public void OnClickTagSort ()
	{
		//Debug.Log (this.name + "click");

		if (temp.temp [0] == data) {
			select.transform.position = new Vector3 (Screen.width * 2, 0, 0);

			temp.SetTemp ("6", tempNum);
		} else {

			select.transform.position = this.transform.position;

			temp.SetTemp (data, tempNum);
		}

	}
}