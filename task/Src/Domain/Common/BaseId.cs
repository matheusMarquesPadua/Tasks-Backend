namespace task.Src.Domain.Common;

public class BaseId <TId>
{
    public TId Id { get; set; }
}

public abstract class BaseId : BaseId<int>
{

}
