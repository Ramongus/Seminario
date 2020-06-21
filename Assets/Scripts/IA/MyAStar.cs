using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyAStar : IUpdate
{
	private static MyAStar _instance;
	public static MyAStar Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new MyAStar();
			}
			return _instance;
		}
	}

	private ANode initialNode;
	private ANode finalNode;
	private List<ANode> closedNodes;
	private List<ANode> openNodes;
	private Stack<ANode> pathNodes;
	private Stack<ANode> resultPath;

	private Queue<IUsePathfinding> requestPath;
	private bool isDoingPath;

	private MyAStar()
	{
		requestPath = new Queue<IUsePathfinding>();
		closedNodes = new List<ANode>();
		openNodes = new List<ANode>();
		pathNodes = new Stack<ANode>();
		resultPath = new Stack<ANode>();
		EventsManager.TriggerEvent("SuscribeToUpdateManager", this);
		Debug.Log("A* Initialized");
	}

	public void MyUpdate()
	{
		Debug.Log("A* is updating");
		while(requestPath.Count > 0)
		{
			if (!isDoingPath)
			{
				IUsePathfinding actualRequest = requestPath.Dequeue();
				SetInitialNode(actualRequest.GetInitialNode());
				SeTFinalNode(actualRequest.GetFinalNode());
				actualRequest.SetPath(GetPath());
			}
		}
	}

	public void AddRequester(IUsePathfinding requester)
	{
		requestPath.Enqueue(requester);
	}

	public Stack<ANode> GetPath()
	{
		Debug.LogWarning("GettingPath");
		isDoingPath = true;
		pathNodes.Clear();
		pathNodes = new Stack<ANode>(ExecuteDijkstra(initialNode, finalNode));
		isDoingPath = false;
		return pathNodes;
	}

	public Stack<ANode> ExecuteDijkstra(ANode initial, ANode end)
	{
		//Al reutilizar la función, reseteamos los valores de los nodos.
		foreach (var item in closedNodes)
		{
			item.Reset();
		}
		foreach (var item in openNodes)
		{
			item.Reset();
		}

		//Limpiamos las listas.
		closedNodes.Clear();
		openNodes.Clear();
		resultPath.Clear();

		//Agregamos nuestro nodo inicial a la lista de los nodos a visitar
		openNodes.Add(initialNode);
		//Reiniciamos los valores del nodo.
		initialNode.G = 0;
		initialNode.previous = null;

		int iterations = 0;

		while (openNodes.Count > 0)
		{
			iterations++;
			//Tomamos un elemento de la lista de openNodes. Vamos a conseguir el nodo con menor F.
			ANode current = LookForLowerF();

			//Si el nodo actual es el destino.
			if (current == finalNode)
			{
				//Uso un stack para el camino final ya que como estamos empezando desde el nodo final hasta el nodo inicial vamos a tener el camino al revés, pero
				//si me lo guardo en un stack ya lo invierto.
				while (current != null)
				{
					resultPath.Push(current);
					current = current.previous;
				}

				return resultPath;
			}

			foreach (var item in current.neighbours)
			{
				var neighNode = item.Key;
				var neighDist = item.Value;

				//Si el nodo vecino ya fue analizado continuamos a la siguiente iteración.
				if (closedNodes.Contains(neighNode))
				{
					continue;
				}

				//Evitamos repetir nodos preguntando primero si están en el Queue antes de agregarlo.
				if (!openNodes.Contains(neighNode))
				{
					//Calculo por unica vez el costo de la H del nodo. En este caso, la forma más corta de llegar al objetivo
					//es yendo en linea recta.
					neighNode.H = Vector3.Distance(neighNode.transform.position, end.transform.position);

					openNodes.Add(neighNode);
				}

				//Preguntamos si el camino que estamos analizando es más barato que el que ya tiene el nodo.
				if (current.G + neighDist < neighNode.G)
				{
					//Reemplazo los valores en caso de que hayamos encontrado una mejor opción.
					neighNode.G = current.G + neighDist;
					neighNode.previous = current;
				}

			}

			//Analizado todos sus vecinos, agregamos el nodo actual a la lista de nodos ya revisados para evitar loops infinitos.
			closedNodes.Add(current);
			//Remuevo el nodo ya que terminamos de trabajar con él.
			openNodes.Remove(current);
		}
		Debug.LogError("TE ESTA DEVOLVIENDO NULO EL PATH");
		//Si llegué hasta acá es porque no había forma de llegar al nodo final desde el nodo inicial.
		return null;
	}

	public ANode LookForLowerF()
	{
		ANode nextNode = openNodes[0];

		foreach (var node in openNodes)
		{
			if (node.F < nextNode.F)
			{
				nextNode = node;
			}
		}
		return nextNode;
	}

	private void SetInitialNode(ANode node)
	{
		initialNode = node;
	}

	private void SeTFinalNode(ANode node)
	{
		finalNode = node;
	}
}
