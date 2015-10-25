using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TimerController : MonoBehaviour {
	private Text timerLabel;
	public float time;
	public void ResetTimer(){
		time = 0;
	}
	private bool isPlay = false;
	public void PauseTimer(){
		isPlay = false;
	}
	public void PlayTimer(){
		isPlay = true;
		time = 0;
	}
	public void ResumeTimer(){
		isPlay = true;
	}
	void Start(){
		timerLabel = (Text)GetComponent<Text> ();
		time = 0;
		isPlay = false;
	}
	void Update(){
		if (isPlay) {
			time += Time.deltaTime;
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
			timerLabel.text = hours.ToString() + ":" + minutesString + ":" + secondsString; 
		}
	}
}
