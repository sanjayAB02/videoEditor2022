using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TimelineManager : MonoBehaviour
{
    public GameObject _videoTimeLinePrefab;
    public GameObject _timelineView;
    public GameObject _videoDurationView;
    public GameObject _previewVideoManager;
    public TMP_Text durationText;
    public VideoClip[] videoclips;
    private List<VideoTimeline> _allTimelineManager;
    private int _timelineCount = 0;
    public float _totalDurationInSec = 0;
    public float _currentDurationInSec;
    public Scrollbar _scrollbar;

    void Start()
    {
        _allTimelineManager = new List<VideoTimeline>();
        _scrollbar.onValueChanged.AddListener((float value) =>
        {
            OnScrollValueChange(value);
        });
        
    }

   

    public void AddVideoTimeline()
    {
        _allTimelineManager.Add(Instantiate(_videoTimeLinePrefab, _timelineView.transform).transform.GetChild(2).GetChild(0).GetComponent<VideoTimeline>());
        _allTimelineManager[_timelineCount].SetUpTimelineData(videoclips[_timelineCount],this);
        _videoDurationView.GetComponent<VideoDuration>().AddTimeBlocks(_allTimelineManager[_timelineCount]._videoDuration);
        _previewVideoManager.GetComponent<PreviewVideoManager>().AddVideoTexture(videoclips[_timelineCount],_allTimelineManager[_timelineCount]);
        _timelineCount++;
        UpdateDuration();
    }

    public void RemoveVideoTimeline(VideoTimeline vtRef)
    {
        _allTimelineManager.Remove(vtRef);
        DestroyImmediate(vtRef.gameObject);
    }

    public void UpdateDuration()
    {
        _totalDurationInSec = 0f;
        foreach (VideoTimeline videoTimeline in _allTimelineManager)
        {
            Debug.LogError("Add And UpdateDuration : "+videoTimeline._videoDuration);
            _totalDurationInSec = _totalDurationInSec + videoTimeline._videoDuration;
            if(videoTimeline._trimVideo != null)
                videoTimeline._trimVideo.Invoke();
        }
    }

    
    public void OnScrollValueChange(float arg0)
    {
        _currentDurationInSec = arg0 * _totalDurationInSec;
        durationText.text = GetTimeFromSec(_currentDurationInSec);
        _previewVideoManager.GetComponent<PreviewVideoManager>().PlayVideoWithFrame(_currentDurationInSec);
        // Debug.LogError("ScrollValue : "+_currentDurationInSec);
    }
    
    
    
    public void HideAllControls(VideoTimeline _videoTimeline)
    {
        foreach (VideoTimeline videoTimeline in _allTimelineManager)
        {
            if(videoTimeline != _videoTimeline)
                videoTimeline.HideControls();
        }
    }
    
    private string GetTimeFromSec(float timeBlockCount)
    {
        if (timeBlockCount < 60)
        {
            return timeBlockCount + "s";
        }
        string minutes = Mathf.Floor(timeBlockCount / 60).ToString("00");
        string seconds = (timeBlockCount % 60).ToString("00");
        return string.Format("{0}:{1}s", minutes, seconds);
    }
    
}
