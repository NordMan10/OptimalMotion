using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public static class ISerialAccessZoneExtensions
    {

        /// <summary>
        /// Находим ближайшие моменты прибытия слева и справа относительно момента прибытия обратившегося судна
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static Tuple<Interval, Interval> GetLeftAndRightIntervalsRelative(this ISerialAccessZone zone, IInterval interval)
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

            // Через соседние индексы получаем начальные моменты(по сути ключи словаря) левого и правого интервала;
            var leftIntervalStartMoment = orderedKeysList[currentIntervalIndex - 1];
            var rightIntervalStartMoment = orderedKeysList[currentIntervalIndex + 1];

            // Находим интервалы;
            var leftInterval = new Interval(leftIntervalStartMoment, zone.OccupationIntervals[leftIntervalStartMoment]);
            var rightInterval = new Interval(rightIntervalStartMoment, zone.OccupationIntervals[rightIntervalStartMoment]);

            // Возвращаем эти моменты;
            return Tuple.Create(leftInterval, rightInterval);
        }

        public static bool DoesIntervalIntersect(this ISerialAccessZone zone, IInterval interval)
        {
            // Принимаем интервал обратившегося судна;
            var currentInterval = interval;

            // Получаем левый и правый интервалы;
            var leftAndRightIntervals = zone.GetLeftAndRightIntervalsRelative(currentInterval);

            // Если начальный момент текущего интервала меньше конечного момента левого интервала => пересечение;
            if (currentInterval.StartMoment < leftAndRightIntervals.Item1.EndMoment)
                return true;
            // Если конечный момент текущего интервала больше начального момента правого интервала => пересечение;
            if (currentInterval.EndMoment > leftAndRightIntervals.Item2.StartMoment)
                return true;

            // Если ни то и ни другое => нет пересечения;
            return false;
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
