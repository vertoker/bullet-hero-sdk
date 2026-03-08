using System;

namespace BHSDK.Interfaces
{
    /// <summary>
    /// Base interface for all interface models, have functionality for work
    /// with version classes only through interfaces
    /// </summary>
    /// <typeparam name="TModel">
    /// Type of actual model. Must be class, has default constructor and inherit from TInterfaceModel
    /// </typeparam>
    /// <typeparam name="TInterfaceModel">Interface type for actual model</typeparam>
    public interface IModelReflection<TModel, TInterfaceModel> where TModel : class, TInterfaceModel, new()
    {
        /// <summary> Return actual model type </summary>
        public static Type GetModelType => typeof(TModel);
        
        /// <summary> Create object of model only using interface </summary>
        public static TInterfaceModel CreateNew()
        {
            var node = (TInterfaceModel)Activator.CreateInstance(GetModelType);
            return node;
        }
    }
}