using UnityEngine;
using System.Collections;

public interface IState {

	/// <summary>
	/// Funcion para setear a que maquina de estado pertenece este estado.
	/// </summary>
	/// <param name="sm">La maquina de estados que maneja este estado</param>
	void SetStateMachine();

	/// <summary>
	/// Función que se llama cuando se entra al estado.
	/// </summary>
	void StateAwake();

	/// <summary>
	/// Función que se llama cuando se sale del estado.
	/// </summary>
	void StateSleep();

	/// <summary>
	/// Función que se llama constantemente mientras se encuentre en el estado.
	/// </summary>
	void StateExecute();

	string GetStateName();
}
