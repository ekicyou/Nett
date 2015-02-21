﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nett
{
    public sealed class TomlTable
    {
        public string Name { get; private set; } = "";
        public Dictionary<string, object> Rows { get; } = new Dictionary<string, object>();

        private readonly List<TomlTable> tables = new List<TomlTable>();
        public IEnumerable<TomlTable> Tables => this.tables;

        public TomlTable()
        {
        }

        public TomlTable(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            this.Name = name;
        }

        public object this[string key]
        {
            get { return this.Rows[key]; }
        }

        public T Get<T>(string key)
        {
            return Converter.Convert<T>(this[key]);
        }

        public void Add(string key, object value)
        {
            this.Rows.Add(key, value);
        }

        public void Add(TomlTable table)
        {
            this.tables.Add(table);
        }
    }
}
