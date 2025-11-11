namespace BJackSim
{
    public interface IStrategyService
    {
        Decision MakeDecision(ParticipantType participantType, HandType handType, int dealerHandValue, int playerHandValue = 0);
    }
}