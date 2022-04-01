using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
	[SerializeField] Text timeText;
	public int seconds, minutes;

	private void Start()
	{
		minutes = 3;
		seconds = 1;
		RemoveToSecond();
	}

	void RemoveToSecond()
	{
		seconds--;
		if(seconds < 0)
		{
			seconds = 59;
			minutes--;
		}
		timeText.text = (minutes < 10 ? "0" :"") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
		Invoke(nameof(RemoveToSecond), time: 1);
	}

	public void StopTimer()
	{
		CancelInvoke(nameof(RemoveToSecond));
	}
}
