﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.PlayerLoop;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public GameObject video;    
	public AudioClip firstBgmSource, secondBgmSource, thirdBgmSource;
	public AudioClip keyboardSource;
	public VideoClip cizLogo, ideaLogo;
	public VideoClip outro;
	public GameObject key;
	public int stageCount = 0;
	public bool textScene;
	string story = "";
	Text txt;
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != null)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);		
	}
	void Start()
	{
		txt = GetComponentInChildren<Text>();
		StartCoroutine(VideoPlay(stageCount));
	}
    private void Update()
    {
		StageClear();
	}
	IEnumerator VideoPlay(int stageCount)
    {
		video.SetActive(true);
		if (stageCount == 0) // 첫 시작 씬
		{
			VideoManager.instance.VIdeoSIngle(cizLogo);
			yield return new WaitForSeconds(4f);
			VideoManager.instance.VIdeoSIngle(ideaLogo);
			yield return new WaitForSeconds(2f);
			VideoManager.instance.player.clip = null;
			video.SetActive(false);
			Text(stageCount);
		}
	}
    public void StageClear()
    {
        if (gravity.clearCheck == true && stageCount < 3)
        {
			SceneManager.LoadScene(0);
			Text(stageCount);
			stageCount++;
			gravity.clearCheck = false;
		}
		else if (gravity.clearCheck == true && stageCount == 3)
			SceneManager.LoadScene("LastScene");
		if (textScene && Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.RawButton.B))
		{
			textScene = false;
			if (stageCount == 0)
				stageCount++;
			StopAllCoroutines();
			SoundManager.instance.StopKeyboard();
			SceneManager.LoadScene(stageCount);
		}
	}
	public void Text(int stageCount)
    {
		textScene = true;
		story = "";		
		txt = txt.GetComponent<Text>();
		if(stageCount == 0) 
		{
			SoundManager.instance.BgmSingle(firstBgmSource);
			txt.text = "현조는 나 \nNIS(국정원) 안에 서울에 내가 같혀 있었다.\n침이 날이 가뜩이나 서 있는 주사를 맞아버렸다.\n졸려서 찢고 싶은 잠이다." +
			        	"\n세상이 나를 중심으로 궁그려 돌고 있다.\n내가 움직일수 있다.\n나는 이데아";
		}
		else if(stageCount == 1)
        {
			SoundManager.instance.BgmSingle(secondBgmSource);
			txt.text = "1라운드 클리어";

        }
		else if(stageCount == 2)
        {
			SoundManager.instance.BgmSingle(thirdBgmSource);
			txt.text = "2라운드 클리어";
        }
		story = txt.text;		
		txt.text = "";
		//TODO: add optional delay when to start
		StartCoroutine("PlayText");	
	}
	IEnumerator PlayText()
	{	
		SoundManager.instance.KeyboardSingle(keyboardSource);
		foreach (char c in story)
		{
			if (textScene)
			{
				txt.text += c;
				yield return new WaitForSeconds(0.13f);
			}
		}
		if (stageCount == 0)
		{
			key.SetActive(true);
		}
		story = "";
		//textScene = false;
		SoundManager.instance.StopKeyboard();

	}
}
