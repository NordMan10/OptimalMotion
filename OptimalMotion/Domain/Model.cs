using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class Model : IModel
    {
        public Model(int runwayCount, int specPlatformCount, ITable table, Label clockLabel)
        {
            clock = clockLabel;
            aircraftGenerator = new AircraftGenerator(aircraftIdGenerator);
            InitRunways(runwayCount);
            InitSpecPlatforms(specPlatformCount);
            this.table = table;
            InitTimers();
        }

        private readonly Timer takingOffAircraftTimer = new Timer();
        private readonly Timer landingAircraftTimer = new Timer();
        private readonly Timer clockTimer = new Timer();
        private readonly Stopwatch generalStopwatch = new Stopwatch();
        private readonly IAircraftIdGenerator aircraftIdGenerator = AircraftIdGenerator.GetInstance(1);
        private IAircraftGenerator aircraftGenerator;
        private Dictionary<int, IRunway> runways;
        private Dictionary<int, ISpecPlatform> specPlatforms;
        private readonly ITable table;
        private readonly IPlannedMomentFeasibilityController controller = PlannedMomentFeasibilityController.GetInstance();
        private ModelStages stage = ModelStages.Started;
        private Label clock;

        private void InitTimers()
        {
        takingOffAircraftTimer.Interval = 5000;
        takingOffAircraftTimer.Tick += TakingOffAircraftTimerOnTick;

        landingAircraftTimer.Interval = 10000;
        landingAircraftTimer.Tick += LandingAircraftTimerOnTick;

        clockTimer.Interval = 50;
        clockTimer.Tick += ClockTimerOnTick;
        }

        public void ResetClock()
        {
            clock.Text = $"{0:00}:{0:00}:{0:00}";
        }

        public void ResetIdGenerator()
        {
            aircraftIdGenerator.Reset();
        }

        private void ClockTimerOnTick(object sender, EventArgs e)
        {
            var elapsedTime = generalStopwatch.Elapsed;

            clock.Text = string.Format("{0:00}:{1:00}:{2:00}",
                elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds / 10);
        }

        private void TakingOffAircraftTimerOnTick(object sender, EventArgs e)
        {
            var aircraft = aircraftGenerator.GetTakingOffAircraft((int)generalStopwatch.Elapsed.Seconds,
                runways[0], specPlatforms[0]);
            FullWorkProcessWithTakingOffAircraft(aircraft);
        }

        private void LandingAircraftTimerOnTick(object sender, EventArgs e)
        {
            var aircraft = aircraftGenerator.GetLandingAircraft((int)generalStopwatch.ElapsedMilliseconds,
                0);
            FullWorkProcessWithLandingAircraft(aircraft);
        }

        /// <summary>
        /// Обработка события изменения {Стадии выполнения}: (6)
        /// </summary>
        /// <param name="stage"></param>
        public void ChangeStage(ModelStages stage)
        {
            this.stage = stage;

            switch (stage)
            {
                case ModelStages.Preparing:
                    PrepareStageHandler();
                    break;
                case ModelStages.Started:
                    StartedStageHandler();
                    break;
                case ModelStages.Paused:
                    PausedStageHandler();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, "Передана неизвестная стадия выполнения");
            }
        }

        private void PrepareStageHandler()
        {
            // Сбрасываем секундомер;
            generalStopwatch.Reset();

            // Останавливаем таймеры;
            takingOffAircraftTimer.Stop();
            landingAircraftTimer.Stop();
            clockTimer.Stop();

            // Очищаем интервалы {Спец площадки} и {ВПП}
            foreach (var specPlatform in specPlatforms.Values)
            {
                specPlatform.ResetOccupationIntervals();
            }

            foreach (var runway in runways.Values)
            {
                runway.ResetOccupationIntervals();
            }

            // Очищаем {Таблицу}
            table.Reset();
        }

        private void StartedStageHandler()
        {
            takingOffAircraftTimer.Start();
            landingAircraftTimer.Start();
            clockTimer.Start();
            generalStopwatch.Start();
        }

        private void PausedStageHandler()
        {
            takingOffAircraftTimer.Stop();
            landingAircraftTimer.Stop();
            clockTimer.Stop();
            generalStopwatch.Stop();
        }

        private void InitRunways(int runwayCount)
        {
            runways = new Dictionary<int, IRunway>();
            for (var i = 0; i < runwayCount; i++)
            {
                var runway = new Runway(i);
                runways.Add(i, runway);
            }
        }

        private void InitSpecPlatforms(int specPlatformCount)
        {
            specPlatforms = new Dictionary<int, ISpecPlatform>();
            for (var i = 0; i < specPlatformCount; i++)
            {
                var specPlatform = new SpecPlatform(i);
                specPlatforms.Add(i, specPlatform);
            }
        }

        /// <summary>
        /// Регистрация {ВС} на {ВПП}: (3)
        /// </summary>
        /// <param name="runwayId"></param>
        /// <param name="occupationInterval"></param>
        private void RegisterAircraftAtRunway(int runwayId, IInterval occupationInterval)
        { // Мы не должны добавлять интервал занимания до того, как рассчитаем все временные данные в ВС!
            // Получаем интервал занимания ВПП текущим судном;
            var currentInterval = occupationInterval;
            
            // Добавляем интервал занимания ВПП через метод (3) ЗПД;
            runways[runwayId].AddInterval(currentInterval);
        }

        /// <summary>
        /// Регистрация {Взлетающего ВС} на {Спец площадке}: (4)
        /// </summary>
        /// <param name="specPlatformId"></param>
        /// <param name="occupationInterval"></param>
        private void RegisterAircraftAtSpecPlatform(int specPlatformId, IInterval occupationInterval)
        {
            // Получаем интервал занимания Спец площадки текущим судном;
            var currentInterval = occupationInterval;

            // Добавляем интервал занимания Спец площадки через метод (3) ЗПД;
            specPlatforms[specPlatformId].AddInterval(currentInterval);
        }

        /// <summary>
        /// Вывод информации в таблицу: (5)
        /// </summary>
        /// <param name="tableCreationData"></param>
        private void DisplayInformationInTable(ITableRowCreationData tableCreationData)
        {
            var data = tableCreationData;
            table.AddRow(data);
        }

        /// <summary>
        /// Полный рабочий цикл {Взлетающего ВС}: (7)
        /// </summary>
        /// <param name="aircraft"></param>
        private void FullWorkProcessWithTakingOffAircraft(ITakingOffAircraft aircraft)
        {
            // Проверяем выполнимость планового выхода на ПРСТ
            // Если выполнимо =>
            if (IsPlannedMomentFeasible(aircraft))
            {    
                // Получаем интервал занимания ВПП через метод {ВВС} (И.2)
                var runwayOccupationInterval = aircraft.GetRunwayOccupationInterval();

                // Регистрируем {ВВС} на ВПП через метод (3);
                RegisterAircraftAtRunway(aircraft.GetRunwayId(), runwayOccupationInterval);

                var engineStartMoment = aircraft.GetEngineStartMoment();

                //  Если обработка нужна =>
                if (aircraft.ProcessingIsNeeded)
                {
                    // Получаем интервал занимания {Спец площадки} через метод {ВВС} (И.3)
                    var specPlatformOccupationInterval = aircraft.GetSpecPlatformOccupationInterval();

                    // Регистрируем {ВВС} на {Спец площадке} через метод (4)
                    RegisterAircraftAtSpecPlatform(aircraft.GetSpecPlatformId(), specPlatformOccupationInterval);
                }

                

                // Добавляем информацию о ВС в {Таблицу} через метод (5)
                // Собираем данные для публикации в таблице
                var tableRowData = GetTableRowCreationData(aircraft, engineStartMoment.Value.ToString(), true);

                // Добавляем информацию о ВС в {Таблицу} через метод (5)
                DisplayInformationInTable(tableRowData);
            }
            else
            {
                // Если не выполнимо => 

                var engineStartMoment = aircraft.GetEngineStartMoment();

                // Собираем данные для публикации в таблице
                var tableRowData = GetTableRowCreationData(aircraft, engineStartMoment.Value.ToString(), false);

                // Добавляем информацию о ВС в {Таблицу} через метод (5);
                DisplayInformationInTable(tableRowData);
            }
        }

        private TableRowCreationData GetTableRowCreationData(ITakingOffAircraft aircraft, string engineStartMoment, bool feasibility)
        {
            return new TableRowCreationData(aircraft.Id.GetId(), aircraft.Moments[Moments.Appearance].Value.ToString(),
                aircraft.Moments[Moments.SpecPlatformLeave].Value.ToString(),
                aircraft.Moments[Moments.PreliminaryStartArrival].Value.ToString(),
                aircraft.Moments[Moments.PlannedPreliminaryStartArrival].Value.ToString(),
                engineStartMoment, aircraft.Intervals[Intervals.ProcessingWaiting].ToString(),
                aircraft.Intervals[Intervals.SafeMergeWaiting].ToString(), 
                aircraft.Intervals[Intervals.PreliminaryStartWaiting].ToString(),
                aircraft.ProcessingIsNeeded, feasibility);
        }

        private void FullWorkProcessWithLandingAircraft(ILandingAircraft aircraft)
        {
            var runwayOccupationInterval = aircraft.GetRunwayOccupationInterval();
            RegisterAircraftAtRunway(aircraft.GetRunwayId(), runwayOccupationInterval);
        }

        private bool IsPlannedMomentFeasible(ITakingOffAircraft aircraft)
        {
            // Если обработка нужна => вызываем метод {Контроллера} (И.2) и передаем {ВВС}
            if (aircraft.ProcessingIsNeeded)
                return controller.IsFeasibleWithProcessing(aircraft);

            // Если обработка не нужна => вызываем метод {Контроллера} (И.1) и передаем {ВВС};
            return controller.IsFeasibleWithoutProcessing(aircraft);
        }
    }
}
