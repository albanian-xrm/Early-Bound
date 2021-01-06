using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Extensions
{
    static class DatabindExtensions
    {
        public static Binding Bind<T, TSource, TProperty>(this T target, Expression<Func<T, TProperty>> targetProperty, TSource source, Expression<Func<TSource, TProperty>> sourceProperty)
            where T : IBindableComponent
            where TSource : INotifyPropertyChanged
        {
            var targetPropertyInfo = targetProperty.GetProperty();
            var sourcePropertyInfo = sourceProperty.GetProperty();
            var binding = target.DataBindings.Add(
                 targetPropertyInfo.Name,
                 source,
                 sourcePropertyInfo.Name,
                 formattingEnabled: true);

            targetPropertyInfo.SetValue(target, sourcePropertyInfo.GetValue(source));
            return binding;
        }
    }
}
