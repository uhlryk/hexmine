using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artwave.GameState;
public class CreateBoardState : AbstractState {
	public int hexColNumber = 10;
	public int hexRowNumber = 10;
	public int hexesPerMine = 20;

	public MapController mapController;
	public TimerController timer;
	public GameObject playGUI, gameMenu, finishWindow;
	public Image disableSoundImage;
	public override void OnActive(){
		timer.ResetTimer ();
		playGUI.SetActive (false);
		gameMenu.SetActive (false);
		finishWindow.SetActive (false);
		Data data = ((StateController)StateManager.GetController ()).GetData ();
		data.numberOfGames ++;
		if (data.sound == true) {
			disableSoundImage.enabled = false;
		} else {
			disableSoundImage.enabled = true;
			
		}
		switch (data.selectedLevel) {
		case Data.LevelName.EASY:
			hexColNumber = data.EasyColNumber;
			hexRowNumber = data.EasyRowNumber;
			hexesPerMine = data.EasyHexesPerMine;
			break;
		case Data.LevelName.NORMAL:
			hexColNumber = data.NormalColNumber;
			hexRowNumber = data.NormalRowNumber;
			hexesPerMine = data.NormalHexesPerMine;
			break;
		case Data.LevelName.HARD:
			hexColNumber = data.HardColNumber;
			hexRowNumber = data.HardRowNumber;
			hexesPerMine = data.HardHexesPerMine;
			break;
		case Data.LevelName.VERY_HARD:
			hexColNumber = data.VeryHardColNumber;
			hexRowNumber = data.VeryHardRowNumber;
			hexesPerMine = data.VeryHardHexesPerMine;
			break;
		case Data.LevelName.EXTRA_LARGE:
			hexColNumber = data.ExtraLargeColNumber;
			hexRowNumber = data.HExtraLargeRowNumber;
			hexesPerMine = data.ExtraLargeHexesPerMine;
			break;
		}
		mapController.Init ();
		mapController.Build (hexColNumber, hexRowNumber, hexesPerMine);
	}
	public override void OnReceiveEvent(string message){
		switch(message){
		case "MapBuildFinish":
			StateManager.ChangeState("State.PlayState");
			break;
		}
	}
}
