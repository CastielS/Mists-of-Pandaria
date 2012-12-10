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
using Framework.Singleton;
using System;
using System.Collections.Generic;
using WorldServer.Game.WorldEntities;

namespace WorldServer.Game.Managers
{
    public class DataManager : SingletonBase<DataManager>
    {
        List<Creature> CreatureDataList;

        DataManager()
        {
            CreatureDataList = new List<Creature>();

            Initialize();
        }

        public void Add(Creature creature)
        {
            CreatureDataList.Add(creature);
        }

        public void Remove(Creature creature)
        {
            CreatureDataList.Remove(creature);
        }

        public List<Creature> GetCreatures()
        {
            return CreatureDataList;
        }

        public Creature FindData(uint id)
        {
            foreach (Creature c in CreatureDataList)
                if (c.Id == id)
                    return c;

            return null;
        }

        public void LoadCreatureData()
        {
            SQLResult result = DB.World.Select("SELECT * FROM creature_data");

            for (int i = 0; i < result.Count; i++)
            {
                Add(new Creature()
                {
                    Id        = result.Read<UInt32>(i, "Id"),
                    Name      = result.Read<String>(i, "Name"),
                    SubName   = result.Read<String>(i, "SubName"),
                    IconName  = result.Read<String>(i, "IconName"),
                    Health    = result.Read<UInt32>(i, "Health"),
                    Level     = result.Read<Byte>(i, "Level"),
                    DisplayID = result.Read<UInt32>(i, "DisplayID"),
                    Class     = result.Read<Byte>(i, "Class"),
                    Faction   = result.Read<UInt32>(i, "Faction"),
                    Scale     = result.Read<Single>(i, "Scale"),
                    Type      = result.Read<Byte>(i, "Type"),
                    Flags     = result.Read<UInt32>(i, "Flags"),
                    Flags2    = result.Read<UInt32>(i, "Flags2"),
                    NpcFlags  = result.Read<UInt32>(i, "NpcFlags")
                });
            }
        }

        public void Initialize()
        {
            LoadCreatureData();
        }
    }
}
