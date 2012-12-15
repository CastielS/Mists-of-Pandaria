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
    public class GameObjectStats
    {
        public Int32 Id;
        public Int32 Type;
        public Int32 DisplayInfoId;
        public String Name;
        public String IconName;
        public String CastBarCaption;
        public List<Int32> Data = new List<Int32>(32);
        public Single Size;
        public List<Int32> QuestItemId = new List<Int32>(6);
        public Int32 ExpansionRequired;
    }
}
