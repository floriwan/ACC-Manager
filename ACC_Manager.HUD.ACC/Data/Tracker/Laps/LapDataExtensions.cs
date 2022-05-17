﻿using ACC_Manager.Util.NumberExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCManager.HUD.ACC.Data.Tracker.Laps
{
    public static class LapDataExtensions
    {
        public static float GetLapTime(this LapData lap)
        {
            return lap.Time / 1000;
        }

        public static float GetSector1(this LapData lap)
        {
            return lap.Sector1 / 1000;
        }

        public static float GetSector2(this LapData lap)
        {
            return lap.Sector2 / 1000;
        }

        public static float GetSector3(this LapData lap)
        {
            return lap.Sector3 / 1000;
        }

        public static float GetFuelLeft(this LapData lap)
        {
            return lap.FuelLeft / 1000;
        }


        public static int GetAverageLapTime(this List<LapData> laps)
        {
            return laps.GetAverageLapTime(laps.Count);
        }

        public static int GetAverageLapTime(this List<LapData> laps, int lapAmount)
        {
            lapAmount.ClipMax(laps.Count);
            if (lapAmount == 0)
                return 0;

            int total = 0;
            for (int i = 0; i < lapAmount; i++)
                total += laps[laps.Count - 1 - (lapAmount - i)].Time;

            return total / lapAmount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sector">1, 2 or 3</param>
        /// <param name="time">laptime as int</param>
        /// <returns>true if the given sector time is faster than any others of that sector in these laps</returns>
        public static bool IsSectorFastest(this List<LapData> laps, int sector, int time)
        {
            List<LapData> data = laps;

            sector.Clip(1, 3);

            switch (sector)
            {
                case 1:
                    foreach (LapData timing in data)
                        if (timing.IsValid && timing.Sector1 < time)
                            return false; break;

                case 2:
                    foreach (LapData timing in data)
                        if (timing.IsValid && timing.Sector2 < time)
                            return false; break;

                case 3:
                    foreach (LapData timing in data)
                        if (timing.IsValid && timing.Sector3 < time)
                            return false; break;

                default: return true;
            }

            return true;
        }
    }
}
