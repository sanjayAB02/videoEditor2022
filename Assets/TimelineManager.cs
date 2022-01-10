using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TimelineManager : MonoBehaviour
{
    public GameObject _videoTimeLinePrefab;
    public GameObject _timelineView;
    public VideoClip[] videoclips;
    private List<VideoTimeline> _allTimelineManager;
    private int _timelineCount = 0;

    void Start()
    {
        _allTimelineManager = new List<VideoTimeline>();
    }

    public void AddVideoTimeline()
    {
        _allTimelineManager.Add(Instantiate(_videoTimeLinePrefab, _timelineView.transform).transform.GetChild(2).GetChild(0).GetComponent<VideoTimeline>());
        _allTimelineManager[_timelineCount].SetUpTimelineData(videoclips[_timelineCount],this);
        _timelineCount++;
    }

    public void HideAllControls(VideoTimeline _videoTimeline)
    {
        foreach (VideoTimeline videoTimeline in _allTimelineManager)
        {
            if(videoTimeline != _videoTimeline)
                videoTimeline.HideControls();
        }
    }
    
}
