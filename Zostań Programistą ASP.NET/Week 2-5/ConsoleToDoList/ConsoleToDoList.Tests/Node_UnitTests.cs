using ConsoleToDoList.App;
using FluentAssertions;
using System;
using Xunit;

namespace ConsoleToDoList.Tests
{
    public class Node_UnitTests
    {
        [Fact]
        public void Default()
        {
            //Arrange
            var x = new Node();
            //Act
            //Assert
            x.Data.Should().BeNull();
            x.NextNodes.Should().BeNull();
            x.LastNode.Should().BeNull();
        }

        private class NodeData : INodeDataIntegration
        {
            public Node Node { get; set; } = null;
        }

        [Fact]
        public void NodeDataIntegration()
        {
            //Arrange
            var node = new Node();
            var data = new NodeData();

            //Act
            node.Data = data;

            //Assert
            data.Node.Should().NotBeNull();
            node.Data.Should().NotBeNull();
            data.Node.Should().Be(node);
            node.Data.Should().Be(data);
        }

        [Fact]
        public void NodeDataDisintegration()
        {
            //Arrange
            var node = new Node();
            var data = new NodeData();
            node.Data = data;

            //Act
            node.Data = null;

            //Assert
            data.Node.Should().BeNull();
            node.Data.Should().BeNull();
            data.Node.Should().NotBe(node);
            node.Data.Should().NotBe(data);
        }

        [Fact]
        public void NodeDataReintegration()
        {
            //Arrange
            var node = new Node();
            var data1 = new NodeData();
            var data2 = new NodeData();
            node.Data = data1;

            //Act
            node.Data = data2;

            //Assert
            data1.Node.Should().BeNull();
            data2.Node.Should().NotBeNull();
            node.Data.Should().NotBeNull();
            data1.Node.Should().NotBe(node);
            data2.Node.Should().Be(node);
            node.Data.Should().NotBe(data1);
            node.Data.Should().Be(data2);
        }

        private class NodeDataMinLogic : INodeDataIntegration
        {
            private Node node = null;
            public Node Node 
            { 
                get => node;
                set
                {
                    if (node != null) node.Data = null;
                    node = value;
                    if (node != null) node.Data = this;
                }
            }
        }

        [Fact]
        public void NodeDataIntegrationMinLogic()
        {
            //Arrange
            var node = new Node();
            var data = new NodeDataMinLogic();

            //Act
            node.Data = data;

            //Assert
            data.Node.Should().NotBeNull();
            node.Data.Should().NotBeNull();
            data.Node.Should().Be(node);
            node.Data.Should().Be(data);
        }

        [Fact]
        public void NodeDataDisintegrationMinLogic()
        {
            //Arrange
            var node = new Node();
            var data = new NodeDataMinLogic();
            node.Data = data;

            //Act
            node.Data = null;

            //Assert
            data.Node.Should().BeNull();
            node.Data.Should().BeNull();
            data.Node.Should().NotBe(node);
            node.Data.Should().NotBe(data);
        }

        [Fact]
        public void NodeDataReintegrationMinLogic()
        {
            //Arrange
            var node = new Node();
            var data1 = new NodeDataMinLogic();
            var data2 = new NodeDataMinLogic();
            node.Data = data1;

            //Act
            node.Data = data2;

            //Assert
            data1.Node.Should().BeNull();
            data2.Node.Should().NotBeNull();
            node.Data.Should().NotBeNull();
            data1.Node.Should().NotBe(node);
            data2.Node.Should().Be(node);
            node.Data.Should().NotBe(data1);
            node.Data.Should().Be(data2);
        }
        
        [Fact]
        public void CreateNewNode_1to1to1()
        {
            //Arrange
            var node1 = new Node();
            var node2 = node1.CreateNewNode();
            var node3 = node2.CreateNewNode();
            //Act
            //Assert
            node1.LastNode.Should().BeNull();
            node1.NextNodes.Should().NotBeNullOrEmpty();
            node1.NextNodes[0].Should().NotBeNull();
            node1.NextNodes[0].Should().Be(node2);
            node1.Should().NotBe(node2);
            node1.Should().NotBe(node3);
            node2.Should().NotBeNull();
            node2.LastNode.Should().NotBeNull();
            node2.LastNode.Should().Be(node1);
            node2.NextNodes.Should().NotBeNullOrEmpty();
            node2.NextNodes[0].Should().NotBeNull();
            node2.NextNodes[0].Should().Be(node3);
            node2.Should().NotBe(node3);
            node3.Should().NotBeNull();
            node3.LastNode.Should().NotBeNull();
            node3.LastNode.Should().Be(node2);
            node3.NextNodes.Should().BeNull();
        }

