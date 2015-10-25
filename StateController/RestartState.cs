using UnityEngine;
using System.Collections;
using Artwave.GameState;
public class RestartState : AbstractState {

	public MapController mapController;
	public TimerController timer;
	public override void OnActive(){
		timer.ResetTimer ();
		mapController.Reset ();
		StateManager.ChangeState("State.CreateBoardState");
	}
}
