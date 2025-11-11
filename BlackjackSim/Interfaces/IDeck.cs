
namespace BJackSim
{
    public interface IDeck
    {
        List<string> Deal(int numCards);
        void Shuffle();
    }
}