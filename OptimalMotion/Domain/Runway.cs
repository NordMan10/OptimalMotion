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
        /// Метод выделения интервала в конце списка
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public Tuple<int, int> GetNewLastInterval(IInterval interval)
        {
            // 1) Принимаем интервал обратившегося судна;
            var currentInterval = interval;

            // Получаем начальный момент (ключ для словаря) последнего обратившегося судна
            var lastAircraftInterval = OccupationIntervals.Keys.OrderBy(key => key).Last();

            // Получаем момент покидания ВПП последнего обратившегося судна
            var leaveMoment = 

            // 2) Рассчитываем интервал ожидания на ПРСТ = момент покидания ВПП последним записанным судном 
            // минус момент прибытия (без задержки) обратившегося судна;

            // 3) Возвращаем сохраненный интервал;
        }

        public int GetPreliminaryStartMinWaitingTime(IMoment startMoment, IMoment endMoment)
        {
            // Создаем интервал занимания обратившегося судна из переданных им данных;
            var currentInterval = new Interval(startMoment, endMoment);

            // 2) Проверяем пересечение полученного интервала с записанными в ВПП интервалами (метод (2) ЗПД):
            //  2.1) Если пересечений нет => возвращаем ноль;
            if (!this.DoesIntervalIntersect(currentInterval))
                return 0;
            // 2.2) Если есть => возвращаем значение метода выделения интервала в конце списка;
            throw new NotImplementedException();
            

        }
    }
}
