using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemitanoDevTask.Validation;

namespace RemitanoDevTask.UnitTest.Validation
{
    [TestClass()]
    public class NumberValidationTests
    {
        private readonly NumberValidation _validation;

        public NumberValidationTests()
        {
            _validation = new NumberValidation();
        }


        [TestMethod()]
        public void IsValid_ValidNumber_ReturnsTrue()
        {
            Assert.IsTrue(_validation.IsValid("123-4543234576-23"));
        }

        [DataTestMethod]
        [DataRow("2321-868726332-01")]
        [DataRow("2321-868726332-01")]
        [DataRow("1273-2848723627-01")]
        public void IsValid_NumberFirstPartWrong_ReturnsFalse(string accountNumber)
        {
            Assert.IsFalse(_validation.IsValid(accountNumber));
        }

        [DataTestMethod]
        [DataRow("2321-868726332-0")]
        [DataRow("1273-284872367527-01")]
        public void IsValid_NumberMiddlePartWrong_ReturnsFalse(string accNumber)
        {
            Assert.IsFalse(_validation.IsValid(accNumber));
        }

        [DataTestMethod]
        [DataRow("2321-868726332-4")]
        [DataRow("12273-28486277-01")]
        public void IsValid_NumberLastPartWrong_ReturnsFalse(string accNumber)
        {
            Assert.IsFalse(_validation.IsValid(accNumber));
        }

    }
}