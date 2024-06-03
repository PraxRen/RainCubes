using System;

public interface ILifetime
{
    public float MinLifetime { get; }
    public float MaxLifetime { get; }

    public event Action<ILifetime> Died;
}