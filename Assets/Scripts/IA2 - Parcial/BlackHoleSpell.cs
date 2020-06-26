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
		if(entitysInSpellRange.Any())
		{
			queryManager.RemoveQuery(this);
			destroy = true;
		}
        //IA2-P3
        var orderEntities = entitysInSpellRange.Where(x => x.gameObject.activeInHierarchy).OrderBy(x => x.weight);
		foreach (var entitys in orderEntities)
		{
			Destroy(entitys.gameObject);
		}


		if (destroy) StartCoroutine("TimerToDestroy");

	}

	void OnDrawGizmos()
	{
		if (targetGrid == null) return;
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, r);
	}

    public IEnumerator TimerToDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
