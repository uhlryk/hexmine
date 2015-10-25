using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artwave.GameState;
public class PlayState : AbstractState {
	public GameObject playGUI, gameMenu, finishWindow;
	public MapController mapController;
	public Button flag, show;
	public Color active, deactive;
	private Data data;
	public TimerController timer;
	public override void OnActive(){
		timer.ResumeTimer ();
		Debug.Log ("PlayState Active");
		playGUI.SetActive (true);
		gameMenu.SetActive (false);
		finishWindow.SetActive (false);
		data = ((StateController)StateManager.GetController ()).GetData ();
		SetGameMode (data.gameMode);
		mapController.isHexDownBlock = false;
	}
	private void SetGameMode(Data.GameMode gameMode){
		data.gameMode = gameMode;
		if (gameMode == Data.GameMode.SHOW) {
			mapController.isFlagMode = false;
			mapController.isShowMode = true;
			flag.image.color = deactive;

			show.image.color = active;
		} else {
			mapController.isFlagMode = true;
			mapController.isShowMode = false;
			flag.image.color = active;
			show.image.color = deactive;
		}

	}
	public override void OnReceiveEvent(string message){
		switch(message){
		case "ButtonFlag":
			SetGameMode(Data.GameMode.FLAG);
			break;
		case "ButtonShow":
			SetGameMode(Data.GameMode.SHOW);
			break;
		case "OpenOptions":
			StateManager.ChangeState("State.PauseState");
			break;
		case "GameFailure":
			StateManager.ChangeState("State.FailureState");
			break;
		case "GameSuccess":
			StateManager.ChangeState("State.SuccessState");
			break;
		}

	}
}
