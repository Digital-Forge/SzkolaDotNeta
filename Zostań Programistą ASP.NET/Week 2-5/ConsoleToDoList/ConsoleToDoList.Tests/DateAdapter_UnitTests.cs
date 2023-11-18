using ConsoleToDoList.App;
using FluentAssertions;
using System;
using Xunit;

namespace ConsoleToDoList.Tests
{
    public class DateAdapter_UnitTests
    {
        [Fact]
        public void EmptyAdapter_NoAction()
        {
            //Arrange
            var x = new DateAdapter();
            //Act
            //Assert
            x.Year.Should().BeNull();
            x.Month.Should().BeNull();
            x.Day.Should().BeNull();
            x.Date.Should().BeNull();
        }

        [Fact]
        public void EmptyAdapter_RightActionDay()
        {
            //Arrange
            var x = new DateAdapter();

            //Act
            x.Day = 3;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
        }

        [Fact]
        public void EmptyAdapter_WrongActionDay()
        {
            //Arrange
            var x = new DateAdapter();

            //Act
            x.Day = 50;

            //Assert
            x.Year.Should().BeNull();
            x.Month.Should().BeNull();
            x.Day.Should().BeNull();
            x.Date.Should().BeNull();
        }

        [Fact]
        public void EmptyAdapter_RightActionMonth()
        {
            //Arrange
            var x = new DateAdapter();

            //Act
            x.Month = 3;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
        }

        [Fact]
        public void EmptyAdapter_WrongActionMonth()
        {
            //Arrange
            var x = new DateAdapter();

            //Act
            x.Month = 50;

            //Assert
            x.Year.Should().BeNull();
            x.Month.Should().BeNull();
            x.Day.Should().BeNull();
            x.Date.Should().BeNull();
        }

        [Fact]
        public void EmptyAdapter_RightActionYear()
        {
            //Arrange
            var x = new DateAdapter();

            //Act
            x.Year = 2022;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
        }

        [Fact]
        public void EmptyAdapter_WrongActionYear()
        {
            //Arrange
            var x = new DateAdapter();

            //Act
            x.Year = -2444;

            //Assert
            x.Year.Should().BeNull();
            x.Month.Should().BeNull();
            x.Day.Should().BeNull();
            x.Date.Should().BeNull();
        }

        [Fact]
        public void NoEmptyAdapter_NoAction()
        {
            //Arrange
            var date = DateTime.Now;
            var x = new DateAdapter(date);
            //Act
            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
            x.Date.Should().Equals(date);
        }

        [Fact]
        public void NoEmptyAdapter_ActionDay()
        {
            //Arrange
            var x = new DateAdapter(new DateTime(2021, 11, 12));

            //Act
            x.Day = 14;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
            x.Date.Should().Equals(new DateTime(2021, 11, 14));
        }

        [Fact]
        public void NoEmptyAdapter_WrongActionDay()
        {
            //Arrange
            var date = new DateTime(2021, 11, 12);
            var x = new DateAdapter(date);

            //Act
            x.Day = 434;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
            x.Date.Should().Equals(date);
        }

        [Fact]
        public void NoEmptyAdapter_ActionMonth()
        {
            //Arrange
            var x = new DateAdapter(new DateTime(2021, 11, 12));

            //Act
            x.Month = 7;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
            x.Date.Should().Equals(new DateTime(2021, 7, 12));
        }

        [Fact]
        public void NoEmptyAdapter_WrongActionMonth()
        {
            //Arrange
            var date = new DateTime(2021, 11, 12);
            var x = new DateAdapter(date);

            //Act
            x.Month = 434;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
            x.Date.Should().Equals(date);
        }

        [Fact]
        public void NoEmptyAdapter_ActionYear()
        {
            //Arrange
            var x = new DateAdapter(new DateTime(2021, 11, 12));

            //Act
            x.Year = 2022;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
            x.Date.Should().Equals(new DateTime(2022, 11, 12));
        }

        [Fact]
        public void NoEmptyAdapter_WrongActionYear()
        {
            //Arrange
            var date = new DateTime(2021, 11, 12);
            var x = new DateAdapter(date);

            //Act
            x.Year = -434;

            //Assert
            x.Year.Should().NotBeNull();
            x.Month.Should().NotBeNull();
            x.Day.Should().NotBeNull();
            x.Date.Should().NotBeNull();
            x.Date.Should().Equals(date);
        }
    }
}
