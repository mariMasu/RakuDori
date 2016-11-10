using UnityEngine;
using System.Collections;

public class SceneData : MonoBehaviour
{

	[SerializeField]
	private string _drillName;

	[SerializeField]
	private int _colNum = 0;

	[SerializeField]
	private int _inputPat = 0;

	[SerializeField]
	private string _sentaku;

	[SerializeField]
	private string _quesKey = "\n";

	[SerializeField]
	private string _ansKey = "!!";

	[SerializeField]
	private string _sepKey;

	[SerializeField]
	private string _expKey = "##";

	[SerializeField]
	private string _dummyKey;

	[SerializeField]
	private string _perKey;

	[SerializeField]
	private string _quesText;

	public string DrillName {
		get {
			return _drillName;
		}
	}

	public int ColNum {
		get {
			return _colNum;
		}
	}

	public int InputPat {
		get {
			return _inputPat;
		}
	}

	public string Sentaku {
		get {
			return _sentaku;
		}
	}

	public string QuesKey {
		get {
			return _quesKey;
		}
	}

	public string AnsKey {
		get {
			return _ansKey;
		}
	}

	public string SepKey {
		get {
			return _sepKey;
		}
	}

	public string ExpKey {
		get {
			return _expKey;
		}
	}

	public string DummyKey {
		get {
			return _dummyKey;
		}
	}

	public string PerKey {
		get {
			return _perKey;
		}
	}

	public string QuesText {
		get {
			return _quesText;
		}
	}


	public void SetData (string data, string type)
	{

		if (type == null) {
			Debug.Log ("null data");

		} else
			switch (type) {

			case "DrillName":
				_drillName = data;

				break;
			case "ColNum":
				_colNum = int.Parse (data);

				break;
			case "InputPat":
				_inputPat = int.Parse (data);

				break;
			case "Sentaku":
				_sentaku = data;

				break;
			case "QuesKey":
				_quesKey = data;

				break;
			case "AnsKey":
				_ansKey = data;

				break;
			case "SepKey":
				_sepKey = data;

				break;
			case "ExpKey":
				_expKey = data;

				break;
			case "DummyKey":
				_dummyKey = data;

				break;
			case "PerKey":
				_perKey = data;

				break;
			case "QuesText":
				_quesText = data;

				break;
			default:
				Debug.Log ("Incorrect data");
				break;
			}

	}

	public static bool strNull (string s)
	{
		if (s == "" || s == null) {
			return true;
		} else {
			return false;
		}
	}


}
