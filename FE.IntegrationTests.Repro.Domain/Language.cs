using FE.IntegrationTests.Repro.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FE.IntegrationTests.Repro.Domain
{
    public class Language : Entity
    {
        public string Name { get; protected set; }
        
        public string Culture { get; protected set; }
        public Language(string name, string culture)
        {
            Name = name;
            Culture = culture;
        }
    }
}
