using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTimeline : MonoBehaviour
{
    public GameObject _thumbTimelineImagePrefab;
    private VideoPlayer _videoPlayer;
    private double _videoLenth;
    private int _onePartSize = 6;
    private int FrameIndex = 0;
    private Image img;
    private bool FrameIsReady = false;
    private double _onePartTime;
    public Image[] TimelineImages;


    [Header("Resize Controls")] 
    public GameObject LeftMover;
    public GameObject RightMover;
    public GameObject CenterSelectedPanel;
    private TimelineManager _timelineManagerInstance;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            // Debug.LogError("Button Click");
            if (!CenterSelectedPanel.activeSelf)
            {
                CenterSelectedPanel.SetActive(true);
                LeftMover.SetActive(true);
                RightMover.SetActive(true);
            }

            _timelineManagerInstance.HideAllControls(this);
        });
        
        
    }

    public void HideControls()
    {
        CenterSelectedPanel.SetActive(false);
        LeftMover.SetActive(false);
        RightMover.SetActive(false);
    }

    // Start is called before the first frame update
  

    public void SetUpTimelineData(VideoClip videoclip, TimelineManager timelineManager)
    {
        // Debug.LogError("videoclip lenth : "+videoclip.length);
        _timelineManagerInstance = timelineManager;
        _onePartSize = ((int)videoclip.length / 5) + 1;
        
        // if(videoclip.length < 10f)
        //     _onePartSize = (int)videoclip.length / 2;//onePartSize;
        //     //onePartSize;
        // else if(videoclip.length < 50f)
        //     _onePartSize = (int)videoclip.length / 4;//onePartSize;
        // else if(videoclip.length < 90f)
        //     _onePartSize = (int)videoclip.length / 6;//onePartSize;
        // else if(videoclip.length < 180f)
        //     _onePartSize = (int)videoclip.length / 8;//onePartSize;
        // else if(videoclip.length < 540f)
        //     _onePartSize = (int)videoclip.length / 16;//onePartSize;

        TimelineImages = new Image[_onePartSize];
        for (int i = 0; i < _onePartSize; i++)
        {
            TimelineImages[i] =  Instantiate(_thumbTimelineImagePrefab, transform).GetComponent<Image>();
        }
        
        // Debug.LogError(" abc : "+_timelineView.transform.childCount);
        // TimelineImages = new Image[_timelineView.transform.childCount];
        // for (int i = 0; i < _timelineView.transform.childCount; i++)
        // {
        //     TimelineImages[i] = transform.GetChild(i).GetComponent<Image>();
        // }
      
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.clip = videoclip;
        _videoPlayer.renderMode = VideoRenderMode.APIOnly;
        _videoPlayer.playOnAwake = false;
        _videoPlayer.time = 2;
        _videoPlayer.Prepare();
        _videoPlayer.sendFrameReadyEvents = true;
        _videoPlayer.frameReady += Vp_frameReady;
        _videoPlayer.prepareCompleted += Vp_prepareCompleted;
        _videoLenth = _videoPlayer.clip.length;
        print("_videoLenth : " + _videoLenth);
        _onePartTime = _videoLenth / _onePartSize;
        print("_onePartTime : " + _onePartTime);
    }
    

    private void Vp_prepareCompleted(VideoPlayer source)
    {
        Debug.Log("prepared");
        source.Play();
    }

    private void Vp_frameReady(VideoPlayer source, long frameidx)
    {
        // img = _timelineView.transform.GetChild(FrameIndex).GetComponent<Image>();
        // Debug.LogError("Image NAme : "+TimelineImages.Length +"FrameIndex : "+FrameIndex);
        // Debug.LogError("FrameIndex-1 : "+FrameIndex+  "< _onePartSize : "+_onePartSize);
        if (FrameIndex < _onePartSize)
        {
            GetSpriteFromTexture(source.texture, TimelineImages[FrameIndex]);
            FrameIndex++;
            if((_videoPlayer.time + _onePartTime) < _videoPlayer.length) ;
                _videoPlayer.time += _onePartTime;
        }
        else
        {
            transform.parent.parent.GetComponent<RectTransform>().sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
            transform.parent.parent.GetChild(0).GetComponent<LeftResizePanel>().maxSize = transform.GetComponent<RectTransform>().sizeDelta;
            transform.parent.parent.GetChild(1).GetComponent<LeftResizePanel>().maxSize = transform.GetComponent<RectTransform>().sizeDelta;
        //     transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().rect.width, transform.GetComponent<RectTransform>().rect.height);
        //     Debug.LogError("size delta2 : "+  transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta);
        }
    }

    public void GetSpriteFromTexture(Texture texx, Image imagee)
    {
        // Debug.LogError("Image NAme : " + imagee.name);
        var thumbnail = new Texture2D(texx.width, texx.height, TextureFormat.RGBA32, false);
        RenderTexture cTexture = RenderTexture.active;
        RenderTexture rTexture = new RenderTexture(texx.width, texx.height, 32);
        UnityEngine.Graphics.Blit(texx, rTexture);

        thumbnail.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0);
        thumbnail.Apply();

        Sprite sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height),
            new Vector2(thumbnail.width / 2, thumbnail.height / 2));
        imagee.sprite = sprite;
    }
}