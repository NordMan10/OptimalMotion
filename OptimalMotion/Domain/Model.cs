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
        public Model(int runwayCount, int specPlatformCount, ITable table)
        {
            aircraftGenerator = new AircraftGenerator(aircraftIdGenerator);
            InitRunways(runwayCount);
            InitSpecPlatforms(specPlatformCount);
            this.table = table;
        }

        private Timer takingOffAircrafTimer = new Timer();
        private Timer landingAircrafTimer = new Timer();
        private Stopwatch generalStopwatch = new Stopwatch();
        private IAircraftIdGenerator aircraftIdGenerator = AircraftIdGenerator.GetInstance(1);
        private IAircraftGenerator aircraftGenerator;
        private Dictionary<int, IRunway> runways;
        private Dictionary<int, ISpecPlatform> specPlatforms;
        private ITable table;
        private IPlannedMomentFeasibilityController controller = PlannedMomentFeasibilityController.GetInstance();
        private ModelStages stage = ModelStages.Started;


        public void ChangeStage(ModelStages stage)
        {
            this.stage = stage;

            switch (stage)
            {
                case ModelStages.Preparing:
                    PrepareStageHandler();
                    break;
                case ModelStages.Started:
                    
                    break;
                case ModelStages.Paused:

                    break;

            }
        }

        private void PrepareStageHandler()
        {
            // Сбрасываем секундомер;
            generalStopwatch.Reset();

            // Останавливаем таймеры;
            takingOffAircrafTimer.Stop();
            landingAircrafTimer.Stop();

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

        }

        private void PausedStageHandler()
        {

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
    }
}
