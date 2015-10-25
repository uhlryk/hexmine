using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artwave.GameState;
public class FailureState : AbstractState {
	public TimerController timer;
	public GameObject playGUI, gameMenu, finishWindow;
	public MapController mapController;
	private float time;
	private bool proccessEvent;
	private string message;
	private bool showAd = false;
	private AdMob adMob;
	public override void OnActive(){
		time = 0;
		proccessEvent = false;

		timer.PauseTimer ();
		playGUI.SetActive (false);
		gameMenu.SetActive (false);
		finishWindow.SetActive (true);
		GameObject successMessage = GameObject.Find ("SuccessMessage");
		GameObject failureMessage = GameObject.Find ("FailureMessage");
		successMessage.GetComponent<Image> ().enabled = false;
		failureMessage.GetComponent<Image> ().enabled = true;
		successMessage.gameObject.GetComponentInChildren<Text> ().enabled = false;
		failureMessage.gameObject.GetComponentInChildren<Text> ().enabled = true;
		mapController.isHexDownBlock = true;

		Data data = ((StateController)StateManager.GetController ()).GetData ();

		if (data.numberOfGames > data.adPopupTime) {
			data.numberOfGames = 0;
			showAd = true;

			adMob = new AdMob();
			adMob.StartInterstitial();
		}
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
		if (showAd == true) {
			adMob.ShowInterstitial();
		}
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
