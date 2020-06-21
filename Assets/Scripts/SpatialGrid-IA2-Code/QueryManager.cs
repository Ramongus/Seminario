using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueryManager : MonoBehaviour
{
	//IA2-P1

	private List<IQuery> _allQuerys = new List<IQuery>();
	private List<IQuery> _querysToAdd = new List<IQuery>();
	private List<IQuery> _querysToRemove = new List<IQuery>();

	private void Update()
	{
		DoQuerys();
	}

	private void LateUpdate()
	{
		AddQuerys();
		RemoveQuerys();
	}

	public void AddQuery(IQuery q)
	{
		_querysToAdd.Add(q);
	}

	public void RemoveQuery(IQuery q)
	{
		_querysToRemove.Add(q);
	}

	private void DoQuerys()
	{
		foreach (var query in _allQuerys)
		{
			query.DoSpeelEffect(query.Query());
		}
	}

	private void AddQuerys()
	{
		foreach (var query in _querysToAdd)
		{
			_allQuerys.Add(query);
		}
		_querysToAdd.Clear();
	}
	private void RemoveQuerys()
	{
		foreach (var query in _querysToRemove)
		{
			_allQuerys.Remove(query);
		}
		_querysToRemove.Clear();
	}
}
