using ConsoleToDoList.App;
using FluentAssertions;
using System;
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
        public void EmptyTask()
        {
            //Arrange
            var x = new TaskHook();
            Action actDate = () => { var y = x.Date; };
            Action actFinishStatus = () => { var y = x.FinishStatus; };

            //Act
            x.Task = null;

            //Assert
            x.Task.Should().BeNull();
            actDate.Should().Throw<NullReferenceException>();
            actFinishStatus.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ActionFinishTask()
        {
            //Arrange
            LogicCORE.___Testing = true;
            var node = new Node();
            node.Data = new TaskHook();
            node.CreateNewNode(new TaskHook());

            //Act
            (node.Data as TaskHook).FinishStatus = true;

            //Assert
            (node.Data as TaskHook).FinishStatus.Should().BeTrue();
            (node.Data as TaskHook).Date.Should().NotBeNull();
            node.NextNodes[0].Data.Should().NotBeNull();
            (node.NextNodes[0].Data as TaskHook).FinishStatus.Should().BeTrue();
            (node.NextNodes[0].Data as TaskHook).Date.Should().NotBeNull();

            //Clean
            LogicCORE.___Testing = false;
        }

        [Fact]
        public void NoFinishTask_ActionChangeDate()
        {
            //Arrange
            var x = new TaskHook();
            var date = new DateTime(2021, 12, 12);
            LogicCORE.___Testing = true;

            //Act
            x.FinishStatus = false;
            x.Date = date;

            //Assert
            x.FinishStatus.Should().BeFalse();
            x.Date.Should().NotBeNull();
            x.Date.Should().Be(date);

            //Clean
            LogicCORE.___Testing = false;
        }

        [Fact]
        public void FinishTask_ActionChangeDate()
        {
            //Arrange
            var x = new TaskHook();
            var date = new DateTime(2021, 12, 11);
            LogicCORE.___Testing = true;

            //Act
            x.Date = date;
            x.FinishStatus = true;

            //Assert
            x.FinishStatus.Should().BeTrue();
            x.Date.Should().NotBeNull();
            x.Date.Should().NotBe(date);

            //Clean
            LogicCORE.___Testing = false;
        }

        [Fact]
        public void DeleteTask()
        {
            //Arrange
            LogicCORE.___Testing = true;
            var x = new TaskHook();
            x.Node = new Node();

            //Act
            x.DeleteTask();

            //Assert
            x.Node.Should().BeNull();

            //Clean
            LogicCORE.___Testing = false;
        }
    }
}
