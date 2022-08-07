using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchOnScreen : MonoBehaviour
{
   Camera cam;
    public static bool isTouchDetectNeeded;
    public ShowMapDescription MapDescriptionShower;
    public GUIManager guiManager;

    private void Awake()
	{
        cam = transform.GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
    {
        if(isTouchDetectNeeded)
		{
            /*
			#region MobileTouch
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.current.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;


                //if Layer is "MapPin"
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == 6)
                {
                    Debug.Log(hit.transform.parent.name);   
                }
            }
            #endregion
            */

            #region PCTouch
            if(guiManager.MapStage != 3)
			{
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;


                    //if Layer is "MapPin"
                    if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == 6)
                    {
                        MapDescriptionShower.BuildingName = hit.transform.parent.name;

                        if(MapDescriptionShower.StartIndex > -1)
                            guiManager.MapStage = 3;

                        MapDescriptionShower.StartIndex = -1;
                    }
                }
            }
            #endregion
        }
	}
}
