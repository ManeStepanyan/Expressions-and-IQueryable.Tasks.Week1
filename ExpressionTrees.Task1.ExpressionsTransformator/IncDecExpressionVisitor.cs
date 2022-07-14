using System;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Right is ConstantExpression c && c.Type.IsNumericDatatype() && (Int32)c.Value == 1)
            {
                if (node.NodeType == ExpressionType.Add || node.NodeType == ExpressionType.Subtract)
                {
                    if (node.Left is ParameterExpression)
                    {
                        if (node.NodeType == ExpressionType.Add)
                            return Expression.Increment(node.Left);
                        else
                            return Expression.Decrement(node.Left);
                    }
                }
            }
            return base.VisitBinary(node);
        }

    }
}
