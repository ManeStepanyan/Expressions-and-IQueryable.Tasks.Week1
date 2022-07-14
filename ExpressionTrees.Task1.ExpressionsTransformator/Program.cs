/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();
            Expression<Func<int, int>> expression = x => x + 1;
            var transaltor = new IncDecExpressionVisitor();
            var exp =(Expression<Func<int, int>>)transaltor.Visit(expression);
            Console.WriteLine(exp.ToString());
            var result = exp.Compile()(5);
            Console.WriteLine(result);

            Expression<Func<int, int>> before = (x) => x - 1;
            var after = ReplaceParameter(before, 3);
            Console.WriteLine(after);

            Console.ReadLine();
        }
        public static Expression<Func<TElement>> ReplaceParameter<TElement>
        (
            Expression<Func<TElement, TElement>> inputExpression,
            TElement element
        )
        {
            var replacedParams = new List<KeyValuePair<Expression, Expression>>()
            {
                new KeyValuePair<Expression, Expression>(inputExpression.Parameters[0],
                    Expression.Constant(element, typeof(TElement)))
            };
            var replacer = new Replacer(replacedParams);
            var body = replacer.Visit(inputExpression.Body);
            return Expression.Lambda<Func<TElement>>(body,
                null);
        }
        class Replacer : ExpressionVisitor
        {
            private List<KeyValuePair<Expression, Expression>> _replacedParams;

            public Replacer(List<KeyValuePair<Expression, Expression>> replacedParams)
            {
                _replacedParams = replacedParams;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                foreach (var item in _replacedParams)
                {
                    if (node == item.Key)
                    {
                        return item.Value;
                    }
                }

                return base.VisitParameter(node);
            }
        }
    }
}
