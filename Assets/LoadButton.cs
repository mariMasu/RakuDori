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
		SceneData.nowDrill = this.GetComponent<DrillNum> ().id;
		SceneData.nowColor = this.GetComponent<DrillNum> ().color;
		SceneData.nowName = this.GetComponent<DrillNum> ().nameD;

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

	public void LoadTitle ()
	{
		SceneManager.LoadScene ("title");

	}
}
