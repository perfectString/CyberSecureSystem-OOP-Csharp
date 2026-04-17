using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Repositories;
using CyberSecurityDS.Repositories.Contracts;

namespace CyberSecurityDS.Models
{
    public class SystemManager : ISystemManager
    {
        private IRepository<ICyberAttack> _cyberAttackRepository;
        private IRepository<IDefensiveSoftware> _defenseSoftwareRepository;

        public SystemManager()
        {
            _cyberAttackRepository = new CyberAttackRepository();
            _defenseSoftwareRepository = new DefensiveSoftwareRepository();
        }

        public IRepository<ICyberAttack> CyberAttacks
        {
            get => _cyberAttackRepository;
        }

        public IRepository<IDefensiveSoftware> DefensiveSoftwares
        {
            get => _defenseSoftwareRepository;
        }
    }
}
