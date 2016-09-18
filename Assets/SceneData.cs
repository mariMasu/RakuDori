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
	private string _quesKey ;

	[SerializeField]
	private string _ansKey1;

	[SerializeField]
	private string _ansKey2;

	[SerializeField]
	private string _dummyKey;

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

	public string QuesKey {
		get {
			return _quesKey;
		}
	}

	public string AnsKey1 {
		get {
			return _ansKey1;
		}
	}

	public string AnsKey2 {
		get {
			return _ansKey2;
		}
	}

	public string DummyKey {
		get {
			return _dummyKey;
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
			case "QuesKey":
				_quesKey = data;

				break;
			case "AnsKey1":
				_ansKey1 = data;

				break;
			case "AnsKey2":
				_ansKey2 = data;

				break;
			case "DummyKey":
				_dummyKey = data;

				break;
			case "QuesText":
				_quesText = data;

				break;
			default:
				Debug.Log ("Incorrect data");
				break;
			}

	}


}
