
namespace BJackSim
{
    public interface IHandService
    {
        int CalculateValue(List<string> cards);
        HandType GetHandType(List<string> cards);
        bool IsBust(List<string> cards);
    }
}