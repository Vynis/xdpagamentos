using FiltrDinamico.Core.Models;
using System.Linq.Expressions;

namespace FiltrDinamico.Core.Interpreters
{
    public class NotEqualsInterpreter<TType> : FilterTypeInterpreter<TType>
    {
        public NotEqualsInterpreter(FiltroItem filtroItem) : base(filtroItem)
        {
        }

        internal override Expression CreateExpression(MemberExpression property, ConstantExpression constant) 
            => Expression.NotEqual(property, constant);
    }
}
