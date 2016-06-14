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
using OpenData.Data; 
namespace OpenData.Framework
{
    public class EntityModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (typeof( DynamicEntity).IsAssignableFrom(modelType))
            {
                var idValue = bindingContext.ValueProvider.GetValue("id");
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}
