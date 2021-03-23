using System;
using System.Collections.Generic;

namespace Search_algorithms_AandDS {


   sealed class BorTree<T> {
      sealed class Node {
         public char Symbol { get; private set; }
         public Dictionary<char, Node> NextNodes { get; internal set; }
         public T Data { get; set; }
         public bool IsWord { get; internal set; }

         public Node(char symbol, T data) {
            Symbol = symbol;
            Data = data;
            NextNodes = new Dictionary<char, Node>();
         }

         public Node TryFind(char symbol) {
            if (NextNodes.TryGetValue(symbol, out Node value)) {
               return value;
            }
            else {
               return null;
            }
         }

         public override bool Equals(object obj) {
            if (obj is Node item) {
               return Data.Equals(item);
            }
            else {
               return false;
            }
         }

         public override int GetHashCode() {
            Func<double, double, double> p = Math.Pow;
            return (int)(p(Symbol.GetHashCode(), 3) + p(Data.GetHashCode(), 2) + (IsWord ? 1 : 0));
         }

         public override string ToString() {
            return Data.ToString();
         }
      }

      private Node root;

      public BorTree() {
         root = new Node('\0', default(T));
      }


      public void Add(string key, T data) {
         _addNode(key, data, root);
      }

      private void _addNode(string key, T data, Node node) {
         if (string.IsNullOrEmpty(key)) {
            if (!node.IsWord) {
               node.Data = data;
               node.IsWord = true;
            }
            //Write to last tree branch data.
         }
         else {
            var symbol = key[0];
            var subnode = node.TryFind(symbol);
            if (subnode != null) {
               _addNode(key.Substring(1), data, subnode);
            }
            else {
               var newNode = new Node(key[0], data);
               node.NextNodes.Add(key[0], newNode);
               _addNode(key.Substring(1), data, newNode);
            }
         }

      }

      public bool TrySearch(string key, out T value) {
         return _searchNode(key, root, out value);
      }

      private bool _searchNode(string key, Node node, out T value) {
         value = default(T);
         bool result = false;
         if (string.IsNullOrEmpty(key)) {
            if (node.IsWord) {
               value = node.Data;
               result = true;
            }
         }
         else {
            var subnode = node.TryFind(key[0]);
            if (subnode != null) {
               result = _searchNode(key.Substring(1), subnode, out value);
            }
         }

         return result;
      }
   }
}
