using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueManager : MonoBehaviour
{
	#region Initialization

	    #region Variables

	    List<Dictionary<string, object>> MainDialogue;
	    List<Dictionary<string, object>> SelectDialogue;


        //public GameObject SelectEventPanel;

        //public GameObject DialogueFolder;
        public GameObject DialogueNameBox;

        public TextMeshProUGUI DialogueNameText;
        public TextMeshProUGUI DialogueMainText;


        //[SerializeField]
        //GameObject QuestGenerator;
        [SerializeField]
        NpcManager NpcManager;
        [SerializeField]
        DataManager DataManager;
        [SerializeField]
        GUIManager GUIManager;

        [SerializeField]
        float TalkingSpeed = 0.05f;

        List<int> EventStartPages;
        List<int> SelectStartPages;

        //int questNum;
        int finalChapter;

        string playerName;

        #endregion


    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        MainDialogue = CSVReader.Read("Kor_Dialogue_MythOfBisa_CSV");
        SelectDialogue = CSVReader.Read("Kor_SelectEvent_MythOfBisa_CSV");

        EventStartPages = new List<int>();
        SelectStartPages = new List<int>();
        finalChapter = int.Parse(MainDialogue[MainDialogue.Count - 2]["EventNum"].ToString());

        for (int page = 0; page < MainDialogue.Count; page++)
        {
            //퀘스트 별로 시작 페이지를 EventStartPageNums에 저장
            if (MainDialogue[page]["PageNum"].ToString() == "0")
            {
                EventStartPages.Add(page);
            }
        }
        for (int page = 0; page < SelectDialogue.Count; page++)
        {
            //선택 분기 별 시작 페이지를 SelectStartPageNums에 저장
            if (SelectDialogue[page]["SelectEvent"].ToString() != "")
            {
                SelectStartPages.Add(page);
            }
        }

        playerName = DataManager.playerName;
    }

    #endregion



    int currentDialoguePage;

    /// <summary>
    /// 현재 퀘스트 다이얼로그가 끝나는 페이지
    /// </summary>
    int chapterFInalPage;

    #region Start/End Dialogue 
    public void StartDialogue(int QuestNum)
	{
        int Chapter = QuestNum;

        if(Chapter <= finalChapter)
		{
            GameManager.currentGameMode = GameManager.GameMode.DialogueMode;
            //questNum = QuestNum;
            currentDialoguePage = EventStartPages[Chapter];

            if (Chapter < finalChapter)
                chapterFInalPage = EventStartPages[Chapter + 1] - 1;
            else
                chapterFInalPage = MainDialogue.Count - 1;


            //DialogueFolder.SetActive(true);
            GUIManager.SetUIState(GUIManager.State.DialogueState);

            ChangeDialogueText();
        }
        else
		{
            Debug.Log("ERROR: Chapter 번호가 FinalChapter를 초과함");
		}
    }

    void EndDialogue()
	{
        DataManager.Instance.plusPlayerQuestNum();

        //DialogueFolder.SetActive(false);
        GUIManager.SetUIState(GUIManager.State.UsualState);

        NpcManager.SpawnNextNpcNWatchPlayer();
    }
    #endregion


    #region Change Page (Repeated till the event is end)

    int clickedNum;
    bool isTextChangeDone;
    bool isSelectEvent;
    void ChangeDialogueText()
	{
        clickedNum = 0;
        isTextChangeDone = false;
        isSelectEvent = (MainDialogue[currentDialoguePage]["SelectEvent"].ToString() != "");

        //Debug.Log(pageNum);

        //eventEndNum = 이벤트 이후 공백 넘버까지
        if (currentDialoguePage < chapterFInalPage)
		{
            ChangeNameText();
            ChangeMainText();
        }
        else
		{
            EndDialogue();
		}
	}

    void ChangeNameText()
	{
        string name = MainDialogue[currentDialoguePage]["Name"].ToString();
        
        switch (name)
		{
            case "":
            case null:
                DialogueNameBox.SetActive(false);
                break;
            //TODO: 좀 더 효율적인 방법 찾을 것
            case "player":
                DialogueNameText.text = playerName;
                if (!DialogueNameBox.activeSelf)
                    DialogueNameBox.SetActive(true);
                break;
            default:
                DialogueNameText.text = name;
                if (!DialogueNameBox.activeSelf)
                    DialogueNameBox.SetActive(true);
                break;
        }
    }

    void ChangeMainText()
	{
        string content = MainDialogue[currentDialoguePage]["Dialogue"].ToString();

        //ReplaceAll 아니라 Replace지만 다 바뀜
        if (content.Contains("player"))
        {
            Debug.Log("player");
            content = content.Replace("player", playerName);
        }

        StartCoroutine(AnimateText(content));
    }

    IEnumerator AnimateText(string content)
	{
        string animatedText = "";
        
        for (int i = 0; i < content.Length; i++)
		{
            animatedText += content[i];

            if (clickedNum > 0)
			{
                DialogueMainText.text = content;
                break;
            }

            DialogueMainText.text = animatedText;

            yield return new WaitForSecondsRealtime(TalkingSpeed);
        }

        isTextChangeDone = true;

        yield return null;
    }

    //클릭됐을 때

    public void EndCurrentPage()
    {
        if (isTextChangeDone)
        {
            if (!isSelectEvent)
            {
                currentDialoguePage++;
                ChangeDialogueText();
            }
            else
            {
                MakeSelectEvent();
            }
        }
        else clickedNum++;
    }

    #endregion

    #region SelectEvent Maker

    //[SerializeField]
    //GameObject SelectPanel;

    public TextMeshProUGUI SelectText1;
    public TextMeshProUGUI SelectText2;
    int selectEventPage;
    void MakeSelectEvent()
	{
        //SelectPanel.SetActive(true);
        GUIManager.SetDialogueState(GUIManager.DialogueUIState.SelectEvent);

        var currentSelectEvent = int.Parse(MainDialogue[currentDialoguePage]["SelectEvent"].ToString());
        selectEventPage = SelectStartPages[currentSelectEvent];

        SelectText1.text = SelectDialogue[selectEventPage]["SelectText"].ToString();
        SelectText2.text = SelectDialogue[selectEventPage + 1]["SelectText"].ToString();
    }

    public void ChooseSelectEvent(int order)
	{
        switch (order)
		{
            case 0:
                currentDialoguePage = int.Parse(SelectDialogue[selectEventPage]["BackToDialogue"].ToString());
                break;
            case 1:
                currentDialoguePage = int.Parse(SelectDialogue[selectEventPage + 1]["BackToDialogue"].ToString());
                break;
        }

        //SelectPanel.SetActive(false);
        GUIManager.SetDialogueState(GUIManager.DialogueUIState.Usual);
        ChangeDialogueText();
    }

	#endregion
}
