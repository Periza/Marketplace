using System.Diagnostics.CodeAnalysis;

namespace Marketplace.Domain;

public abstract class Entity
{
    private readonly List<object> _events;

    protected Entity() => _events = new List<object>();

    protected void Apply(object @event)
    {
        When(@event);
        EnsureValidState();
    }

    protected abstract void When(object @event);
    
    public IEnumerable<object> GetChanges() => _events.AsEnumerable();
    
    public void ClearChanges() => _events.Clear();
    
    protected abstract void EnsureValidState();
}