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

using Framework.Constants;
using System;

namespace Framework.ObjectDefines
{
    public class ObjectGuid
    {
        public UInt64 Guid { get; set; }

        public ObjectGuid(ulong low, int id, HighGuidType highType)
        {
            Guid = (ulong)(low | ((ulong)id << 32) | (ulong)highType << 48);
        }

        public static HighGuidType GetGuidType(ulong guid)
        {
            return (HighGuidType)(guid >> 48);
        }

        public static int GetId(ulong guid)
        {
            return (int)((guid >> 32) & 0xFFFF);
        }

        public static ulong GetGuid(ulong guid)
        {
            return guid & 0xFFFFFFFF;
        }
    }
}
