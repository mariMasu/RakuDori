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

	[Default (0)]
	public int ANSNUM { get; set; }

	[Default ("なし")]
	public string LAST { get; set; }

	[Default (0)]
	public int LEVEL { get; set; }

	[Default (0)]
	public int REVIEW { get; set; }

	[Default ("なし")]
	public string IMAGE { get; set; }


}