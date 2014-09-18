using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Lunggo.Framework.I18nMessage
{
    public class MessageManager
    {
        private const String MessageXmlUrl = @"i18n_message.xml";
        private const String NotFoundMessage = @"Message is not found";
        private static readonly MessageManager Instance = new MessageManager();
        private Dictionary<string, I18NMessage> _messageDictionary;
        private bool _isInitialized;

        private MessageManager()
        {

        }

        public static MessageManager GetInstance()
        {
            return Instance;
        }

        public void Init(string directoryPath)
        {
            if (!_isInitialized)
            {
                _messageDictionary = LoadMessageFileToDictionary(Path.Combine(directoryPath, MessageXmlUrl));
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("Message Manager is already initialized");
            }
        }
        
        public string GetMessageValue(string messageCode, string cultureCode)
        {
            try
            {
                var messageValue = _messageDictionary[messageCode];
                return messageValue.Value[cultureCode];
            }
            catch (KeyNotFoundException ex)
            {
                return NotFoundMessage;
            }
        }

        private Dictionary<string, I18NMessage> LoadMessageFileToDictionary(String filePath)
        {
            var messageDoc = LoadMessageFile(filePath);
            return MessagesToDictionary(messageDoc);   
        }
        

        private XElement LoadMessageFile(String filePath)
        {
            return XElement.Load(filePath);
        }
        
        private Dictionary<string, I18NMessage> MessagesToDictionary(XElement doc)
        {
            var messageNodes = doc.Elements();
            var messageDictionary = messageNodes.
                GroupBy
                (
                    n => n.Attribute("code").Value
                ).
                ToDictionary
                (
                    @group => @group.Key.ToString(CultureInfo.InvariantCulture), 
                    @group => new I18NMessage
                    {
                        Value = @group.ToDictionary(n => n.Attribute("lang").Value, n => n.Attribute("value").Value)
                    }
                );
            return messageDictionary;
        }

        private class I18NMessage
        {
            public Dictionary<string,string> Value { get; set;}
        }
    }
}
