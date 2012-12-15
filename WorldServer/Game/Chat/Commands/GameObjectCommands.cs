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
using WorldServer.Game.Spawns;
using WorldServer.Game.WorldEntities;
using WorldServer.Network;

namespace WorldServer.Game.Chat.Commands
{
    public class GameObjectCommands : Globals
    {
        [ChatCommand("addobject")]
        public static void AddObject(string[] args, ref WorldClass session)
        {
            var pChar = session.Character;

            int objectId = CommandParser.Read<int>(args, 1);

            GameObject gObject = DataMgr.FindGameObject(objectId);
            if (gObject != null)
            {
                GameObjectSpawn spawn = new GameObjectSpawn()
                {
                    Guid = GameObjectSpawn.GetLastGuid() + 1,
                    Id = objectId,
                    GameObject = gObject,
                    Position = pChar.Position,
                    Map = pChar.Map
                };

                if (spawn.AddToDB())
                {
                    spawn.AddToWorld();
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Spawn successfully added.");
                }
                else
                    ChatHandler.SendMessageByType(ref session, 0, 0, "Spawn can't be added.");
            }
        }
    }
}
