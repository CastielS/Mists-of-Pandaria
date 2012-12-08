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
using System.Collections;
using System.Reflection;

namespace Framework.Singleton
{
    public static class Singleton
    {
        static Hashtable ObjectList = new Hashtable();
        static Object Sync = new Object();

        public static T GetInstance<T>() where T : class
        {
            var typeName = typeof(T).FullName;

            lock (Sync)
            {
                if (ObjectList.ContainsKey(typeName))
                    return (T)ObjectList[typeName];
            }

            ConstructorInfo constructorInfo = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
            T instance = (T)constructorInfo.Invoke(new object[] { });

            ObjectList.Add(instance.ToString(), instance);

            return (T)ObjectList[typeName];
        }
    }
}
