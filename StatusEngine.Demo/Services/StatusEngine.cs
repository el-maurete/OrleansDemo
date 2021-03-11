using StatusEngine.Demo.Model;

namespace StatusEngine.Demo.Services
{
    public interface IStatusEngine
    {
        Colour Calculate(Colour currentStatus, int data);
    }
    
    public class StatusEngine : IStatusEngine
    {
        public Colour Calculate(Colour currentStatus, int data)
        {
            return data < 1 ? Colour.red
                : data < 5 ? Colour.amber
                : Colour.green;
        }
    }
}