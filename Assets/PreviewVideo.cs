using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PreviewVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    public float _startTime = 0;
    public float _endTime = 0;
    private Action<bool> playedCallback;
    private VideoTimeline _videoTimelineRef;
    private PreviewVideoManager _previewVideoManagerRef;

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        // var tex = new RenderTexture (512, 512, 16, RenderTextureFormat.ARGB32);
        // tex.Create();
        // _videoPlayer.targetTexture = tex;
        // GetComponent<RawImage>().texture = tex;
        _videoPlayer.Prepare();
    }

    private void LateUpdate()
    {
        if (_videoPlayer.isPlaying)
            Debug.LogError("update : _endTime : " + _endTime + "< _videoPlayer.time : " + _videoPlayer.time);
        
        if (_endTime <= _videoPlayer.time && _videoPlayer.isPlaying && _previewVideoManagerRef._videoPlaying)
        {
            _videoPlayer.Pause();
            playedCallback.Invoke(true);
            if (GetComponent<RawImage>().enabled)
                GetComponent<RawImage>().enabled = false;
        }
    }

    public void SetUpVideoData(VideoClip videoclip, PreviewVideoManager previewVideoManager, RenderTexture tex,
        VideoTimeline videoTimeline)
    {
        _previewVideoManagerRef = previewVideoManager;
        GetComponent<VideoPlayer>().targetTexture = tex;
        GetComponent<RawImage>().texture = tex;
        GetComponent<VideoPlayer>().clip = videoclip;
        _videoTimelineRef = videoTimeline;
        // _videoTimelineRef._trimVideo = () =>
        // {
        //     Debug.LogError("Trim Video : ");
        //     _startTime = _videoTimelineRef._startTime;
        //     _endTime = _videoTimelineRef._endTime;
        // };
    }

    public void PlayVideoWithFrame(float time)
    {
        // Debug.LogError("Videoplayer time1 : " + time);
        if (_startTime <= time && _endTime >= time)
        {
            if (!GetComponent<RawImage>().enabled)
                GetComponent<RawImage>().enabled = true;
            
            _videoPlayer.frame = ((long) _videoPlayer.frameCount * (long) time) / (long) _videoPlayer.length;
            _videoPlayer.Play();
            _videoPlayer.Pause();
            // Debug.LogError("Videoplayer time2 : " + _videoPlayer.frameCount);
        }
        else
        {
            if (GetComponent<RawImage>().enabled)
                GetComponent<RawImage>().enabled = false;
        }
        
    }

    public void PlayVideoFull(float time, Action<bool> played)
    {
        playedCallback = played;
        time += _startTime;
        Debug.LogError("time : "+time);
        if (_startTime <= time && _endTime >= time)
        {
            if (!GetComponent<RawImage>().enabled)
                GetComponent<RawImage>().enabled = true;

            _videoPlayer.frame = ((long) _videoPlayer.frameCount * (long) time) / (long) _videoPlayer.length;
            _videoPlayer.Play();
            // _videoPlayer.
            // Debug.LogError("Videoplayer time2 : " + _videoPlayer.frameCount);
        }
        else
        {
            _videoPlayer.Pause();
            played.Invoke(true);
            if (GetComponent<RawImage>().enabled)
                GetComponent<RawImage>().enabled = false;
        }
    }
}