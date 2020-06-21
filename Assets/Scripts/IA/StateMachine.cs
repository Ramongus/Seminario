using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StateMachine : MonoBehaviour {
	
	[SerializeField] protected string defaultState;
    protected IStateMono _currentState;
    protected List<IStateMono> _states = new List<IStateMono>();

	protected virtual void Awake()
	{
		IStateMono[] allStates = GetComponents<IStateMono>();
		for (int i = 0; i < allStates.Length; i++)
		{
			AddState(allStates[i]);
		}
		if (_states.Count == 0)
		{
			Debug.LogError("RECUERDE AGREGAR AL OBJETO AL MENOS UN ESTADO");
			Debug.LogError("#####");
			Debug.LogError("REMEMBER AT LEAST ATTACH ONE STATE TO THE OBJECT");
			throw new MissingComponentException();
		}

		_currentState = FindStateByName(defaultState);
		if (_currentState == null)
		{
			_currentState = _states[0];
		}
	}

	/// <summary>
	/// Llama al execute del estado actual.
	/// </summary>
	protected virtual void Update()
    {
        if (_currentState != null)
            _currentState.StateExecute();
    }

    /// <summary>
    /// Agrega un estado.
    /// </summary>
    /// <param name="s">El estado a agregar.</param>
    protected void AddState(IStateMono s)
    {
        _states.Add(s);
        if (_currentState == null)
            _currentState = s;
    }

    /// <summary>
    /// Cambia de estado.
    /// </summary>
    public void SetState<T>() where T : IStateMono
    {
        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i].GetType() == typeof(T))
            {
                _currentState.StateSleep();
                _currentState = _states[i];
                _currentState.StateAwake();
            }
        }
    }

    public bool IsActualState<T>() where T : IStateMono
    {
        return _currentState.GetType() == typeof(T);
    }

    /// <summary>
    /// Busca el índice de un estado por su tipo.
    /// </summary>
    /// <param name="t">Tipo de estado.</param>
    /// <returns>Devuelve el índice.</returns>
    private int SearchState(Type t)
    {
        int ammountOfStates = _states.Count;
        for (int i = 0; i < ammountOfStates; i++)
            if (_states[i].GetType() == t)
                return i;
        return -1;
    }
	/// <summary>
	/// Busca y devuelve un estado perteneciente a la maquina de estados por nombre, si no encuentra ninguno con dicho nombre devuelve nulo.
	/// </summary>
	/// <param name="name">El nombre del estado a buscar</param>
	/// <returns></returns>
	protected IStateMono FindStateByName(string name)
	{
		for (int i = 0; i < _states.Count; i++)
		{
			if(_states[i].GetStateName() == name)
			{
				return _states[i];
			}
		}
		return null;
	}

	public void SetStateByName(string name)
	{
		for (int i = 0; i < _states.Count; i++)
		{
			if (_states[i].GetStateName() == name)
			{
				_currentState.StateSleep();
				_currentState = _states[i];
				_currentState.StateAwake();
			}
		}
	}
}
