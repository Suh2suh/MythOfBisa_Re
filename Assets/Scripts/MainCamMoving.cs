using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamMoving : MonoBehaviour
{
    public Transform mainCamTransform;
    public Transform topViewTransform;
    Transform playerViewTransform = null;

    [SerializeField]
    float speedOfConvert;

    float lerpPercent = 0f;

    [SerializeField]
    bool isTopViewMode;


    public void OnOffTopViewMode()
	{
        isTopViewMode = !isTopViewMode;
    }


    void Update()
    {
        ConvertCamera();
    }

    void ConvertCamera()
    {
        if (isTopViewMode)
        {
            GoToTopView();
        }
        else
        {
            GoToPlayerView();
        }

        ChangeLerpPercentAndCheckIsDone();
    }


    /*
     <When you Press "Map Btn" in Game>
      From PlayerView -> TopView
      Make MainCam as TopViewPos's child to lift it off from Player
    */
    void GoToTopView()
	{
        if (!isTopViewMode)
            isTopViewMode = true;

        if (mainCamTransform.parent != topViewTransform.transform)
            mainCamTransform.parent = topViewTransform.transform;

        mainCamTransform.rotation = Quaternion.Slerp(mainCamTransform.rotation, topViewTransform.rotation, lerpPercent);
        mainCamTransform.position = Vector3.Slerp(mainCamTransform.position, topViewTransform.position, lerpPercent);
    }

     /*
      <When you Press "Back Btn" in Map>
       From TopView -> PlayerView
       Make MainCam as Player's child For Player View
    */
    void GoToPlayerView()
	{
        if (isTopViewMode)
            isTopViewMode = false;

        if (!playerViewTransform)
		{
            playerViewTransform = GameObject.Find("PlayerCameraPos").transform;
        }

        if (!isTopViewMode)
        {
            if (mainCamTransform.parent != playerViewTransform.transform)
                mainCamTransform.parent = playerViewTransform.transform;
        }

        mainCamTransform.rotation = Quaternion.Slerp(mainCamTransform.rotation, playerViewTransform.rotation, lerpPercent);
        mainCamTransform.position = Vector3.Slerp(mainCamTransform.position, playerViewTransform.position, lerpPercent);
    }

    void ChangeLerpPercentAndCheckIsDone()
	{
        if (lerpPercent < 1)
        {
            lerpPercent = Mathf.MoveTowards(lerpPercent, 1f, speedOfConvert * Time.deltaTime);
        }
        else
        {
            if (lerpPercent != 0)
            {
                lerpPercent = 0;
            }
        }
    }
}
