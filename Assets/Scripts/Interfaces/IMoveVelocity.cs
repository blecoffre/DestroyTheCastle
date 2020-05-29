using UnityEngine;

/// <summary>
/// Attached to all Entities we wanted to be allowed to move
/// </summary>
public interface IMoveVelocity
{
    public void SetVelocity(Vector3 velocityVector);

    public void Disable();

    public void Enable();
}
