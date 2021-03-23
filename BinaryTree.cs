using System;
using System.Collections.Generic;

namespace Search_algorithms_AandDS {
	sealed class BinaryTree<T> where T : IComparable<T> {
		private BinaryTree<T> left, right;
		public T Value { get; private set; }
		private List<T> printList = new List<T>();

		public BinaryTree(T Value) {
			this.Value = Value;
		}

		public void Add(T Value) {
			if (Value.CompareTo(this.Value) < 0) {
				if (left == null) {
					left = new BinaryTree<T>(Value);
				}
				else
					left.Add(Value);
			}
			else {
				if (right == null) {
					right = new BinaryTree<T>(Value);
				}
				else
					right.Add(Value);
			}
		}

		private BinaryTree<T> _search(BinaryTree<T> tree, T Value) {
			
			if (tree == null) {
				return null;
			}
			switch (Value.CompareTo(tree.Value)) {
				case 1: 
					return _search(tree.right, Value);
				case -1: 
					return _search(tree.left, Value);
				case 0: 
					return tree;
				default:
					return null;
			}
		}

		public BinaryTree<T> Search(T Value) {
			return _search(this, Value);
		}

		public void _buildList(BinaryTree<T> node) {
			if (node == null)
				return;
			_buildList(node.left);
			printList.Add(node.Value);
			if (node.right != null)
				_buildList(node.right);
		}

		public List<T> BuildList() {
			printList.Clear();
			_buildList(this);
			return printList;
		}

		public override string ToString() {
			return Value.ToString();
		}
	}

}
