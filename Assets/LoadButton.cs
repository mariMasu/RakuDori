using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour
{
	//GameObject es;
	//SceneData sd;

	void Awake ()
	{
		//es = GameObject.Find ("EventSystem");
	}

	public void SetDrillIC ()
	{
		Statics.nowDrill = this.GetComponent<DrillNum> ().id;
		Statics.nowColor = this.GetComponent<DrillNum> ().color;
		Statics.nowName = this.GetComponent<DrillNum> ().nameD;

	}

	public void LoadQuesSentaku ()
	{
		SceneManager.LoadScene ("quesSentaku");

	}

	public void LoadQuesInput ()
	{
		SceneManager.LoadScene ("quesInput");

	}

	public void LoadDrillSentaku ()
	{
		SceneManager.LoadScene ("drillSentaku");

	}

	public void LoadDrillAns ()
	{
		SceneManager.LoadScene ("drillAns");

	}

	public void LoadTitle ()
	{
		SceneManager.LoadScene ("title");

	}
}
