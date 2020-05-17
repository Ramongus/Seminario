using UnityEngine;
using System.Collections;

public class State {

    protected StateMachineClassic _sm;

    public State(StateMachineClassic sm)
    {
        _sm = sm;
    }

    public virtual void Awake() { }

    public virtual void Sleep() { }

    public virtual void Execute() { }

    public virtual void LateExecute() { }
}
