using Framework.Constants;
using Framework.ObjectDefines;
using System;
using WorldServer.Game.WorldEntities;

namespace WorldServer.Game.Spawns
{
    public class CreatureSpawn : WorldObject, ISpawn
    {
        public UInt32 Id;
        public Creature Data;

        public CreatureSpawn(int updateLength = (int)UnitFields.End) : base(updateLength) { }

        public void CreateFullGuid()
        {
            Guid = new ObjectGuid(Guid, Id, HighGuidType.Unit).Guid;
        }

        public void CreateData()
        {
            Data = new Creature(Id);
        }

        public void SetCreatureFields()
        {
            // ObjectFields
            SetUpdateField<UInt64>((int)ObjectFields.Guid, Guid);
            SetUpdateField<UInt64>((int)ObjectFields.Data, 0);
            SetUpdateField<UInt64>((int)ObjectFields.Entry, Id);
            SetUpdateField<Int32>((int)ObjectFields.Type, 0x9);
            SetUpdateField<Single>((int)ObjectFields.Scale, Data.Scale);

            // UnitFields
            SetUpdateField<UInt64>((int)UnitFields.Charm, 1);
            SetUpdateField<UInt64>((int)UnitFields.Summon, 1);
            SetUpdateField<UInt64>((int)UnitFields.Critter, 0);
            SetUpdateField<UInt64>((int)UnitFields.CharmedBy, 0);
            SetUpdateField<UInt64>((int)UnitFields.SummonedBy, 0);
            SetUpdateField<UInt64>((int)UnitFields.CreatedBy, 0);
            SetUpdateField<UInt64>((int)UnitFields.Target, 0);
            SetUpdateField<UInt64>((int)UnitFields.ChannelObject, 0);

            SetUpdateField<UInt32>((int)UnitFields.Health, Data.Health);

            for (int i = 0; i < 5; i++)
                SetUpdateField<Int32>((int)UnitFields.Power + i, 0);

            SetUpdateField<UInt32>((int)UnitFields.MaxHealth, Data.Health);

            for (int i = 0; i < 5; i++)
                SetUpdateField<Int32>((int)UnitFields.MaxPower + i, 0);

            SetUpdateField<Int32>((int)UnitFields.PowerRegenFlatModifier, 0);
            SetUpdateField<Int32>((int)UnitFields.PowerRegenInterruptedFlatModifier, 0);
            SetUpdateField<Int32>((int)UnitFields.BaseHealth, 1);
            SetUpdateField<Int32>((int)UnitFields.BaseMana, 0);
            SetUpdateField<Int32>((int)UnitFields.Level, Data.Level);
            SetUpdateField<UInt32>((int)UnitFields.FactionTemplate, Data.Faction);
            SetUpdateField<UInt32>((int)UnitFields.Flags, Data.Flags);
            SetUpdateField<UInt32>((int)UnitFields.Flags2, Data.Flags2);
            SetUpdateField<UInt32>((int)UnitFields.NpcFlags, Data.NpcFlags);

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

            SetUpdateField<UInt32>((int)UnitFields.DisplayID, Data.DisplayID);
            SetUpdateField<UInt32>((int)UnitFields.NativeDisplayID, Data.DisplayID);
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
