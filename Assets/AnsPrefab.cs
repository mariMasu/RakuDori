using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnsPrefab : MonoBehaviour
{

	public GameObject back;
	public GameObject text;
	public string ansText = "";
	public string id = "";

	int textR = 13;

	public IEnumerator SetTextC1 ()
	{  
		if (Statics.HaveKaigyo (ansText) != true && ansText.Length >= textR) {

			string editText = ansText;

			int leng = ansText.Length;
			int gyo = leng / textR;
			string[] stra = new string[gyo + 1];

			for (int i = 0; i < gyo + 1; i++) {

				if (i == gyo) {
					stra [i] = editText;
				} else {
					stra [i] = editText.Substring (0, textR);
					editText = editText.Substring (textR);
				}
			}

			editText = "";

			foreach (string s in stra) {
				editText += "\n" + s;
			}

			ansText = editText.Substring (1);

		}

		yield return StartCoroutine ("SetTextC2");  

		Vector2 rt = text.GetComponent<RectTransform> ().sizeDelta;

		rt.x += Statics.prefabGap;
		rt.y += (Statics.prefabGap * 0.4f);

		back.GetComponent<RectTransform> ().sizeDelta = rt;

		Vector2 nsize = new Vector2 (rt.x + (Statics.prefabGap * 0.8f), rt.y + (Statics.prefabGap * 0.8f));
		this.GetComponent<RectTransform> ().sizeDelta = nsize;

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
