namespace Broker.Local
{
    public class RequestEnvelope<T>
    {
        public T Resource { get; set; }
        public string Destination { get; set; }
    }
}
