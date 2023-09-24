using Godot;
using System;

public partial class CharacterStateMachine : Node
{
    [Export] public CharacterState currentState;
    [Export] public CharacterBody2D character;
    [Export] public AnimationTree animationTree;

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is CharacterState state)
            {
                state.character = character;
                state.animationTree = animationTree;
            }
            else
                GD.PushWarning($"Child {child.Name} is not a State for CharacterStateMachine.");
        }
        if ( currentState != null)
            currentState.OnEnter();
        animationTree.Active = true;
    }
    public override void _PhysicsProcess(double delta)
    {
        currentState.StateProcess(delta);
        if (currentState.nextState != null)
            SwitchState(currentState.nextState);
        if (!currentState.canMove)
            character.Velocity = Vector2.Zero;
    }

    private void SwitchState(CharacterState newState)
    {
        if(currentState != null)
        {
            currentState.OnExit();
            currentState.nextState = null;
        }
        currentState = newState;
        currentState.OnEnter();
    }
}
