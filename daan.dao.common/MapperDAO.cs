using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBatisNet.DataMapper;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Configuration;
using System.Reflection;
using System.IO;
using System.Xml;

namespace daan.dao.common
{
    public class MapperDAO
    {

        private static volatile ISqlMapper _mapper = null;
        public static string path = "daan.sqlserver.config";

        private static readonly string key = "daanhealth2011";
        private static readonly string datakeyfile = "prop.config";

        protected static void Configure(object obj)
        {
            _mapper = null;
        }

        protected static void InitMapper()
        {
            ConfigureHandler handler = new ConfigureHandler(Configure);
            DomSqlMapBuilder builder = new DomSqlMapBuilder();

            XmlDocument doc = GetConfig();

            _mapper = builder.Configure(doc);
            //_mapper = builder.ConfigureAndWatch(path, handler);
        }

        private static XmlDocument GetConfig()
        { 
            string uid = string.Empty;
            string pwd = string.Empty;
            string db = string.Empty;
            string source = string.Empty;
            XmlDocument doc = Resources.GetResourceAsXmlDocument(datakeyfile);
            XmlNodeList nodeList = doc.SelectNodes("settings/add");
            int n = nodeList.Count;
            if (n >= 3)
            {
                for (int i = 0; i < n; i++)
                {
                    switch (nodeList[i].Attributes["key"].Value.ToLower())
                    {
                        case "userid":
                            uid = nodeList[i].Attributes["value"].Value;
                            break;
                        case "password":
                            pwd = nodeList[i].Attributes["value"].Value;
                            break;
                        case "database":
                            db = nodeList[i].Attributes["value"].Value;
                            break;
                        case "datasource":
                            source = nodeList[i].Attributes["value"].Value;
                            break;
                    }
                }
            }

            DESEncrypt des = new DESEncrypt();
            //if (!string.IsNullOrEmpty(uid))
            //    uid = des.Decrypt(uid, key);

            //if (!string.IsNullOrEmpty(pwd))
            //    pwd = des.Decrypt(pwd, key);

            //if (!string.IsNullOrEmpty(db))
            //    db = des.Decrypt(db, key);

            //if (!string.IsNullOrEmpty(source))
            //    source = des.Decrypt(source, key);

            XmlDocument configDoc = Resources.GetResourceAsXmlDocument(path);
            configDoc.InnerXml = configDoc.InnerXml.Replace("${datasource}", source).Replace("${userid}", uid).Replace("${password}", pwd);

            return configDoc;
        }


        public static ISqlMapper NewMapper()
        {
            DomSqlMapBuilder builder = new DomSqlMapBuilder();

            XmlDocument doc = GetConfig();

            return builder.Configure(doc);
        }

        public static ISqlMapper NewMapper(String _path)
        {
            DomSqlMapBuilder builder = new DomSqlMapBuilder();

            return builder.Configure(_path);
        }


        public static ISqlMapper Instance()
        {
            if (_mapper == null)
            {
                //TODO, should be thread static
                lock (typeof(SqlMapper))
                {
                    if (_mapper == null) // double-check
                    {
                        InitMapper();
                    }
                }
            }
            return _mapper;
        }


        public static ISqlMapper Get()
        {
            return Instance();
           // return NewMapper();
        }


        
    }
}
