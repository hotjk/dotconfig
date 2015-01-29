using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Grit.Utility.Web.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Get model name from a expression
        /// Since System.Web.Mvc.ExpressionHelper.GetExpressionText need a LambdaExpression parameter
        /// This helper method convert System.Linq.Expressions.Expression to System.Linq.Expressions.LambdaExpression
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString GetExpressionText<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression
        )
        {
            var metaData = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string value = ExpressionHelper.GetExpressionText(expression);
            return MvcHtmlString.Create(value);
        }


        public static MvcHtmlString GetDisplayName<TModel, TProperty>(
        this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string value = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            return MvcHtmlString.Create(value);
        }
    }
}
