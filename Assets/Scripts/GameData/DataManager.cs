using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	#region Initialization when Load

	private static DataManager instance = null;

	SaveData playerData;
	string saveFileName;

	public static int maxQuestNum = 8;

	private void Awake()
	{
		#region Sigleton

		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		#endregion

		#region Save/Load Initialization

		/* ※DO NOT CHANGE SAVEFILENAME */
		saveFileName = "MythOfBisa_PlayerData";
		loadPlayerData();

		//Save Initial PlayerData if player is new (doesn't have a save file)
		if (playerData == null)
		{
			playerData = new SaveData("캐롤", 0, 0);
			SaveSystem.Save(playerData, saveFileName);
		}

		printPlayerData();

		#endregion
	}

	#endregion


	#region Getter/Setter

	public static DataManager Instance
	{
		get
		{
			return instance;
		}
		set
		{
			instance = value;
		}
	}

	public int questNum
	{
		get
		{
			return playerData.questNum;
		}
	}
	public string playerName
	{
		get
		{
			return playerData.playerName;
		}
	}

	public int highScore
	{
		get
		{
			return playerData.highScore;
		}
	}

	#endregion


	#region DataManager Methods

	#region <Change Data>

	public void ChangePlayerName(string playerName)
	{
		SaveData newData = new SaveData(playerData.playerName, questNum, highScore);
		SaveSystem.Save(newData, saveFileName);

		loadPlayerData();
		printPlayerData();
	}

	public void ChangePlayerQuestNum(int questNum)
	{
		SaveData newData = new SaveData(playerData.playerName, Mathf.Clamp(questNum, 0, maxQuestNum), highScore);
		SaveSystem.Save(newData, saveFileName);

		loadPlayerData();
		printPlayerData();
	}

	public void ChangeHighScore(int score)
	{
		SaveData newData = new SaveData(playerData.playerName, questNum, score);
		SaveSystem.Save(newData, saveFileName);

		loadPlayerData();
		printPlayerData();
	}

	///<summary>
	/// QuestNum += 1
	/// </summary>
	public void plusPlayerQuestNum()
	{
		if(playerData.questNum <= maxQuestNum)
		{
			int newQuestNum = playerData.questNum + 1;
			ChangePlayerQuestNum(newQuestNum);
		}
	}

	#endregion

	#region <Load Data>

	void loadPlayerData()
	{
		playerData = SaveSystem.Load(saveFileName);
	}


	#endregion

	#region <For Test>

	void printPlayerData()
	{
		Debug.Log(playerData.playerName + ": Now doing Quest." + playerData.questNum);
		Debug.Log(playerData.highScore + "점이 최고");
	}

	[ContextMenu("ResetGameData")]
	void ResetGameData()
	{
		ChangePlayerName("캐롤");
		ChangePlayerQuestNum(0);
		ChangeHighScore(0);
	}

	[ContextMenu("CamTestWithBisa")]
	void SetGameDataForCam_Bell()
	{
		ChangePlayerName("캐롤");
		ChangePlayerQuestNum(0);
	}

	[ContextMenu("CamTestWithBell")]
	void SetGameDataForCam_Belll()
	{
		ChangePlayerName("캐롤");
		ChangePlayerQuestNum(3);
	}

	[ContextMenu("CamTestWithSusan")]
	void SetGameDataForCam_Susan()
	{
		ChangePlayerName("캐롤");
		ChangePlayerQuestNum(4);
	}


	#endregion


	#endregion
}
