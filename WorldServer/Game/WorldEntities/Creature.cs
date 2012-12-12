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
using System;
using WorldServer.Game.ObjectDefines;

namespace WorldServer.Game.WorldEntities
{
    public class Creature
    {
        public CreatureStats Stats;
        public CreatureData Data;

        public Creature() { }
        public Creature(int id)
        {
            SQLResult result = DB.World.Select("SELECT * FROM creature_stats WHERE id = ?", id);

            if (result.Count != 0)
            {
                Stats = new CreatureStats();

                Stats.Id       = result.Read<Int32>(0, "Id");
                Stats.Name     = result.Read<String>(0, "Name");
                Stats.SubName  = result.Read<String>(0, "SubName");
                Stats.IconName = result.Read<String>(0, "IconName");

                for (int i = 0; i < Stats.Flag.Capacity; i++)
                    Stats.Flag.Add(result.Read<Int32>(0, "Flag", i));

                Stats.Type   = result.Read<Int32>(0, "Type");
                Stats.Family = result.Read<Int32>(0, "Family");
                Stats.Rank   = result.Read<Int32>(0, "Rank");

                for (int i = 0; i < Stats.QuestKillNpcId.Capacity; i++)
                    Stats.QuestKillNpcId.Add(result.Read<Int32>(0, "QuestKillNpcId", i));

                for (int i = 0; i < Stats.DisplayInfoId.Capacity; i++)
                    Stats.DisplayInfoId.Add(result.Read<Int32>(0, "DisplayInfoId", i));

                Stats.HealthModifier = result.Read<Single>(0, "HealthModifier");
                Stats.PowerModifier  = result.Read<Single>(0, "PowerModifier");
                Stats.RacialLeader   = result.Read<Byte>(0, "RacialLeader");

                for (int i = 0; i < Stats.QuestItemId.Capacity; i++)
                    Stats.QuestItemId.Add(result.Read<Int32>(0, "QuestItemId", i));

                Stats.MovementInfoId    = result.Read<Int32>(0, "MovementInfoId");
                Stats.ExpansionRequired = result.Read<Int32>(0, "ExpansionRequired");
            }

            result = DB.World.Select("SELECT * FROM creature_data WHERE id = ?", id);

            if (result.Count != 0)
            {
                Data = new CreatureData();

                Data.Health     = result.Read<Int32>(0, "Health");
                Data.Level      = result.Read<Byte>(0, "Level");
                Data.Class      = result.Read<Byte>(0, "Class");
                Data.Faction    = result.Read<Int32>(0, "Faction");
                Data.Scale      = result.Read<Int32>(0, "Scale");
                Data.UnitFlags  = result.Read<Int32>(0, "UnitFlags");
                Data.UnitFlags2 = result.Read<Int32>(0, "UnitFlags2");
                Data.NpcFlags   = result.Read<Int32>(0, "NpcFlags");
            }
        }
    }
}
