﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlliancesPlugin.NewCaptureSite
{
    public class CaptureSite
    {
        public string Name = "Default, change this";
        public Boolean AllianceSite = false;
        public int amountCaptured = 0;
        public int MinutesBeforeCaptureStarts = 10;
        public Boolean ChangeCapSiteOnUnlock = true;
        public String LinkedTerritory = "Change this";
        public Boolean LockAfterSoManyHoursOpen = false;
        public int LockAgainAfterThisManyHours = 2;
        public Boolean CaptureStarted = false;
        public Boolean HasTerritory = false;

        public Boolean Locked = false;
        public Boolean UnlockAtTheseTimes = false;
        public List<int> HoursToUnlockAfter = new List<int>();

        public Location GetNewCapSite(Location ignore)
        {
            Random random = new Random();
            List<Location> temp = new List<Location>();
            foreach (Location loc in locations)
            {
                if (!loc.Name.Equals(ignore))
                {
                    if (random.NextDouble() <= loc.chance)
                    {
                        temp.Add(loc);
                    }
                }
            }
            if (temp.Count == 1)
            {
                return temp[0];
            }
          random = new Random();
            int r = random.Next(temp.Count);
            return temp[r];
        }

        public int SecondsBetweenCaptureCheck = 60;
        public int PointsPerCap = 10;
        public int PointsToCap = 100;
        public int CurrentSite = 1;
        public Guid AllianceOwner = Guid.Empty;
        public Guid CapturingAlliance = Guid.Empty;
        public long FactionOwner = 1;
        public long CapturingFaction = 1;
        public int hourCooldownAfterFail = 1;
        public int hoursToLockAfterCap = 12;

        public DateTime nextCaptureAvailable = DateTime.Now;
        public Boolean doChatMessages = true;
        public Boolean doDiscordMessages = true;

        public DateTime nextCaptureInterval = DateTime.Now;

        public DateTime unlockTime = DateTime.Now;

        public Location GetCurrentLocation()
        {
            return null;
        }
        public List<Location> locations = new List<Location>();
        public List<LootLocation> loot = new List<LootLocation>();
    }
}
