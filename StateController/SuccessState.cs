using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artwave.GameState;
public class SuccessState : AbstractState {
	public TimerController timer;
	public GameObject playGUI, gameMenu, finishWindow;
	public MapController mapController;
	private float time;
	private bool proccessEvent;
	private string message;
	public override void OnActive(){
		time = 0;
		proccessEvent = false;
		timer.PauseTimer ();
		playGUI.SetActive (false);
		gameMenu.SetActive (false);
		finishWindow.SetActive (true);
		GameObject successMessage = GameObject.Find ("SuccessMessage");
		GameObject failureMessage = GameObject.Find ("FailureMessage");
		successMessage.GetComponent<Image> ().enabled = true;
		failureMessage.GetComponent<Image> ().enabled = false;
		Text label = successMessage.gameObject.GetComponentInChildren<Text> ();
		label.enabled = true;

		Data data = ((StateController)StateManager.GetController ()).GetData ();
		bool isNewRecord = false;
		switch (data.selectedLevel) {
		case Data.LevelName.EASY:
			GameCenter.AddLeaderboard (GameCenter.Leaderboard.EASY, (long)timer.time);
			if(timer.time < data.EasyTime){
				isNewRecord=true;
				data.EasyTime = (int)timer.time;
				PlayerPrefs.SetInt ("EasyTime", (int)timer.time ); 
			}
			break;
		case Data.LevelName.NORMAL:
			GameCenter.AddLeaderboard (GameCenter.Leaderboard.NORMAL, (long)timer.time);
			if(timer.time < data.NormalTime){
				isNewRecord=true;
				data.NormalTime = (int)timer.time;
				PlayerPrefs.SetInt ("NormalTime", (int)timer.time ); 
			}
			break;
		case Data.LevelName.HARD:
			GameCenter.AddLeaderboard (GameCenter.Leaderboard.HARD, (long)timer.time);
			if(timer.time < data.HardTime){
				isNewRecord=true;
				data.HardTime = (int)timer.time;
				PlayerPrefs.SetInt ("HardTime", (int)timer.time ); 
			}
			break;
		case Data.LevelName.VERY_HARD:
			GameCenter.AddLeaderboard (GameCenter.Leaderboard.VERYHARD, (long)timer.time);
			if(timer.time < data.VeryHardTime){
				isNewRecord=true;
				data.VeryHardTime = (int)timer.time;
				PlayerPrefs.SetInt ("VeryHardTime", (int)timer.time ); 
			}
			break;
		case Data.LevelName.EXTRA_LARGE:
			GameCenter.AddLeaderboard (GameCenter.Leaderboard.EXTRALARGE, (long)timer.time);
			if(timer.time < data.ExtraLargeTime){
				isNewRecord=true;
				data.ExtraLargeTime = (int)timer.time;
				PlayerPrefs.SetInt ("ExtraLargeTime", (int)timer.time ); 
			}
			break;
		}
		if (isNewRecord == true) {//new record
			label.text ="NEW RECORD";
		} else {
			label.text ="SUCCESS";
		}



		failureMessage.gameObject.GetComponentInChildren<Text> ().enabled = false;
		mapController.isHexDownBlock = true;
	}
	public override void OnReceiveEvent(string message){
		if (message == "BackMainMenu") {
			if (proccessEvent) {
				return;
			}
			this.message = message;
			proccessEvent = true;
		} else {
			switch (message) {
			case "RestartButtonClick":
				StateManager.ChangeState ("State.RestartState");
				break;
			}
		}
	}
	void Update(){
		if (proccessEvent) {
			time += Time.deltaTime;
			if (time > 0.35f) {
				switch (message) {
				case "BackMainMenu":
					StateManager.ChangeState ("MainMenu", "State.MainMenuState");
					break;
				}
			}
		}
	}
}
