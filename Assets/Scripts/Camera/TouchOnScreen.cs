using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchOnScreen : MonoBehaviour
{
	#region Initialization

	    Camera cam;

        public static bool isTouchDetectNeeded;
        bool isNpcClicked;

        public ShowMapDescription MapDescriptionShower;
        public GUIManager guiManager;
        Npc recentClickedNpc;

        private void Awake()
	    {
            cam = transform.GetComponent<Camera>();
            recentClickedNpc = null;
        }

	#endregion


	#region RayCheck

	private void Start()
	{
        //Debug.Log("Start Touch");
	}

	void Update()
        {
            //Debug.Log(isTouchDetectNeeded);
            
            if(isTouchDetectNeeded)
		    {

#if UNITY_ANDROID && !UNITY_EDITOR

			        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    {    
                        Ray ray = Camera.current.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit hit;

                        //if Layer is "MapPin"
                        if (Physics.Raycast(ray, out hit))
                        {
                            Debug.Log(hit.transform.parent.name);

                            switch (hit.transform.gameObject.layer)
                            {
                            #region MapPin Touch Detect
                                case 6:
                                    MapDescriptionShower.BuildingName = hit.transform.parent.name;

                                    if (MapDescriptionShower.BuildingStartIndex > -1)
                                        guiManager.MapState = GUIManager.MapUIState.InfoPanelOn;

                                    MapDescriptionShower.BuildingStartIndex = -1;
                                    break;
                    #endregion

                            #region Npc Touch Detect
                                case 7:
                                    Npc npc = hit.transform.GetComponent<Npc>();

                                    if (npc.IsTouchable)
                                    {
                                        //npc is clicked once
                                        if (recentClickedNpc != npc)
                                        {
                                            cancleNpcClick();

                                            recentClickedNpc = npc;
                                            recentClickedNpc.ClickedOnce();

                                            Debug.Log(recentClickedNpc + " clicked");
                                        }
                                        //npc is clicked twice
                                        else
                                        {
                                            recentClickedNpc.ClickedTwice();

                                            Debug.Log(recentClickedNpc + " double clicked");
                                        }
                                    }
                                    break;

                                default:
                                    cancleNpcClick();

                                    break;
            #endregion
                            }
                        }
                    }


#else
            if (guiManager.MapState != GUIManager.MapUIState.InfoPanelOn)
			    {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        //Debug.Log("Touch");

                        if (Physics.Raycast(ray, out hit))
                        {
						    switch (hit.transform.gameObject.layer){
                            #region MapPin Touch Detect
							    case 6:
                                    MapDescriptionShower.BuildingName = hit.transform.parent.name;

                                    if (MapDescriptionShower.BuildingStartIndex > -1)
                                        guiManager.MapState = GUIManager.MapUIState.InfoPanelOn;

                                    MapDescriptionShower.BuildingStartIndex = -1;
                                    break;
            #endregion

                            #region Npc Touch Detect
							    case 7:
                                    Npc npc = hit.transform.GetComponent<Npc>();

                                    if (npc.IsTouchable)
								    {
                                        //npc is clicked once
                                        if(recentClickedNpc != npc)
									    {
                                            cancleNpcClick();

                                            recentClickedNpc = npc;
                                            recentClickedNpc.ClickedOnce();

                                            //Debug.Log(recentClickedNpc + " clicked");
									    }
                                        //npc is clicked twice
                                        else
									    {
                                            recentClickedNpc.ClickedTwice();

                                            //Debug.Log(recentClickedNpc + " double clicked");
                                        }
                                    }
                                    break;

                                default:
                                    cancleNpcClick();

                                    break;
            #endregion
						    }
					    }
                        else
					    {
                            cancleNpcClick();
                        }
				    }
                }
#endif   
        }
	    }

#endregion



#region Necessary Methods

	    void cancleNpcClick()
        {
            if (recentClickedNpc)
            {
                //Debug.Log(recentClickedNpc + " click cancled");
                recentClickedNpc.CancleTouched();

                recentClickedNpc = null;
            }
        }

#endregion
}

