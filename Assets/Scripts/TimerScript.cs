using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
	[SerializeField] Text timeText;
    public int seconds, minutes;

	private void Start()
	{
		seconds = -1;
		AddToSecond();
	}

	void AddToSecond()
	{
		seconds++;
		if(seconds > 59)
		{
			seconds = 0;
			minutes++;
		}
		timeText.text = (minutes < 10 ? "0" :" ") + minutes + ":" + (seconds < 10 ? "0" : " ") + seconds;
		Invoke(nameof(AddToSecond), time: 1);
	}

	public void StopTimer()
	{
		CancelInvoke(nameof(AddToSecond));
	}
}
