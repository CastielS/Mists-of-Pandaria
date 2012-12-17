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
        Dictionary<Int32, GameObject> GameObjects;

        DataManager()
        {
            Creatures = new Dictionary<Int32, Creature>();
            GameObjects = new Dictionary<Int32, GameObject>();

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

        public Creature FindCreature(int id)
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

                Stats.Id       = result.Read<Int32>(r, "Id");
                Stats.Name     = result.Read<String>(r, "Name");
                Stats.SubName  = result.Read<String>(r, "SubName");
                Stats.IconName = result.Read<String>(r, "IconName");

                for (int i = 0; i < Stats.Flag.Capacity; i++)
                    Stats.Flag.Add(result.Read<Int32>(r, "Flag", i));

                Stats.Type   = result.Read<Int32>(r, "Type");
                Stats.Family = result.Read<Int32>(r, "Family");
                Stats.Rank   = result.Read<Int32>(r, "Rank");

                for (int i = 0; i < Stats.QuestKillNpcId.Capacity; i++)
                    Stats.QuestKillNpcId.Add(result.Read<Int32>(r, "QuestKillNpcId", i));

                for (int i = 0; i < Stats.DisplayInfoId.Capacity; i++)
                    Stats.DisplayInfoId.Add(result.Read<Int32>(r, "DisplayInfoId", i));

                Stats.HealthModifier = result.Read<Single>(r, "HealthModifier");
                Stats.PowerModifier  = result.Read<Single>(r, "PowerModifier");
                Stats.RacialLeader   = result.Read<Byte>(r, "RacialLeader");

                for (int i = 0; i < Stats.QuestItemId.Capacity; i++)
                    Stats.QuestItemId.Add(result.Read<Int32>(r, "QuestItemId", i));

                Stats.MovementInfoId    = result.Read<Int32>(r, "MovementInfoId");
                Stats.ExpansionRequired = result.Read<Int32>(r, "ExpansionRequired");

                Creature creature = new Creature()
                {
                    Stats = Stats,
                };

                Add(creature);
            }

            SQLResult dataResult = DB.World.Select("SELECT Id FROM creature_stats WHERE Id NOT IN (SELECT Id FROM creature_data)");

            if (dataResult.Count != 0)
            {
                var missingIds = dataResult.ReadAllValuesFromField("Id");
                DB.World.ExecuteBigQuery("creature_data", "Id", 1, dataResult.Count, missingIds);

                Log.Message(LogType.DB, "Added {0} default data definition for creatures.", missingIds.Length);
            }

            dataResult = DB.World.Select("SELECT * FROM creature_data WHERE Id IN (SELECT Id FROM creature_stats)");

            for (int i = 0; i < Creatures.Count; i++)
            {
                int id = dataResult.Read<Int32>(i, "Id");

                Creatures[id].Data = new CreatureData()
                {
                    Health     = dataResult.Read<Int32>(i, "Health"),
                    Level      = dataResult.Read<Byte>(i, "Level"),
                    Class      = dataResult.Read<Byte>(i, "Class"),
                    Faction    = dataResult.Read<Int32>(i, "Faction"),
                    Scale      = dataResult.Read<Int32>(i, "Scale"),
                    UnitFlags  = dataResult.Read<Int32>(i, "UnitFlags"),
                    UnitFlags2 = dataResult.Read<Int32>(i, "UnitFlags2"),
                    NpcFlags   = dataResult.Read<Int32>(i, "NpcFlags")
                };
            }

            Log.Message(LogType.DB, "Loaded {0} creatures.", Creatures.Count);
        }

        public void Add(GameObject gameobject)
        {
            GameObjects.Add(gameobject.Stats.Id, gameobject);
        }

        public void Remove(GameObject gameobject)
        {
            GameObjects.Remove(gameobject.Stats.Id);
        }

        public Dictionary<Int32, GameObject> GetGameObjects()
        {
            return GameObjects;
        }

        public GameObject FindGameObject(int id)
        {
            foreach (var c in GameObjects)
                if (c.Key == id)
                    return c.Value;

            return null;
        }

        public void LoadGameObject()
        {
            SQLResult result = DB.World.Select("SELECT * FROM gameobject_stats");

            for (int r = 0; r < result.Count; r++)
            {
                GameObjectStats Stats = new GameObjectStats();

                Stats.Id = result.Read<Int32>(r, "Id");
                Stats.Type = result.Read<Int32>(r, "Type");
                Stats.DisplayInfoId = result.Read<Int32>(r, "DisplayInfoId");
                Stats.Name = result.Read<String>(r, "Name");
                Stats.IconName = result.Read<String>(r, "IconName");
                Stats.CastBarCaption = result.Read<String>(r, "CastBarCaption");

                for (int i = 0; i < Stats.Data.Capacity; i++)
                    Stats.Data.Add(result.Read<Int32>(r, "Data", i));

                Stats.Size = result.Read<Single>(r, "Size");

                for (int i = 0; i < Stats.QuestItemId.Capacity; i++)
                    Stats.QuestItemId.Add(result.Read<Int32>(r, "QuestItemId", i));

                Stats.ExpansionRequired = result.Read<Int32>(r, "ExpansionRequired");

                GameObject gameobject = new GameObject()
                {
                    Stats = Stats,
                };

                Add(gameobject);
            }

            Log.Message(LogType.DB, "Loaded {0} gameobjects.", GameObjects.Count);
        }

        public void Initialize()
        {
            LoadCreatureData();
            LoadGameObject();
        }
    }
}
