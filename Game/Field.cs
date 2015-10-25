using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {
	public SpriteRenderer top, bottom, flag;
	public enum TypeNames{EMPTY, VAL_1, VAL_2, VAL_3, VAL_4, VAL_5, VAL_6, BOMB}
	public TypeNames type = 0;
}
