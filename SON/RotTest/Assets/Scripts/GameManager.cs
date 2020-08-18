using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text playerText;
    public Text firstLoadText;
    string storyText;
    PlayerController player;
    private void Awake()
    {
        firstLoadText = firstLoadText.GetComponent<Text>();
        firstLoadText.text = "";

        // TODO: add optional delay when to start
        StartCoroutine("PlayText");
    }
    // Start is called before the first frame update
    void Start()
    {
        playerText = playerText.GetComponent<Text>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerText.text = "Hp : " + player.playerHp;
    }
    IEnumerator PlayText()
    {
        foreach (char c in storyText)
        {
            firstLoadText.text += c;
            yield return new WaitForSeconds(0.125f);
        }
        SceneManager.LoadScene(1);
    }
}
