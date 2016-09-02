﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slack.Webhooks;

namespace Lunggo.Framework.Log
{
    public partial class LogService
    {
        private class SlackLogClient : LogClient
        {
            private static readonly SlackLogClient ClientInstance = new SlackLogClient();
            private bool _isInitialized;
            private SlackClient _client;

            private SlackLogClient()
            {

            }

            internal static SlackLogClient GetClientInstance()
            {
                return ClientInstance;
            }

            internal override void Init(string webhookUrl)
            {
                if (!_isInitialized)
                {
                    _client = new SlackClient(webhookUrl);
                    _isInitialized = true;
                }
            }

            internal override void Post(string text)
            {
                var msg = new SlackMessage {Text = text};
                _client.Post(msg);
            }
        }

    }
}
