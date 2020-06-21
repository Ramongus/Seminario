using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackHoleSpell : MonoBehaviour, IQuery
{
	//IA2-P1
	public SpatialGrid targetGrid;
	public IEnumerable<GridEntity> selected = new List<GridEntity>();
	public float r;

	private QueryManager queryManager;

	private void Awake()
	{
		targetGrid = FindObjectOfType<SpatialGrid>();
		queryManager = FindObjectOfType<QueryManager>();
		queryManager.AddQuery(this);
	}

	public IEnumerable<GridEntity> Query()
	{
		Debug.LogWarning("LA QUERY SE EJECUTA");
		return targetGrid.Query(
			transform.position + new Vector3(-r, 0, -r),
			transform.position + new Vector3(r, 0, r),
			x => Vector3.Distance(transform.position, x) <= r);
	}

	public void DoSpeelEffect(IEnumerable<GridEntity> entitysInSpellRange)
	{
		bool destroy = false;
		//SI LE APLICO EL EFECTO A ALGUNO
		if(entitysInSpellRange.Count() > 0)
		{
			queryManager.RemoveQuery(this);
			destroy = true;
		}

		foreach (var entitys in entitysInSpellRange)
		{
			Destroy(entitys.gameObject);
		}

		if (destroy) Destroy(this.gameObject);

	}

	void OnDrawGizmos()
	{
		if (targetGrid == null) return;

		//Flatten the sphere we're going to draw
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, r);
	}
}
