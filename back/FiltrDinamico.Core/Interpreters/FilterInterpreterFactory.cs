using FiltrDinamico.Core.Models;
using System;

namespace FiltrDinamico.Core.Interpreters
{
    public class FilterInterpreterFactory : IFilterInterpreterFactory
    {
        public IFilterTypeInterpreter<TType> Create<TType>(FiltroItem filtroItem)
        {
            switch (filtroItem.FilterType)
            {
                case FilterTypeConstants.Equals:
                    return new EqualsInterpreter<TType>(filtroItem);
                case FilterTypeConstants.Contains:
                    return new ContainsInterpreter<TType>(filtroItem);
                case FilterTypeConstants.GreaterThan:
                    return new GreaterThanInterpreter<TType>(filtroItem);
                case FilterTypeConstants.LessThan:
                    return new LessThanInterpreter<TType>(filtroItem);
                case FilterTypeConstants.StartsWith:
                    return new StartsWithInterpreter<TType>(filtroItem);
                case FilterTypeConstants.GreaterThanEquals:
                    return new GreaterThanEqualsInterpreter<TType>(filtroItem);
                case FilterTypeConstants.LessThanEquals:
                    return new LessThanEqualsInterpreter<TType>(filtroItem);
                default:
                    throw new ArgumentException($"the filter type {filtroItem.FilterType} is invalid");
            }
        }
    }
}
