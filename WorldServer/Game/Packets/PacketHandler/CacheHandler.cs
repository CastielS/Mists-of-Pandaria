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

using Framework.Configuration;
using Framework.Constants;
using Framework.Database;
using Framework.Network.Packets;
using WorldServer.Game.Managers;
using WorldServer.Game.ObjectDefines;
using WorldServer.Game.WorldEntities;
using WorldServer.Network;
using Framework.Logging;

namespace WorldServer.Game.Packets.PacketHandler
{
    public class CacheHandler : Globals
    {
        [Opcode(ClientMessage.CreatureStats, "16357")]
        public static void HandleCreatureStats(ref PacketReader packet, ref WorldClass session)
        {
            int id = packet.ReadInt32();
            ulong guid = packet.ReadUInt64();

            Creature creature = DataMgr.FindCreature(id);
            if (creature != null)
            {
                CreatureStats stats = creature.Stats;

                PacketWriter creatureStats = new PacketWriter(LegacyMessage.CreatureStats);

                creatureStats.WriteInt32(stats.Id);
                creatureStats.WriteCString(stats.Name);

                for (int i = 0; i < 7; i++)
                    creatureStats.WriteCString("");

                creatureStats.WriteCString(stats.SubName);
                creatureStats.WriteCString("");
                creatureStats.WriteCString(stats.IconName);

                foreach (var v in stats.Flag)
                    creatureStats.WriteInt32(v);

                creatureStats.WriteInt32(stats.Type);
                creatureStats.WriteInt32(stats.Family);
                creatureStats.WriteInt32(stats.Rank);

                foreach (var v in stats.QuestKillNpcId)
                    creatureStats.WriteInt32(v);

                foreach (var v in stats.DisplayInfoId)
                    creatureStats.WriteInt32(v);

                creatureStats.WriteFloat(stats.HealthModifier);
                creatureStats.WriteFloat(stats.PowerModifier);

                creatureStats.WriteUInt8(stats.RacialLeader);

                foreach (var v in stats.QuestItemId)
                    creatureStats.WriteInt32(v);

                creatureStats.WriteInt32(stats.MovementInfoId);
                creatureStats.WriteInt32(stats.ExpansionRequired);

                session.Send(ref creatureStats);
            }
            else
                Log.Message(LogType.DEBUG, "Creature (Id: {0}) not found.", id);
        }

        [Opcode(ClientMessage.GameObjectStats, "16357")]
        public static void HandleGameObjectStats(ref PacketReader packet, ref WorldClass session)
        {
            int id = packet.ReadInt32();
            ulong guid = packet.ReadUInt64();

            GameObject gObject = DataMgr.FindGameObject(id);
            if (gObject != null)
            {
                GameObjectStats stats = gObject.Stats;

                PacketWriter gameObjectStats = new PacketWriter(LegacyMessage.GameObjectStats);

                gameObjectStats.WriteInt32(stats.Id);
                gameObjectStats.WriteInt32(stats.Type);
                gameObjectStats.WriteInt32(stats.DisplayInfoId);
                gameObjectStats.WriteCString(stats.Name);

                for (int i = 0; i < 3; i++)
                    gameObjectStats.WriteCString("");

                gameObjectStats.WriteCString(stats.IconName);
                gameObjectStats.WriteCString(stats.CastBarCaption);
                gameObjectStats.WriteCString("");

                foreach (var v in stats.Data)
                    gameObjectStats.WriteInt32(v);

                gameObjectStats.WriteFloat(stats.Size);

                foreach (var v in stats.QuestItemId)
                    gameObjectStats.WriteInt32(v);

                gameObjectStats.WriteInt32(stats.ExpansionRequired);

                session.Send(ref gameObjectStats);
            }
            else
                Log.Message(LogType.DEBUG, "Gameobject (Id: {0}) not found.", id);
        }

        [Opcode(ClientMessage.NameCache, "16357")]
        public static void HandleNameCache(ref PacketReader packet, ref WorldClass session)
        {
            ulong guid = packet.ReadUInt64();
            uint realmId = packet.ReadUInt32();

            var pSession = WorldMgr.GetSession(guid);
            if (pSession != null)
            {
                var pChar = pSession.Character;

                if (pChar != null)
                {
                    PacketWriter nameCache = new PacketWriter(LegacyMessage.NameCache);

                    nameCache.WriteGuid(guid);
                    nameCache.WriteUInt8(0);
                    nameCache.WriteCString(pChar.Name);
                    nameCache.WriteUInt32(realmId);
                    nameCache.WriteUInt8(pChar.Race);
                    nameCache.WriteUInt8(pChar.Gender);
                    nameCache.WriteUInt8(pChar.Class);
                    nameCache.WriteUInt8(0);

                    session.Send(ref nameCache);
                }
            }
        }

        [Opcode(ClientMessage.RealmCache, "16357")]
        public static void HandleRealmCache(ref PacketReader packet, ref WorldClass session)
        {
            Character pChar = session.Character;

            uint realmId = packet.ReadUInt32();

            SQLResult result = DB.Realms.Select("SELECT name FROM realms WHERE id = ?", WorldConfig.RealmId);
            string realmName = result.Read<string>(0, "Name");

            PacketWriter nameCache = new PacketWriter(LegacyMessage.RealmCache);

            nameCache.WriteUInt32(realmId);
            nameCache.WriteUInt8(0);              // < 0 => End of packet
            nameCache.WriteUInt8(1);              // Unknown
            nameCache.WriteCString(realmName);
            nameCache.WriteCString(realmName);

            session.Send(ref nameCache);
        }
    }
}
