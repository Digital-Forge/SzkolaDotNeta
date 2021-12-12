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
                if (value == null)
                {
                    if (_data != null)
                    {
                        var buff = _data;
                        _data = null;
                        buff.Node = null;
                    }
                }
                else
                {
                    if (value != _data)
                    {
                        var buff = _data;
                        _data = null;
                        if (buff != null) buff.Node = null;
                        _data = value;
                        _data.Node = this;
                    }
                }
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

        public void RemoveThisNode()
        {
            Data = null;

            if (_lastNode != null)
            {
                _lastNode.NextNodes.Remove(this);
            }
            _removeNextNodes();
        }

        public void RemoveNextNodes()
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
            Data = null;
        }

        public Node CreateNewNode()
        {
            if (_nextNodes == null) _nextNodes = new List<Node>();

            Node buff = new Node(this);
            _nextNodes.Add(buff);
            return buff;
        }

        public Node CreateNewNode(INodeDataIntegration data)
        {
            Node buff;

            if (data?.Node == null)
            {
                buff = CreateNewNode();
                buff.Data = data;
            }
            else
            {
                buff = data.Node;
                buff._data = data;
                if (_nextNodes == null) _nextNodes = new List<Node>();
                buff._lastNode = this;
                _nextNodes.Add(buff);
            }
            return buff;
        }

        public static List<Node> NormalizationNodeToList(Node startNode)
        {
            if (startNode != null)
            {
                if (startNode.NextNodes != null)
                {
                    List<Node> buff = new List<Node>() { startNode };
                    foreach (var item in startNode.NextNodes)
                    {
                        buff.AddRange(NormalizationNodeToList(item));
                    }
                    return buff;
                }
                else
                {
                    return new List<Node>() { startNode }; 
                }
            }
            return new List<Node>();
        }
    }
}
