using System;
using System.Collections.Generic;
using System.Linq;

namespace Lunggo.ApCommon.Model
{
    public class TrieNode
    {
        private readonly Dictionary<char,TrieNode> _children = new Dictionary<char, TrieNode>();
        private bool _isWord;
        private readonly List<int> _containerId = new List<int>();

        public void InsertWordsBySentence(string sentence, int id)
        {
            foreach (var word in sentence.Split(' '))
            {
                InsertWord(word,id);
            }
        }

        public void InsertWord(string word, int id)
        {
            var node = this;
            foreach (var c in word.ToLower())
            {
                if (!node._children.ContainsKey(c))
                {
                    node._children.Add(c, new TrieNode());
                }
                node = node._children[c];
            }
            node._isWord = true;
            node._containerId.Add(id);
        }

        public IEnumerable<int> GetAllSuggestionIds(string prefix)
        {
            if (prefix == null) return new List<int>();
            var node = this;
            var matched = true;
            foreach (var c in prefix.ToLower())
            {
                if (!node._children.ContainsKey(c))
                {
                    matched = false;
                    break;
                }
                node = node._children[c];
            }
            return matched ? GetAllChildIds(node).Distinct() : new List<int>();
        }

        public IEnumerable<String> GetAllSuggestions(string prefix)
        {
            var node = this;
            var matched = true;
            foreach (var c in prefix.ToLower())
            {
                if (!node._children.ContainsKey(c))
                {
                    matched = false;
                    break;
                }
                node = node._children[c];
            }
            return matched ? GetAllChildSuffixes(node, prefix) : null;
        }

        private static IEnumerable<String> GetAllChildSuffixes(TrieNode node, string prefix)
        {
            if (node._isWord)
            {
                yield return prefix;
            }
            var childSuffixes = Enumerable.Empty<String>();
            childSuffixes = node._children.Aggregate(childSuffixes, (current, entry) => current.Concat(GetAllChildSuffixes(entry.Value, prefix + entry.Key)));
            foreach (var str in childSuffixes)
            {
                yield return str;
            }
        }

        private static IEnumerable<int> GetAllChildIds(TrieNode node)
        {
            var childIds = new List<int>();
            if (node._isWord)
            {
                childIds.AddRange(node._containerId);
            }
            foreach (var entry in node._children)
            {
                childIds.AddRange(GetAllChildIds(entry.Value));
            }
            return childIds;
        }

    }
}