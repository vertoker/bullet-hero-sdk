using System;
using System.Collections.Generic;
using System.Text;

namespace BHSDK.Services
{
    public class TextFormatService
    {
        private readonly Dictionary<string, Func<string>> _variables;
        private readonly StringBuilder _expressionBuilder;
        private readonly StringBuilder _variableBuilder;

        public TextFormatService(int variablesCapacity = 16, 
            int expressionBuilderCapacity = 256, int variableBuilderCapacity = 16)
        {
            _variables = new Dictionary<string, Func<string>>(variablesCapacity);
            _expressionBuilder = new StringBuilder(expressionBuilderCapacity);
            _variableBuilder = new StringBuilder(variableBuilderCapacity);
        }

        public bool AddVariable(string variableKey, Func<string> method)
        {
            return _variables.TryAdd(variableKey, method);
        }
        public bool RemoveVariable(string variableKey)
        {
            return _variables.Remove(variableKey);
        }
        public void Clear()
        {
            _variables.Clear();
            _expressionBuilder.Clear();
            _variableBuilder.Clear();
        }

        public string Process(string expression)
        {
            _expressionBuilder.Clear();
            _variableBuilder.Clear();
            
            var inBrackets = false;
            
            foreach (var currentChar in expression)
            {
                switch (currentChar)
                {
                    case '{':
                        if (inBrackets) 
                            throw new ArgumentException("Brackets already opened");
                        inBrackets = true;
                        break;
                    
                    case '}':
                        if (!inBrackets)
                            throw new ArgumentException("Brackets already closed");
                        inBrackets = false;

                        var variable = _variableBuilder.ToString();
                        _variableBuilder.Clear();
                        var variableValue = _variables[variable]();
                        _expressionBuilder.Append(variableValue);
                        break;
                    
                    default:
                        if (inBrackets)
                            _variableBuilder.Append(currentChar);
                        else _expressionBuilder.Append(currentChar);
                        break;
                }
            }
            
            if (inBrackets)
                throw new ArgumentException("Brackets is not closed");
            
            return _expressionBuilder.ToString();
        }
    }
}