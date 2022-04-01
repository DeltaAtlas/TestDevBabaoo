using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaquinTiles : MonoBehaviour
{
	public int number;
    public Vector3 targetPosition;
	private Vector3 correctPosition;
	public bool correct;

	void Awake()
	{
		targetPosition = transform.position;
		correctPosition = transform.position;
	}

	void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1f);
		if(transform.position == correctPosition)
		{
			correct = true;
		}
		else 
		{ 
			correct = false; 
		}
    }
}
