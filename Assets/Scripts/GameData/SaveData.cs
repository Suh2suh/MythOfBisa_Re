
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public string playerName;
	public int questNum;

	public SaveData(string playerName, int questNum)
	{
		this.playerName = playerName;
		this.questNum = questNum;
	}
}

public static class SaveSystem
{
	private static string SavePath => Application.persistentDataPath + "/saves/";

	public static void Save(SaveData saveData, string saveFileName)
	{
		if(!Directory.Exists(SavePath))
		{
			Directory.CreateDirectory(SavePath);
		}

		string saveJason = JsonUtility.ToJson(saveData);
		string saveFilePath = SavePath + saveFileName + ".json";

		File.WriteAllText(saveFilePath, saveJason);
		Debug.Log("Save Success: " + saveFilePath + "/ questNum: " + saveData.questNum);
	}

	public static SaveData Load(string saveFileName)
	{
		string saveFilePath = SavePath + saveFileName + ".json";

		if(!File.Exists(saveFilePath))
		{
			Debug.Log("No such saveFile exists: " + saveFilePath);

			return null;
		}

		string saveFile = File.ReadAllText(saveFilePath);
		SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);

		return saveData;
	}
}