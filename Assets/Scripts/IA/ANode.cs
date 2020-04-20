using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANode : MonoBehaviour
{
	public float G { get; set; }
	public float H { get; set; }
	public float F { get { return G + H; } }
	public ANode Previous { get; set; }

	private Dictionary<ANode, float> neighbours = new Dictionary<ANode, float>();
	public float neighboursCount;

	[SerializeField] float radiusToNeighbours;

	void Awake()
	{
		List<ANode> nodes = FindObjectOfType<NodesList>().GetNodes(); ;
		if (nodes.Contains(this))
		{
			nodes.Remove(this);
		}
		foreach (ANode node	in nodes)
		{
			float distance = DistanceToNode(node);
			if(distance <= radiusToNeighbours)
			{
				neighbours.Add(node, distance);
				neighboursCount++;
			}
		}
	}

	public void Reset()
	{
		G = Mathf.Infinity;
		Previous = null;
	}

	private float DistanceToNode(ANode node)
	{
		return (node.transform.position - transform.position).magnitude;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		foreach (var neighbour in neighbours)
		{
			Gizmos.DrawLine(transform.position, neighbour.Key.transform.position);
		}
	}
}
