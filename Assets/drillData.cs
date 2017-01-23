using SimpleSQL;

public class DrillData
{

	[PrimaryKey, AutoIncrement]
	public int ID { get; set; }

	[Default (" ")]
	public string NAME { get; set; }

	[Default (0)]
	public int COLOR { get; set; }

	[Default (0)]
	public int QUES_ORDER { get; set; }

	[Default (0)]
	public int ANS_ORDER { get; set; }

	[Default (0)]
	public int DUMMY_USE { get; set; }

	[Default (0)]
	public int DUMMY_TAG { get; set; }

	[Default (0)]
	public int SEVERE { get; set; }

	[Default (0)]
	public int QA_RAND { get; set; }

	[Default (0)]
	public int TIMER { get; set; }
}
