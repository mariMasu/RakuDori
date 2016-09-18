using SimpleSQL;

public class DrillData
{

	[PrimaryKey, AutoIncrement]
	public int ID { get; set; }

	[Default(" ")]
	public string NAME { get; set; }

	[Default("なし")]
	public string LAST { get; set; }

	[Default(0)]
	public int OKNUM { get; set; }

	[Default(0)]
	public int COLOR { get; set; }	

	[Default(0)]
	public int QNUM { get; set; }

	[Default(0)]
	public int FLAG { get; set; }
}
