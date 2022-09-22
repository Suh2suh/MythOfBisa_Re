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

        public GameObject DialogueFolder;
        public GameObject DialogueNameBox;

        public TextMeshProUGUI DialogueNameText;
        public TextMeshProUGUI DialogueMainText;



        [SerializeField]
        GameObject QuestGenerator;
        [SerializeField]
        NpcManager NpcManager;

        [SerializeField]
        float TalkingSpeed = 0.05f;

        List<int> EventStartPageNums;
        List<int> SelectStartPageNums;

        int questNum;
        int finalEventNum;

        #endregion


    private void Awake()
    {
        MainDialogue = CSVReader.Read("Kor_Dialogue_MythOfBisa_CSV");
        SelectDialogue = CSVReader.Read("Kor_SelectEvent_MythOfBisa_CSV");

        EventStartPageNums = new List<int>();
        SelectStartPageNums = new List<int>();
        finalEventNum = int.Parse(MainDialogue[MainDialogue.Count - 2]["EventNum"].ToString());

        for (int page = 0; page < MainDialogue.Count; page++)
		{
            if(MainDialogue[page]["PageNum"].ToString() == "0")
            {
                EventStartPageNums.Add(page);
            }
		}
        for (int page = 0; page < MainDialogue.Count; page++)
        {
            if (SelectDialogue[page]["SelectEvent"].ToString() != "")
            {
                SelectStartPageNums.Add(page);
            }
        }
    }

    #endregion


    int pageNum;
    int eventEndNum;

    #region Start/End Dialogue 
    public void StartDialogue(int questNum)
	{
        if(questNum <= finalEventNum)
		{
            GameManager.currentGameMode = GameManager.GameMode.DialogueMode;
            this.questNum = questNum;
            pageNum = EventStartPageNums[questNum];

            if (questNum < finalEventNum)
                eventEndNum = EventStartPageNums[questNum + 1] - 1;
            else
                eventEndNum = MainDialogue.Count - 1;

            DialogueFolder.SetActive(true);

            ChangeDialogueText();
        }
    }

    void EndDialogue()
	{
        DataManager.Instance.plusPlayerQuestNum();

        DialogueFolder.SetActive(false);
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
        isSelectEvent = (MainDialogue[pageNum]["SelectEvent"].ToString() != "");

        //Debug.Log(pageNum);

        //eventEndNum = 이벤트 이후 공백 넘버까지
        if (pageNum < eventEndNum)
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
        string name = MainDialogue[pageNum]["Name"].ToString();
        
        switch (name)
		{
            case "":
            case null:
                DialogueNameBox.SetActive(false);
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
        string content = MainDialogue[pageNum]["Dialogue"].ToString();

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
                pageNum++;
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

    [SerializeField]
    GameObject SelectPanel;

    public TextMeshProUGUI SelectText1;
    public TextMeshProUGUI SelectText2;
    int selectEventPage;
    void MakeSelectEvent()
	{
        SelectPanel.SetActive(true);

        var currentSelectEvent = int.Parse(MainDialogue[pageNum]["SelectEvent"].ToString());
        selectEventPage = SelectStartPageNums[currentSelectEvent];

        SelectText1.text = SelectDialogue[selectEventPage]["SelectText"].ToString();
        SelectText2.text = SelectDialogue[selectEventPage + 1]["SelectText"].ToString();
    }

    public void ChooseSelectEvent(int order)
	{
        switch (order)
		{
            case 0:
                pageNum = int.Parse(SelectDialogue[selectEventPage]["BackToDialogue"].ToString());
                break;
            case 1:
                pageNum = int.Parse(SelectDialogue[selectEventPage + 1]["BackToDialogue"].ToString());
                break;
        }

        SelectPanel.SetActive(false);
        ChangeDialogueText();
    }

	#endregion
}
