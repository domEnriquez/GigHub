using NUnit.Framework;
using System;
using System.Transactions;

namespace GigHub.IntegrationTests
{
    public class Isolated : Attribute, ITestAction
    {
        private TransactionScope transactionScope;

        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }

        public void BeforeTest(TestDetails testDetails)
        {
            transactionScope = new TransactionScope();
        }

        public void AfterTest(TestDetails testDetails)
        {
            transactionScope.Dispose();
        }

    }
}
