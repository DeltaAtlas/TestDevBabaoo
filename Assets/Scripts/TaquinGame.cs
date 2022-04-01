using UnityEngine;
using UnityEngine.UI;

public class TaquinGame : MonoBehaviour
{
	#region Scripts
	[SerializeField] private TimerScript time;
	[SerializeField] private TaquinTiles[] tiles;
	#endregion
	
	[SerializeField] private Transform emptySpace;

	#region UI
	[SerializeField] private GameObject panelEnd;
	[SerializeField] private GameObject panelOver;
	[SerializeField] private Text panelEndTimeText;
	[SerializeField] private Text recordText;
	#endregion

	private Camera _camera;
	private int emptySpaceIndex = 0;
	private bool _isFinish;
	private bool _timeOver;

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
			//Raycast au click
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			if(hit)
			{
				//Check si le pièce peut se déplacer sur la case vide
				if(Vector2.Distance(emptySpace.position, hit.transform.position) < 0.025f)
				{
					//Mouvement de la case et l'emplacement vide
					Vector2 lastEmptySpacePosition = emptySpace.position;
					TaquinTiles thisTiles = hit.transform.GetComponent<TaquinTiles>();
					emptySpace.GetComponent<TaquinTiles>().targetPosition = thisTiles.targetPosition;
					thisTiles.targetPosition = lastEmptySpacePosition;
					int tileIndex = findIndex(thisTiles);

					//Modification des index
					tiles[emptySpaceIndex] = tiles[tileIndex];
					tiles[tileIndex] = emptySpace.GetComponent<TaquinTiles>();
					emptySpaceIndex = tileIndex;
				}
				Debug.Log(hit.transform.name);
			}
		}

		#region Fin de Partie
		//Win
		if (!_isFinish)
		{
			//Compte le nombre de case au bon emplacement
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

				#region activation et modification UI
				panelEnd.SetActive(true);
				time.StopTimer();
				if (time.seconds == 0)
				{
					time.seconds = 60;
					time.minutes--;
				}
				panelEndTimeText.text = "Your Time :   " + (time.minutes < 10 ? "0" : " ") + (2- time.minutes) + ":" + (time.seconds < 10 ? "0" : "") + (time.seconds > 59 ? "0" : "") + (60 - time.seconds);
				#endregion

				#region Création Meilleur Score
				
			    int bestTime = PlayerPrefs.GetInt("bestTime");
				int playerTime = time.minutes * 60 + time.seconds;
				if (playerTime < bestTime)
					PlayerPrefs.SetInt("bestTime", playerTime);
				else
				{
					int minutes = bestTime / 60;
					int seconds = bestTime - minutes*60;
					recordText.text = (minutes < 10 ? "0" : " ") + minutes + ":" + (seconds < 10 ? "0" : " ") + seconds;
				}
				#endregion
			}
		}

		//Lose
		if (!_timeOver)
		{
			var t = GetComponent<TimerScript>();
			if (time.minutes == 0 && time.seconds == 0)
			{
				panelOver.SetActive(true);
				t.StopTimer();
			}
		}
		#endregion
	}

	//Tirage aléatoire des positions de cases
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

	//Connaître l'index d'une case
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

	//Faire des Inversions afin d'avoir un puzzle soluble
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
