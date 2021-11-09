using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleToDoList.App
{
    [Serializable]
    public class Node
    {
        private Node _lastNode = null; 
        private List<Node> _nextNodes = null;
        private INodeDataIntegration _data = null;

        public Node LastNode { get => _lastNode; }
        public List<Node> NextNodes { get => _nextNodes; }
        public INodeDataIntegration Data
        {
            get { return _data; }
            set 
            {
                if (_data != null)
                {
                    _data.Node = null;
                    _data = null;
                }

                _data = value;
                _data.Node = this;
            }
        }

        public Node()
        {

        }

        private Node(Node parent)
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

            if (_data != null)
            {
                _data.Node = null;
                _data = null;
            }
        }

        public Node CreateNewNode()
        {
            if (_nextNodes == null)
            {
                _nextNodes = new List<Node>();
            }

            Node buff = new Node(this);
            _nextNodes.Add(buff);
            return buff;
        }

        public Node CreateNewNode(INodeDataIntegration data)
        {
            Node buff;

            if (data.Node == null)
            {
                buff = CreateNewNode();
                buff._data = data;
                buff._data.Node = buff;
            }
            else
            {
                buff = data.Node;
                if (_nextNodes == null) _nextNodes = new List<Node>();
                buff._lastNode = this;
                _nextNodes.Add(buff);
            }
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
