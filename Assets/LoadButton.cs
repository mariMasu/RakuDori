using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour
{

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

	public void LoadDrillAnsSingle ()
	{
		Statics.ResetReviewList ();
		Statics.AddReviewList ();
		SceneManager.LoadScene ("drillAns");

	}

	public void LoadTitle ()
	{
		SceneManager.LoadScene ("title");

	}
}
