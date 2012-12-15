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
using Framework.Logging;
using Framework.Network.Packets;
using Framework.ObjectDefines;
using System;
using WorldServer.Game.Managers;
using WorldServer.Game.WorldEntities;

namespace WorldServer.Game.Spawns
{
    public class CreatureSpawn : WorldObject
    {
        public Int32 Id;
        public Creature Creature;

        public CreatureSpawn(int updateLength = (int)UnitFields.End) : base(updateLength) { }


        public static ulong GetLastGuid()
        {
            SQLResult result = DB.World.Select("SELECT * FROM `creature_spawns` ORDER BY `guid` DESC LIMIT 1");
            if (result.Count != 0)
                return result.Read<ulong>(0, "guid");

            return 0;
        }

        public void CreateFullGuid()
        {
            Guid = new ObjectGuid(Guid, Id, HighGuidType.Unit).Guid;
        }

        public void CreateData(Creature creature)
        {
            Creature = creature;
        }

        public bool AddToDB()
        {
            if (DB.World.Execute("INSERT INTO creature_spawns (Id, Map, X, Y, Z, O) VALUES (?, ?, ?, ?, ?, ?)", Id, Map, Position.X, Position.Y, Position.Z, Position.W))
            {
                Log.Message(LogType.DB, "Creature (Id: {0}) successfully spawned (Guid: {1})", Id, Guid);
                return true;
            }

            return false;
        }

        public void AddToWorld()
        {
            CreateFullGuid();
            CreateData(Creature);

            Globals.SpawnMgr.AddSpawn(this, ref Creature);

            SetCreatureFields();

            WorldObject obj = this;
            UpdateFlag updateFlags = UpdateFlag.Alive | UpdateFlag.Rotation;

            foreach (var v in Globals.WorldMgr.Sessions)
            {
                Character pChar = v.Value.Character;
                if (pChar.Map != Map)
                    continue;

                PacketWriter updateObject = new PacketWriter(LegacyMessage.UpdateObject);

                updateObject.WriteUInt16((ushort)Map);
                updateObject.WriteUInt32(1);
                updateObject.WriteUInt8(1);
                updateObject.WriteGuid(Guid);
                updateObject.WriteUInt8(3);

                Globals.WorldMgr.WriteUpdateObjectMovement(ref updateObject, ref obj, updateFlags);

                WriteUpdateFields(ref updateObject);
                WriteDynamicUpdateFields(ref updateObject);

                v.Value.Send(ref updateObject);
            }
        }

