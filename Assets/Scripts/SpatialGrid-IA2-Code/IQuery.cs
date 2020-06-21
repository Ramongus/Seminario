using System.Collections.Generic;

public interface IQuery
{
	IEnumerable<GridEntity> Query();

	void DoSpeelEffect(IEnumerable<GridEntity> entitysInSpellRange);
}
