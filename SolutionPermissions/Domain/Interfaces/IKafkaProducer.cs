
namespace Domain.Interfaces
{
    public interface IKafkaProducer<T>
    {
        Task SendMessageAsync(T message);
    }
}
