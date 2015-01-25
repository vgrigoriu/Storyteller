﻿using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Storyteller.Core.Model;
using Storyteller.Core.Testing.Grammars.Sets;

namespace Storyteller.Core.Testing.Grammars.Paragraphs
{
    [TestFixture]
    public class VerifyPropertiesOf_specs : SpecRunningContext
    {
        [Test]
        public void execute()
        {
            AddressVerificationFixture.Address.City = "Austin";
            AddressVerificationFixture.Address.PostalCode = "78750";
            AddressVerificationFixture.Address.StateOrProvince = "Texas";

            execute(@"
=> AddressVerification
* VerifyAddress#1: City=Jasper, StateOrProvince=Missouri, PostalCode=78750
* VerifyAddress#2: City=Austin, StateOrProvince=Texas, PostalCode=64755
");

            CountsShouldBe(3, 3, 0, 0);

            Step("1").Cell("City").FailedWithActual("Austin");
            Step("1").Cell("StateOrProvince").FailedWithActual("Texas");
            Step("1").Cell("PostalCode").Succeeded();

            Step("2").Cell("City").Succeeded();
            Step("2").Cell("StateOrProvince").Succeeded();
            Step("2").Cell("PostalCode").FailedWithActual("78750");

        }

        [Test]
        public void spot_check_the_model()
        {
            var paragraph = ModelFor<Paragraph>("AddressVerification", "VerifyAddress");

            paragraph.title.ShouldEqual("The current address");
            paragraph.children.ShouldHaveCount(3);
            paragraph.children[0].ShouldBeOfType<Sentence>().cells.Single().Key.ShouldEqual("City");
            paragraph.children[1].ShouldBeOfType<Sentence>().cells.Single().Key.ShouldEqual("PostalCode");
            paragraph.children[2].ShouldBeOfType<Sentence>().cells.Single().Key.ShouldEqual("StateOrProvince");
        }
    }

    public class AddressVerificationFixture : Fixture
    {
        public static Address Address = new Address();

        public override void SetUp()
        {
            Context.State.CurrentObject = Address;
        }

        public IGrammar VerifyAddress()
        {
            return VerifyPropertiesOf<Address>("The current address", _ =>
            {
                _.Check(x => x.City);
                _.Check(x => x.PostalCode);
                _.Check(x => x.StateOrProvince);
            });
        }
    }
}