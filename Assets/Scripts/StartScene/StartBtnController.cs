using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtnController : MonoBehaviour
{
    public void moveToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
