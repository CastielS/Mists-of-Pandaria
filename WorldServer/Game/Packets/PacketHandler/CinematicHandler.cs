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
using Framework.Database;
using Framework.DBC;
using Framework.Network.Packets;
using WorldServer.Game.WorldEntities;
using WorldServer.Network;

namespace WorldServer.Game.Packets.PacketHandler
{
    public class CinematicHandler
    {
        public static void HandleStartCinematic(ref WorldClass session)
        {
            Character pChar = session.Character;

            PacketWriter startCinematic = new PacketWriter(LegacyMessage.StartCinematic);

            startCinematic.WriteUInt32(DBCStorage.RaceStorage[pChar.Race].CinematicSequence);

            session.Send(startCinematic);

            if (pChar.LoginCinematic)
                DB.Characters.Execute("UPDATE characters SET loginCinematic = 0 WHERE guid = {0}", pChar.Guid);
        }
    }
}
