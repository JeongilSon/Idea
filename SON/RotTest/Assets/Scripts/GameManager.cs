using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
    public Text txt;
	public GameObject clearStageUI;
	public AudioClip firstBgmSource, secondBgmSource;
	public AudioClip keyboardSource;
	public int stageCount = 0;
	string story = "";
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	void Start()
	{
		SoundManager.instance.BgmSingle(firstBgmSource);
		txt = txt.GetComponent<Text>();
		story = txt.text;
		txt.text = "";

		// TODO: add optional delay when to start
		StartCoroutine("PlayText");
	}
    private void Update()
    {
		StageClear();
    }
    IEnumerator PlayText()
	{
		SoundManager.instance.KeyboardSingle(keyboardSource);
		foreach (char c in story)
		{		
			txt.text += c;
			yield return new WaitForSeconds(0.13f);
		}
		SoundManager.instance.KeyboardSingle(null);
	}
	public void StageClear()
    {
		if(Input.GetMouseButtonDown(0))
        {
			if (stageCount == 0)
			{
				++stageCount;
				SceneManager.LoadScene(stageCount);
				SoundManager.instance.KeyboardSingle(null);
			}
        }
		if(PlayerController.clearCheck == true)
        {
			PlayerController.roundCount++;
			clearStageUI.SetActive(true);
        }		
		
    }
	public void NextStageButton()
    {
		SceneManager.LoadScene(2);
		//if(PlayerController.roundCount < 3)
		clearStageUI.SetActive(false);
		//PlayerController.clearCheck = false;
    }
}
