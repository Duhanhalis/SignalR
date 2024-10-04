using SignalRDenemesi.Models;

namespace SignalRDenemesi.Interface
{
    public interface IMessageClient<T> where T : class
    {
        Task ReceiveMessage(List<T> appContexts);
    }
}
