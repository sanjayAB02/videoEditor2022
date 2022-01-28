using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VideoDuration : MonoBehaviour
{

    public GameObject FiveSecPartPrefab;

    private List<GameObject> _timeBlocks;
    private int _timeBlockCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _timeBlocks = new List<GameObject>();
    }

    public void AddTimeBlocks(float seconds)
    {
        int part = (int)((seconds % 5 == 0) ? seconds / 5 : ((seconds / 5) + 1));
        for (int i = 0; i < part; i++)
        {
            _timeBlocks.Add(Instantiate(FiveSecPartPrefab, transform));
            _timeBlocks[_timeBlockCount].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = GetTimeFromSec(_timeBlockCount * 5);//.ToString();
            _timeBlockCount++;
        }
    }

    private string GetTimeFromSec(int timeBlockCount)
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
