using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject achievment;
    public Image[] achievmentCheck;
    public Text clearCountText;

    void Start(){
        Application.targetFrameRate = 60;

        achievment.SetActive(false);

        if(!PlayerPrefs.HasKey("ClearCount")){
            InitAchievement();
        }
    }

    void Update(){
        
    }

    public void GameStart(){
        SceneManager.LoadScene("Game");
    }

    void InitAchievement(){
        PlayerPrefs.SetInt("ClearCount",0);
        PlayerPrefs.SetInt("Achieve_Red",0);
        PlayerPrefs.SetInt("Achieve_Green",0);
        PlayerPrefs.SetInt("Achieve_Blue",0);
        PlayerPrefs.SetInt("Achieve_Black",0);
        PlayerPrefs.SetInt("Achieve_White",0);
    }

    public void ShowAchievment(){
        achievment.SetActive(true);
        clearCountText.text=$"Clear: {PlayerPrefs.GetInt("ClearCount")}";
        if(PlayerPrefs.GetInt("Achieve_Red")==1) achievmentCheck[0].color=new UnityEngine.Color(74/255f,1f,80/255f);
        else achievmentCheck[0].color=new UnityEngine.Color(1f,73/255f,73/255f);
        if(PlayerPrefs.GetInt("Achieve_Green")==1) achievmentCheck[1].color=new UnityEngine.Color(74/255f,1f,80/255f);
        else achievmentCheck[1].color=new UnityEngine.Color(1f,73/255f,73/255f);
        if(PlayerPrefs.GetInt("Achieve_Blue")==1) achievmentCheck[2].color=new UnityEngine.Color(74/255f,1f,80/255f);
        else achievmentCheck[2].color=new UnityEngine.Color(1f,73/255f,73/255f);
        if(PlayerPrefs.GetInt("Achieve_Black")==1) achievmentCheck[3].color=new UnityEngine.Color(74/255f,1f,80/255f);
        else achievmentCheck[3].color=new UnityEngine.Color(1f,73/255f,73/255f);
        if(PlayerPrefs.GetInt("Achieve_White")==1) achievmentCheck[4].color=new UnityEngine.Color(74/255f,1f,80/255f);
        else achievmentCheck[4].color=new UnityEngine.Color(1f,73/255f,73/255f);
    }

    public void HideAchievment(){
        achievment.SetActive(false);
    }
}
