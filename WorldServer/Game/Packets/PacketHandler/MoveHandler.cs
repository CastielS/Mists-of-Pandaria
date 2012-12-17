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
using WorldServer.Game.Managers;
using System;
using WorldServer.Game.WorldEntities;

namespace WorldServer.Game.Packets.PacketHandler
{
    public class MoveHandler : Globals
    {
        [Opcode(ClientMessage.MoveStartForward, "16357")]
        public static void HandleMoveStartForward(ref PacketReader packet, ref WorldClass session)
        {
            ObjectMovementValues movementValues = new ObjectMovementValues();
            BitUnpack BitUnpack = new BitUnpack(packet);

            bool[] guidMask = new bool[8];
            byte[] guidBytes = new byte[8];

            Vector4 vector = new Vector4()
            {
                X = packet.ReadFloat(),
                Y = packet.ReadFloat(),
                Z = packet.ReadFloat()
            };

            guidMask[5] = BitUnpack.GetBit();
            guidMask[4] = BitUnpack.GetBit();

            bool HasSplineElevation = !BitUnpack.GetBit();

            guidMask[3] = BitUnpack.GetBit();

            movementValues.IsTransport = BitUnpack.GetBit();
            movementValues.HasRotation = !BitUnpack.GetBit();

            bool Unknown = BitUnpack.GetBit();

            guidMask[6] = BitUnpack.GetBit();

            movementValues.HasMovementFlags2 = !BitUnpack.GetBit();

            guidMask[0] = BitUnpack.GetBit();
            guidMask[2] = BitUnpack.GetBit();

            bool Unknown2 = BitUnpack.GetBit();
            bool Unknown3 = BitUnpack.GetBit();

            guidMask[7] = BitUnpack.GetBit();

            movementValues.IsAlive = !BitUnpack.GetBit();

            uint counter = BitUnpack.GetBits<uint>(24);

            guidMask[1] = BitUnpack.GetBit();

            bool HasTime = !BitUnpack.GetBit();
            bool HasPitch = !BitUnpack.GetBit();

            movementValues.HasMovementFlags = !BitUnpack.GetBit();
            bool Unknown4 = BitUnpack.GetBit();


            if (movementValues.HasMovementFlags2)
                movementValues.MovementFlags2 = (MovementFlag2)BitUnpack.GetBits<uint>(13);

            /*if (movementValues.IsTransport)
            {

            }
            
            if (IsInterpolated)
            {

            }*/

            if (movementValues.HasMovementFlags)
                movementValues.MovementFlags = (MovementFlag)BitUnpack.GetBits<uint>(30);

            if (guidMask[1]) guidBytes[1] = (byte)(packet.ReadUInt8() ^ 1);

            for (int i = 0; i < counter; i++)
                packet.ReadUInt32();

            if (guidMask[0]) guidBytes[0] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[4]) guidBytes[4] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[2]) guidBytes[2] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[5]) guidBytes[5] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[3]) guidBytes[3] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[7]) guidBytes[7] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[6]) guidBytes[6] = (byte)(packet.ReadUInt8() ^ 1);

            /*if (movementValues.IsTransport)
            {

            }
            
            if (IsInterpolated)
            {

            }*/

            if (movementValues.IsAlive)
                movementValues.Time = packet.ReadUInt32();

            if (movementValues.HasRotation)
                vector.O = packet.ReadFloat();

            if (HasSplineElevation)
                packet.ReadFloat();

            if (HasTime)
                movementValues.Time = packet.ReadUInt32();

            if (HasPitch)
                packet.ReadFloat();

            var guid = BitConverter.ToUInt64(guidBytes, 0);
            HandleMoveUpdate(guid, movementValues, vector);
        }

        [Opcode(ClientMessage.MoveStartBackward, "16357")]
        public static void HandleMoveStartBackward(ref PacketReader packet, ref WorldClass session)
        {
            ObjectMovementValues movementValues = new ObjectMovementValues();
            BitUnpack BitUnpack = new BitUnpack(packet);

            bool[] guidMask = new bool[8];
            byte[] guidBytes = new byte[8];

            Vector4 vector = new Vector4()
            {
                X = packet.ReadFloat(),
                Z = packet.ReadFloat(),
                Y = packet.ReadFloat()
            };

            guidMask[3] = BitUnpack.GetBit();
            guidMask[6] = BitUnpack.GetBit();

            movementValues.HasMovementFlags = !BitUnpack.GetBit();

            movementValues.IsInterpolated = BitUnpack.GetBit();
            bool HasSplineElevation = !BitUnpack.GetBit();

            movementValues.HasRotation = !BitUnpack.GetBit();

            guidMask[4] = BitUnpack.GetBit();

            movementValues.IsAlive = !BitUnpack.GetBit();

            guidMask[1] = BitUnpack.GetBit();

            movementValues.IsTransport = BitUnpack.GetBit();

            bool Unknown2 = BitUnpack.GetBit();

            guidMask[0] = BitUnpack.GetBit();

            bool Unknown = BitUnpack.GetBit();

            movementValues.HasMovementFlags2 = !BitUnpack.GetBit();

            guidMask[2] = BitUnpack.GetBit();

            bool HasPitch = !BitUnpack.GetBit();
            bool Unknown3 = BitUnpack.GetBit();

            guidMask[5] = BitUnpack.GetBit();
            guidMask[7] = BitUnpack.GetBit();

            bool HasTime = !BitUnpack.GetBit();

            uint counter = BitUnpack.GetBits<uint>(24);

            /*if (movementValues.IsTransport)
            {

            }*/

            if (movementValues.IsInterpolated)
                movementValues.IsInterpolated2 = BitUnpack.GetBit();

            if (movementValues.HasMovementFlags2)
                movementValues.MovementFlags2 = (MovementFlag2)BitUnpack.GetBits<uint>(13);
            
            if (movementValues.HasMovementFlags)
                movementValues.MovementFlags = (MovementFlag)BitUnpack.GetBits<uint>(30);

            if (guidMask[6]) guidBytes[6] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[4]) guidBytes[4] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[0]) guidBytes[0] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[1]) guidBytes[1] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[5]) guidBytes[5] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[2]) guidBytes[2] = (byte)(packet.ReadUInt8() ^ 1);

            for (int i = 0; i < counter; i++)
                packet.ReadUInt32();

            if (guidMask[7]) guidBytes[7] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[3]) guidBytes[3] = (byte)(packet.ReadUInt8() ^ 1);

            /*if (movementValues.IsTransport)
            {

            }*/

            if (HasSplineElevation)
                packet.ReadFloat();

            if (movementValues.IsInterpolated)
            {
                if (movementValues.IsInterpolated2)
                {
                    packet.ReadFloat();
                    packet.ReadFloat();
                    packet.ReadFloat();
                }

                packet.ReadUInt32();
                packet.ReadFloat();
            }

            if (HasTime)
                movementValues.Time = packet.ReadUInt32();

            if (HasPitch)
                packet.ReadFloat();

            if (movementValues.HasRotation)
                vector.O = packet.ReadFloat();

            if (movementValues.IsAlive)
                movementValues.Time = packet.ReadUInt32();

            var guid = BitConverter.ToUInt64(guidBytes, 0);
            HandleMoveUpdate(guid, movementValues, vector);
        }

        [Opcode(ClientMessage.MoveHeartBeat, "16357")]
        public static void HandleMoveHeartBeat(ref PacketReader packet, ref WorldClass session)
        {
            ObjectMovementValues movementValues = new ObjectMovementValues();
            BitUnpack BitUnpack = new BitUnpack(packet);

            bool[] guidMask = new bool[8];
            byte[] guidBytes = new byte[8];

            Vector4 vector = new Vector4()
            {
                X = packet.ReadFloat(),
                Y = packet.ReadFloat(),
                Z = packet.ReadFloat()
            };

            movementValues.HasMovementFlags = !BitUnpack.GetBit();
            movementValues.IsInterpolated = BitUnpack.GetBit();

            uint counter = BitUnpack.GetBits<uint>(24);

            movementValues.IsAlive = !BitUnpack.GetBit();
            movementValues.HasMovementFlags2 = !BitUnpack.GetBit();

            bool HasPitch = !BitUnpack.GetBit();

            guidMask[4] = BitUnpack.GetBit();

            movementValues.IsTransport = BitUnpack.GetBit();

            guidMask[7] = BitUnpack.GetBit();
            guidMask[0] = BitUnpack.GetBit();

            bool Unknown2 = BitUnpack.GetBit();

            guidMask[3] = BitUnpack.GetBit();

            bool HasSplineElevation = !BitUnpack.GetBit();

            guidMask[1] = BitUnpack.GetBit();

            bool Unknown3 = BitUnpack.GetBit();

            guidMask[5] = BitUnpack.GetBit();
            guidMask[2] = BitUnpack.GetBit();

            movementValues.HasRotation = !BitUnpack.GetBit();
            bool Unknown4 = BitUnpack.GetBit();

            guidMask[6] = BitUnpack.GetBit();

            bool HasTime = !BitUnpack.GetBit();

            /*if (movementValues.IsTransport)
            {

            }*/

            if (movementValues.IsInterpolated)
                movementValues.IsInterpolated2 = BitUnpack.GetBit();

            if (movementValues.HasMovementFlags)
                movementValues.MovementFlags = (MovementFlag)BitUnpack.GetBits<uint>(30);

            if (movementValues.HasMovementFlags2)
                movementValues.MovementFlags2 = (MovementFlag2)BitUnpack.GetBits<uint>(13);

            if (guidMask[7]) guidBytes[7] = (byte)(packet.ReadUInt8() ^ 1);

            for (int i = 0; i < counter; i++)
                packet.ReadUInt32();

            if (guidMask[1]) guidBytes[1] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[3]) guidBytes[3] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[0]) guidBytes[0] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[5]) guidBytes[5] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[4]) guidBytes[4] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[6]) guidBytes[6] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[2]) guidBytes[2] = (byte)(packet.ReadUInt8() ^ 1);

            if (movementValues.IsInterpolated)
            {
                if (movementValues.IsInterpolated2)
                {
                    packet.ReadFloat();
                    packet.ReadFloat();
                    packet.ReadFloat();
                }

                packet.ReadFloat();
                packet.ReadUInt32();
            }


            if (HasTime)
                movementValues.Time = packet.ReadUInt32();

            /*if (movementValues.IsTransport)
            {

            }*/

            if (movementValues.HasRotation)
                vector.O = packet.ReadFloat();

            if (HasPitch)
                packet.ReadFloat();

            if (movementValues.IsAlive)
                movementValues.Time = packet.ReadUInt32();

            if (HasSplineElevation)
                packet.ReadFloat();

            var guid = BitConverter.ToUInt64(guidBytes, 0);
            HandleMoveUpdate(guid, movementValues, vector);
        }

        [Opcode(ClientMessage.MoveStop, "16357")]
        public static void HandleMoveStop(ref PacketReader packet, ref WorldClass session)
        {
            ObjectMovementValues movementValues = new ObjectMovementValues();
            BitUnpack BitUnpack = new BitUnpack(packet);

            bool[] guidMask = new bool[8];
            byte[] guidBytes = new byte[8];

            Vector4 vector = new Vector4()
            {
                X = packet.ReadFloat(),
                Z = packet.ReadFloat(),
                Y = packet.ReadFloat()
            };

            movementValues.HasMovementFlags = !BitUnpack.GetBit();
            movementValues.IsTransport = BitUnpack.GetBit();

            bool HasPitch = !BitUnpack.GetBit();

            movementValues.HasRotation = !BitUnpack.GetBit();

            bool Unknown = BitUnpack.GetBit();
            bool HasSplineElevation = !BitUnpack.GetBit();

            uint counter = BitUnpack.GetBits<uint>(24);

            bool HasTime = !BitUnpack.GetBit();

            guidMask[4] = BitUnpack.GetBit();

            bool Unknown2 = BitUnpack.GetBit();

            guidMask[6] = BitUnpack.GetBit();
            guidMask[0] = BitUnpack.GetBit();
            guidMask[5] = BitUnpack.GetBit();
            guidMask[1] = BitUnpack.GetBit();

            movementValues.IsAlive = !BitUnpack.GetBit();

            guidMask[7] = BitUnpack.GetBit();
            guidMask[2] = BitUnpack.GetBit();

            bool Unknown3 = BitUnpack.GetBit();

            guidMask[3] = BitUnpack.GetBit();

            movementValues.HasMovementFlags2 = !BitUnpack.GetBit();
            bool Unknown4 = BitUnpack.GetBit();

            /*if (IsInterpolated)
            {

            }
            
            if (movementValues.IsTransport)
            {

            }*/

            if (movementValues.HasMovementFlags2)
                movementValues.MovementFlags2 = (MovementFlag2)BitUnpack.GetBits<uint>(13);

            if (movementValues.HasMovementFlags)
                movementValues.MovementFlags = (MovementFlag)BitUnpack.GetBits<uint>(30);

            if (guidMask[1]) guidBytes[1] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[2]) guidBytes[2] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[4]) guidBytes[4] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[3]) guidBytes[3] = (byte)(packet.ReadUInt8() ^ 1);

            for (int i = 0; i < counter; i++)
                packet.ReadUInt32();

            if (guidMask[5]) guidBytes[5] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[0]) guidBytes[0] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[6]) guidBytes[6] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[7]) guidBytes[7] = (byte)(packet.ReadUInt8() ^ 1);

            /*if (movementValues.IsTransport)
            {

            }*/

            if (HasTime)
                movementValues.Time = packet.ReadUInt32();

            if (HasPitch)
                packet.ReadFloat();

            /*if (IsInterpolated)
            {

            }*/

            if (movementValues.IsAlive)
                movementValues.Time = packet.ReadUInt32();

            if (movementValues.HasRotation)
                vector.O = packet.ReadFloat();

            if (HasSplineElevation)
                packet.ReadFloat();

            var guid = BitConverter.ToUInt64(guidBytes, 0);
            HandleMoveUpdate(guid, movementValues, vector);
        }

        [Opcode(ClientMessage.MoveStartTurnLeft, "16357")]
        public static void HandleMoveStartTurnLeft(ref PacketReader packet, ref WorldClass session)
        {
            ObjectMovementValues movementValues = new ObjectMovementValues();
            BitUnpack BitUnpack = new BitUnpack(packet);

            bool[] guidMask = new bool[8];
            byte[] guidBytes = new byte[8];

            Vector4 vector = new Vector4()
            {
                Z = packet.ReadFloat(),
                Y = packet.ReadFloat(),
                X = packet.ReadFloat()
            };

            bool Unknown = BitUnpack.GetBit();
            bool Unknown2 = BitUnpack.GetBit();

            uint counter = BitUnpack.GetBits<uint>(24);

            guidMask[2] = BitUnpack.GetBit();
            guidMask[4] = BitUnpack.GetBit();
            guidMask[7] = BitUnpack.GetBit();
            guidMask[1] = BitUnpack.GetBit();

            bool HasPitch = !BitUnpack.GetBit();

            guidMask[0] = BitUnpack.GetBit();

            movementValues.IsAlive = !BitUnpack.GetBit();
            movementValues.IsTransport = BitUnpack.GetBit();

            bool Unknown3 = BitUnpack.GetBit();

            guidMask[6] = BitUnpack.GetBit();

            movementValues.HasMovementFlags = !BitUnpack.GetBit();

            bool Unknown4 = BitUnpack.GetBit();
            movementValues.HasRotation = !BitUnpack.GetBit();

            movementValues.HasMovementFlags2 = !BitUnpack.GetBit();

            guidMask[3] = BitUnpack.GetBit();
            guidMask[5] = BitUnpack.GetBit();

            bool HasTime = !BitUnpack.GetBit();
            bool HasSplineElevation = !BitUnpack.GetBit();

            /*if (movementValues.IsTransport)
            {

            }
            
            if (IsInterpolated)
            {

            }*/

            if (movementValues.HasMovementFlags2)
                movementValues.MovementFlags2 = (MovementFlag2)BitUnpack.GetBits<uint>(13);

            if (movementValues.HasMovementFlags)
                movementValues.MovementFlags = (MovementFlag)BitUnpack.GetBits<uint>(30);

            if (guidMask[4]) guidBytes[4] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[6]) guidBytes[6] = (byte)(packet.ReadUInt8() ^ 1);

            for (int i = 0; i < counter; i++)
                packet.ReadUInt32();

            if (guidMask[1]) guidBytes[1] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[2]) guidBytes[2] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[0]) guidBytes[0] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[7]) guidBytes[7] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[5]) guidBytes[5] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[3]) guidBytes[3] = (byte)(packet.ReadUInt8() ^ 1);

            /*if (movementValues.IsTransport)
            {

            }*/

            if (movementValues.IsAlive)
                movementValues.Time = packet.ReadUInt32();

            if (HasPitch)
                packet.ReadFloat();

            /*if (IsInterpolated)
            {
            
            }*/


            if (movementValues.HasRotation)
                vector.O = packet.ReadFloat();

            if (HasSplineElevation)
                packet.ReadFloat();

            if (HasTime)
                movementValues.Time = packet.ReadUInt32();

            var guid = BitConverter.ToUInt64(guidBytes, 0);
            HandleMoveUpdate(guid, movementValues, vector);
        }

        [Opcode(ClientMessage.MoveStartTurnRight, "16357")]
        public static void HandleMoveStartTurnRight(ref PacketReader packet, ref WorldClass session)
        {
            ObjectMovementValues movementValues = new ObjectMovementValues();
            BitUnpack BitUnpack = new BitUnpack(packet);

            bool[] guidMask = new bool[8];
            byte[] guidBytes = new byte[8];

            Vector4 vector = new Vector4()
            {
                Y = packet.ReadFloat(),
                Z = packet.ReadFloat(),
                X = packet.ReadFloat()
            };

            guidMask[5] = BitUnpack.GetBit();
            guidMask[3] = BitUnpack.GetBit();

            bool HasTime = !BitUnpack.GetBit();

            guidMask[1] = BitUnpack.GetBit();

            movementValues.HasMovementFlags2 = !BitUnpack.GetBit();

            guidMask[0] = BitUnpack.GetBit();

            bool Unknown4 = BitUnpack.GetBit();

            bool Unknown = BitUnpack.GetBit();

            movementValues.HasRotation = !BitUnpack.GetBit();

            bool HasSplineElevation = !BitUnpack.GetBit();

            uint counter = BitUnpack.GetBits<uint>(24);

            guidMask[4] = BitUnpack.GetBit();

            movementValues.IsTransport = BitUnpack.GetBit();

            bool Unknown2 = BitUnpack.GetBit();

            guidMask[2] = BitUnpack.GetBit();

            movementValues.IsAlive = !BitUnpack.GetBit();

            bool HasPitch = !BitUnpack.GetBit();

            movementValues.HasMovementFlags = !BitUnpack.GetBit();

            guidMask[7] = BitUnpack.GetBit();
            guidMask[6] = BitUnpack.GetBit();

            bool Unknown3 = BitUnpack.GetBit();

            if (movementValues.HasMovementFlags)
                movementValues.MovementFlags = (MovementFlag)BitUnpack.GetBits<uint>(30);

            /*if (movementValues.IsTransport)
            {

            }*/

            if (movementValues.HasMovementFlags2)
                movementValues.MovementFlags2 = (MovementFlag2)BitUnpack.GetBits<uint>(13);

            /*if (IsInterpolated)
            {

            }*/

            if (guidMask[3]) guidBytes[3] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[5]) guidBytes[5] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[4]) guidBytes[4] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[6]) guidBytes[6] = (byte)(packet.ReadUInt8() ^ 1);

            for (int i = 0; i < counter; i++)
                packet.ReadUInt32();

            if (guidMask[2]) guidBytes[2] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[7]) guidBytes[7] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[1]) guidBytes[1] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[0]) guidBytes[0] = (byte)(packet.ReadUInt8() ^ 1);


            if (movementValues.HasRotation)
                vector.O = packet.ReadFloat();

            /*if (IsInterpolated)
            {

            }*/

            /*if (movementValues.IsTransport)
            {

            }*/

            if (HasTime)
                movementValues.Time = packet.ReadUInt32();

            if (HasPitch)
                packet.ReadFloat();

            if (HasSplineElevation)
                packet.ReadFloat();

            if (movementValues.IsAlive)
                movementValues.Time = packet.ReadUInt32();

            var guid = BitConverter.ToUInt64(guidBytes, 0);
            HandleMoveUpdate(guid, movementValues, vector);
        }

        [Opcode(ClientMessage.MoveStopTurn, "16357")]
        public static void HandleMoveStopTurn(ref PacketReader packet, ref WorldClass session)
        {
            ObjectMovementValues movementValues = new ObjectMovementValues();
            BitUnpack BitUnpack = new BitUnpack(packet);

            bool[] guidMask = new bool[8];
            byte[] guidBytes = new byte[8];

            Vector4 vector = new Vector4()
            {
                X = packet.ReadFloat(),
                Z = packet.ReadFloat(),
                Y = packet.ReadFloat()
            };

            bool HasTime = !BitUnpack.GetBit();

            guidMask[5] = BitUnpack.GetBit();

            bool Unknown = BitUnpack.GetBit();

            movementValues.IsTransport = BitUnpack.GetBit();

            bool Unknown2 = BitUnpack.GetBit();

            guidMask[3] = BitUnpack.GetBit();

            bool HasSplineElevation = !BitUnpack.GetBit();

            guidMask[0] = BitUnpack.GetBit();

            bool HasPitch = !BitUnpack.GetBit();

            uint counter = BitUnpack.GetBits<uint>(24);

            guidMask[1] = BitUnpack.GetBit();
            guidMask[7] = BitUnpack.GetBit();

            movementValues.HasMovementFlags = !BitUnpack.GetBit();
            movementValues.IsAlive = !BitUnpack.GetBit();

            guidMask[2] = BitUnpack.GetBit();
            guidMask[6] = BitUnpack.GetBit();

            movementValues.HasRotation = !BitUnpack.GetBit();

            bool Unknown3 = BitUnpack.GetBit();

            movementValues.HasMovementFlags2 = !BitUnpack.GetBit();

            bool Unknown4 = BitUnpack.GetBit();

            guidMask[4] = BitUnpack.GetBit();

            /*if (movementValues.IsTransport)
            {

            }
            
            if (IsInterpolated)
            {

            }*/

            if (movementValues.HasMovementFlags)
                movementValues.MovementFlags = (MovementFlag)BitUnpack.GetBits<uint>(30);

            if (movementValues.HasMovementFlags2)
                movementValues.MovementFlags2 = (MovementFlag2)BitUnpack.GetBits<uint>(13);

            if (guidMask[6]) guidBytes[6] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[0]) guidBytes[0] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[5]) guidBytes[5] = (byte)(packet.ReadUInt8() ^ 1);

            for (int i = 0; i < counter; i++)
                packet.ReadUInt32();

            if (guidMask[1]) guidBytes[1] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[7]) guidBytes[7] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[3]) guidBytes[3] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[4]) guidBytes[4] = (byte)(packet.ReadUInt8() ^ 1);
            if (guidMask[2]) guidBytes[2] = (byte)(packet.ReadUInt8() ^ 1);

            /*if (movementValues.IsTransport)
            {

            }*/

            if (movementValues.HasRotation)
                vector.O = packet.ReadFloat();

            if (movementValues.IsAlive)
                movementValues.Time = packet.ReadUInt32();

            /*if (IsInterpolated)
            {

            }*/

            if (HasPitch)
                packet.ReadFloat();

            if (HasSplineElevation)
                packet.ReadFloat();

            if (HasTime)
                movementValues.Time = packet.ReadUInt32();

            var guid = BitConverter.ToUInt64(guidBytes, 0);
            HandleMoveUpdate(guid, movementValues, vector);
        }

        public static void HandleMoveUpdate(ulong guid, ObjectMovementValues movementValues, Vector4 vector)
        {
            PacketWriter moveUpdate = new PacketWriter(JAMCMessage.MoveUpdate);
            BitPack BitPack = new BitPack(moveUpdate, guid);

            BitPack.WriteGuidMask(0);
            BitPack.Write(!movementValues.HasMovementFlags);
            BitPack.Write(!movementValues.HasRotation);
            BitPack.WriteGuidMask(2, 6);
            BitPack.Write(!movementValues.HasMovementFlags2);
            BitPack.WriteGuidMask(7);
            BitPack.Write<uint>(0, 24);
            BitPack.WriteGuidMask(1);

            if (movementValues.HasMovementFlags)
                BitPack.Write((uint)movementValues.MovementFlags, 30);

            BitPack.WriteGuidMask(4);
            BitPack.Write(!movementValues.IsAlive);
            BitPack.Write(0);

            if (movementValues.HasMovementFlags2)
                BitPack.Write((uint)movementValues.MovementFlags2, 13);

            BitPack.Write(0);
            BitPack.WriteGuidMask(5);
            BitPack.Write(true);
            BitPack.Write(0);
            BitPack.Write(movementValues.IsInterpolated);
            BitPack.Write(0);
            BitPack.Write(true);
            BitPack.WriteGuidMask(3);
            BitPack.Write(true);

            if (movementValues.IsInterpolated)
                BitPack.Write(movementValues.IsInterpolated2);

            BitPack.Flush();

            if (movementValues.IsInterpolated)
            {
                moveUpdate.WriteUInt32(0);

                if (movementValues.IsInterpolated2)
                {
                    moveUpdate.WriteFloat(0);
                    moveUpdate.WriteFloat(0);
                    moveUpdate.WriteFloat(0);
                }

                moveUpdate.WriteFloat(0);
            }

            if (movementValues.IsAlive)
                moveUpdate.WriteUInt32(movementValues.Time);

            BitPack.WriteGuidBytes(5, 7);

            moveUpdate.WriteFloat(vector.Z);

            BitPack.WriteGuidBytes(4, 3, 2, 6, 0);

            moveUpdate.WriteFloat(vector.X);

            if (movementValues.HasRotation)
                moveUpdate.WriteFloat(vector.O);

            moveUpdate.WriteFloat(vector.Y);

            var session = WorldMgr.GetSession(guid);
            if (session != null)
            {
                Character pChar = WorldMgr.GetSession(guid).Character;
                ObjectMgr.SetPosition(ref pChar, vector, false);

                WorldMgr.SendToAllOtherInZone(guid, moveUpdate);
            }
        }

        public static void HandleMoveSetWalkSpeed(ref WorldClass session, float speed = 2.5f)
        {
            PacketWriter setWalkSpeed = new PacketWriter(JAMCMessage.MoveSetWalkSpeed);
            BitPack BitPack = new BitPack(setWalkSpeed, session.Character.Guid);

            setWalkSpeed.WriteUInt32(0);
            setWalkSpeed.WriteFloat(speed);

            BitPack.WriteGuidMask(6, 2, 1, 4, 5, 3, 7, 0);
            BitPack.Flush();

            BitPack.WriteGuidBytes(1, 6, 3, 0, 7, 4, 2, 5);

            session.Send(ref setWalkSpeed);
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

            session.Send(ref setRunSpeed);
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

            session.Send(ref setSwimSpeed);
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

            session.Send(ref setFlightSpeed);
        }

        public static void HandleMoveSetCanFly(ref WorldClass session)
        {
            PacketWriter setCanFly = new PacketWriter(JAMCMessage.MoveSetCanFly);
            BitPack BitPack = new BitPack(setCanFly, session.Character.Guid);

            setCanFly.WriteUInt32(0);

            BitPack.WriteGuidMask(4, 5, 0, 6, 2, 1, 3, 7);
            BitPack.Flush();

            BitPack.WriteGuidBytes(5, 3, 1, 6, 7, 2, 4, 0);

            session.Send(ref setCanFly);
        }

        public static void HandleMoveUnsetCanFly(ref WorldClass session)
        {
            PacketWriter unsetCanFly = new PacketWriter(JAMCMessage.MoveUnsetCanFly);
            BitPack BitPack = new BitPack(unsetCanFly, session.Character.Guid);

            unsetCanFly.WriteUInt32(0);

            BitPack.WriteGuidMask(1, 4, 6, 2, 5, 7, 0, 3);
            BitPack.Flush();

            BitPack.WriteGuidBytes(3, 1, 5, 7, 0, 6, 2, 4);

            session.Send(ref unsetCanFly);
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
            moveTeleport.WriteFloat(vector.O);

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

            session.Send(ref moveTeleport);
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
            
            session.Send(ref transferPending);
        }

        public static void HandleNewWorld(ref WorldClass session, Vector4 vector, uint mapId)
        {
            PacketWriter newWorld = new PacketWriter(JAMCMessage.NewWorld);

            newWorld.WriteUInt32(mapId);
            newWorld.WriteFloat(vector.Y);
            newWorld.WriteFloat(vector.O);
            newWorld.WriteFloat(vector.X);
            newWorld.WriteFloat(vector.Z);

            session.Send(ref newWorld);
        }
    }
}
