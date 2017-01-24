using SimpleSQL;

public class QuesData
{

	[PrimaryKey, AutoIncrement]
	public int ID { get; set; }

	[Default (0)]
	public int DRILL_ID { get; set; }

	[Default ("なし")]
	public string TEXT { get; set; }

	[Default (0)]
	public int TAG { get; set; }

	[Default (0)]
	public int JUN { get; set; }

	[Default ("なし")]
	public string LAST { get; set; }

	[Default (0)]
	public int LEVEL { get; set; }

	[Default (0)]
	public int REVIEW { get; set; }

	[Default (0)]
	public int CORRECT { get; set; }

	[Default (0)]
	public int WRONG { get; set; }

	[Default ("なし")]
	public string IMAGE_Q { get; set; }

	[Default ("なし")]
	public string IMAGE_A { get; set; }

	[Default ("なし")]
	public string SOUND_Q { get; set; }

	[Default ("なし")]
	public string SOUND_A { get; set; }

	[Default (0)]
	public int EX1 { get; set; }

	[Default ("なし")]
	public string EX2 { get; set; }
}