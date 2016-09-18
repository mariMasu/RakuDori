using UnityEngine;
using System.Collections;

public class ButtonPoint : MonoBehaviour {

	public GameObject select;
	GameObject es;

	public string number = "0";
	public string type;

	void Awake() {
		es = GameObject.Find ("EventSystem");	
	}

	public void OnClick() {
		Debug.Log(this.name + "click");

		select.transform.position = this.transform.position;

		SceneData sd = es.GetComponent<SceneData>();
		sd.SetData (number, type);

	}
}