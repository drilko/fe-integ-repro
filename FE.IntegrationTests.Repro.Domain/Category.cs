using FE.IntegrationTests.Repro.Domain.Common;

namespace FE.IntegrationTests.Repro.Domain
{
    public class Category : Entity
    {
        public string Name { get; protected set; }
        public string Icon { get; protected set; }        
        public int Order { get; protected set; }
        public Category(string name, string icon, int order)
        {
            Name = name;
            Icon = icon;
            Order = order;
        }
    }
}
