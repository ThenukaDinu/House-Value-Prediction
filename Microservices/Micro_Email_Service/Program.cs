using Micro_Email_Service.Services;

public class Program
{
    static void Main(string[] args)
    {
        var service = new MessageConsumer();
        service.Consume();
    }
}