using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            Dictionary<string, string> mappings = new Dictionary<string, string>();
            mappings.Add("Bar1", "Foo1");
            mappings.Add("Bar2", "Foo2");
            mappings.Add("Bar3", "Foo3");

            var expressions = new List<Expression>();
            var sourceParam = Expression.Parameter(typeof(TSource));
            var destinationResult = Expression.New(typeof(TDestination));

            var sourceType = typeof(TSource);
            var sourceProperties = sourceType.GetProperties();

            var destinationType = typeof(TDestination);
            var destinationProperties = destinationType.GetProperties().ToDictionary(x => x.Name);

            var sourceInstance = Expression.Variable(typeof(TSource), "input");
            var destinationInstance = Expression.Variable(typeof(TDestination), "result");

            expressions.Add(Expression.Assign(sourceInstance, sourceParam));
            expressions.Add(Expression.Assign(destinationInstance, destinationResult));

            foreach (var sourceProperty in sourceProperties)
            {
                PropertyInfo destinationProperty;

                if (destinationProperties.TryGetValue(mappings[sourceProperty.Name], out destinationProperty))
                {
                    var sourceValue = Expression.Property(sourceInstance, sourceProperty.Name);
                    var destinationValue = Expression.Property(destinationInstance, destinationProperty);

                    var sourceValueType = sourceProperty.PropertyType;
                    var destValueType = destinationProperty.PropertyType;
                    if (sourceValueType != destValueType)
                    {
                        //TO BE ADDED
                        // HAVE A PROBLEM OF CONVERTING DATA TYPES
                        //var objConv = (MemberExpression)Expression.Convert(sourceValue, typeof(object)).Operand;
                        ////expressions.Add(Expression.Assign(sourceValue, objConv));
                        //var conv = ConvertToType(Type.GetTypeCode(destValueType), objConv);
                        //expressions.Add(conv);
                        //expressions.Add(Expression.Assign(destinationValue, conv));
                    }
                    else
                    {
                        expressions.Add(Expression.Assign(destinationValue, sourceValue));
                    }

                }
            }

            expressions.Add(destinationInstance);

            var body = Expression.Block(new[] { sourceInstance, destinationInstance }, expressions);

            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    body,
                    sourceParam
                );

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }
        private MethodCallExpression ConvertToType(
            TypeCode typeCode,
            MemberExpression sourceExpressionProperty)
        {
            //var sourceExpressionProperty = Expression.Property(sourceParameter, sourceProperty);
            var changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(TypeCode) });
            var callExpressionReturningObject = Expression.Call(changeTypeMethod, sourceExpressionProperty, Expression.Constant(typeCode));
            return callExpressionReturningObject;
        }
    }
}
