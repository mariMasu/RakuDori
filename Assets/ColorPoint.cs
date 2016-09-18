using UnityEngine;
using System.Collections;

public class ColorPoint : MonoBehaviour {

	GameObject es;

	[SerializeField, Range(0, 5)]
	private int color;

	void Awake() {
		es = GameObject.Find ("EventSystem");	
	}

	public void OnClick() {
		Debug.Log(this.name + "click");

		GameObject point = GameObject.Find ("colSelect");
		point.transform.position = this.transform.position;

		PopupWindow bi = es.GetComponent<PopupWindow>();
		bi.ColNum = color;

	}
}