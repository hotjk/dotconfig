using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Grit.Utility.Web.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Get property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="src"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string GetDisplayName<T, U>(this T src, Expression<Func<T, U>> exp)
        {
            var me = exp.Body as MemberExpression;
            if (me == null)
                throw new ArgumentException("Must be a MemberExpression.", "exp");

            var attr = me.Member
                         .GetCustomAttributes(typeof(DisplayAttribute), false)
                         .Cast<DisplayAttribute>()
                         .SingleOrDefault();

            return (attr != null) ? attr.Name : me.Member.Name;
        }

        /// <summary>
        /// Get html control name
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="model"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExpressionText<M, P>(this M model, Expression<Func<M, P>> ex)
        {
            return ExpressionHelper.GetExpressionText(ex);
        }
    }
}