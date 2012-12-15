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
using Framework.Network.Packets;
using System.Collections.Generic;
using WorldServer.Game.Managers;
using WorldServer.Game.WorldEntities;
using WorldServer.Network;

namespace WorldServer.Game.PacketHandler
{
    public class ObjectHandler : Globals
    {
        public static void HandleUpdateObject(ref WorldClass session)
        {
            WorldObject character = session.Character;
            PacketWriter updateObject = new PacketWriter(LegacyMessage.UpdateObject);

            updateObject.WriteUInt16((ushort)character.Map);
            updateObject.WriteUInt32(1);
            updateObject.WriteUInt8(1);
            updateObject.WriteGuid(character.Guid);
            updateObject.WriteUInt8(4);

            UpdateFlag updateFlags = UpdateFlag.Alive | UpdateFlag.Rotation | UpdateFlag.Self;
            WorldMgr.WriteUpdateObjectMovement(ref updateObject, ref character, updateFlags);

            character.WriteUpdateFields(ref updateObject);
            character.WriteDynamicUpdateFields(ref updateObject);
            session.Send(ref updateObject);

            var tempSessions = new Dictionary<ulong, WorldClass>(WorldMgr.Sessions);
            tempSessions.Remove(character.Guid);

            if (tempSessions != null)
            {
                foreach (var s in tempSessions)
                {
                    if (character.Map != s.Value.Character.Map)
                        continue;

                    updateObject = new PacketWriter(LegacyMessage.UpdateObject);

                    updateObject.WriteUInt16((ushort)character.Map);
                    updateObject.WriteUInt32(1);
                    updateObject.WriteUInt8(1);
                    updateObject.WriteGuid(character.Guid);
                    updateObject.WriteUInt8(4);

                    updateFlags = UpdateFlag.Alive | UpdateFlag.Rotation;
                    WorldMgr.WriteUpdateObjectMovement(ref updateObject, ref character, updateFlags);

                    character.WriteUpdateFields(ref updateObject);
                    character.WriteDynamicUpdateFields(ref updateObject);

                    s.Value.Send(ref updateObject);
                }

                foreach (var s in tempSessions)
                {
                    WorldObject pChar = s.Value.Character;

                    if (pChar.Map != character.Map)
                        continue;

                    updateObject = new PacketWriter(LegacyMessage.UpdateObject);

                    updateObject.WriteUInt16((ushort)pChar.Map);
                    updateObject.WriteUInt32(1);
                    updateObject.WriteUInt8(1);
                    updateObject.WriteGuid(pChar.Guid);
                    updateObject.WriteUInt8(4);

                    updateFlags = UpdateFlag.Alive | UpdateFlag.Rotation;
                    WorldMgr.WriteUpdateObjectMovement(ref updateObject, ref pChar, updateFlags);

                    pChar.WriteUpdateFields(ref updateObject);
                    pChar.WriteDynamicUpdateFields(ref updateObject);

                    session.Send(ref updateObject);
                }
            }

            character.AddCreatureSpawnsToWorld(ref session);
        }

        public static PacketWriter HandleObjectDestroy(ref WorldClass session, ulong guid)
        {
            PacketWriter objectDestroy = new PacketWriter(LegacyMessage.ObjectDestroy);

            objectDestroy.WriteUInt64(guid);
            objectDestroy.WriteUInt8(0);

            return objectDestroy;
        }
    }
}
