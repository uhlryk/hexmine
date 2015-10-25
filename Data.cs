using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {
	[Header("LEVELS")]

	public int EasyColNumber = 10;
	public int EasyRowNumber = 10;
	public int EasyHexesPerMine = 10;

	public int NormalColNumber = 15;
	public int NormalRowNumber = 15;
	public int NormalHexesPerMine = 20;

	public int HardColNumber = 20;
	public int HardRowNumber = 20;
	public int HardHexesPerMine = 50;

	public int VeryHardColNumber = 20;
	public int VeryHardRowNumber = 20;
	public int VeryHardHexesPerMine = 50;

	public int ExtraLargeColNumber = 20;
	public int HExtraLargeRowNumber = 20;
	public int ExtraLargeHexesPerMine = 50;


	public enum LevelName {NONE, EASY, NORMAL, HARD, VERY_HARD, EXTRA_LARGE}
	public enum GameMode {SHOW, FLAG}
	/**
	 * co ile gier ma się popup wyświetlać
	 */ 
	public int adPopupTime = 2;
	[Header("ACTUAL GAME DATA")]
	public GameMode gameMode = GameMode.SHOW;
	public int numberOfGames = 0;

	public LevelName selectedLevel;
	[Header("PERSISTENT GAME DATA")]
	public bool sound = true;
	public int EasyTime = 30;
	public int NormalTime = 60;
	public int HardTime = 90;
	public int VeryHardTime = 130;
	public int ExtraLargeTime = 130;


}
