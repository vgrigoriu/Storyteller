﻿using System;
using Storyteller.Core.Model;

namespace Storyteller.Core
{
    public abstract class ModifyCellAttribute : Attribute
    {
        public abstract void Modify(Cell cell);
    }

    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Method, Inherited = false)]
    public class AliasAsAttribute : ModifyCellAttribute
    {
        private readonly string _alias;

        public AliasAsAttribute(string alias)
        {
            _alias = alias;
        }

        public override void Modify(Cell cell)
        {
            cell.Key = _alias;
        }

        public string Alias { get { return _alias; } }
    }
}