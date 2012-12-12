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

using System;

namespace WorldServer.Game.ObjectDefines
{
    public class CreatureData
    {
        public Int32 Health;
        public Byte Level;
        public Byte Class;
        public Int32 Faction;
        public Single Scale;
        public Int32 UnitFlags;
        public Int32 UnitFlags2;
        public Int32 NpcFlags;
    }
}
