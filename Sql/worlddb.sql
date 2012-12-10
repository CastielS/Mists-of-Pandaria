/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50519
Source Host           : localhost:3306
Source Database       : world

Target Server Type    : MYSQL
Target Server Version : 50519
File Encoding         : 65001

Date: 2012-12-10 06:13:54
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `creature_data`
-- ----------------------------
DROP TABLE IF EXISTS `creature_data`;
CREATE TABLE `creature_data` (
  `id` int(10) unsigned NOT NULL,
  `name` varchar(100) DEFAULT NULL,
  `subName` varchar(100) DEFAULT NULL,
  `iconName` varchar(100) DEFAULT NULL,
  `health` int(10) unsigned NOT NULL,
  `level` tinyint(3) unsigned NOT NULL,
  `displayId` int(10) unsigned NOT NULL,
  `class` int(11) NOT NULL,
  `faction` int(11) unsigned NOT NULL,
  `scale` float NOT NULL,
  `type` int(10) unsigned NOT NULL,
  `flags` int(10) unsigned NOT NULL,
  `flags2` int(11) NOT NULL,
  `npcFlags` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of creature_data
-- ----------------------------

-- ----------------------------
-- Table structure for `creature_spawns`
-- ----------------------------
DROP TABLE IF EXISTS `creature_spawns`;
CREATE TABLE `creature_spawns` (
  `guid` bigint(20) NOT NULL AUTO_INCREMENT,
  `id` int(10) unsigned NOT NULL,
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
