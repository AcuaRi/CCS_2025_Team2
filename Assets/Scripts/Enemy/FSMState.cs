using System;

public class FSMState
{
    public Action OnEnter;
    public Action OnUpdate;
    public Action OnExit;
    
    public FSMState(Action onEnter, Action onUpdate, Action onExit)
    {
        OnEnter = onEnter;
        OnUpdate = onUpdate;
        OnExit = onExit;
    }
}