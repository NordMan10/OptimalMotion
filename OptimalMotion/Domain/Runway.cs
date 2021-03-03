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

        /// <summary>
        /// Возвращает минимальное время ожидания на ПРСТ
        /// </summary>
        /// <param name="startMoment">Момент выхода на ИСПСТ</param>
        /// <param name="endMoment">Момент покидания ВПП (взлета)</param>
        /// <returns></returns>
        public int GetPreliminaryStartMinDelayTime(IMoment startMoment, IMoment endMoment)
        {
            // Создаем интервал занимания обратившегося судна из переданных им данных;
            var currentInterval = new Interval(startMoment, endMoment);

            // Получаем левый и правый интервалы (метод ЗПД) относительно принятого интервала;
            var leftAndRightIntervals = this.GetLeftAndRightIntervalsRelative(currentInterval);
            var leftInterval = leftAndRightIntervals.Item1;
            var rightInterval = leftAndRightIntervals.Item2;

            // Проверяем пересечение полученного интервала с записанными в ВПП интервалами (метод (2) ЗПД):
            // Если пересечений нет => возвращаем ноль;
            if (leftInterval == null || !this.DoesIntervalsIntersect(currentInterval, leftInterval))
                if (rightInterval == null || !this.DoesIntervalsIntersect(currentInterval, rightInterval))
                    return 0;

            // Если есть => получаем начальный момент (ключ для словаря) последнего обратившегося судна
            var lastAircraftStartMoment = OccupationIntervals.Keys.OrderBy(key => key).Last();

            // Получаем момент покидания ВПП последнего обратившегося судна
            var leaveMoment = OccupationIntervals[lastAircraftStartMoment];

            // Рассчитываем интервал ожидания на ПРСТ = момент покидания ВПП последним записанным судном 
            // минус момент прибытия (без задержки) обратившегося судна;
            var waitingInterval = leaveMoment.Value - currentInterval.StartMoment.Value;

            // Возвращаем полученный интервал;
            return waitingInterval;
        }
    }
}
