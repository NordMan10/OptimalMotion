using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class Runway : IRunway
    {
        public Runway(int id)
        {
            Id = id;
            OccupationIntervals = new Dictionary<IMoment, IMoment>();
        }

        public int Id { get; }

        public Dictionary<IMoment, IMoment> OccupationIntervals { get; }

        public int GetPreliminaryStartMinWaitingTime(IMoment startMoment, IMoment endMoment)
        {
            // Создаем интервал занимания обратившегося судна из переданных им данных;
            var currentInterval = new Interval(startMoment, endMoment);

            // Проверяем пересечение полученного интервала с записанными в ВПП интервалами (метод (2) ЗПД):
            // Если пересечений нет => возвращаем ноль;
            if (!this.DoesIntervalIntersect(currentInterval))
                return 0;

            // Если есть => получаем начальный момент (ключ для словаря) последнего обратившегося судна
            var lastAircraftStartMoment = OccupationIntervals.Keys.OrderBy(key => key).Last();

            // Получаем момент покидания ВПП последнего обратившегося судна
            var leaveMoment = OccupationIntervals[lastAircraftStartMoment];

            // Рассчитываем интервал ожидания на ПРСТ = момент покидания ВПП последним записанным судном 
            // минус момент прибытия (без задержки) обратившегося судна;
            var waitingInterval = leaveMoment - currentInterval.StartMoment;

            // Возвращаем полученный интервал;
            return waitingInterval;
        }
    }
}
