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
using System;

namespace WorldServer.Game.WorldEntities
{
    public class Creature : ISpawnData
    {
        public UInt32 Id;
        public String Name;
        public String SubName;
        public String IconName;
        public UInt32 Health;
        public Byte Level;
        public UInt32 DisplayID;
        public Byte Class;
        public UInt32 Faction;
        public Single Scale;
        public UInt32 Type;
        public UInt32 Flags;
        public UInt32 Flags2;
        public UInt32 NpcFlags;

        public Creature() { }
        public Creature(uint id)
        {
            SQLResult result = DB.World.Select("SELECT * FROM creature_data WHERE id = ?", id);

            if (result.Count != 0)
            {
                Id        = result.Read<UInt32>(0, "Id");
                Name      = result.Read<String>(0, "Name");
                SubName   = result.Read<String>(0, "SubName");
                IconName  = result.Read<String>(0, "IconName");
                Health    = result.Read<UInt32>(0, "Health");
                Level     = result.Read<Byte>(0, "Level");
                DisplayID = result.Read<UInt32>(0, "DisplayID");
                Class     = result.Read<Byte>(0, "Class");
                Faction   = result.Read<UInt32>(0, "Faction");
                Scale     = result.Read<Single>(0, "Scale");
                Type      = result.Read<Byte>(0, "Type");
                Flags     = result.Read<UInt32>(0, "Flags");
                Flags2    = result.Read<UInt32>(0, "Flags2");
                NpcFlags  = result.Read<UInt32>(0, "NpcFlags");
            }
        }
    }
}
