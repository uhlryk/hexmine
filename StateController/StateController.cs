using UnityEngine;

using System.Collections;
using Artwave.GameState;
public class StateController : AbstractStateController {
	public Data data;
	public override void OnInit(){
		GameCenter.Authenticate();
		int sound = PlayerPrefs.GetInt ("Sound"); 

		if (sound == 2 || sound == 0) {
			data.sound = true;
		} else {
			data.sound = false;
		}
	}
	public override bool OnReceiveEventStart(string message){
		Debug.Log ("StateController " + message);

		return true;
	}
	public Data GetData(){
		return data;
	}
}
