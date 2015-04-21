﻿using MystiflyOnePointAPI.OnePointService;

namespace MystiflyOnePointAPI.Handler
{
    public class ClientHandler : OnePointClient
    {
        private static string _accountNumber;
        private static string _userName;
        private static string _password;
        private static Target _target;
        public string SessionId;

        public ClientHandler()
        {
            CreateSession();
        }

        public static void Init(string accountNumber, string userName, string password, Target target)
        {
            _accountNumber = accountNumber;
            _userName = userName;
            _password = password;
            _target = target;
        }

        public void CreateSession()
        {
            var request = new SessionCreateRQ()
            {
                AccountNumber = _accountNumber,
                UserName = _userName,
                Password = _password,
                Target = _target
            };
            var response = CreateSession(request);
            SessionId = response.SessionId;
        }
    }
}