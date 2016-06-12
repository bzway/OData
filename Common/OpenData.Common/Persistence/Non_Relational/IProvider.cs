#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Web.Common.Persistence.Non_Relational
{
    public interface IProvider<T>
    {
        IEnumerable<T> All();

        T Get(T dummy);
        void Add(T item);
        void Update(T @new, T old);
        void Remove(T item);
    }
}
