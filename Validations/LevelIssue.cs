using System;
using System.Reflection;

namespace BHSDK.Validations
{
    public readonly struct LevelIssue
    {
        public readonly LevelIssueGroup Group;
        public readonly LevelErrorType ErrorType;
        public readonly object Context;
        public readonly PropertyInfo Property;

        public LevelIssue(LevelIssueGroup group, LevelErrorType errorType,
            object context = null, PropertyInfo property = null)
        {
            Group = group;
            ErrorType = errorType;
            Context = context;
            Property = property;
        }
        
        public Type TypeIdentifier => Context?.GetType() ?? typeof(object);
        public bool IsOk() => ErrorType == LevelErrorType.Ok;

        public static LevelIssue Ok()
            => new(LevelIssueGroup.None, LevelErrorType.Ok);
        public static LevelIssue Ok(object context)
            => new(LevelIssueGroup.None, LevelErrorType.Ok, context);
        public static LevelIssue Ok(object context, string propertyName)
            => new(LevelIssueGroup.None, LevelErrorType.Ok, context, context.GetType().GetProperty(propertyName));
        
        public static LevelIssue Error(LevelErrorType errorType)
            => new(LevelIssueGroup.Error, errorType);
        public static LevelIssue Error(LevelErrorType errorType, object context)
            => new(LevelIssueGroup.Error, errorType, context);
        public static LevelIssue Error(LevelErrorType errorType, object context, string propertyName)
            => new(LevelIssueGroup.Error, errorType, context, context.GetType().GetProperty(propertyName));
        
        public static LevelIssue Warning(LevelErrorType errorType)
            => new(LevelIssueGroup.Warning, errorType);
        public static LevelIssue Warning(LevelErrorType errorType, object context)
            => new(LevelIssueGroup.Warning, errorType, context);
        public static LevelIssue Warning(LevelErrorType errorType, object context, string propertyName)
            => new(LevelIssueGroup.Warning, errorType, context, context.GetType().GetProperty(propertyName));
        
        public static LevelIssue Advice(LevelErrorType errorType)
            => new(LevelIssueGroup.Advice, errorType);
        public static LevelIssue Advice(LevelErrorType errorType, object context)
            => new(LevelIssueGroup.Advice, errorType, context);
        public static LevelIssue Advice(LevelErrorType errorType, object context, string propertyName)
            => new(LevelIssueGroup.Advice, errorType, context, context.GetType().GetProperty(propertyName));
    }
}