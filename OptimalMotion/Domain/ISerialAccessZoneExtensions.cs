using System;
using System.Linq;

namespace OptimalMoving.Domain
{
    public static class ISerialAccessZoneExtensions
    {
        /// <summary>
        /// Находим ближайшие моменты прибытия слева и справа относительно момента прибытия обратившегося судна
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="interval"></param>
        /// <returns>Если интервалы найдены — возвращает эти интервалы. Если нет, возвращает Tuple с двумя null</returns>
        public static Tuple<IInterval, IInterval> GetLeftAndRightIntervalsRelative(this ISerialAccessZone zone, IInterval interval)
        {
            // Получаем интервал обратившегося судна;
            var currentInterval = interval;
            // Локально получаем список ключей словаря из полей класса;
            var occupationIntervalsKeys = zone.OccupationIntervals.Keys.ToList();
            // Добавляем в список начальный момент полученного интервала;
            occupationIntervalsKeys.Add(currentInterval.StartMoment);

            // Сортируем список по возрастанию;
            // Проследи, чтобы было реализовано сравнение моментов
            var orderedKeysList = occupationIntervalsKeys.OrderBy(key => key).ToList();

            // Получаем индекс начального момента текущего интервала;
            var currentIntervalIndex = orderedKeysList.IndexOf(currentInterval.StartMoment);

            // Через соседние индексы получаем начальные моменты(по сути ключи словаря) левого и правого интервала если они есть:
            IInterval leftInterval = null;
            IInterval rightInterval = null;

            // Если есть левый
            if (currentIntervalIndex > 0)
            {
                // Находим интервал;
                var leftIntervalStartMoment = orderedKeysList[currentIntervalIndex - 1];
                leftInterval = new Interval(leftIntervalStartMoment, zone.OccupationIntervals[leftIntervalStartMoment]);
            }

            // Если есть правый
            if (currentIntervalIndex + 1 < orderedKeysList.Count)
            {
                // Находим интервал;
                var rightIntervalStartMoment = orderedKeysList[currentIntervalIndex + 1];
                rightInterval = new Interval(rightIntervalStartMoment, zone.OccupationIntervals[rightIntervalStartMoment]);
            }

            // Возвращаем эти моменты;
            return Tuple.Create(leftInterval, rightInterval);
        }

        /// <summary>
        /// Определяет пересекаются ли два интервала
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="interval1Param"></param>
        /// <param name="interval2Param"></param>
        /// <returns></returns>
        public static bool DoesIntervalsIntersect(this ISerialAccessZone zone, IInterval interval1Param, IInterval interval2Param)
        {
            // Принимаем интервалы;
            var interval1 = interval1Param;
            var interval2 = interval2Param;

            // Определяем какой из них левый, а какой правый:
            // Если начальный момент одного меньше начального момента другого => первый является левым
            IInterval leftInterval;
            IInterval rightInterval;

            if (interval1.StartMoment.Value < interval2.StartMoment.Value)
            {
                leftInterval = interval1;
                rightInterval = interval2;
            }
            else
            {
                leftInterval = interval2;
                rightInterval = interval1;
            }

            // Если начальный момент правого интервала меньше конечного момента левого интервала => пересечение;
            // Если нет => нет пересечения;
            return rightInterval.StartMoment.Value < leftInterval.EndMoment.Value;
        }

        public static void AddInterval(this ISerialAccessZone zone, IInterval interval)
        {
            // Получаем интервал;
            // Добавляем в словарь;
            zone.OccupationIntervals.Add(interval.StartMoment, interval.EndMoment);
        }

        public static void RemoveInterval(this ISerialAccessZone zone, IInterval interval)
        {
            // Получаем интервал;
            // Удаляем интервал по ключу(начальному моменту переданного интервала);
            zone.OccupationIntervals.Remove(interval.StartMoment);
        }
    }
}
