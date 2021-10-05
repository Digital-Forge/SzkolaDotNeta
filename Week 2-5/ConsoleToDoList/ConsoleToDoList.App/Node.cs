using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleToDoList.App
{
    class Node<T> where T : class
    {
        private Node<T> _lastNode = null; 
        private List<Node<T>> _nextNodes = null;

        public Node<T> LastNode { get => _lastNode; }
        public List<Node<T>> NextNodes { get => _nextNodes; }

        public T Data { get; set; } = null;


        public Node()
        {

        }

        private Node(Node<T> parent)
        {
            _lastNode = parent;
        }

        ~Node()
        {
            RemoveNextNodes();
        }


        public async void RemoveNextNodes()
        {
            if (_nextNodes != null)
            {
                for (int i = 0; i < _nextNodes.Count; i++)
                {
                    try
                    {
                        _nextNodes[i]._removeNextNodes();
                        _nextNodes[i] = null;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                _nextNodes = null;
            }

        }

        private void _removeNextNodes()
        {
            RemoveNextNodes();
            _lastNode = null;
        }

        public Node<T> CreateNewNode()
        {
            if (_nextNodes == null)
            {
                _nextNodes = new List<Node<T>>();
            }

            Node<T> buff = new Node<T>(this);
            _nextNodes.Add(buff);
            return buff;
        }

        public Node<T> CreateNewNode(T data)
        {
            Node<T> buff = CreateNewNode();
            buff.Data = data;
            return buff;
        }

        public void RemoveThisNode()
        {
            if (_lastNode != null)
            {
                _lastNode.NextNodes.Remove(this);
            }
            _removeNextNodes();
        }

        
    }
}
