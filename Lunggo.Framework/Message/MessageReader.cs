using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lunggo.Framework.Message
{
    public class MessageReader
    {
        private const String MessageXmlUrl = @"Message.xml";
        private static readonly MessageReader Instance = new MessageReader();
        private Dictionary<string, CultureAndValue> _messageDictionary;
        private bool _isInitialized;

        private MessageReader()
        {

        }

        public static MessageReader GetInstance()
        {
            return Instance;
        }

        public void Init(string directoryPath)
        {
            if (!_isInitialized)
            {
                _messageDictionary = LoadConfigFileToDictionary(Path.Combine(directoryPath, MessageXmlUrl));
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("Message Manager is already initialized");
            }
        }
        
        public string GetMessageValue(string MessageCode, string CultureCode)
        {
            var messageValue = _messageDictionary[MessageCode];
            if (messageValue != null)
            {
                return messageValue.Dictionary[CultureCode];
            }
            else
            {
                throw new ArgumentException(String.Format("Message Code {0} does not exist", MessageCode));
            }
        }

        private Dictionary<string, CultureAndValue> LoadConfigFileToDictionary(String filePath)
        {
            XElement configDoc = LoadConfigurationFile(filePath);
            return ConfigToDictionary(configDoc);   
        }
        

        private XElement LoadConfigurationFile(String filePath)
        {
            return XElement.Load(filePath);
        }
        
        private Dictionary<string, CultureAndValue> ConfigToDictionary(XElement doc)
        {
            var configNodes = doc.Elements();
            var configDictionary = configNodes.
                GroupBy
                (
                    n => n.Attribute("code").Value
                ).
                ToDictionary
                (
                    @group => @group.Key.ToString(CultureInfo.InvariantCulture), 
                    @group => new CultureAndValue
                    {
                        Dictionary = @group.ToDictionary(n => n.Attribute("lang").Value, n => n.Attribute("value").Value)
                    }
                );
            return configDictionary;
        }

        private class CultureAndValue
        {
            public Dictionary<string,string> Dictionary { get; set;}
        }
    }
}
