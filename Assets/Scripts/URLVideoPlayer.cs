using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class URLVideoPlayer : MonoBehaviour {
	
	private RawImage rawImage;
	private VideoPlayer videoPlayer;

    // Use this for initialization
    void Start () {
		rawImage = this.GetComponent <RawImage>();
		videoPlayer = this.GetComponent <VideoPlayer> ();
	}

	// Update is called once per frame
	void Update () {
		rawImage.texture = videoPlayer.texture;
	}
}
