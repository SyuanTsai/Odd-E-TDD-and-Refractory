using NSubstitute;

namespace BudgetSystem.UnitTest;

public class BudgetSystemUnitTest
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void Query_Month()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202103", Amount = 31}
            });

          var cal=  new BudgetCalculator(repo);
          var query = cal.Query(new DateTime(2021,3,1), new DateTime(2021, 3, 31));
          Assert.That(query, Is.EqualTo(31));
        }
      
        [Test]
        public void Query_One_Day()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202103", Amount = 310}
            });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 3, 1), new DateTime(2021, 3, 1));
            Assert.That(query, Is.EqualTo(10));
        }
        [Test]
        public void Query_No_Data()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202102", Amount = 310}
            });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 3, 1), new DateTime(2021, 3, 1));
            Assert.That(query, Is.EqualTo(0));
        }
        [Test]
        public void Query_Illegal_DateTime()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202103", Amount = 310}
            });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 3, 1), new DateTime(2021, 2, 1));
            Assert.That(query, Is.EqualTo(0));
        }

        [Test]
        public void Query_Cross_Month()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202102", Amount = 28},
                new Budget {YearMonth = "202103", Amount = 310}
            });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2021, 3, 3));
            Assert.That(query, Is.EqualTo(32));
        }
        [Test]
        public void Query_Cross_Two_Months()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202102", Amount = 28},
                new Budget {YearMonth = "202103", Amount = 310},
                new Budget {YearMonth = "202104", Amount = 3000}
            });
            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2021, 4, 3));
            Assert.That(query, Is.EqualTo(612));
        }
        [Test]
        public void Query_Cross_Two_Months_WithNoDATA()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202102", Amount = 28},
               // new Budget {YearMonth = "202103", Amount = 310},
                new Budget {YearMonth = "202104", Amount = 3000}
            });
            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2021, 4, 3));
            Assert.That(query, Is.EqualTo(302));
        }
        [Test]
        public void Query_Cross_Year()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202102", Amount = 28},
                // new Budget {YearMonth = "202103", Amount = 310},
                new Budget {YearMonth = "202204", Amount = 3000}
            });
            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2022, 4, 1));
            Assert.That(query, Is.EqualTo(102));
        }
    }
}