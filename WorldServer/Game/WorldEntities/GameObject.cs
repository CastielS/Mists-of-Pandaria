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
    public class GameObject
    {
        public GameObjectStats Stats;

        public GameObject() { }
        public GameObject(int id)
        {
            SQLResult result = DB.World.Select("SELECT * FROM gameobject_stats WHERE id = ?", id);

            if (result.Count != 0)
            {
                Stats = new GameObjectStats();

                Stats.Id             = result.Read<Int32>(0, "Id");
                Stats.Type           = result.Read<Int32>(0, "Type");
                Stats.DisplayInfoId  = result.Read<Int32>(0, "DisplayInfoId");
                Stats.Name           = result.Read<String>(0, "Name");
                Stats.IconName       = result.Read<String>(0, "IconName");
                Stats.CastBarCaption = result.Read<String>(0, "CastBarCaption");

                for (int i = 0; i < Stats.Data.Capacity; i++)
                    Stats.Data.Add(result.Read<Int32>(0, "Data", i));

                Stats.Size = result.Read<Single>(0, "Size");

                for (int i = 0; i < Stats.QuestItemId.Capacity; i++)
                    Stats.QuestItemId.Add(result.Read<Int32>(0, "QuestItemId", i));

                Stats.ExpansionRequired = result.Read<Int32>(0, "ExpansionRequired");
            }
        }
    }
}
