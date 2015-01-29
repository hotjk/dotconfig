using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Settings.Model;

namespace Settings.Repository.MySql
{
    public class SqlRepository : BaseRepository, ISqlRepository
    {
        public SqlRepository(SqlOption option) : base(option) { }

        public bool InitDatabase()
        {
            using (IDbConnection connection = OpenConnection())
            {
                try
                {
                    connection.Execute("SELECT 1 FROM settings_user LIMIT 1;");
                    return true;
                }
                catch
                {
                }
                connection.Execute(
    @"

CREATE TABLE `settings_tree` (
  `Tree` int(11) NOT NULL,
  `Id` int(11) NOT NULL,
  `Parent` int(11) DEFAULT NULL,
  `Data` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`,`Tree`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `settings_client` (
  `ClientId` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `PublicKey` varchar(2000) NOT NULL,
  `Version` int(11) NOT NULL,
  `Deleted` int(11) NOT NULL DEFAULT '0',
  `CreateAt` datetime NOT NULL,
  `UpdateAt` datetime NOT NULL,
  `DeleteAt` datetime DEFAULT NULL,
  PRIMARY KEY (`ClientId`),
  UNIQUE KEY `ix_settings_client_name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `settings_node` (
  `NodeId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Version` int(11) NOT NULL,
  `CreateAt` datetime NOT NULL,
  `UpdateAt` datetime NOT NULL,
  `Deleted` int(11) NOT NULL DEFAULT '0',
  `DeleteAt` datetime DEFAULT NULL,
  PRIMARY KEY (`NodeId`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

CREATE TABLE `settings_entry` (
  `NodeId` int(11) NOT NULL,
  `Key` varchar(100) NOT NULL,
  `Value` varchar(4000) NOT NULL,
  PRIMARY KEY (`NodeId`,`Key`),
  CONSTRAINT `fk_settings_entry_node` FOREIGN KEY (`NodeId`) REFERENCES `settings_node` (`NodeId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `settings_client_node` (
  `ClientId` int(11) NOT NULL,
  `NodeId` int(11) NOT NULL,
  PRIMARY KEY (`ClientId`,`NodeId`),
  KEY `fk_settings_client_node_client_idx` (`NodeId`),
  KEY `fk_settings_client_node_node_idx` (`ClientId`),
  CONSTRAINT `fk_settings_client_node_client` FOREIGN KEY (`ClientId`) REFERENCES `settings_client` (`ClientId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_settings_client_node_node` FOREIGN KEY (`NodeId`) REFERENCES `settings_node` (`NodeId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `settings_user` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(100) NOT NULL,
  `Version` int(11) NOT NULL,
  `Deleted` int(11) NOT NULL DEFAULT '0',
  `CreateAt` datetime NOT NULL,
  `UpdateAt` datetime NOT NULL,
  `DeleteAt` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `idx_settings_user_username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

INSERT INTO `settings_user` VALUES (1,'admin','1000:LBtm29kdYeO/yj627CzHnp4otNRBjyZ8:ElN/ssx1YsFjfaB/n+u5iEna/pTQGhdw',2,0,'2015-01-18 16:52:59','2015-01-18 16:52:59',NULL);

CREATE TABLE IF NOT EXISTS `sequence` (
  `Id` int(11) NOT NULL,
  `Value` int(11) NOT NULL DEFAULT '0',
  `Comment` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

INSERT INTO `sequence` VALUES (1000,1,'Settings Node'),(1001,1,'Settings Client'),(1002,1,'Settings User');

");
                return true;
            }
        }
    }
}

