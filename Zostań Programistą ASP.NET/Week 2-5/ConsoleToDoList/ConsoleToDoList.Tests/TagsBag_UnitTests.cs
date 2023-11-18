using ConsoleToDoList.App;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace ConsoleToDoList.Tests
{
    public class TagsBag_UnitTests
    {
        [Fact]
        public void TagsList()
        {
            //Arrange
            var bag = new TagsBag();
            var tag = new Tag("Test");

            //Act
            bag.AddTag(tag);

            //Assert
            bag.TagsList.Should().NotBeNullOrEmpty();
            bag.TagsList.Should().Contain(tag);
        }

        [Fact]
        public void AddTag_TagNormal()
        {
            //Arrange
            var bag = new TagsBag();
            var tag = new Tag("Test");

            //Act
            bag.AddTag(tag);

            //Assert
            bag.CellsOfTagsList.Should().NotBeNullOrEmpty();
            bag.CellsOfTagsList[0].Lock.Should().BeFalse();
            bag.CellsOfTagsList[0].Tag.Should().Be(tag);
        }

        [Fact]
        public void AddTag_TagProtected()
        {
            //Arrange
            var bag = new TagsBag();
            var tag = new Tag("Test");

            //Act
            bag.AddTag(tag, true);

            //Assert
            bag.CellsOfTagsList.Should().NotBeNullOrEmpty();
            bag.CellsOfTagsList[0].Lock.Should().BeTrue();
            bag.CellsOfTagsList[0].Tag.Should().Be(tag);
        }

        [Fact]
        public void AddTag_TagsBagNormal()
        {
            //Arrange
            var bag1 = new TagsBag();
            var bag2 = new TagsBag();
            var tag1 = new Tag("Test1");
            var tag2 = new Tag("Test2");
            var tag3 = new Tag("Test3");

            bag1.AddTag(tag1);
            bag1.AddTag(tag2, true);
            bag2.AddTag(tag2);
            bag2.AddTag(tag3);

            //Act
            bag1.AddTag(bag2);

            //Assert
            bag1.CellsOfTagsList.Should().Contain(x => x.Tag == tag1 && x.Lock == false);
            bag1.CellsOfTagsList.Should().Contain(x => x.Tag == tag2 && x.Lock == true);
            bag1.CellsOfTagsList.Should().Contain(x => x.Tag == tag3 && x.Lock == false);
        }

        [Fact]
        public void AddTag_TagsBagProtected()
        {
            //Arrange
            var bag1 = new TagsBag();
            var bag2 = new TagsBag();
            var tag1 = new Tag("Test1");
            var tag2 = new Tag("Test2");
            var tag3 = new Tag("Test3");

            bag1.AddTag(tag1);
            bag1.AddTag(tag2);
            bag2.AddTag(tag2);
            bag2.AddTag(tag3);

            //Act
            bag1.AddTag(bag2, true);

            //Assert
            bag1.CellsOfTagsList.Should().Contain(x => x.Tag == tag1 && x.Lock == false);
            bag1.CellsOfTagsList.Should().Contain(x => x.Tag == tag2 && x.Lock == true);
            bag1.CellsOfTagsList.Should().Contain(x => x.Tag == tag3 && x.Lock == true);
        }

        [Fact]
        public void RemoveTag_TagNormal()
        {
            //Arrange
            var bag = new TagsBag();
            var tag1 = new Tag("Test1");
            var tag2 = new Tag("Test2");

            bag.AddTag(tag1);
            bag.AddTag(tag2, true);

            //Act
            bag.RemoveTag(tag1);
            bag.RemoveTag(tag2);

            //Assert
            bag.CellsOfTagsList.Should().NotContain(x => x.Tag == tag1);
            bag.CellsOfTagsList.Should().Contain(x => x.Tag == tag2);
        }

        [Fact]
        public void RemoveTag_TagProtected()
        {
            //Arrange
            var bag = new TagsBag();
            var tag1 = new Tag("Test1");
            var tag2 = new Tag("Test2");

            bag.AddTag(tag1);
            bag.AddTag(tag2, true);

            //Act
            bag.RemoveTag(tag1, true);
            bag.RemoveTag(tag2, true);

            //Assert
            bag.CellsOfTagsList.Should().NotContain(x => x.Tag == tag1);
            bag.CellsOfTagsList.Should().NotContain(x => x.Tag == tag2);
        }

        [Fact]
        public void RemoveTag_TagsBagNormal()
        {
            //Arrange
            var bag1 = new TagsBag();
            var bag2 = new TagsBag();
            var tag1 = new Tag("Test1");
            var tag2 = new Tag("Test2");
            var tag3 = new Tag("Test3");

            bag1.AddTag(tag1, true);
            bag1.AddTag(tag2, true);
            bag1.AddTag(tag3);
            bag2.AddTag(tag1);
            bag2.AddTag(tag2, true);
            bag2.AddTag(tag3, true);

            //Act
            bag1.RemoveTag(bag2);

            //Assert
            bag1.CellsOfTagsList.Should().Contain(x => x.Tag == tag1 && x.Lock == true);
            bag1.CellsOfTagsList.Should().NotContain(x => x.Tag == tag2);
            bag1.CellsOfTagsList.Should().NotContain(x => x.Tag == tag3);
        }

        [Fact]
        public void RemoveTag_TagsBagProtected()
        {
            //Arrange
            var bag1 = new TagsBag();
            var bag2 = new TagsBag();
            var tag1 = new Tag("Test1");
            var tag2 = new Tag("Test2");
            var tag3 = new Tag("Test3");

            bag1.AddTag(tag1, true);
            bag1.AddTag(tag2, true);
            bag1.AddTag(tag3);
            bag2.AddTag(tag1);
            bag2.AddTag(tag2, true);
            bag2.AddTag(tag3, true);

            //Act
            bag1.RemoveTag(bag2, true);

            //Assert
            bag1.CellsOfTagsList.Should().NotContain(x => x.Tag == tag1);
            bag1.CellsOfTagsList.Should().NotContain(x => x.Tag == tag2);
            bag1.CellsOfTagsList.Should().NotContain(x => x.Tag == tag3);
            bag1.CellsOfTagsList.Should().BeEmpty();
        }
    }
}
