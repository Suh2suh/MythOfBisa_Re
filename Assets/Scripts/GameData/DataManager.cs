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

		/* ¡ØDO NOT CHANGE SAVEFILENAME */
		saveFileName = "MythOfBisa_PlayerData";
		loadPlayerData();

		//Save Initial PlayerData if player is new (doesn't have a save file)
		if (playerData == null)
		{
			playerData = new SaveData("???", 0);
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


	#endregion


	#region DataManager Methods

	#region <Change Data>

	public void ChangePlayerName(string playerName)
	{
		SaveData newData = new SaveData(playerName, playerData.questNum);
		SaveSystem.Save(newData, saveFileName);

		loadPlayerData();
		printPlayerData();
	}

	public void ChangePlayerQuestNum(int questNum)
	{
		SaveData newData = new SaveData(playerData.playerName, questNum);
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
	}

	[ContextMenu("ResetGameData")]
	void ResetGameData()
	{
		ChangePlayerName("");
		ChangePlayerQuestNum(0);
	}

	#endregion


	#endregion
}
