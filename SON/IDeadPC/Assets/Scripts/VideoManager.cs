using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;
    public VideoPlayer player;
    // Start is called before the first frame update
    private void Awake()
    {
        if (GameManager.instance.stageCount < 4)
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        player = GetComponent<VideoPlayer>();
    }
    public void VIdeoSIngle(VideoClip clip)
    {
        player.clip = clip;
        player.Play();
    }
}
