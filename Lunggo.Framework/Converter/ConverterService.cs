using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codaxy.WkHtmlToPdf;

namespace Lunggo.Framework.Converter
{
    public class ConverterService
    {
        private static readonly ConverterService Instance = new ConverterService();
        private bool _isInitialized;

        private ConverterService()
        {
            
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("ConverterService is already initialized");
            }
        }

        public static ConverterService GetInstance()
        {
            return Instance;
        }
    }
}
