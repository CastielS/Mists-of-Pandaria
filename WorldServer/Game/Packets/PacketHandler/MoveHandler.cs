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
using WorldServer.Network;
using System.Windows;
using Framework.ObjectDefines;
using WorldServer.Game.PacketHandler;

namespace WorldServer.Game.Packets.PacketHandler
{
    public class MoveHandler
    {
        public static void HandleMoveSetWalkSpeed(ref WorldClass session, float speed = 2.5f)
        {
            PacketWriter setWalkSpeed = new PacketWriter(JAMCMessage.MoveSetWalkSpeed);
            BitPack BitPack = new BitPack(setWalkSpeed, session.Character.Guid);

            setWalkSpeed.WriteUInt32(0);
            setWalkSpeed.WriteFloat(speed);

            BitPack.WriteGuidMask(6, 2, 1, 4, 5, 3, 7, 0);
            BitPack.Flush();

            BitPack.WriteGuidBytes(1, 6, 3, 0, 7, 4, 2, 5);

            session.Send(setWalkSpeed);
        }

        public static void HandleMoveSetRunSpeed(ref WorldClass session, float speed = 7f)
        {
            PacketWriter setRunSpeed = new PacketWriter(JAMCMessage.MoveSetRunSpeed);
            BitPack BitPack = new BitPack(setRunSpeed, session.Character.Guid);

            BitPack.WriteGuidMask(0, 4, 1, 6, 3, 5, 7, 2);
            BitPack.Flush();

            setRunSpeed.WriteFloat(speed);
            BitPack.WriteGuidBytes(7);
            setRunSpeed.WriteUInt32(0);
            BitPack.WriteGuidBytes(3, 6, 0, 4, 1, 5, 2);

            session.Send(setRunSpeed);
        }

        public static void HandleMoveSetSwimSpeed(ref WorldClass session, float speed = 4.72222f)
        {
            PacketWriter setSwimSpeed = new PacketWriter(JAMCMessage.MoveSetSwimSpeed);
            BitPack BitPack = new BitPack(setSwimSpeed, session.Character.Guid);

            BitPack.WriteGuidMask(4, 0, 7, 5, 6, 1, 2, 3);
            BitPack.Flush();

            setSwimSpeed.WriteUInt32(0);

            BitPack.WriteGuidBytes(3, 7, 0, 1, 4, 5, 2);
            setSwimSpeed.WriteFloat(speed);
            BitPack.WriteGuidBytes(6);

            session.Send(setSwimSpeed);
        }

        public static void HandleMoveSetFlightSpeed(ref WorldClass session, float speed = 7f)
        {
            PacketWriter setFlightSpeed = new PacketWriter(JAMCMessage.MoveSetFlightSpeed);
            BitPack BitPack = new BitPack(setFlightSpeed, session.Character.Guid);

            BitPack.WriteGuidMask(6, 1, 7, 4, 5, 3, 0, 2);
            BitPack.Flush();

            BitPack.WriteGuidBytes(0, 4, 6);
            setFlightSpeed.WriteFloat(speed);
            BitPack.WriteGuidBytes(7, 2);
            setFlightSpeed.WriteUInt32(0);
            BitPack.WriteGuidBytes(5, 1, 3);

            session.Send(setFlightSpeed);
        }

        public static void HandleMoveSetCanFly(ref WorldClass session)
        {
            PacketWriter setCanFly = new PacketWriter(JAMCMessage.MoveSetCanFly);
            BitPack BitPack = new BitPack(setCanFly, session.Character.Guid);

            setCanFly.WriteUInt32(0);

            BitPack.WriteGuidMask(4, 5, 0, 6, 2, 1, 3, 7);
            BitPack.Flush();

            BitPack.WriteGuidBytes(5, 3, 1, 6, 7, 2, 4, 0);

            session.Send(setCanFly);
        }

        public static void HandleMoveUnsetCanFly(ref WorldClass session)
        {
            PacketWriter unsetCanFly = new PacketWriter(JAMCMessage.MoveUnsetCanFly);
            BitPack BitPack = new BitPack(unsetCanFly, session.Character.Guid);

            unsetCanFly.WriteUInt32(0);

            BitPack.WriteGuidMask(1, 4, 6, 2, 5, 7, 0, 3);
            BitPack.Flush();

            BitPack.WriteGuidBytes(3, 1, 5, 7, 0, 6, 2, 4);

            session.Send(unsetCanFly);
        }

        public static void HandleMoveTeleport(ref WorldClass session, Vector4 vector)
        {
            bool IsTransport = false;
            bool Unknown = false;

            PacketWriter moveTeleport = new PacketWriter(JAMCMessage.MoveTeleport);
            BitPack BitPack = new BitPack(moveTeleport, session.Character.Guid);

            moveTeleport.WriteUInt32(0);
            moveTeleport.WriteFloat(vector.X);
            moveTeleport.WriteFloat(vector.Y);
            moveTeleport.WriteFloat(vector.Z);
            moveTeleport.WriteFloat(vector.W);

            BitPack.WriteGuidMask(3, 1, 7);
            BitPack.Write(Unknown);
            BitPack.WriteGuidMask(6);

            // Unknown bits
            if (Unknown)
            {
                BitPack.Write(false);
                BitPack.Write(false);
            }

            BitPack.WriteGuidMask(0, 4);

            BitPack.Write(IsTransport);
            BitPack.WriteGuidMask(2);

            // Transport guid
            if (IsTransport)
                BitPack.WriteTransportGuidMask(7, 5, 2, 1, 0, 4, 3, 6);

            BitPack.WriteGuidMask(5);

            BitPack.Flush();

            if (IsTransport)
                BitPack.WriteTransportGuidBytes(1, 5, 7, 0, 3, 4, 6, 2);

            BitPack.WriteGuidBytes(3);

            if (Unknown)
                moveTeleport.WriteUInt8(0);

            BitPack.WriteGuidBytes(2, 1, 7, 5, 6, 4, 0);

            session.Send(moveTeleport);
        }

        public static void HandleTransferPending(ref WorldClass session, uint mapId)
        {
            bool Unknown = false;
            bool IsTransport = false;

            PacketWriter transferPending = new PacketWriter(JAMCMessage.TransferPending);
            BitPack BitPack = new BitPack(transferPending);

            BitPack.Write(IsTransport);
            BitPack.Write(Unknown);

            if (Unknown)
                transferPending.WriteUInt32(0);

            transferPending.WriteUInt32(mapId);

            if (IsTransport)
            {
                transferPending.WriteUInt32(0);
                transferPending.WriteUInt32(0);
            }
            
            session.Send(transferPending);
        }

        public static void HandleNewWorld(ref WorldClass session, Vector4 vector, uint mapId)
        {
            PacketWriter newWorld = new PacketWriter(JAMCMessage.NewWorld);

            newWorld.WriteUInt32(mapId);
            newWorld.WriteFloat(vector.Y);
            newWorld.WriteFloat(vector.W);
            newWorld.WriteFloat(vector.X);
            newWorld.WriteFloat(vector.Z);

            session.Send(newWorld);
        }
    }
}
