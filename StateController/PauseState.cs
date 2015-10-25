using UnityEngine;
using System.Collections;
using Artwave.GameState;
public class PauseState : AbstractState {
	public GameObject playGUI, gameMenu;
	public TimerController timer;
	public MapController mapController;
	private float time;
	private bool proccessEvent;
	private string message;
	public override void OnActive(){
		time = 0;
		proccessEvent = false;

		playGUI.SetActive (false);
		gameMenu.SetActive (true);
		timer.PauseTimer ();
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
			case "ClosePause":
				StateManager.ChangeState ("State.PlayState");
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
					StateManager.ChangeState ("MainMenu","State.MainMenuState");
					break;
				}
			}
		}
	}
}
