using UnityEngine;
using UnityEngine.Video;

public class CustomVidShip2 : MonoBehaviour
{
   private VideoPlayer videoPlayer;
    private bool isVideoSet = false; // Flag to check if video is already set
    private float elapsedTime = 0f; // Time tracker
    
    private void Start()
    {
        // Get the VideoPlayer component
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (!isVideoSet)
        {
            elapsedTime += Time.deltaTime; // Increase elapsed time

            // If isHypno becomes true or 1 second has passed
            if (GlobalVariables.isHypno || elapsedTime >= 3.0f)
            {
                SetVideo();
                isVideoSet = true; // Mark the video as set
            }
        }
    }

    private void SetVideo()
    {
        // Based on the value of isHypno, set the clip
        if (GlobalVariables.isHypno)
        {
            videoPlayer.clip = Resources.Load<VideoClip>("Video/hypno");
        }
        else
        {
            videoPlayer.clip = Resources.Load<VideoClip>("Video/code");
        }

        // Play the video
        videoPlayer.Play();
    }
}
