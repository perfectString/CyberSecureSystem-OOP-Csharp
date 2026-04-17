using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberSecurityDS.Core.Contracts;
using CyberSecurityDS.Models;
using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Utilities.Messages;

namespace CyberSecurityDS.Core
{
    public class Controller : IController
    {
        private SystemManager systemManager;
        private readonly string[] validAttacks = new[]
        {
            nameof(PhishingAttack), nameof(MalwareAttack)
        };

        private readonly string[] defenseTypes = new[]
        {
            nameof(Firewall), nameof(Antivirus)
        };

        public Controller()
        {
            systemManager = new SystemManager();
        }

        public string AddCyberAttack(string attackType, string attackName, int severityLevel, string extraParam)
        {
            if (!validAttacks.Contains(attackType))
            {
                return string.Format(OutputMessages.TypeInvalid, attackType);
            }

            if (systemManager.CyberAttacks.Exists(attackName))
            {
                return string.Format(OutputMessages.EntryAlreadyExists, attackName);
            }

            ICyberAttack newAttack = null;

            if (attackType == nameof(PhishingAttack))
            {
                newAttack = new PhishingAttack(attackName, severityLevel, extraParam);
            }
            else if (attackType == nameof(MalwareAttack))
            {
                newAttack = new MalwareAttack(attackName, severityLevel, extraParam);
            }
            systemManager.CyberAttacks.AddNew(newAttack);

            return string.Format(OutputMessages.EntryAddedSuccessfully, attackType, attackName);
        }

        public string AddDefensiveSoftware(string softwareType, string softwareName, int effectiveness)
        {
            if (!defenseTypes.Contains(softwareType))
            {
                return string.Format(OutputMessages.TypeInvalid, softwareType);
            }

            if (systemManager.DefensiveSoftwares.Exists(softwareName))
            {
                return string.Format(OutputMessages.EntryAlreadyExists, softwareName);
            }

            IDefensiveSoftware newDefensiveSoftware = null;
            if (softwareType == nameof(Antivirus))
            {
                newDefensiveSoftware = new Antivirus(softwareName, effectiveness);
            }
            else if (softwareType == nameof(Firewall))
            {
                newDefensiveSoftware = new Firewall(softwareName, effectiveness);
            }

            systemManager.DefensiveSoftwares.AddNew(newDefensiveSoftware);
            return string.Format(OutputMessages.EntryAddedSuccessfully, softwareType, softwareName);

        }

        public string AssignDefense(string cyberAttackName, string defensiveSoftwareName)
        {
            if (systemManager.CyberAttacks.GetByName(cyberAttackName) == null)
            {
                return string.Format(OutputMessages.EntryNotFound, cyberAttackName);
            }

            if (systemManager.DefensiveSoftwares.GetByName(defensiveSoftwareName) == null)
            {
                return string.Format(OutputMessages.EntryNotFound, defensiveSoftwareName);
            }

            var defense = systemManager.DefensiveSoftwares.GetByName(defensiveSoftwareName);
            
            bool attackExist = false;
            string assignedSoftware = string.Empty;

            foreach
            (var defenseSystem in systemManager.DefensiveSoftwares.Models) // ne znam dali logikata ti e vqrna
            {
                if (defenseSystem.AssignedAttacks.Contains(cyberAttackName))
                {
                    assignedSoftware = defenseSystem.Name;
                    attackExist = true;
                    break;
                }
            }

            if (attackExist)
            {
                return string.Format(OutputMessages.AttackAlreadyAssigned, cyberAttackName, assignedSoftware);
            }

            defense.AssignAttack(cyberAttackName);

            return string.Format(OutputMessages.AttackAssignedSuccessfully, cyberAttackName, defensiveSoftwareName);

        }

        public string GenerateReport()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Security:");

            List<IDefensiveSoftware> defenses = systemManager.DefensiveSoftwares.Models.OrderBy(x => x.Name).ToList();
            foreach (var defense in defenses)
            {
                sb.AppendLine(defense.ToString());
            }
            sb.AppendLine($"Threads:");
            sb.AppendLine($"-Mitigated:");

            List<ICyberAttack> attacksMitigated = systemManager.CyberAttacks.Models.Where(a => a.Status == true).OrderBy(a => a.AttackName).ToList();
            List<ICyberAttack> pending = systemManager.CyberAttacks.Models.Where(a => a.Status == false).OrderBy(a => a.AttackName).ToList();

            foreach (var mittigatedAttack in attacksMitigated)
            {
                sb.AppendLine(mittigatedAttack.ToString());
            }
            
            sb.AppendLine($"-Pending:");
            
            foreach (var attacksPending in pending)
            {
                sb.AppendLine(attacksPending.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        public string MitigateAttack(string cyberAttackName)
        {
            if (systemManager.CyberAttacks.GetByName(cyberAttackName) == null)
            {
                return string.Format(OutputMessages.EntryNotFound, cyberAttackName);
            }

            var cyberAttack = systemManager.CyberAttacks.GetByName(cyberAttackName);

            if (cyberAttack.Status == true)
            {
                return string.Format(OutputMessages.AttackAlreadyMitigated, cyberAttackName);
            }

            bool IsAssigned = false;
            var defenseSoftwareName = string.Empty;
            IDefensiveSoftware defenseType = null;
            foreach (var defense in systemManager.DefensiveSoftwares.Models)
            {
                if (defense.AssignedAttacks.Contains(cyberAttackName))
                {

                    defenseSoftwareName = defense.Name;
                    defenseType = defense;
                    IsAssigned = true;
                    break;
                }
            }

            if (!IsAssigned)
            {
                return string.Format(OutputMessages.AttackNotAssignedYet, cyberAttackName);
            }

            //if (defenseSoftwareName.GetType().Name == "Firewall" && cyberAttackName.GetType().Name != "MalwareAttack")
            if (defenseType is Firewall && !(cyberAttack is MalwareAttack))
            {
                return string.Format(OutputMessages.CannotMitigateDueToCompatibility, nameof(Firewall), nameof(PhishingAttack));
            }
            //if (defenseSoftwareName.GetType().Name == "Antivirus" && cyberAttackName.GetType().Name != "PhishingAttack")
            if (defenseType is Antivirus && !(cyberAttack is PhishingAttack))
            {
                return string.Format(OutputMessages.CannotMitigateDueToCompatibility, nameof(Antivirus), nameof(MalwareAttack));
            }

            var severity = cyberAttack.SeverityLevel;
            int effectivness = defenseType.Effectiveness;

            if (effectivness >= severity)
            {
                cyberAttack.MarkAsMitigated();

                return string.Format(OutputMessages.AttackMitigatedSuccessfully, cyberAttackName);
            }

            return string.Format(OutputMessages.SoftwareNotEffectiveEnough, cyberAttackName, defenseSoftwareName);
        }
    }
}
