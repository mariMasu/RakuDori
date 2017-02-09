using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class AnsPrefab : MonoBehaviour
{

	public GameObject back;
	public GameObject text;
	public string ansText = "";
	public string id = "";

	int textR = 24;

	public IEnumerator SetTextC1 ()
	{  

		char[] chars = ansText.ToCharArray ();
		int[] charsByte = Statics.GetByteLengthSJis (chars);

		int ansLen = Statics.IntSum (charsByte);

		if (Statics.HaveKaigyo (ansText) == false && ansLen > textR) {

			string editText = ansText;

			List<string> strl = new List<string> ();
			int gyoLength = 0;
			int strLength = 0;

			for (int i = 0; i < chars.Length; i++) {
				strLength += 1;
				gyoLength += charsByte [i];

				if (gyoLength > textR) {
					strl.Add (editText.Substring (0, strLength));
					editText = editText.Substring (strLength);

					strLength = 0;
					gyoLength = 0;
				}
			}

			if (editText.Length > 0) {
				strl.Add (editText);
			}

			editText = "";

			foreach (string s in strl) {
				editText += "\n" + s;
			}

			ansText = editText.Substring (1);

		}

		yield return StartCoroutine ("SetTextC2");  

		Vector2 rt = text.GetComponent<RectTransform> ().sizeDelta;

		rt.x += (Statics.prefabGap * 2.2f);
		rt.y += (Statics.prefabGap * 0.4f);

		back.GetComponent<RectTransform> ().sizeDelta = rt;

		Vector2 nsize = new Vector2 (rt.x + (Statics.prefabGap * 1.1f), rt.y + (Statics.prefabGap * 0.8f));
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
