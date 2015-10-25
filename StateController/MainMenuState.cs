using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Artwave.GameState;
public class MainMenuState : AbstractState {
	private float time;
	private bool proccessEvent;
	private string message;
	public Image disableSoundImage;
	public GameObject exitButton;
	public Text score1, mode1, score2, mode2;

	private Data data;
	/**
	 * jak długo dany czas jest wyświetlany zanim pojawi się czas z innego poziomu trudności
	 */ 
	[Range(3f, 20.0f)]
	public float maxPresentationTime = 4f;
	private bool startPresentation = false; //czy działa prezentacja
	private int actPresentation; // która prezentacja jest teraz
	private float timePresentation;// czas prezentacji
	private bool isFirstLabel; //mamy dwie pary labelów do prezentacji, które się na zmianę zastępują

	public override void OnActive(){
#if UNITY_IOS
		exitButton.SetActive(false);
#endif
		data = ((StateController)StateManager.GetController ()).GetData ();
		int easyTime = PlayerPrefs.GetInt ("EasyTime"); 
		int normalTime = PlayerPrefs.GetInt ("NormalTime"); 
		int hardTime = PlayerPrefs.GetInt ("HardTime"); 
		int veryHardTime = PlayerPrefs.GetInt ("VeryHardTime"); 
		int extraLargeTime = PlayerPrefs.GetInt ("ExtraLargeTime"); 
		if (easyTime > 0) {
			data.EasyTime = easyTime;
		}
		if (normalTime > 0) {
			data.NormalTime = normalTime;
		}
		if (hardTime > 0) {
			data.HardTime = hardTime;
		}
		if (veryHardTime > 0) {
			data.VeryHardTime = veryHardTime;
		}
		if (extraLargeTime > 0) {
			data.ExtraLargeTime = extraLargeTime;
		}

		time = 0;
		proccessEvent = false;

		if (data.sound == true) {
				disableSoundImage.enabled = false;
		} else {
				disableSoundImage.enabled = true;

		}
		isFirstLabel = true;
		actPresentation = 4;//ustawiamy na prezentacji ostatniej
		timePresentation = maxPresentationTime;//ustawiamy na koniec czasu więc na starcie prezentacji wjedzie pierwsza
		startPresentation = true;

		score1.text = "";
		mode1.text = "";
	}
	public override void OnDeactive(){
		startPresentation = false;
	}

	public override void OnReceiveEvent(string message){
		if (message == "StartGameEasy" || message == "StartGameNormal" || message == "StartGameHard" || message == "StartGameVeryHard" || message == "StartGameExtraLarge") {
			if (proccessEvent) {
				return;
			}
			this.message = message;
			proccessEvent = true;
		} else {
			switch (message) {
			case "ExitGame":
				Debug.Log("EXIT !");
				Application.Quit();
				break;
			}
		}
	}
	private string convertPointsToTime(int time){
		int hours = (int)time / 3600;
		int minutes = (int)((time % 3600)/60);
		string minutesString;
		if(minutes < 10){
			minutesString = "0" + minutes.ToString();
		} else {
			minutesString = minutes.ToString();
		}
		int seconds = (int)((time % 3600)%60);
		string secondsString;
		if(seconds < 10){
			secondsString = "0" + seconds.ToString();
		} else {
			secondsString = seconds.ToString();
		}
		return hours.ToString() + ":" + minutesString + ":" + secondsString; 
	}
	private float positionChange = 0, alphaChange = 0;
	private string scoreMessage, modelMessage;
	void Update(){
		if (startPresentation) {
			timePresentation += Time.deltaTime;
			if(timePresentation >= maxPresentationTime){
				actPresentation++;
				timePresentation =0;
				isFirstLabel = !isFirstLabel;
				if(actPresentation > 4){
					actPresentation = 0;
				}
				switch(actPresentation){
				case 4:
					scoreMessage = convertPointsToTime(data.EasyTime);
					modelMessage = "EASY";
					break;
				case 3:
					scoreMessage = convertPointsToTime(data.NormalTime);
					modelMessage = "Normal";
					break;
				case 2:
					scoreMessage = convertPointsToTime(data.HardTime);
					modelMessage = "Hard";
					break;
				case 1:
					scoreMessage = convertPointsToTime(data.VeryHardTime);
					modelMessage = "Very Hard";
					break;
				case 0:
					scoreMessage = convertPointsToTime(data.ExtraLargeTime);
					modelMessage = "Extra Large";
					break;
				}
				if(isFirstLabel){
					score1.text = scoreMessage;
					mode1.text = modelMessage;
				} else {
					score2.text = scoreMessage;
					mode2.text = modelMessage;
				}
				positionChange = 0;
				alphaChange = 0;
			}
			positionChange += Time.deltaTime * 100;
			if(positionChange >= 100)positionChange = 100;
			alphaChange += Time.deltaTime;
			if(alphaChange >= 1)alphaChange = 1;

			Vector3 pos1a = score1.rectTransform.localPosition;
			Color col1a = score1.color;
			Vector3 pos1b = mode1.rectTransform.localPosition;
			Color col1b = mode1.color;
			Vector3 pos2a = score2.rectTransform.localPosition;
			Color col2a = score2.color;
			Vector3 pos2b = mode2.rectTransform.localPosition;
			Color col2b = mode2.color;
			if(isFirstLabel){
				pos1a.x = 100 - positionChange;
				col1a.a = alphaChange;
				pos1b.x = 100 - positionChange;
				col1b.a = alphaChange;
				pos2a.x = - positionChange;
				col2a.a = 1 - alphaChange;
				pos2b.x = - positionChange;
				col2b.a = 1 - alphaChange;
			}
			else{
				pos1a.x =  - positionChange;
				col1a.a = 1 - alphaChange;
				pos1b.x =  - positionChange;
				col1b.a = 1 - alphaChange;
				pos2a.x = 100 - positionChange;
				col2a.a = alphaChange;
				pos2b.x = 100 - positionChange;
				col2b.a = alphaChange;
			}
			score1.rectTransform.localPosition = pos1a;
			mode1.rectTransform.localPosition = pos1b;
			score2.rectTransform.localPosition = pos2a;
			mode2.rectTransform.localPosition = pos2b;
			score1.color = col1a;
			mode1.color = col1b;
			score2.color = col2a;
			mode2.color = col2b;
		}
		if (proccessEvent) {
			time += Time.deltaTime;
			if (time > 0.35f) {
				switch (message) {
				case "StartGameEasy":
					((StateController)StateManager.GetController ()).GetData ().selectedLevel = Data.LevelName.EASY;
					StateManager.ChangeState ("Game", "State.CreateBoardState");
					break;
				case "StartGameNormal":
					((StateController)StateManager.GetController ()).GetData ().selectedLevel = Data.LevelName.NORMAL;
					StateManager.ChangeState ("Game", "State.CreateBoardState");
					break;
				case "StartGameHard":
					((StateController)StateManager.GetController ()).GetData ().selectedLevel = Data.LevelName.HARD;
					StateManager.ChangeState ("Game", "State.CreateBoardState");
					break;
				case "StartGameVeryHard":
					((StateController)StateManager.GetController ()).GetData ().selectedLevel = Data.LevelName.VERY_HARD;
					StateManager.ChangeState ("Game", "State.CreateBoardState");
					break;
				case "StartGameExtraLarge":
					((StateController)StateManager.GetController ()).GetData ().selectedLevel = Data.LevelName.EXTRA_LARGE;
					StateManager.ChangeState ("Game", "State.CreateBoardState");
					break;
				}
			}
		}
	}
}
