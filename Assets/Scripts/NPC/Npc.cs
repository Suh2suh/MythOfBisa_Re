using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
	#region Initialization

	[SerializeField]
	bool isTouchable = false;

	// Make Npc Untouchable when the conversation is over
	public bool IsTouchable
	{
		get
		{
			return isTouchable;
		}
		set
		{
			isTouchable = value;
			Debug.Log(transform.name + " Touchable: " + isTouchable);

			if(isTouchable)
				TouchOnScreen.isTouchDetectNeeded = true;
			else
				TouchOnScreen.isTouchDetectNeeded = false;
		}
	}
	#endregion


	#region Npc: ClickCheck / ActiveConversation


	bool isClicked = false;
	public bool IsClicked
	{
		get{ return isClicked; }
		set{ isClicked = value; }
	}
	Outline outliner;

	public void CheckClickAndConversationOn()
	{
		if(!IsClicked)
		{
			IsClicked = true;

			outliner = gameObject.GetComponent<Outline>();
			outliner.OutlineMode = Outline.Mode.OutlineVisible;
		}
		else
		{
			IsTouchable = false;
			outliner.OutlineMode = Outline.Mode.OutlineHidden;

			Debug.Log(DataManager.Instance.questNum + "번 퀘스트 대화 시작");
			//npc quest number pass
			//대화 시작, 대화 끝나면 questnum ++;

			//<Test>
			ProcessConversationEnd();

		}
	}

	void ProcessConversationEnd()
	{
		DataManager.Instance.plusPlayerQuestNum();
		transform.root.GetComponent<NpcManager>().IsNpcActivated = false;
		transform.root.GetComponent<NpcManager>().FindRightSpawnPosNActiveNpc();
	}


	#endregion
}