        [Fact]
        public void CreateNewNode_1toX()
        {
            //Arrange
            var node = new Node();
            var node1 = node.CreateNewNode();
            var node2 = node.CreateNewNode();
            var node3 = node.CreateNewNode();
            //Act
            //Assert
            node.LastNode.Should().BeNull();
            node.NextNodes.Should().NotBeNullOrEmpty();
            node.NextNodes.Should().Contain(node1);
            node.NextNodes.Should().Contain(node2);
            node.NextNodes.Should().Contain(node3);
            node1.Should().NotBe(node2);
            node1.Should().NotBe(node3);
            node2.Should().NotBe(node3);
            node1.LastNode.Should().NotBeNull();
            node1.LastNode.Should().Be(node);
            node2.LastNode.Should().NotBeNull();
            node2.LastNode.Should().Be(node);
            node3.LastNode.Should().NotBeNull();
            node3.LastNode.Should().Be(node);
        }

        [Fact]
        public void CreateNewNode_Data()
        {
            //Arrange
            var data1 = new NodeData();
            var data2 = new NodeData();
            var node = new Node();

            //Act
            node.CreateNewNode(data1);
            node.CreateNewNode(data2);

            //Assert
            node.NextNodes.Should().NotBeNullOrEmpty();
            node.NextNodes[0].Data.Should().NotBeNull();
            node.NextNodes[0].Data.Should().Be(data1);
            node.NextNodes[1].Data.Should().NotBeNull();
            node.NextNodes[1].Data.Should().Be(data2);
        }

        [Fact]
        public void RemoveNextNodes()
        {
            //Arrange
            var data1 = new NodeData();
            var data1_1 = new NodeData();
            var data2 = new NodeData();

            var node = new Node();
            var node1 = node.CreateNewNode(data1);
            var node2 = node.CreateNewNode(data2);
            var node1_1 = node1.CreateNewNode(data1_1);
            var node1_2 = node1.CreateNewNode();

            //Act
            node.RemoveNextNodes();

            //Assert
            node.NextNodes.Should().BeNull();
            node1.LastNode.Should().BeNull();
            node1.Data.Should().BeNull();
            node1.NextNodes.Should().BeNull();
            data1.Node.Should().BeNull();
            node2.LastNode.Should().BeNull();
            node2.Data.Should().BeNull();
            node2.NextNodes.Should().BeNull();
            data2.Node.Should().BeNull();
            node1_1.LastNode.Should().BeNull();
            node1_1.Data.Should().BeNull();
            node1_1.NextNodes.Should().BeNull();
            data1_1.Node.Should().BeNull();
            node1_2.LastNode.Should().BeNull();
            node1_2.Data.Should().BeNull();
            node1_2.NextNodes.Should().BeNull();
        }

        [Fact]
        public void RemoveThisNode()
        {
            //Arrange
            var data1 = new NodeData();
            var data1_1 = new NodeData();
            var data2 = new NodeData();

            var node = new Node();
            var node1 = node.CreateNewNode(data1);
            var node2 = node.CreateNewNode(data2);
            var node1_1 = node1.CreateNewNode(data1_1);

            //Act
            node1.RemoveThisNode();

            //Assert
            node.NextNodes.Should().NotBeNullOrEmpty();
            node.NextNodes.Should().Contain(node2);
            node.NextNodes.Should().NotContain(node1);
            node1.LastNode.Should().BeNull();
            node1.Data.Should().BeNull();
            node1.NextNodes.Should().BeNull();
            data1.Node.Should().BeNull();
            node2.LastNode.Should().NotBeNull();
            node2.LastNode.Should().Be(node);
            node2.Data.Should().NotBeNull();
            node2.NextNodes.Should().BeNull();
            data2.Node.Should().NotBeNull();
            node1_1.LastNode.Should().BeNull();
            node1_1.Data.Should().BeNull();
            node1_1.NextNodes.Should().BeNull();
            data1_1.Node.Should().BeNull();
        }

        [Fact]
        public void NormalizationNodeToList()
        {
            //Arrange
            var node0 = new Node();
            var node1 = node0.CreateNewNode();
            var node1_1 = node1.CreateNewNode();
            var node1_2 = node1.CreateNewNode();
            var node2 = node0.CreateNewNode();
            var node2_1 = node2.CreateNewNode();
            var node2_2 = node2.CreateNewNode();

            //Act
            var list = Node.NormalizationNodeToList(node0);

            //Assert
            list.Should().NotBeNullOrEmpty();
            list.Should().Contain(node0);
            list.Should().Contain(node1);
            list.Should().Contain(node2);
            list.Should().Contain(node1_1);
            list.Should().Contain(node1_2);
            list.Should().Contain(node2_1);
            list.Should().Contain(node2_2);
        }
    }
}
