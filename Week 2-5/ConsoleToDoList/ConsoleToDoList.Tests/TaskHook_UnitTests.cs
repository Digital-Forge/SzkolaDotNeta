using ConsoleToDoList.App;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleToDoList.Tests
{
    public class TaskHook_UnitTests
    {
        [Fact]
        public void Default()
        {
            //Arrange
            var x = new TaskHook();
            //Act
            //Assert
            x.Task.Should().NotBeNull();
            x.TagsBag.Should().NotBeNull();
            x.Date.Should().BeNull();
            x.FinishStatus.Should().BeFalse();
            x.Node.Should().BeNull();
            x.Header().Should().NotBeNull();
            x.RecordHeader().Should().NotBeNull();
        }

        [Fact]
        public void Default_EmptyTask()
        {
            //Arrange
            var x = new TaskHook();
            Action actDate = () => { var y = x.Date; };
            Action actFinishStatus = () => { var y = x.FinishStatus; };

            //Act
            x.Task = null;

            //Assert
            x.Task.Should().NotBeNull();
            actDate.Should().Throw<ArgumentNullException>();
            actFinishStatus.Should().Throw<ArgumentNullException>();
        }
    }
}
