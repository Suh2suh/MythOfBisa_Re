using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
     * Game Mode is different from Camera Mode.
     * Game mode should only be changed by MainCamMoving.cs
    */
    public enum GameMode
	{
        FieldMode,
        UIMode,
        DialogueMode
	};

    public static GameMode currentGameMode;

	private void Awake()
	{
        currentGameMode = GameMode.FieldMode;
	}

}
