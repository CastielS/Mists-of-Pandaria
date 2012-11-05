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

using Framework.Console;
using WorldServer.Game.Managers;
using WorldServer.Game.Packets.PacketHandler;
using Framework.ObjectDefines;
using WorldServer.Game.PacketHandler;
using Framework.Database;

namespace WorldServer.Game.Chat.Commands
{
    public class MovementCommands : Globals
    {
        [ChatCommand("fly", "Usage: !fly #state (Turns the fly mode 'on' or 'off')")]
        public static void Fly(string[] args)
        {
            string state = CommandParser.Read<string>(args, 1);
            var session = GetSession();

            if (state == "on")
            {
                MoveHandler.HandleMoveSetCanFly(ref session);
                ChatHandler.SendMessageByType(ref session, 0, 0, "Fly mode enabled.");
            }
            else if (state == "off")
            {
                MoveHandler.HandleMoveUnsetCanFly(ref session);
                ChatHandler.SendMessageByType(ref session, 0, 0, "Fly mode disabled.");
            }
        }

        [ChatCommand("walkspeed", "Usage: !walkspeed #speed (Set the current walk speed)")]
        public static void WalkSpeed(string[] args)
        {
            var session = GetSession();

            if (args.Length == 1)
                MoveHandler.HandleMoveSetWalkSpeed(ref session);
            else
            {
                var speed = CommandParser.Read<float>(args, 1);

                if (speed <= 50 && speed > 0)
                {
                    MoveHandler.HandleMoveSetWalkSpeed(ref session, speed);
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Walk speed set to " + speed + "!");
                }
                else
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Please enter a value between 0.0 and 50.0!");

                return;
            }

            ChatHandler.SendMessageByType(ref session, 0, 0, "Walk speed set to default.");
        }

        [ChatCommand("runspeed", "Usage: !runspeed #speed (Set the current run speed)")]
        public static void RunSpeed(string[] args)
        {
            var session = GetSession();

            if (args.Length == 1)
                MoveHandler.HandleMoveSetRunSpeed(ref session);
            else
            {
                var speed = CommandParser.Read<float>(args, 1);
                if (speed <= 50 && speed > 0)
                {
                    MoveHandler.HandleMoveSetRunSpeed(ref session, speed);
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Run speed set to " + speed + "!");
                }
                else
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Please enter a value between 0.0 and 50.0!");

                return;
            }

            ChatHandler.SendMessageByType(ref session, 0, 0, "Run speed set to default.");
        }

        [ChatCommand("swimspeed", "Usage: !swimspeed #speed (Set the current swim speed)")]
        public static void SwimSpeed(string[] args)
        {
            var session = GetSession();

            if (args.Length == 1)
                MoveHandler.HandleMoveSetSwimSpeed(ref session);
            else
            {
                var speed = CommandParser.Read<float>(args, 1);
                if (speed <= 50 && speed > 0)
                {
                    MoveHandler.HandleMoveSetSwimSpeed(ref session, speed);
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Swim speed set to " + speed + "!");
                }
                else
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Please enter a value between 0.0 and 50.0!");

                return;
            }

            ChatHandler.SendMessageByType(ref session, 0, 0, "Swim speed set to default.");
        }

        [ChatCommand("flightspeed", "Usage: !flightspeed #speed (Set the current flight speed)")]
        public static void FlightSpeed(string[] args)
        {
            var session = GetSession();

            if (args.Length == 1)
                MoveHandler.HandleMoveSetFlightSpeed(ref session);
            else
            {
                var speed = CommandParser.Read<float>(args, 1);

                if (speed <= 50 && speed > 0)
                {
                    MoveHandler.HandleMoveSetFlightSpeed(ref session, speed);
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Flight speed set to " + speed + "!");
                }
                else
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Please enter a value between 0.0 and 50.0!");

                return;
            }

            ChatHandler.SendMessageByType(ref session, 0, 0, "Flight speed set to default.");
        }

        [ChatCommand("tele", "Usage: !tele [#x #y #z #o #map] or [#location] (Force teleport to a new location by coordinates or location)")]
        public static void Teleport(string[] args)
        {
            var session = GetSession();
            var pChar = session.Character;
            Vector4 vector;
            uint mapId;

            if (args.Length > 2)
            {
                vector = new Vector4()
                {
                    X = CommandParser.Read<float>(args, 1),
                    Y = CommandParser.Read<float>(args, 2),
                    Z = CommandParser.Read<float>(args, 3),
                    W = CommandParser.Read<float>(args, 4)
                };

                mapId = CommandParser.Read<uint>(args, 5);
            }
            else
            {
                string location = CommandParser.Read<string>(args, 1);
                SQLResult result = DB.World.Select("SELECT * FROM teleport_locations WHERE location = '{0}'", location);

                vector = new Vector4()
                {
                    X = result.Read<float>(0, "X"),
                    Y = result.Read<float>(0, "Y"),
                    Z = result.Read<float>(0, "Z"),
                    W = result.Read<float>(0, "O"),
                };

                mapId = result.Read<uint>(0, "Map");
            }

            if (pChar.Map == mapId)
            {
                MoveHandler.HandleMoveTeleport(ref session, vector);
                ObjectMgr.SetPosition(ref pChar, vector);
            }
            else
            {
                MoveHandler.HandleTransferPending(ref session, mapId);
                MoveHandler.HandleNewWorld(ref session, vector, mapId);

                ObjectMgr.SetPosition(ref pChar, vector);
                ObjectMgr.SetMap(ref pChar, mapId);

                UpdateHandler.HandleUpdateObject(ref session);
            }
        }
    }
}
