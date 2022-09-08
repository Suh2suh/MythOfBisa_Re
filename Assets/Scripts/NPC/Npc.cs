using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
	#region Initialization

		bool isTouchable = false;
		bool isClicked = false;

		Outline outliner;

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
		public bool IsClicked
		{
			get { return isClicked; }
			set { isClicked = value; }
		}


		private void Start()
		{
			outliner = gameObject.GetComponent<Outline>();
		}

	#endregion


	#region Clicked in Screen / Quest Process

		public void ClickedOnce()
		{
			IsClicked = true;
			ShowOutline();
		}

		public void ClickedTwice()
		{
			IsTouchable = false;
			HideOutline();

			Debug.Log(DataManager.Instance.questNum + "번 퀘스트 대화 시작");

			//<Test>
			ProcessConversationEnd();
		}

		void ProcessConversationEnd()
		{
			DataManager.Instance.plusPlayerQuestNum();
			transform.root.GetComponent<NpcManager>().IsNpcActivated = false;
			transform.root.GetComponent<NpcManager>().FindRightSpawnPosNActiveNpc();
		}

		public void CancleTouched()
		{
			IsClicked = false;
			HideOutline();
		}

	#endregion


	#region OutLine Control

		public void ShowOutline()
		{
			outliner.OutlineMode = Outline.Mode.OutlineVisible;
		}
		public void HideOutline()
		{
			outliner.OutlineMode = Outline.Mode.OutlineHidden;
		}

	#endregion
}
