using System.ComponentModel;

namespace Wholesale.Desktop.Utils.Forms.Abstractions
{
    public interface IFilterRepository<T>
    {
        BindingList<T> Filter(string query = null);
    }
}
