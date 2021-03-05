using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public interface IModel
    {
        void ChangeStage(ModelStages stage);
        void ResetClock();
        void ResetIdGenerator();
    }
}
