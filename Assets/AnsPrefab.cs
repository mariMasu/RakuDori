using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnsPrefab : MonoBehaviour
{

	public GameObject back;
	public GameObject text;
	public string ansText = "";
	public string id = "";


	public IEnumerator SetTextC1 ()
	{  

		yield return StartCoroutine ("SetTextC2");  

		Vector2 rt = text.GetComponent<RectTransform> ().sizeDelta;

		rt.x += Statics.prefabX;

		back.GetComponent<RectTransform> ().sizeDelta = rt;
		this.GetComponent<LayoutElement> ().minWidth = (rt.x + (Statics.prefabX * 0.2f));
		this.GetComponent<LayoutElement> ().minHeight = (rt.y + (Statics.prefabX * 0.2f));

	}

	private IEnumerator SetTextC2 ()
	{  
		text.GetComponent<Text> ().text = ansText;
		yield return new WaitForSeconds (0.1f);  

	}

	public void AddSentaku ()
	{
		GameObject es = GameObject.Find ("EventSystem");

		string timeID = TimeFunctions.GetTimeID ();

		string[] ans = { timeID, ansText };

		es.GetComponent<DrillAnsMaster> ().senAnsList.Add (ans);
		es.GetComponent<DrillAnsMaster> ().ViewNowSentaku ();
	}

	public void RemoveSentaku ()
	{
		GameObject es = GameObject.Find ("EventSystem");

		DrillAnsMaster dm = es.GetComponent<DrillAnsMaster> ();

		int i = dm.SearchTimeID (id);

		dm.senAnsList.RemoveAt (i);
		es.GetComponent<DrillAnsMaster> ().ViewNowSentaku ();
	}
}
