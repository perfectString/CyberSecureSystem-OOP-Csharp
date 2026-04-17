using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Repositories.Contracts;

namespace CyberSecurityDS.Repositories
{
    public class DefensiveSoftwareRepository : IRepository<IDefensiveSoftware>
    {
        private readonly List<IDefensiveSoftware> _defensiveSoftwareList;

        public DefensiveSoftwareRepository()
        {
            _defensiveSoftwareList = new List<IDefensiveSoftware>();
        }

        public IReadOnlyCollection<IDefensiveSoftware> Models { get => _defensiveSoftwareList.AsReadOnly(); }

        public void AddNew(IDefensiveSoftware model)
        {
            _defensiveSoftwareList.Add(model);
        }

        public bool Exists(string name)
        {
            return _defensiveSoftwareList.Any(d => d.Name == name);
        }

        public IDefensiveSoftware GetByName(string name)
        {
            if (Exists(name))
            {
                return _defensiveSoftwareList.FirstOrDefault(d => d.Name == name);
            }
            return null;

        }
    }
}
