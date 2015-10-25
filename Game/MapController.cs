using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artwave.Board;
using Artwave.GameState;
public class MapController : MonoBehaviour, BoardListenerInterface, BuilderListenerInterface  {
	public BoardManager boardManager;
	public Sprite bomb,top, flag,empty,hex1,hex2,hex3,hex4,hex5,hex6, flag_true, flag_false;
	private int hexesPerMine;
	public bool isFlagMode, isShowMode;
	public AudioClip hexClick, findBomb, success, setFlag;
	private bool isInit = false;
	public bool isHexDownBlock = false;
	private Data data;
	public void Init(){
		if (isInit == false) {
			isInit = true;
			data = ((StateController)StateManager.GetController ()).GetData ();
			isHexDownBlock = false;
			boardManager.Init ();
			boardManager.GetPatternHex ().ShowColor (Color.white);
			boardManager.AddBoardListener(this);
			boardManager.AddBuildListener(this);
			boardManager.SetTableSizeFullScreen (0.85f, 1.05f);
		}
	}
	public void Build (int colNum, int rowNum, int hexesPerMine) {
		this.hexesPerMine = hexesPerMine;
		boardManager.Build (colNum, rowNum);
		isHexDownBlock = false;
	}
	public void Reset(){
		boardManager.Reset ();
	}
	public void OnHexBoardDown(HexField hex){}
	public void OnHexBoardUp(HexField hex){
		if (isHexDownBlock) {
			return;
		}

		Debug.Log ("Hex Up");
		Field field = (Field)hex.hexLogic;
		if (isFlagMode) {
			if(field.top.enabled == true){
				Debug.Log ("FLag old:" + field.flag.enabled);
				if(data.sound == true){
					AudioSource.PlayClipAtPoint(setFlag, transform.position);
				}
				field.flag.enabled = !field.flag.enabled;
			}
		} else {
			if(field.top.enabled == true && field.flag.enabled == false){
				if(field.type == Field.TypeNames.EMPTY){
					ProccessEmpty(hex);
				}
				field.top.enabled = false;
				if(field.type == Field.TypeNames.BOMB){

					ProcessBomb(hex);
				} else {

					CheckFinish();
				}
			}
		}
//		boardManager.AnimationGoToHex (hex, 5, false);
	}
	/**
	 * Obsługuje sytuację gdy trafimy na bombę 
	*/
	private void ProcessBomb(HexField hex){
		if (data.sound == true) {
			AudioSource.PlayClipAtPoint (findBomb, transform.position);
		}
		List<HexField> listAll = boardManager.GetAllHex ();
		foreach (HexField currentHex in listAll) {
			Field currentField = (Field)currentHex.hexLogic;
			/**
			 * po porażce odsłaniamy mapę, jeśli na danym polu była flaga i bomba to dajemy grafikę flag_true, jeśli na danym polu była flaga ale nie było bomby to dajemy flag_false
			 */ 
			if(currentField.flag.enabled){
				if(currentField.type == Field.TypeNames.BOMB){
					currentField.flag.sprite = flag_true;
				} else {
					currentField.flag.sprite = flag_false;
				}
			}
			currentField.top.enabled = false;
		}
		StateManager.SendEvent ("GameFailure");
	}
	/**
	 * sprawdza czy jeszcze są jakieś zasłonięte pola lub czy zaflagowane są poprawnie. 
	 * By wygrać muszą być odsłonięte wszystkie pola które nie mają bomby. By przegrać musi być odsłonięta bomba
	 */ 
	private void CheckFinish(){
		List<HexField> listAll = boardManager.GetAllHex ();
		bool isSuccess = true;
		foreach (HexField currentHex in listAll) {
			Field currentField = (Field)currentHex.hexLogic;
			if(currentField.type == Field.TypeNames.BOMB && currentField.top.enabled == false){
				if(data.sound == true){
					AudioSource.PlayClipAtPoint(findBomb, transform.position);
				}
				StateManager.SendEvent ("GameFailure");
				break;
			} else if(currentField.type != Field.TypeNames.BOMB && currentField.top.enabled == true){
				isSuccess = false;
			}
		}
		if (isSuccess == true) {
			if(data.sound == true){
				AudioSource.PlayClipAtPoint(success, transform.position);
			}
			StateManager.SendEvent ("GameSuccess");
		} else {
			if(data.sound == true){
				AudioSource.PlayClipAtPoint(hexClick, transform.position);
			}
		}
	}
	/**
	 * przekazujemy hex który jest pusty. wtedy metoda sprawdza sąsiednie, jeśli sąsiedni jest pusty i zakrtyty to rekurencyjnie wywołuje tą metodę jeszcze raz
	 */ 
	private void ProccessEmpty(HexField hex){
		Field field = (Field)hex.hexLogic;
		if(field.top.enabled == true && field.flag.enabled == false){
			field.top.enabled = false;
			List<HexField> listNeigbors = boardManager.GetNeigbors(hex.GetCoordinates());
			foreach (HexField neigborHex in listNeigbors) {
				if(field.type == Field.TypeNames.EMPTY){
					ProccessEmpty(neigborHex);
				}
			}
		}
	}
	public void OnBoardDrag(){}
	private int actMineInGroupOfMinePerHexes;
	public void OnBuildStart(HexField patternHex, int colNumber, int rowNumber, Vector2 hexSize, bool isEven, bool symmetricHorizontal){

	}
	public void OnBuildEnd(){
		actMineInGroupOfMinePerHexes = hexesPerMine;
		List<HexField> listAll = boardManager.GetAllHex ();
		foreach (HexField currentHex in listAll) {
			Field currentField = (Field)currentHex.hexLogic;
			if(currentField.type != Field.TypeNames.BOMB){
				List<HexField> listNeigbors = boardManager.GetNeigbors(currentHex.GetCoordinates());
				int bombs = 0;
				foreach (HexField neigborHex in listNeigbors) {
					Field neigborField = (Field)neigborHex.hexLogic;
					if(neigborField.type == Field.TypeNames.BOMB){
						bombs++;
					}
				}
				switch(bombs){
				case 0:
					currentField.type = Field.TypeNames.EMPTY;
					currentField.bottom.sprite = empty;
					break;
				case 1:
					currentField.type = Field.TypeNames.VAL_1;
					currentField.bottom.sprite = hex1;
					break;
				case 2:
					currentField.type = Field.TypeNames.VAL_2;
					currentField.bottom.sprite = hex2;
					break;
				case 3:
					currentField.type = Field.TypeNames.VAL_3;
					currentField.bottom.sprite = hex3;
					break;
				case 4:
					currentField.type = Field.TypeNames.VAL_4;
					currentField.bottom.sprite = hex4;
					break;
				case 5:
					currentField.type = Field.TypeNames.VAL_5;
					currentField.bottom.sprite = hex5;
					break;
				case 6:
					currentField.type = Field.TypeNames.VAL_6;
					currentField.bottom.sprite = hex6;
					break;
				}
			}
		}
		StateManager.SendEvent ("MapBuildFinish");
	}
	public void OnResetStart(){}
	public void OnResetEnd(){}
	private int hexWithBomb;
	public bool OnCreateHexStart(Vector3 coordinatesCube, Vector2 coordinatesOffset, bool isInverse){
		return true;
	}
	public void OnCreateHexEnd(HexField hex){
		if (actMineInGroupOfMinePerHexes > hexesPerMine - 1) {
			actMineInGroupOfMinePerHexes = 0;
			hexWithBomb = Random.Range(0, hexesPerMine);
		}
		if(actMineInGroupOfMinePerHexes == hexWithBomb){
			Field field = (Field)hex.hexLogic;
			field.type = Field.TypeNames.BOMB;
			field.bottom.sprite = bomb; 
		}
		actMineInGroupOfMinePerHexes ++;
	}
	public void OnGoToHexAnimationFinish(){}
}
