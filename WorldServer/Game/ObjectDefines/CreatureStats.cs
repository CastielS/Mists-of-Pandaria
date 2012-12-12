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
using System.Collections.Generic;

namespace WorldServer.Game.ObjectDefines
{
    public class CreatureStats
    {
        public Int32 Id;
        public String Name;
        public String SubName;
        public String IconName;
        public List<Int32> Flag = new List<Int32>(2);
        public Int32 Type;
        public Int32 Family;
        public Int32 Rank;
        public List<Int32> QuestKillNpcId = new List<Int32>(2);
        public List<Int32> DisplayInfoId = new List<Int32>(4);
        public Single HealthModifier;
        public Single PowerModifier;
        public Byte RacialLeader;
        public List<Int32> QuestItemId = new List<Int32>(6);
        public Int32 MovementInfoId;
        public Int32 ExpansionRequired;
    }
}
