using System.Collections.Generic;

public interface IUsePathfinding
{
	void SetPath(Stack<ANode> path);

	ANode GetInitialNode();
	ANode GetFinalNode();
}
