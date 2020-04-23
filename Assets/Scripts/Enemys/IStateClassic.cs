using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateClassic
{
	void OnAwake();
	void OnUpdate();
	void OnChangeState();
}
