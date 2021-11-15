using FiltrDinamico.Core.Models;
using System.Linq.Expressions;

namespace FiltrDinamico.Core.Interpreters
{
    public class LessThanEqualsInterpreter<TType> : FilterTypeInterpreter<TType>
    {
        public LessThanEqualsInterpreter(FiltroItem filtroItem) : base(filtroItem)
        {
        }

        internal override Expression CreateExpression(MemberExpression property, ConstantExpression constant)
            => Expression.LessThanOrEqual(property, constant);
    }
}
