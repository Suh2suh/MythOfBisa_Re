using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Game Mode is different from Camera Mode.
    //After Camera Slerp, Game mode is changed (Unless, cam moves without attention)
    public enum GameMode
	{
        FieldMode,
        UIMode,
        DialogueMode
	};

    public static GameMode currentGameMode;

	// Start is called before the first frame update
	private void Awake()
	{
        currentGameMode = GameMode.FieldMode;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
