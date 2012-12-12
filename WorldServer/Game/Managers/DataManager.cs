/*
 * Copyright (C) 2012 Arctium <http://>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Framework.Database;
using Framework.Logging;
using Framework.Singleton;
using System;
using System.Collections.Generic;
using WorldServer.Game.ObjectDefines;
using WorldServer.Game.WorldEntities;

namespace WorldServer.Game.Managers
{
    public class DataManager : SingletonBase<DataManager>
    {
        Dictionary<Int32, Creature> Creatures;

        DataManager()
        {
            Creatures = new Dictionary<Int32, Creature>();

            Initialize();
        }

        public void Add(Creature creature)
        {
            Creatures.Add(creature.Stats.Id, creature);
        }

        public void Remove(Creature creature)
        {
            Creatures.Remove(creature.Stats.Id);
        }

        public Dictionary<Int32, Creature> GetCreatures()
        {
            return Creatures;
        }

        public Creature FindData(int id)
        {
            foreach (var c in Creatures)
                if (c.Key == id)
                    return c.Value;

            return null;
        }

        public void LoadCreatureData()
        {
            SQLResult result = DB.World.Select("SELECT * FROM creature_stats");

            for (int r = 0; r < result.Count; r++)
            {
                CreatureStats Stats = new CreatureStats();
                CreatureData Data = new CreatureData();

                Stats.Id = result.Read<Int32>(r, "Id");
                Stats.Name = result.Read<String>(r, "Name");
                Stats.SubName = result.Read<String>(r, "SubName");
                Stats.IconName = result.Read<String>(r, "IconName");

                for (int i = 0; i < Stats.Flag.Capacity; i++)
                    Stats.Flag.Add(result.Read<Int32>(r, "Flag", i));

                Stats.Type = result.Read<Int32>(r, "Type");
                Stats.Family = result.Read<Int32>(r, "Family");
                Stats.Rank = result.Read<Int32>(r, "Rank");

                for (int i = 0; i < Stats.QuestKillNpcId.Capacity; i++)
                    Stats.QuestKillNpcId.Add(result.Read<Int32>(r, "QuestKillNpcId", i));

                for (int i = 0; i < Stats.DisplayInfoId.Capacity; i++)
                    Stats.DisplayInfoId.Add(result.Read<Int32>(r, "DisplayInfoId", i));

                Stats.HealthModifier = result.Read<Single>(r, "HealthModifier");
                Stats.PowerModifier = result.Read<Single>(r, "PowerModifier");
                Stats.RacialLeader = result.Read<Byte>(r, "RacialLeader");

                for (int i = 0; i < Stats.QuestItemId.Capacity; i++)
                    Stats.QuestItemId.Add(result.Read<Int32>(r, "QuestItemId", i));

                Stats.MovementInfoId = result.Read<Int32>(r, "MovementInfoId");
                Stats.ExpansionRequired = result.Read<Int32>(r, "ExpansionRequired");

                SQLResult dataResult = DB.World.Select("SELECT * FROM creature_data WHERE id = ?", Stats.Id);

                if (dataResult.Count == 0)
                {
                    Log.Message(LogType.ERROR, "Creature data for Id {0} not found.", Stats.Id);
                    Log.Message(LogType.ERROR, "Insert default value to the database and continue loading...");

                    DB.World.Execute("INSERT INTO creature_data (Id) VALUES (?)", Stats.Id);
                }

                // Let's load it always. Default definitions are inserted if they are missing
                Data.Health = dataResult.Read<Int32>(0, "Health");
                Data.Level = dataResult.Read<Byte>(0, "Level");
                Data.Class = dataResult.Read<Byte>(0, "Class");
                Data.Faction = dataResult.Read<Int32>(0, "Faction");
                Data.Scale = dataResult.Read<Int32>(0, "Scale");
                Data.UnitFlags = dataResult.Read<Int32>(0, "UnitFlags");
                Data.UnitFlags2 = dataResult.Read<Int32>(0, "UnitFlags2");
                Data.NpcFlags = dataResult.Read<Int32>(0, "NpcFlags");

                Creature creature = new Creature()
                {
                    Stats = Stats,
                    Data = Data
                };

                Add(creature);
            }

            Log.Message(LogType.MISC, "Loaded {0} creatures.", Creatures.Count);
        }

        public void Initialize()
        {
            LoadCreatureData();
        }
    }
}
