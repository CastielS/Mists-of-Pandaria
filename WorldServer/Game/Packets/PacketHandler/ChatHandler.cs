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
using WorldServer.Game.Chat;
using WorldServer.Game.Managers;
using WorldServer.Network;

namespace WorldServer.Game.Packets.PacketHandler
{
    public class ChatHandler : Globals
    {
        [Opcode(ClientMessage.ChatMessageSay, "16309")]
        public static void HandleChatMessageSay(ref PacketReader packet, ref WorldClass session)
        {
            BitUnpack BitUnpack = new BitUnpack(packet);
            int language = packet.ReadInt32();

            uint messageLength = BitUnpack.GetBits<uint>(9);
            string chatMessage = packet.ReadString(messageLength);

            if (chatMessage.StartsWith("!"))
            {
                WorldMgr.Session = session;
                ChatCommandParser.ExecuteChatHandler(chatMessage);
            }
            else
                SendMessageByType(ref session, MessageType.ChatMessageSay, language, chatMessage);
        }

        [Opcode(ClientMessage.ChatMessageYell, "16309")]
        public static void HandleChatMessageYell(ref PacketReader packet, ref WorldClass session)
        {
            BitUnpack BitUnpack = new BitUnpack(packet);
            int language = packet.ReadInt32();

            uint messageLength = BitUnpack.GetBits<uint>(9);
            string chatMessage = packet.ReadString(messageLength);
            SendMessageByType(ref session, MessageType.ChatMessageYell, language, chatMessage);
        }

        [Opcode(ClientMessage.ChatMessageWhisper, "16309")]
        public static void HandleChatMessageWhisper(ref PacketReader packet, ref WorldClass session)
        {
            BitUnpack BitUnpack = new BitUnpack(packet);
            int language = packet.ReadInt32();

            uint messageLength = BitUnpack.GetBits<uint>(9);
            uint nameLength = BitUnpack.GetNameLength<uint>(9);

            string chatMessage = packet.ReadString(messageLength);
            string receiverName = packet.ReadString(nameLength);

            WorldClass rSession = WorldMgr.GetSession(receiverName);

            SendMessageByType(ref rSession, MessageType.ChatMessageWhisper, language, chatMessage);
            SendMessageByType(ref session, MessageType.ChatMessageWhisperInform, language, chatMessage);
        }

        public static void SendMessageByType(ref WorldClass session, MessageType type, int language, string chatMessage)
        {
            PacketWriter messageChat = new PacketWriter(LegacyMessage.MessageChat);
            ulong guid = session.Character.Guid;

            messageChat.WriteUInt8((byte)type);
            messageChat.WriteInt32(language);
            messageChat.WriteUInt64(guid);
            messageChat.WriteUInt32(0);
            messageChat.WriteUInt64(guid);
            messageChat.WriteUInt32((uint)chatMessage.Length + 1);
            messageChat.WriteCString(chatMessage);
            messageChat.WriteUInt16(0);

            session.Send(messageChat);
        }
    }
}

