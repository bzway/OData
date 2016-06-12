#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion 
using OpenData.AppEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using OpenData.Reflection; 
namespace OpenData.Web
{
    public class EntityModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            //if (typeof(IEntity).IsAssignableFrom(modelType))
            //{
            //    var idValue = bindingContext.ValueProvider.GetValue("id");
            //    int id = 0;

            //    if (idValue != null && int.TryParse(idValue.AttemptedValue, out id))
            //    {
            //        var providerType = typeof(IProvider<>).MakeGenericType(modelType);
            //        dynamic provider = ApplicationEngine.Current.TryResolve(providerType);
            //        if (provider != null)
            //        {
            //            return provider.QueryById(id);
            //        }
            //    }

            //}
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}
