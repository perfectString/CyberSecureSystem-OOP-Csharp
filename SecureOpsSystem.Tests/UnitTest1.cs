using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace SecureOpsSystem.Tests
{
    [TestFixture]
    public class SecureHubTests
    {
        [Test]
        public void ConstructorWorksCorrectlyCapacityWorks()
        {
            SecureHub hub = new SecureHub(5);

            Assert.IsNotNull(hub);
            Assert.That(hub.Capacity, Is.EqualTo(5));
            Assert.IsNotNull(hub.Capacity);
            
            Assert.IsEmpty(hub.Tools);

        }
        [TestCase(-1), TestCase(-100)]
        public void CapacityTgorwsExceptionIfNegative(int invalid)
        {
            Assert.Throws<ArgumentException>(() => new SecureHub(invalid));


        }
        [Test]
        public void AddingDuplicateNameThrowsException()
        {
            SecureHub hub = new SecureHub(2);

            SecurityTool tool1 = new SecurityTool("antivirus", "AntiVirusProgram", 5);

            hub.AddTool(tool1);
            SecurityTool tool2 = new SecurityTool("antivirus", "AntiVirusProgram", 1);

            string expected = $"Security Tool antivirus already exists in the hub.";
            var actual = hub.AddTool(tool2);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void AddingInFullCollectionThrowsException()
        {
            SecureHub hub = new SecureHub(1);

            SecurityTool tool1 = new SecurityTool("antivirus", "AntiVirusProgram", 5);

            hub.AddTool(tool1);
            SecurityTool tool2 = new SecurityTool("antiSoft", "program", 1);

            string expected = "Secure Hub is at full capacity.";
            var actual = hub.AddTool(tool2);
            Assert.AreEqual(expected, actual);

        }
        [Test]
        public void AddingShouldWork()
        {
            SecureHub hub = new SecureHub(3);

            SecurityTool tool1 = new SecurityTool("antivirus", "AntiVirusProgram", 5);

            hub.AddTool(tool1);
            Assert.IsNotEmpty(hub.Tools);
            Assert.That(hub.Capacity, Is.EqualTo(3));
            Assert.That(hub.Tools.Count, Is.EqualTo(1));


            SecurityTool tool2 = new SecurityTool("antiSoft", "program", 1);

            hub.AddTool(tool2);

            Assert.That(hub.Tools.Count, Is.EqualTo(2));

            SecurityTool tool3 = new SecurityTool("anti", "Soft", 2);
            var actual = hub.AddTool(tool3);
            string expected = $"Security Tool anti added successfully.";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemoveShouldWork()
        {
            SecureHub hub = new SecureHub(3);

            SecurityTool tool1 = new SecurityTool("antivirus", "AntiVirusProgram", 5);

            hub.AddTool(tool1);


            SecurityTool tool2 = new SecurityTool("antiSoft", "program", 1);

            hub.AddTool(tool2);


            SecurityTool tool3 = new SecurityTool("anti", "Soft", 2);
            hub.AddTool(tool3);

            Assert.IsTrue(hub.RemoveTool(tool1));
            Assert.IsFalse(hub.RemoveTool(tool1));

        }

        [Test]
        public void DeployShouldWork()
        {
            SecureHub hub = new SecureHub(3);

            SecurityTool tool1 = new SecurityTool("antivirus", "AntiVirusProgram", 5);

            hub.AddTool(tool1);


            SecurityTool tool2 = new SecurityTool("antiSoft", "program", 1);

            hub.AddTool(tool2);


            SecurityTool tool3 = new SecurityTool("anti", "Soft", 2);
            hub.AddTool(tool3);

            var deploy = hub.DeployTool("anti");
            Assert.IsNotNull(deploy);
            Assert.That(deploy.Name, Is.EqualTo("anti"));
            Assert.That(hub.Tools.Any(t=> t.Name == "anti"), Is.False);

            Assert.That(hub.Tools.Count, Is.EqualTo(2));

            var fakeDeploy = hub.DeployTool("meow");
            Assert.IsNull(fakeDeploy);
            Assert.That(hub.Tools.Count, Is.EqualTo(2));


        }

        [Test]
        public void SystemReportWorks()
        {
            SecureHub hub = new SecureHub(3);

            SecurityTool tool1 = new SecurityTool("antivirus", "AntiVirusProgram", 5);

            hub.AddTool(tool1);


            

            string expected = hub.SystemReport();

            StringBuilder actual = new StringBuilder();
            actual.AppendLine("Secure Hub Report:");
            actual.AppendLine($"Available Tools: 1");
            actual.AppendLine("Name: antivirus, Category: AntiVirusProgram, Effectiveness: 5.00");

            Assert.AreEqual(expected, actual.ToString().TrimEnd());


        }
    }
}
