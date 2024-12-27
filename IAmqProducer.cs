public interface IAmqProducer
{
    Task SendMessageAsync(string message);
}
