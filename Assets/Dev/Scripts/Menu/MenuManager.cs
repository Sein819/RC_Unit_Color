using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start(){
        Application.targetFrameRate = 60;
    }

    void Update(){
        
    }

    public void GameStart(){
        SceneManager.LoadScene("Game");
    }
}
