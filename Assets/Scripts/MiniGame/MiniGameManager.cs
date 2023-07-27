using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
	public GameObject ReadyObj;
	public GameObject StartObj;
	public GameObject GameObj;
	public GameObject EndObj;

	float ReadyTime;
	float StartTime;

	int State;

	private void OnEnable()
	{
		StartCoroutine(Ready());
	}

	IEnumerator Ready()
	{
		ReadyObj.SetActive(true);
		State = 0;

		ReadyTime = 3.0f;

		Debug.Log("Ready");

		while (ReadyTime > 0 && State == 0)
		{
			Debug.Log("Readying");

			ReadyTime -= 1.0f;

			if (ReadyTime <= 0)
			{
				yield return null;

				ReadyObj.SetActive(false);
				StartCoroutine(StartGame());
			}

			yield return new WaitForSecondsRealtime(1.0f);
		}

		yield return null;
	}

	IEnumerator StartGame()
	{
		StartObj.SetActive(true);
		State = 1;

		StartTime = 1.5f;

		Debug.Log("Start");

		while (StartTime > 0 && State == 1)
		{
			Debug.Log("Starting");

			StartTime -= 1.0f;

			if (StartTime <= 0)
			{
				yield return null;

				StartObj.SetActive(false);
				GameStart();
			}

			yield return new WaitForSecondsRealtime(1.0f);
		}
	}


	public TextMeshProUGUI LifeText;
	public TextMeshProUGUI ScoreText;

	int leftLife;
	public int LeftLife
	{
		get { return leftLife; }
		set
		{
			if(leftLife > 0)
			{
				leftLife = value;

				LifeText.text = new string('¢¾', leftLife);
			}

			if (leftLife <= 0)
			{
				GameObj.SetActive(false);
				GameEnd();
			}
		}
	}

	int score;
	public int Score
	{
		get { return score; }
		set
		{
			score = value;

			ScoreText.text = score.ToString();
		}
	}

	bool GamingMode;
	public GameObject miniPlayer;

	void GameStart()
	{
		miniPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, miniPlayer.GetComponent<RectTransform>().anchoredPosition.y);

		score = 0;
		leftLife = 5;
		spawnGap = 2.0f;

		ScoreText.text = score.ToString();
		LifeText.text = new string('¢¾', leftLife);

		newScore.SetActive(false);

		catchDifficulty = 12.0f;

		GamingMode = true;
		GameObj.SetActive(true);
	}


	float spawnTime;
	float spawnGap;
	float difficultTime;
	public List<GameObject> SpawnableItems;
	public Transform ItemFolder;
	public float catchDifficulty = 12.0f;

	public List<GameObject> SpawnedItems;

	private void FixedUpdate()
	{
		if (GamingMode)
		{
			spawnTime += Time.deltaTime;
			difficultTime += Time.deltaTime;

			if (spawnTime > spawnGap)
			{
				GameObject RandomObj = SpawnableItems[Random.Range(0, 4)];
				float RandomSpawnX = Random.Range(-350f, 350f);

				GameObject SpawnedItem = Instantiate(RandomObj, ItemFolder) as GameObject;

				SpawnedItem.GetComponent<RectTransform>().anchoredPosition = new Vector3(RandomSpawnX, 900.0f, 0f);
				SpawnedItem.GetComponent<Item>().ItemType = SpawnedItem.name.Contains("Wing") ? 1 : 0;
				SpawnedItem.GetComponent<Item>().FallingSpeed = catchDifficulty;

				Debug.Log(catchDifficulty);

				SpawnedItems.Add(SpawnedItem);

				spawnTime = 0f;
			}

			if (difficultTime > 7.0f)
			{
				catchDifficulty += 2.0f;

				if (catchDifficulty > 15f)
					spawnGap = 1.0f;

				if (catchDifficulty > 25f)
					spawnGap = 0.5f;

				difficultTime = 0f;
			}


			if (SpawnedItems.Count > 0)
			{
				for (int i = 0; i < SpawnedItems.Count; i++)
				{
					GameObject Item = SpawnedItems[i];
					if (!Item.activeSelf)
					{
						Destroy(Item);
						SpawnedItems.RemoveAt(i);

						continue;
					}

					RectTransform ItemRect = Item.GetComponent<RectTransform>();

					ItemRect.Translate((new Vector3(0, -Item.GetComponent<Item>().FallingSpeed, 0)));

					if (ItemRect.anchoredPosition.y < -950f)
					{
						if (Item.GetComponent<Item>().ItemType == 1)
						{
							LeftLife -= 1;
						}

						Destroy(Item);
						SpawnedItems.RemoveAt(i);
					}
				}
			}
		}
	}


	public GameObject newScore;

	public TextMeshProUGUI highScore;
	public TextMeshProUGUI yourScore;
	void GameEnd()
	{
		GamingMode = false;
		GameObj.SetActive(false);

		for (int i = 0; i < ItemFolder.transform.childCount; i++)
		{
			Destroy(ItemFolder.GetChild(i).gameObject);
		}
		SpawnedItems.Clear();

		if (score > DataManager.Instance.highScore)
		{
			newScore.SetActive(true);
			DataManager.Instance.ChangeHighScore(score);

			highScore.text = score.ToString();
		}
		else
		{
			highScore.text = DataManager.Instance.highScore.ToString();
		}
		yourScore.text = score.ToString();

		EndObj.SetActive(true);
	}


	public void Restart()
	{
		EndObj.SetActive(false);

		StartCoroutine(Ready());
	}

	public void Exit()
	{
		EndObj.SetActive(false);
	}

	public void Back()
	{
		GameObj.SetActive(false);
		GamingMode = false;

		for (int i = 0; i < ItemFolder.transform.childCount; i++)
		{
			Destroy(ItemFolder.GetChild(i).gameObject);
		}
		SpawnedItems.Clear();


		EndObj.SetActive(false);
	}
}
