using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class PreviewVideoManager : MonoBehaviour
{
    public GameObject _previewParent;
    public GameObject _previewVideoTexturePrefab;
    public RenderTexture[] tex;
    public List<PreviewVideo> _allPreviewVideo;
    public bool _videoPlaying = false;
    private int _videoCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddVideoTexture(VideoClip videoclip, VideoTimeline videoTimeline)
    {
        _allPreviewVideo.Add(Instantiate(_previewVideoTexturePrefab, _previewParent.transform)
            .GetComponent<PreviewVideo>());
        _allPreviewVideo[_videoCount].gameObject.name = _allPreviewVideo[_videoCount].gameObject.name + _videoCount;
        _allPreviewVideo[_videoCount]._startTime =
            (float) _videoCount == 0
                ? 0f
                : _allPreviewVideo[_videoCount - 1]
                    ._endTime; //videoTimeline._startTime;//(float)_videoCount == 0 ? 0f : _allPreviewVideo[_videoCount-1]._endTime;
        _allPreviewVideo[_videoCount]._endTime = (float) (_videoCount == 0
                ? videoclip.length
                : _allPreviewVideo[_videoCount - 1]._endTime + videoclip.length
            ); //videoTimeline._endTime;//(float)(_videoCount == 0 ? videoclip.length : _allPreviewVideo[_videoCount-1]._endTime + videoclip.length);
        _allPreviewVideo[_videoCount].SetUpVideoData(videoclip, this, tex[_videoCount], videoTimeline);
        _videoCount++;
    }

    public void RemoveVideoTexture(PreviewVideo pvRef)
    {
        _allPreviewVideo.Remove(pvRef);
        DestroyImmediate(pvRef.gameObject);
    }

    public void PlayVideoWithFrame(float time)
    {
        foreach (PreviewVideo previewVideo in _allPreviewVideo)
        {
            previewVideo.PlayVideoWithFrame(time);
        }
    }


    public void Play(int videoIndex)
    {
        _videoPlaying = true;
        PlayAllVideo(videoIndex, b =>
        {
            videoIndex++;
            if (videoIndex < _allPreviewVideo.Count)
            {
                Play(videoIndex);
            }
            else
            {
                _videoPlaying = false;
            }
        });
    }


    private void PlayAllVideo(int videoIndex, Action<bool> played)
    {
        _allPreviewVideo[videoIndex].PlayVideoFull(_allPreviewVideo[videoIndex]._startTime, played);
    }
}