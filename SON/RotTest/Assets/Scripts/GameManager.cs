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
	public GameObject onGameButton;
	public GameObject clearStageUI;
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

		foreach (char c in story)
		{
			txt.text += c;
			yield return new WaitForSeconds(0.125f);
		}
		onGameButton.SetActive(true);

	}

	public void OnGameStartButton()
	{
		SceneManager.LoadScene(1);
	}
	public void StageClear()
    {
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
