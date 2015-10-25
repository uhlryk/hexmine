using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artwave.GameState;
public class UIEventController : MonoBehaviour {
	public AudioClip buttonClick1, buttonClick2;
	public Image disableSoundImage;
	public void SendEvent(string message){
		Data data = ((StateController)StateManager.GetController ()).GetData ();
		if (message == "ButtonShow" || message == "ButtonFlag") {
			if(data.sound == true){
				AudioSource.PlayClipAtPoint(buttonClick1, transform.position);
			}
		} else {
			if(message ==  "ClickSound"){
				if(data.sound == true){
					data.sound = false;
					PlayerPrefs.SetInt ("Sound", 2); 
					if(disableSoundImage){
						disableSoundImage.enabled = true;
					}
				} else{
					data.sound = true;
					PlayerPrefs.SetInt ("Sound", 1); 
					if(disableSoundImage){
						disableSoundImage.enabled = false;
					}
				}
			}
			if(data.sound == true){
				AudioSource.PlayClipAtPoint(buttonClick2, transform.position);
			}
		}
		StateManager.SendEvent (message);
	}
}
