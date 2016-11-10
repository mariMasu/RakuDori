using UnityEngine;
using System.Collections;

public class DrillColor : MonoBehaviour
{

	public static Color rgb (int red, int green, int blue)
	{
		return new Color (red / 255f, green / 255f, blue / 255f);
	}

	static Color SKY = rgb (234, 255, 253);
	static Color SKYo = rgb (191, 247, 246);
	static Color BLUE = rgb (230, 234, 255);
	static Color BLUEo = rgb (186, 202, 255);
	static Color GREEN = rgb (228, 255, 232);
	static Color GREENo = rgb (176, 255, 195);
	static Color ORANGE = rgb (255, 241, 232);
	static Color ORANGEo = rgb (255, 194, 157);
	static Color PINK = rgb (255, 230, 245);
	static Color PINKo = rgb (255, 193, 228);
	static Color YELLOW = rgb (255, 255, 232);
	static Color YELLOWo = rgb (252, 255, 168);

	public static Color[] GetColorD (int num)
	{

		Color[] retC = new Color[2];

		switch (num) {
		case 0:
			retC [0] = SKY;
			retC [1] = SKYo;

			break;
		case 1:
			retC [0] = PINK;
			retC [1] = PINKo;

			break;
		case 2:
			retC [0] = YELLOW;
			retC [1] = YELLOWo;

			break;
		case 3:
			retC [0] = GREEN;
			retC [1] = GREENo;

			break;
		case 4:
			retC [0] = ORANGE;
			retC [1] = ORANGEo;

			break;
		case 5:
			retC [0] = BLUE;
			retC [1] = BLUEo;

			break;
		default:
			Debug.Log ("Incorrect colorNum");
			retC [0] = SKY;
			retC [1] = SKYo;

			break;
		}
		return retC;
	}

}