        public void SetCreatureFields()
        {
            // ObjectFields
            SetUpdateField<UInt64>((int)ObjectFields.Guid, Guid);
            SetUpdateField<UInt64>((int)ObjectFields.Data, 0);
            SetUpdateField<Int32>((int)ObjectFields.Entry, Id);
            SetUpdateField<Int32>((int)ObjectFields.Type, 0x9);
            SetUpdateField<Single>((int)ObjectFields.Scale, Creature.Data.Scale);

            // UnitFields
            SetUpdateField<UInt64>((int)UnitFields.Charm, 1);
            SetUpdateField<UInt64>((int)UnitFields.Summon, 1);
            SetUpdateField<UInt64>((int)UnitFields.Critter, 0);
            SetUpdateField<UInt64>((int)UnitFields.CharmedBy, 0);
            SetUpdateField<UInt64>((int)UnitFields.SummonedBy, 0);
            SetUpdateField<UInt64>((int)UnitFields.CreatedBy, 0);
            SetUpdateField<UInt64>((int)UnitFields.Target, 0);
            SetUpdateField<UInt64>((int)UnitFields.ChannelObject, 0);

            SetUpdateField<Int32>((int)UnitFields.Health, Creature.Data.Health);

            for (int i = 0; i < 5; i++)
                SetUpdateField<Int32>((int)UnitFields.Power + i, 0);

            SetUpdateField<Int32>((int)UnitFields.MaxHealth, Creature.Data.Health);

            for (int i = 0; i < 5; i++)
                SetUpdateField<Int32>((int)UnitFields.MaxPower + i, 0);

            SetUpdateField<Int32>((int)UnitFields.PowerRegenFlatModifier, 0);
            SetUpdateField<Int32>((int)UnitFields.PowerRegenInterruptedFlatModifier, 0);
            SetUpdateField<Int32>((int)UnitFields.BaseHealth, 1);
            SetUpdateField<Int32>((int)UnitFields.BaseMana, 0);
            SetUpdateField<Int32>((int)UnitFields.Level, Creature.Data.Level);
            SetUpdateField<Int32>((int)UnitFields.FactionTemplate, Creature.Data.Faction);
            SetUpdateField<Int32>((int)UnitFields.Flags, Creature.Data.UnitFlags);
            SetUpdateField<Int32>((int)UnitFields.Flags2, Creature.Data.UnitFlags2);
            SetUpdateField<Int32>((int)UnitFields.NpcFlags, Creature.Data.NpcFlags);

            for (int i = 0; i < 5; i++)
            {
                SetUpdateField<Int32>((int)UnitFields.Stats + i, 0);
                SetUpdateField<Int32>((int)UnitFields.StatPosBuff + i, 0);
                SetUpdateField<Int32>((int)UnitFields.StatNegBuff + i, 0);
            }

            SetUpdateField<Byte>((int)UnitFields.DisplayPower, 0, 0);
            SetUpdateField<Byte>((int)UnitFields.DisplayPower, 0, 1);
            SetUpdateField<Byte>((int)UnitFields.DisplayPower, 0, 2);
            SetUpdateField<Byte>((int)UnitFields.DisplayPower, 0, 3);

            SetUpdateField<Int32>((int)UnitFields.DisplayID, Creature.Stats.DisplayInfoId[0]);
            SetUpdateField<Int32>((int)UnitFields.NativeDisplayID, Creature.Stats.DisplayInfoId[2]);
            SetUpdateField<Int32>((int)UnitFields.MountDisplayID, 0);
            SetUpdateField<Int32>((int)UnitFields.DynamicFlags, 0);

            SetUpdateField<Single>((int)UnitFields.BoundingRadius, 0.389F);
            SetUpdateField<Single>((int)UnitFields.CombatReach, 1.5F);
            SetUpdateField<Single>((int)UnitFields.MinDamage, 0);
            SetUpdateField<Single>((int)UnitFields.MaxDamage, 0);
            SetUpdateField<Single>((int)UnitFields.ModCastingSpeed, 1);
            SetUpdateField<Int32>((int)UnitFields.AttackPower, 0);
            SetUpdateField<Int32>((int)UnitFields.RangedAttackPower, 0);

            for (int i = 0; i < 7; i++)
            {
                SetUpdateField<Int32>((int)UnitFields.Resistances + i, 0);
                SetUpdateField<Int32>((int)UnitFields.ResistanceBuffModsPositive + i, 0);
                SetUpdateField<Int32>((int)UnitFields.ResistanceBuffModsNegative + i, 0);
            }

            SetUpdateField<Byte>((int)UnitFields.AnimTier, 0, 0);
            SetUpdateField<Byte>((int)UnitFields.AnimTier, 0, 1);
            SetUpdateField<Byte>((int)UnitFields.AnimTier, 0, 2);
            SetUpdateField<Byte>((int)UnitFields.AnimTier, 0, 3);

            SetUpdateField<Int16>((int)UnitFields.RangedAttackRoundBaseTime, 0);
            SetUpdateField<Int16>((int)UnitFields.RangedAttackRoundBaseTime, 0);
            SetUpdateField<Single>((int)UnitFields.MinOffHandDamage, 0);
            SetUpdateField<Single>((int)UnitFields.MaxOffHandDamage, 0);
            SetUpdateField<Int32>((int)UnitFields.AttackPowerModPos, 0);
            SetUpdateField<Int32>((int)UnitFields.RangedAttackPowerModPos, 0);
            SetUpdateField<Single>((int)UnitFields.MinRangedDamage, 0);
            SetUpdateField<Single>((int)UnitFields.MaxRangedDamage, 0);
            SetUpdateField<Single>((int)UnitFields.AttackPowerMultiplier, 0);
            SetUpdateField<Single>((int)UnitFields.RangedAttackPowerMultiplier, 0);
            SetUpdateField<Single>((int)UnitFields.MaxHealthModifier, 1);
        }
    }
}
