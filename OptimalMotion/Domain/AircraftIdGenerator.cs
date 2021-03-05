using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class AircraftIdGenerator : IAircraftIdGenerator
    {
        protected AircraftIdGenerator(int id)
        {
            this.id = id;
        }

        private static AircraftIdGenerator instance;
        private static object syncRoot = new object();
        private int id;

        public static IAircraftIdGenerator GetInstance(int initIdValue)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new AircraftIdGenerator(initIdValue);
                }
            }
            return instance;
        }

        public void Reset()
        {
            id = 1;
        }

        // Выделяем Id еще и для садящихся
        public IAircraftId GetUniqueAircraftId()
        {
            return new AircraftId(id++);
        }

        IAircraftIdGenerator IAircraftIdGenerator.GetInstance(int initIdValue)
        {
            return GetInstance(initIdValue);
        }
    }
}
