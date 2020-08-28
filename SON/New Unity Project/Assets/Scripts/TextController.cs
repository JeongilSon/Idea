using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextController : MonoBehaviour
{
	Text txt;
	string story;

	void Start()
	{
		txt = txt.GetComponent<Text>();
		story = txt.text;
		txt.text = "";
		
		// TODO: add optional delay when to start
		StartCoroutine("PlayText");		
	}
	IEnumerator PlayText()
	{
		foreach (char c in story)
		{
			txt.text += c;
			yield return new WaitForSeconds(0.2f);
		}
	}

}
