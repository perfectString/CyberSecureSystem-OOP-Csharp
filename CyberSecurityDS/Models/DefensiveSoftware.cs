using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberSecurityDS.Models.Contracts;
using CyberSecurityDS.Utilities.Messages;

namespace CyberSecurityDS.Models
{
    public abstract class DefensiveSoftware : IDefensiveSoftware
    {
        private string _name;
        private int _effectiveness;
        private readonly List<string> _assignedAttacks;

        protected DefensiveSoftware(string name, int effectiveness)
        {
            Name = name;
            Effectiveness = effectiveness;
            _assignedAttacks = new List<string>();

        }

        public string Name
        {
            get { return _name; }

           private set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.SoftwareNameRequired);
                }

                _name = value;
            }
        }

        public int Effectiveness
        {
            get { return _effectiveness; }

            private set
            {
                if (value == 0)
                {
                    _effectiveness = 1;
                }
                else if (value > 10)
                {

                    _effectiveness = 10;

                }
                else if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.EffectivenessNegative);
                }
                else
                {

                    _effectiveness = value;
                }
            }
        }

        public IReadOnlyCollection<string> AssignedAttacks => _assignedAttacks.AsReadOnly();

        public void AssignAttack(string attackName)
        {
            _assignedAttacks.Add(attackName);
        }

        public override string ToString()
        {
            string attacksDisplay = _assignedAttacks.Any()
                ? string.Join(", ", _assignedAttacks)
                : "[None]";

            return $"Defensive Software: {Name}, Effectiveness: {Effectiveness}, Assigned Attacks: {attacksDisplay}";
        }

    }
}
