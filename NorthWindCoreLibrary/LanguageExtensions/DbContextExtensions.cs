using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NorthWindCoreLibrary.LanguageExtensions
{
    public static class DbContextExtensions
    {
        public static void Reload(this CollectionEntry source)
        {
            if (source.CurrentValue != null)
            {
                foreach (var item in source.CurrentValue)
                {
                    source.EntityEntry.Context.Entry(item).State = EntityState.Detached;
                }

                source.CurrentValue = null;
            }
            source.IsLoaded = false;
            source.Load();
        }
    }
}
