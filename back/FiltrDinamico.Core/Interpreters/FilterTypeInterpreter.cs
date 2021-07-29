using FiltrDinamico.Core.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FiltrDinamico.Core.Interpreters
{
    public abstract class FilterTypeInterpreter<TType> : IFilterTypeInterpreter<TType>
    {
        private FiltroItem _filtroItem;

        public FilterTypeInterpreter(FiltroItem filtroItem)
        {
            _filtroItem = filtroItem;
        }

        public Expression<Func<TType, bool>> Interpret()
        {
            var dynamicType = typeof(TType);
            var parameter = Expression.Parameter(dynamicType, dynamicType.Name.First().ToString());
            if (_filtroItem.Property.Contains("."))
            {
                var prop = _filtroItem.Property.Split('.');
                var property1 = typeof(TType).GetProperty(prop[0]);
                var property2 = property1.PropertyType.GetProperty(prop[1]);
                var inner = Expression.Property(parameter, property1);
                var outer = Expression.Property(inner, property2);
                var propertyInfo = (PropertyInfo)outer.Member;
                var value = Convert.ChangeType(_filtroItem.Value.ToString(), propertyInfo.PropertyType);
                var constant = Expression.Constant(value);
                var expression = CreateExpression(outer, constant);
                return Expression.Lambda<Func<TType, bool>>(expression, parameter);
            }
            else
            {
                var property = Expression.Property(parameter, _filtroItem.Property);
                var propertyInfo = (PropertyInfo)property.Member;
                var value = Convert.ChangeType(_filtroItem.Value.ToString(), propertyInfo.PropertyType);
                var constant = Expression.Constant(value);
                var expression = CreateExpression(property, constant);

                return Expression.Lambda<Func<TType, bool>>(expression, parameter);
            }


            
        }

        internal abstract Expression CreateExpression(MemberExpression property, ConstantExpression constant);
    }
}
