using System;
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
            var result = (int) currentStatus;
            result += data;
            result = Math.Min(2, Math.Max(result, 0));
            return (Colour) result;
        }
    }
}