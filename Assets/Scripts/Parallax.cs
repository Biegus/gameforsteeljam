using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	[SerializeField] private float MovementRate;

	[SerializeField] private Transform Camera;

	[SerializeField] 
	private string LayerName;
	
	[SerializeField] 
	private int OrderInLayer;
	
	[SerializeField] 
	private Color Color;
	
	private void OnValidate()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			var sprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
			sprite.sortingLayerName = LayerName;
			sprite.sortingOrder = OrderInLayer;
			sprite.color = Color;
		}
	}

	private void Update()
	{
		transform.localPosition = new Vector2(-Camera.position.x * MovementRate, transform.position.y);
	}
}
