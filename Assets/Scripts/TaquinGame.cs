using UnityEngine;
using UnityEngine.UI;

public class TaquinGame : MonoBehaviour
{
	[SerializeField] private Transform emptySpace;
	[SerializeField] private TaquinTiles[] tiles;
	[SerializeField] private GameObject panelEnd;
	[SerializeField] private Text panelEndTimeText;
	[SerializeField] private Text recordText;
	private Camera _camera;
	private int emptySpaceIndex = 0;
	private bool _isFinish;

	void Start()
	{
		panelEnd.SetActive(false);
		_isFinish = false;
		_camera = Camera.main;
		ShuffleTiles();
		emptySpaceIndex = findIndex(emptySpace.GetComponent<TaquinTiles>());
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			if(hit)
			{
				if(Vector2.Distance(emptySpace.position, hit.transform.position) < 0.08f)
				{
					Vector2 lastEmptySpacePosition = emptySpace.position;
					TaquinTiles thisTiles = hit.transform.GetComponent<TaquinTiles>();
					emptySpace.GetComponent<TaquinTiles>().targetPosition = thisTiles.targetPosition;
					thisTiles.targetPosition = lastEmptySpacePosition;
					int tileIndex = findIndex(thisTiles);
					tiles[emptySpaceIndex] = tiles[tileIndex];
					tiles[tileIndex] = emptySpace.GetComponent<TaquinTiles>();
					emptySpaceIndex = tileIndex;
				}
				Debug.Log(hit.transform.name);
			}
		}
		if (!_isFinish)
		{
			int correctTile = 0;
			foreach (var a in tiles)
			{
				if (a.correct)
				{
					correctTile++;
				}
			}

			if (correctTile == tiles.Length)
			{
				_isFinish = true;
				panelEnd.SetActive(true);
				var t =	GetComponent<TimerScript>();
				t.StopTimer();
				panelEndTimeText.text = "Your Time :   " + (t.minutes < 10 ? "0" : " ") + t.minutes + ":" + (t.seconds < 10 ? "0" : " ") + t.seconds;
				int bestTime;
				if (PlayerPrefs.HasKey("bestTime"))
				{
					bestTime = PlayerPrefs.GetInt("bestTime");
				}
				else
				{
					bestTime = 999999;
				}
				int playerTime = t.minutes * 60 + t.seconds;
				if (playerTime < bestTime)
					PlayerPrefs.SetInt("bestTime", playerTime);
				else
				{
					int minutes = bestTime / 60;
					int seconds = bestTime - minutes*60;
					recordText.text = (minutes < 10 ? "0" : " ") + minutes + ":" + (seconds < 10 ? "0" : " ") + seconds;
				}
			}
		}
	}

	public void ShuffleTiles()
	{
		int inversion;
		do
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				var lastPosition = tiles[i].targetPosition;
				int randomInt = Random.Range(0, 8);
				tiles[i].targetPosition = tiles[randomInt].targetPosition;
				tiles[randomInt].targetPosition = lastPosition;
				var tile = tiles[i];
				tiles[i] = tiles[randomInt];
				tiles[randomInt] = tile;				
			}
			inversion = GetInversions();
		}
		while (inversion % 2 != 0);
	}

	public int findIndex(TaquinTiles ts)
	{
		for(int i = 0; i < tiles.Length; i++)
		{
			if(tiles[i] == ts)
			{
				return i;
			}
		}
		return -1;
	}

	int GetInversions()
	{
		int inversionsSum = 0;
		for(int i = 0; i < tiles.Length; i++)
		{
			int thisTileInversion = 0;
			for (int j = i; j < tiles.Length; j++)
			{
				if(tiles[j] != null)
				{
					if (tiles[i].number > tiles[j].number)
					{
						thisTileInversion++;
					}
				}
			}
			inversionsSum += thisTileInversion;
		}
		return inversionsSum;
	}

}
