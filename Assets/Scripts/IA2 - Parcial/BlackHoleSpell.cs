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


    private IOrderedEnumerable<GridEntity> myEntities;
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
        StartCoroutine("TimerToDestroy");
        //SI LE APLICO EL EFECTO A ALGUNO
        if (entitysInSpellRange.Any())
		{
            queryManager.RemoveQuery(this);
        }
        //IA2-P3

        // Aca obtenemos todos los objetos de la gridEntity que estan activos y los ordenamos en base a su peso.
        // Entonces a la hora de eliminarlos, va destruyendo de menor a mayor, con un cierto tiempo entre uno y otro.
        // Filtramos por los que estan activos en escena, ya que nos destruia los que tenian grid entity pero estaban desactivados en escena.
        myEntities = entitysInSpellRange.Where(x => x.gameObject.activeInHierarchy).OrderBy(x => x.weight);
        StartCoroutine("TimerToDestroyEntities");
	}


    public IEnumerator TimerToDestroy()
    {
        yield return new WaitForSeconds(1.6f);
        queryManager.RemoveQuery(this);
        Destroy(this.gameObject);
    }
    public IEnumerator TimerToDestroyEntities()
    {
        foreach (var entitys in myEntities)
        {
            Destroy(entitys.gameObject);
            yield return new WaitForSeconds(0.3f);         
        }
    }
	void OnDrawGizmos()
	{
		if (targetGrid == null) return;
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, r);
	}
}
