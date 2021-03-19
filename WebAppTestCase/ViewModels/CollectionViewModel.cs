using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTestCase.ViewModels
{
    public class CollectionViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int ItemNo { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
