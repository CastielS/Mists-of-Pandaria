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
using Framework.ObjectDefines;
using Framework.Singleton;
using System;
using System.Collections.Generic;
using WorldServer.Game.Spawns;

namespace WorldServer.Game.Managers
{
    public sealed class SpawnManager : SingletonBase<SpawnManager>
    {
        public Dictionary<ISpawn, ISpawnData> Spawns;

        SpawnManager()
        {
            Spawns = new Dictionary<ISpawn, ISpawnData>();

            Initialize();
        }

        public void AddSpawn(ISpawn spawn, ref ISpawnData data)
        {
            Spawns.Add(spawn, data);
        }

        public void LoadCreatureSpawns()
        {
            SQLResult result = DB.World.Select("SELECT * FROM creature_spawns");

            for (int i = 0; i < result.Count; i++)
            {
                UInt32 id = result.Read<UInt32>(i, "Id");
                CreatureSpawn spawn = new CreatureSpawn()
                {
                    Guid = result.Read<UInt64>(i, "Guid"),
                    Id   = result.Read<UInt32>(i, "Id"),

                    Position = new Vector4()
                    {
                        X = result.Read<Single>(i, "X"),
                        Y = result.Read<Single>(i, "Y"),
                        Z = result.Read<Single>(i, "Z"),
                        W = result.Read<Single>(i, "O")
                    },

                    Map = result.Read<UInt32>(i, "Map")
                };

                spawn.CreateFullGuid();
                spawn.CreateData();

                ISpawnData data = Globals.DataMgr.FindData(id);

                AddSpawn(spawn, ref data);
            }
        }

        public void Initialize()
        {
            LoadCreatureSpawns();
        }
    }
}
