using System;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
	public event Action<GridEntity> OnMove = delegate { };
	public Vector3 velocity = new Vector3(0, 0, 0);
	public bool onGrid;

    public int weight;

    

	private Renderer _renderer;

	protected virtual void Awake()
	{
		_renderer = GetComponent<Renderer>();
	}

	protected virtual void Update()
	{
		OnMove(this);
	}
}
