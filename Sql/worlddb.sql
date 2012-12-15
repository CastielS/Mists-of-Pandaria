/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50519
Source Host           : localhost:3306
Source Database       : world

Target Server Type    : MYSQL
Target Server Version : 50519
File Encoding         : 65001

Date: 2012-12-15 04:10:39
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `creature_data`
-- ----------------------------
DROP TABLE IF EXISTS `creature_data`;
CREATE TABLE `creature_data` (
  `Id` int(11) NOT NULL,
  `Health` int(11) NOT NULL DEFAULT '1',
  `Level` tinyint(4) NOT NULL DEFAULT '1',
  `Class` tinyint(4) NOT NULL DEFAULT '1',
  `Faction` int(11) NOT NULL DEFAULT '35',
  `Scale` float NOT NULL DEFAULT '1',
  `UnitFlags` int(11) NOT NULL DEFAULT '0',
  `UnitFlags2` int(11) NOT NULL DEFAULT '0',
  `NpcFlags` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of creature_data
-- ----------------------------
INSERT INTO `creature_data` VALUES ('53566', '393941', '90', '1', '35', '1', '0', '0', '3');

-- ----------------------------
-- Table structure for `creature_spawns`
-- ----------------------------
DROP TABLE IF EXISTS `creature_spawns`;
CREATE TABLE `creature_spawns` (
  `guid` bigint(20) NOT NULL AUTO_INCREMENT,
  `id` int(10) NOT NULL,
  `map` int(11) NOT NULL,
  `x` float NOT NULL,
  `y` float NOT NULL,
  `z` float NOT NULL,
  `o` float NOT NULL,
  PRIMARY KEY (`guid`),
  KEY `creatureId` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of creature_spawns
-- ----------------------------

-- ----------------------------
-- Table structure for `creature_stats`
-- ----------------------------
DROP TABLE IF EXISTS `creature_stats`;
CREATE TABLE `creature_stats` (
  `Id` int(10) NOT NULL,
  `Name` text,
  `SubName` text,
  `IconName` text,
  `Flag` int(11) NOT NULL DEFAULT '0',
  `Flag2` int(11) NOT NULL DEFAULT '0',
  `Type` int(10) NOT NULL DEFAULT '0',
  `Family` int(11) NOT NULL DEFAULT '0',
  `Rank` int(11) NOT NULL DEFAULT '0',
  `QuestKillNpcId` int(11) NOT NULL DEFAULT '0',
  `QuestKillNpcId2` int(11) NOT NULL DEFAULT '0',
  `DisplayInfoId` int(11) NOT NULL,
  `DisplayInfoId2` int(11) NOT NULL DEFAULT '0',
  `DisplayInfoId3` int(11) NOT NULL DEFAULT '0',
  `DisplayInfoId4` int(11) NOT NULL DEFAULT '0',
  `HealthModifier` float NOT NULL DEFAULT '1',
  `PowerModifier` float NOT NULL DEFAULT '1',
  `RacialLeader` tinyint(4) NOT NULL DEFAULT '0',
  `QuestItemId` int(11) NOT NULL DEFAULT '0',
  `QuestItemId2` int(11) NOT NULL DEFAULT '0',
  `QuestItemId3` int(11) NOT NULL DEFAULT '0',
  `QuestItemId4` int(11) NOT NULL DEFAULT '0',
  `QuestItemId5` int(11) NOT NULL DEFAULT '0',
  `QuestItemId6` int(11) NOT NULL DEFAULT '0',
  `MovementInfoId` int(11) NOT NULL DEFAULT '0',
  `ExpansionRequired` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- ----------------------------
-- Records of creature_stats
-- ----------------------------
INSERT INTO `creature_stats` VALUES ('53566', 'Master Shang Xi', null, null, '0', '0', '7', '0', '0', '0', '0', '39574', '0', '0', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0');

-- ----------------------------
-- Table structure for `gameobject_spawns`
-- ----------------------------
DROP TABLE IF EXISTS `gameobject_spawns`;
CREATE TABLE `gameobject_spawns` (
  `guid` bigint(20) NOT NULL AUTO_INCREMENT,
  `id` int(10) NOT NULL,
  `map` int(11) NOT NULL,
  `x` float NOT NULL,
  `y` float NOT NULL,
  `z` float NOT NULL,
  `o` float NOT NULL,
  PRIMARY KEY (`guid`),
  KEY `creatureId` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of gameobject_spawns
-- ----------------------------

-- ----------------------------
-- Table structure for `gameobject_stats`
-- ----------------------------
DROP TABLE IF EXISTS `gameobject_stats`;
CREATE TABLE `gameobject_stats` (
  `Id` int(10) NOT NULL,
  `Type` int(11) NOT NULL DEFAULT '0',
  `DisplayInfoId` int(11) NOT NULL DEFAULT '0',
  `Name` text,
  `IconName` text,
  `CastBarCaption` text,
  `Data` int(11) NOT NULL DEFAULT '0',
  `Data2` int(11) NOT NULL DEFAULT '0',
  `Data3` int(11) NOT NULL DEFAULT '0',
  `Data4` int(11) NOT NULL DEFAULT '0',
  `Data5` int(11) NOT NULL,
  `Data6` int(11) NOT NULL DEFAULT '0',
  `Data7` int(11) NOT NULL DEFAULT '0',
  `Data8` int(11) NOT NULL DEFAULT '0',
  `Data9` int(11) NOT NULL,
  `Data10` int(11) NOT NULL,
  `Data11` int(11) NOT NULL,
  `Data12` int(11) NOT NULL,
  `Data13` int(11) NOT NULL,
  `Data14` int(11) NOT NULL,
  `Data15` int(11) NOT NULL,
  `Data16` int(11) NOT NULL,
  `Data17` int(11) NOT NULL,
  `Data18` int(11) NOT NULL,
  `Data19` int(11) NOT NULL,
  `Data20` int(11) NOT NULL,
  `Data21` int(11) NOT NULL,
  `Data22` int(11) NOT NULL,
  `Data23` int(11) NOT NULL,
  `Data24` int(11) NOT NULL,
  `Data25` int(11) NOT NULL,
  `Data26` int(11) NOT NULL,
  `Data27` int(11) NOT NULL,
  `Data28` int(11) NOT NULL,
  `Data29` int(11) NOT NULL,
  `Data30` int(11) NOT NULL,
  `Data31` int(11) NOT NULL,
  `Data32` int(11) NOT NULL,
  `Size` float NOT NULL DEFAULT '0',
  `QuestItemId` int(11) NOT NULL DEFAULT '0',
  `QuestItemId2` int(11) NOT NULL DEFAULT '0',
  `QuestItemId3` int(11) NOT NULL DEFAULT '0',
  `QuestItemId4` int(11) NOT NULL DEFAULT '0',
  `QuestItemId5` int(11) NOT NULL DEFAULT '0',
  `QuestItemId6` int(11) NOT NULL DEFAULT '0',
  `ExpansionRequired` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- ----------------------------
-- Records of gameobject_stats
-- ----------------------------
INSERT INTO `gameobject_stats` VALUES ('210020', '3', '10721', 'Weapon Rack', null, null, '57', '40864', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '13364', '0', '0', '0', '0', '0', '0', '0', '0', '128680', '0', '0', '0', '0', '0', '1', '73210', '0', '0', '0', '0', '0', '0');

-- ----------------------------
-- Table structure for `teleport_locations`
-- ----------------------------
DROP TABLE IF EXISTS `teleport_locations`;
CREATE TABLE `teleport_locations` (
  `location` varchar(255) NOT NULL,
  `x` float NOT NULL,
  `y` float NOT NULL,
  `z` float NOT NULL,
  `o` float NOT NULL,
  `map` int(10) unsigned NOT NULL,
  PRIMARY KEY (`location`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of teleport_locations
-- ----------------------------
