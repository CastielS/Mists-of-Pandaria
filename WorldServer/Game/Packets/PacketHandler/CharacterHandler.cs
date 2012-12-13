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
using Framework.Logging;
using Framework.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldServer.Game.Managers;
using WorldServer.Game.Packets.PacketHandler;
using WorldServer.Game.WorldEntities;
using WorldServer.Network;

namespace WorldServer.Game.PacketHandler
{
    public class CharacterHandler : Globals
    {
        [Opcode(ClientMessage.EnumCharacters, "16357")]
        public static void HandleEnumCharactersResult(ref PacketReader packet, ref WorldClass session)
        {
            DB.Realms.Execute("UPDATE accounts SET online = 1 WHERE id = ?", session.Account.Id);

            SQLResult result = DB.Characters.Select("SELECT * FROM characters WHERE accountid = ?", session.Account.Id);

            PacketWriter enumCharacters = new PacketWriter(JAMCMessage.EnumCharactersResult);
            BitPack BitPack = new BitPack(enumCharacters);

            BitPack.Write(0, 23);
            BitPack.Write(result.Count, 17);

            if (result.Count != 0)
            {
                for (int c = 0; c < result.Count; c++)
                {
                    string name       = result.Read<String>(c, "Name");
                    bool loginCinematic = result.Read<Boolean>(c, "LoginCinematic");

                    BitPack.Guid      = result.Read<UInt64>(c, "Guid");
                    BitPack.GuildGuid = result.Read<UInt64>(c, "GuildGuid");

                    BitPack.WriteGuidMask(7, 0, 4);
                    BitPack.WriteGuildGuidMask(2);
                    BitPack.WriteGuidMask(5, 3);
                    BitPack.Write((uint)Encoding.ASCII.GetBytes(name).Length, 7);
                    BitPack.WriteGuildGuidMask(0, 5, 3);
                    BitPack.Write(loginCinematic);
                    BitPack.WriteGuildGuidMask(6, 7);
                    BitPack.WriteGuidMask(1);
                    BitPack.WriteGuildGuidMask(4, 1);
                    BitPack.WriteGuidMask(2, 6);
                }

                BitPack.Write(1);
                BitPack.Flush();

                for (int c = 0; c < result.Count; c++)
                {
                    string name       = result.Read<String>(c, "Name");
                    BitPack.Guid      = result.Read<UInt64>(c, "Guid");
                    BitPack.GuildGuid = result.Read<UInt64>(c, "GuildGuid");

                    enumCharacters.WriteUInt32(result.Read<UInt32>(c, "CharacterFlags"));
                    enumCharacters.WriteUInt32(result.Read<UInt32>(c, "PetFamily"));
                    enumCharacters.WriteFloat(result.Read<Single>(c, "Z"));

                    BitPack.WriteGuidBytes(7);
                    BitPack.WriteGuildGuidBytes(6);

                    // Not implanted
                    for (int j = 0; j < 23; j++)
                    {
                        enumCharacters.WriteUInt32(0);
                        enumCharacters.WriteUInt8(0);
                        enumCharacters.WriteUInt32(0);
                    }

                    enumCharacters.WriteFloat(result.Read<Single>(c, "X"));
                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "Class"));

                    BitPack.WriteGuidBytes(5);

                    enumCharacters.WriteFloat(result.Read<Single>(c, "Y"));

                    BitPack.WriteGuildGuidBytes(3);
                    BitPack.WriteGuidBytes(6);

                    enumCharacters.WriteUInt32(result.Read<UInt32>(c, "PetLevel"));
                    enumCharacters.WriteUInt32(result.Read<UInt32>(c, "PetDisplayId"));

                    BitPack.WriteGuidBytes(2);
                    BitPack.WriteGuidBytes(1);

                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "HairColor"));
                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "FacialHair"));

                    BitPack.WriteGuildGuidBytes(2);

                    enumCharacters.WriteUInt32(result.Read<UInt32>(c, "Zone"));
                    enumCharacters.WriteUInt8(0);

                    BitPack.WriteGuidBytes(0);
                    BitPack.WriteGuildGuidBytes(1);

                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "Skin"));

                    BitPack.WriteGuidBytes(4);
                    BitPack.WriteGuildGuidBytes(5);

                    enumCharacters.WriteString(name);

                    BitPack.WriteGuildGuidBytes(0);

                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "Level"));

                    BitPack.WriteGuidBytes(3);
                    BitPack.WriteGuildGuidBytes(7);

                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "HairStyle"));

                    BitPack.WriteGuildGuidBytes(4);

                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "Gender"));
                    enumCharacters.WriteUInt32(result.Read<UInt32>(c, "Map"));
                    enumCharacters.WriteUInt32(result.Read<UInt32>(c, "CustomizeFlags"));
                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "Race"));
                    enumCharacters.WriteUInt8(result.Read<Byte>(c, "Face"));
                }
            }
            else
            {
                BitPack.Write(1);
                BitPack.Flush();
            };

            session.Send(enumCharacters);
        }

        [Opcode(ClientMessage.RequestCharCreate, "16357")]
        public static void HandleResponseCharacterCreate(ref PacketReader packet, ref WorldClass session)
        {
            BitUnpack BitUnpack = new BitUnpack(packet);

            byte pClass = packet.ReadByte();
            byte hairStyle = packet.ReadByte();
            byte facialHair = packet.ReadByte();
            byte race = packet.ReadByte();
            byte face = packet.ReadByte();
            byte skin = packet.ReadByte();
            byte gender = packet.ReadByte();
            byte hairColor = packet.ReadByte();
            packet.ReadByte();                      // Always 0

            uint nameLength = BitUnpack.GetNameLength<uint>(7);
            string name = Character.NormalizeName(packet.ReadString(nameLength));

            SQLResult result = DB.Characters.Select("SELECT * from characters WHERE name = ?", name);
            PacketWriter writer = new PacketWriter(LegacyMessage.ResponseCharacterCreate);

            if (result.Count != 0)
            {
                // Name already in use
                writer.WriteUInt8(0x32);
                session.Send(writer);
                return;
            }

            result = DB.Characters.Select("SELECT map, zone, posX, posY, posZ, posO FROM character_creation_data WHERE race = ? AND class = ?", race, pClass);
            if (result.Count == 0)
            {
                writer.WriteUInt8(0x31);
                session.Send(writer);
                return;
            }

            uint map = result.Read<uint>(0, "map");
            uint zone = result.Read<uint>(0, "zone");
            float posX = result.Read<float>(0, "posX");
            float posY = result.Read<float>(0, "posY");
            float posZ = result.Read<float>(0, "posZ");
            float posO = result.Read<float>(0, "posO");

            DB.Characters.Execute("INSERT INTO characters (name, accountid, race, class, gender, skin, zone, map, x, y, z, o, face, hairstyle, haircolor, facialhair) VALUES (" +
                                  "?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                                  name, session.Account.Id, race, pClass, gender, skin, zone, map, posX, posY, posZ, posO, face, hairStyle, hairColor, facialHair);

            // Success
            writer.WriteUInt8(0x2F);
            session.Send(writer);
        }

        [Opcode(ClientMessage.RequestCharDelete, "16357")]
        public static void HandleResponseCharacterDelete(ref PacketReader packet, ref WorldClass session)
        {
            UInt64 guid = packet.ReadUInt64();

            PacketWriter writer = new PacketWriter(LegacyMessage.ResponseCharacterDelete);
            writer.WriteUInt8(0x47);
            session.Send(writer);

            DB.Characters.Execute("DELETE FROM characters WHERE guid = ?", guid);
            DB.Characters.Execute("DELETE FROM character_spells WHERE guid = ?", guid);
            DB.Characters.Execute("DELETE FROM character_skills WHERE guid = ?", guid);
        }

        [Opcode(ClientMessage.RequestRandomCharacterName, "16357")]
        public static void HandleGenerateRandomCharacterNameResult(ref PacketReader packet, ref WorldClass session)
        {
            byte gender = packet.ReadByte();
            byte race = packet.ReadByte();

            List<string> names = DBCStorage.NameGenStorage.Where(n => n.Value.Race == race && n.Value.Gender == gender).Select(n => n.Value.Name).ToList();
            Random rand = new Random(Environment.TickCount);

            string NewName;
            SQLResult result;
            do
            {
                NewName = names[rand.Next(names.Count)];
                result = DB.Characters.Select("SELECT * FROM characters WHERE name = ?", NewName);
            }
            while (result.Count != 0);

            PacketWriter writer = new PacketWriter(JAMCMessage.GenerateRandomCharacterNameResult);
            BitPack BitPack = new BitPack(writer);

            BitPack.Write<int>(NewName.Length, 15);
            BitPack.Write(true);
            BitPack.Flush();

            writer.WriteString(NewName);
            session.Send(writer);
        }

        [Opcode(ClientMessage.PlayerLogin, "16357")]
        public static void HandlePlayerLogin(ref PacketReader packet, ref WorldClass session)
        {
            byte[] guidMask = { 5, 7, 6, 1, 2, 3, 4, 0 };
            byte[] guidBytes = { 6, 4, 3, 5, 0, 2, 7, 1 };

            BitUnpack GuidUnpacker = new BitUnpack(packet);

            ulong guid = GuidUnpacker.GetGuid(guidMask, guidBytes);
            Log.Message(LogType.DEBUG, "Character with Guid: {0}, AccountId: {1} tried to enter the world.", guid, session.Account.Id);

            session.Character = new Character(guid);

            WorldMgr.AddSession(guid, ref session);
            WorldMgr.WriteAccountData(AccountDataMasks.CharacterCacheMask, ref session);

            MiscHandler.HandleMessageOfTheDay(ref session);
            SpellHandler.HandleSendKnownSpells(ref session);

            if (session.Character.LoginCinematic)
                CinematicHandler.HandleStartCinematic(ref session);

            ObjectHandler.HandleUpdateObject(ref session);
        }
    }
}
