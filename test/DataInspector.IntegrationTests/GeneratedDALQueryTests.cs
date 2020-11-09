using System;
using FluentAssertions;
using Sample.Domain;
using Sample.Domain.DAL;
using Xunit;

namespace DataInspector.IntegrationTests {
    public class GeneratedDALQueryTests {
        [Theory]
        [InlineData("DerpChild.ReferentialGlorp.Name", "derp_refGlorp")]
        [InlineData("RootNodeId", "root_100")]
        [InlineData("GlorpChild.Name", "derp_baseGlorp")]
        public void WhenQueryingWithDirectPropertiesAndStringTypes_ThenResultIsReturned(string query, string expectedValue) {
            // Arrange
            var model = GenerateTestObject01();
            var dal = new Sample_Domain_Root();

            // Act
            var exprResults = dal.FetchValue(model, query) as string;

            // Assert
            exprResults.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("DerpChild.AllTheFlerbs[0].FlerbId", "flerb_100")]
        [InlineData("DerpChild.AllTheFlerbs[1].FlerbId", "flerb_101")]        
        public void WhenQueryingWithArrayPropertiesAndStringTypes_ThenResultIsReturned(string query, string expectedValue) {
            // Arrange
            var model = GenerateTestObject01();
            var dal = new Sample_Domain_Root();

            // Act
            var exprResults = dal.FetchValue(model, query) as string;

            // Assert
            exprResults.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("DerpChild.AllTheFlerbs[0].SubIds[0]", 1)]
        [InlineData("DerpChild.AllTheFlerbs[0].SubIds[3]", 4)]
        public void WhenQueryingWithArrayPropertiesAndIntegerTypes_ThenResultIsReturned(string query, int expectedValue) {
            // Arrange
            var model = GenerateTestObject01();
            var dal = new Sample_Domain_Root();

            // Act
            var exprResults = (int)dal.FetchValue(model, query);

            // Assert
            exprResults.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("DerpChild.ReferentialGlorp.ValueF", 3.14159f)]
        public void WhenQueryingWithArrayPropertiesAndFloatTypes_ThenResultIsReturned(string query, float expectedValue) {
            // Arrange
            var model = GenerateTestObject01();
            var dal = new Sample_Domain_Root();

            // Act
            var exprResults = (float)dal.FetchValue(model, query);

            // Assert
            exprResults.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("DerpChild.ReferentialGlorp.IsEnabled", true)]
        [InlineData("GlorpChild.IsEnabled", false)]
        public void WhenQueryingWithArrayPropertiesAndBooleanTypes_ThenResultIsReturned(string query, bool expectedValue) {
            // Arrange
            var model = GenerateTestObject01();
            var dal = new Sample_Domain_Root();

            // Act
            var exprResults = (bool)dal.FetchValue(model, query);

            // Assert
            exprResults.Should().Be(expectedValue);
        }

        [Theory]        
        [InlineData("DerpChild.AllTheFlerbs[1].SubIds[0]")]
        public void WhenQueryingWitArrayPropertiesAndInvalidIndices_ThenExceptionIsThrown(string query) {
            // Arrange
            var model = GenerateTestObject01();
            var dal = new Sample_Domain_Root();

            // Act
            Action a = () => dal.FetchValue(model, query);

            // Assert
            a.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("DerpChild.AllTheFlerbs[0]")]
        [InlineData("DerpChild.AllTheFlerbs[0].SubIds")]
        public void WhenQueryingWitArrayPropertiesAndNoSubProperties_ThenNullIsReturned(string query) {
            // Arrange
            var model = GenerateTestObject01();
            var dal = new Sample_Domain_Root();

            // Act
            var result = dal.FetchValue(model, query);

            // Assert
            result.Should().BeNull();
        }

        private static Root GenerateTestObject01() {
            var r = new Root() {
                DerpChild = new Derp() {
                    Id = 1000,
                    Uuid = "some uuid",
                    AllTheFlerbs = new Flerb[] {
                        new Flerb() {
                            FlerbId = "flerb_100",
                            SubIds = new int[] { 1, 2, 3, 4 }
                        },
                        new Flerb() {
                            FlerbId = "flerb_101"
                            // No SubIds
                        }
                    },
                    ReferentialGlorp = new Glorp() {
                        IsEnabled = true,
                        Name = "derp_refGlorp",
                        ValueF = 3.14159f
                    }
                },
                GlorpChild = new Glorp() {
                    IsEnabled = false,
                    Name = "derp_baseGlorp",
                    ValueF = 1.1f
                },
                RootNodeId = "root_100"
            };

            return r;
        }
    }
}
