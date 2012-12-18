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
using Framework.ObjectDefines;
using Framework.Singleton;
using System;
using System.Collections.Generic;
using WorldServer.Game.Spawns;
using WorldServer.Game.WorldEntities;

namespace WorldServer.Game.Managers
{
    public sealed class SpawnManager : SingletonBase<SpawnManager>
    {
        public Dictionary<CreatureSpawn, Creature> CreatureSpawns;
        public Dictionary<GameObjectSpawn, GameObject> GameObjectSpawns;

        SpawnManager()
        {
            CreatureSpawns = new Dictionary<CreatureSpawn, Creature>();
            GameObjectSpawns = new Dictionary<GameObjectSpawn, GameObject>();

            Initialize();
        }

        public void AddSpawn(CreatureSpawn spawn, ref Creature data)
        {
            CreatureSpawns.Add(spawn, data);
        }

        public void RemoveSpawn(CreatureSpawn spawn)
        {
            CreatureSpawns.Remove(spawn);
            DB.World.Execute("DELETE FROM creature_spawns WHERE Guid = ?", ObjectGuid.GetGuid(spawn.Guid));
        }

        public CreatureSpawn FindSpawn(ulong guid)
        {
            foreach (var c in CreatureSpawns)
                if (c.Key.Guid == guid)
                    return c.Key;

            return null;
        }

        public uint FindCreatureCountByMap(uint map)
        {
            uint count = 0;
            foreach (var c in CreatureSpawns)
                if (c.Key.Map == map)
                    ++count;

            return count;
        }

        public uint FindGameObjectCountByMap(uint map)
        {
            uint count = 0;
            foreach (var c in GameObjectSpawns)
                if (c.Key.Map == map)
                    ++count;

            return count;
        }

        public void LoadCreatureSpawns()
        {
            SQLResult result = DB.World.Select("SELECT * FROM creature_spawns");

            for (int i = 0; i < result.Count; i++)
            {
                CreatureSpawn spawn = new CreatureSpawn()
                {
                    Guid = result.Read<UInt64>(i, "Guid"),
                    Id    = result.Read<Int32>(i, "Id"),

                    Position = new Vector4()
                    {
                        X = result.Read<Single>(i, "X"),
                        Y = result.Read<Single>(i, "Y"),
                        Z = result.Read<Single>(i, "Z"),
                        O = result.Read<Single>(i, "O")
                    },

                    Map = result.Read<UInt32>(i, "Map")
                };

                Creature data = Globals.DataMgr.FindCreature(spawn.Id);

                spawn.CreateFullGuid();
                spawn.CreateData(data);

                AddSpawn(spawn, ref data);
            }

            Log.Message(LogType.DB, "Loaded {0} creature spawns.", CreatureSpawns.Count);
        }

        public void AddSpawn(GameObjectSpawn spawn, ref GameObject data)
        {
            GameObjectSpawns.Add(spawn, data);
        }

        public void RemoveSpawn(GameObjectSpawn spawn)
        {
            GameObjectSpawns.Remove(spawn);
            DB.World.Execute("DELETE FROM creature_spawns WHERE Guid = ?", ObjectGuid.GetGuid(spawn.Guid));
        }

        public GameObjectSpawn FindSpawn(GameObjectSpawn spawn)
        {
            foreach (var c in GameObjectSpawns)
                if (c.Key.Guid == spawn.Guid)
                    return c.Key;

            return null;
        }

        public void LoadGameObjectSpawns()
        {
            SQLResult result = DB.World.Select("SELECT * FROM gameobject_spawns");

            for (int i = 0; i < result.Count; i++)
            {
                GameObjectSpawn spawn = new GameObjectSpawn()
                {
                    Guid = result.Read<UInt64>(i, "Guid"),
                    Id = result.Read<Int32>(i, "Id"),

                    Position = new Vector4()
                    {
                        X = result.Read<Single>(i, "X"),
                        Y = result.Read<Single>(i, "Y"),
                        Z = result.Read<Single>(i, "Z"),
                        O = result.Read<Single>(i, "O")
                    },

                    Map = result.Read<UInt32>(i, "Map")
                };

                GameObject data = Globals.DataMgr.FindGameObject(spawn.Id);

                spawn.CreateFullGuid();
                spawn.CreateData(data);

                AddSpawn(spawn, ref data);
            }

            Log.Message(LogType.DB, "Loaded {0} gameobject spawns.", GameObjectSpawns.Count);
        }

        public void Initialize()
        {
            LoadCreatureSpawns();
            LoadGameObjectSpawns();
        }
    }
}
