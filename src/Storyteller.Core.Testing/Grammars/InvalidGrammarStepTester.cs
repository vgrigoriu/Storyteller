﻿using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using Storyteller.Core.Conversion;
using Storyteller.Core.Engine;
using Storyteller.Core.Grammars;
using Storyteller.Core.Results;

namespace Storyteller.Core.Testing.Grammars
{
    [TestFixture]
    public class InvalidGrammarStepTester
    {
        [Test]
        public void accept_visitor()
        {
            var executor = MockRepository.GenerateMock<ISpecExecutor>();

            var step = new InvalidGrammarStep(new StepValues("foo"), "grammar is wonky");

            step.AcceptVisitor(executor);

            executor.AssertWasCalled(x => x.Line(step));
        }

        [Test]
        public void execute_logs_a_step_result_error()
        {
            var context = SpecContext.ForTesting();

            var step = new InvalidGrammarStep(new StepValues("the id"), "grammar is wonky");

            step.Execute(context);

            var result = context.Results.Single().ShouldBeOfType<StepResult>();

            result.id.ShouldEqual("the id");
            result.status.ShouldEqual(ResultStatus.error);
            result.error.ShouldEqual("grammar is wonky");
        }
    }
}