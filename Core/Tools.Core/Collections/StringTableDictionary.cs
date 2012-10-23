﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Core.Collections
{
    /// <summary>
    /// A dictionary that uses a string table behind.
    /// </summary>
    public class StringTableDictionary : IDictionary<string, string>
    {
        /// <summary>
        /// Holds the string table.
        /// </summary>
        private StringTable _string_table;

        /// <summary>
        /// The dictionary behind.
        /// </summary>
        private Dictionary<uint, uint> _dictionary;

        /// <summary>
        /// Creates a new dictionary.
        /// </summary>
        /// <param name="string_table"></param>
        public StringTableDictionary(StringTable string_table)
        {
            _string_table = string_table;
        }

        /// <summary>
        /// Adds key-value pair of strings.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            uint key_int = _string_table.Add(key);
            uint value_int = _string_table.Add(value);

            _dictionary.Add(key_int, value_int);
        }

        /// <summary>
        /// Returns true if the given key is present.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            uint key_int = _string_table.Add(key);  // TODO: this could be problematic, testing contains adds objects to string table.

            return _dictionary.ContainsKey(key_int);
        }

        /// <summary>
        /// Returns a collection of keys.
        /// </summary>
        public ICollection<string> Keys
        {
            get 
            { // i know pretty naive implementation.
                // TODO: add an ICollection<string> accepting a StringTable object and an ICollection<uint>
                List<string> keys = new List<string>();
                foreach (uint key_int in _dictionary.Keys)
                {
                    keys.Add(_string_table.Get(key_int));
                }
                return keys;
            }
        }

        /// <summary>
        /// Removes a value with the given key from this dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            uint key_int = _string_table.Add(key);

            return _dictionary.Remove(key_int);
        }

        /// <summary>
        /// Tries to get a value from this dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out string value)
        {
            uint key_int = _string_table.Add(key);
            uint value_int;
            value = null;
            if (_dictionary.TryGetValue(key_int, out value_int))
            {
                value = _string_table.Get(value_int);
            }
            return false;
        }

        /// <summary>
        /// Returns all values in this dictionary.
        /// </summary>
        public ICollection<string> Values
        {
            get
            { // i know pretty naive implementation.
                // TODO: add an ICollection<string> accepting a StringTable object and an ICollection<uint>
                List<string> keys = new List<string>();
                foreach (uint key_int in _dictionary.Values)
                {
                    keys.Add(_string_table.Get(key_int));
                }
                return keys;
            }
        }

        /// <summary>
        /// Gets/sets a value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                uint key_int = _string_table.Add(key);
                uint value_int = _dictionary[key_int];
                return _string_table.Get(value_int);
            }
            set
            {
                uint key_int = _string_table.Add(key);
                uint value_int = _string_table.Add(value);
                _dictionary[key_int] = value_int;
            }
        }

        /// <summary>
        /// Adds a key value pair.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, string> item)
        {
            KeyValuePair<uint, uint> item_int = new KeyValuePair<uint, uint>(
                _string_table.Add(item.Key), _string_table.Add(item.Key));
            _dictionary.Add(item_int.Key, item_int.Value);
        }

        /// <summary>
        /// Clears all content from this dictionary.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Returns true if the given key value pair is contained in this dictionary.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            uint value_int;
            if (_dictionary.TryGetValue(_string_table.Add(item.Key), out value_int))
            {
                return value_int == _string_table.Add(item.Value); // TODO: this could be problematic, testing contains adds objects to string table.
            }
            return false;
        }

        /// <summary>
        /// Copies all objects 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<string, string> pair in this)
            {
                array[arrayIndex] = pair;
                arrayIndex++;
            }
        }

        /// <summary>
        /// Returns the number of objects in this collection.
        /// </summary>
        public int Count
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// This dictionary is not readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes an item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return this.Remove(item.Key);
        }

        #region Enumeration

        /// <summary>
        /// Returns a enumerator for this dictionary.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a enumerator for this dictionary.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private class StringTableDictionaryEnumerator : IEnumerator<KeyValuePair<string, string>>
        {
            private KeyValuePair<string, string> _current;

            private IEnumerator<KeyValuePair<uint, uint>> _enumerator;

            private StringTable _string_table;

            public StringTableDictionaryEnumerator(StringTable string_table, IEnumerator<KeyValuePair<uint, uint>> enumerator)
            {
                _enumerator = enumerator;
                _string_table = string_table;
            }

            public KeyValuePair<string, string> Current
            {
                get { return _current; }
            }

            public void Dispose()
            {

            }

            object System.Collections.IEnumerator.Current
            {
                get { return _current; }
            }

            public bool MoveNext()
            {
                if (_enumerator.MoveNext())
                {
                    _current = new KeyValuePair<string, string>(
                        _string_table.Get(_enumerator.Current.Key), _string_table.Get(_enumerator.Current.Value));
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }


        #endregion
    }
}