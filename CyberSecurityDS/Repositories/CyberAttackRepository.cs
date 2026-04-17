using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Repositories.Contracts;

namespace CyberSecurityDS.Repositories
{
    public class CyberAttackRepository : IRepository<ICyberAttack>
    {
        private readonly List<ICyberAttack> _cyberAttacks;

        public CyberAttackRepository()
        {
            _cyberAttacks = new List<ICyberAttack>();
        }

        public IReadOnlyCollection<ICyberAttack> Models {  get => _cyberAttacks.AsReadOnly(); }

        public void AddNew(ICyberAttack model)
        {
            _cyberAttacks.Add(model);
        }

        public bool Exists(string name)
        {
            return _cyberAttacks.Any(n=> n.AttackName == name);
        }

        public ICyberAttack GetByName(string name)
        {
            if (Exists(name))
            {
                return _cyberAttacks.FirstOrDefault(n => n.AttackName == name);
            }
            return null;
        }
    }
}
