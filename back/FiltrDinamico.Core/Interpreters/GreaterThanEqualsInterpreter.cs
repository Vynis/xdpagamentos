using FiltrDinamico.Core.Models;
using System.Linq.Expressions;

namespace FiltrDinamico.Core.Interpreters
{
    public class GreaterThanEqualsInterpreter<TType> : FilterTypeInterpreter<TType>
    {
        public GreaterThanEqualsInterpreter(FiltroItem filtroItem) : base(filtroItem)
        {
        }

        internal override Expression CreateExpression(MemberExpression property, ConstantExpression constant) 
            => Expression.GreaterThanOrEqual(property, constant);
    }
}
